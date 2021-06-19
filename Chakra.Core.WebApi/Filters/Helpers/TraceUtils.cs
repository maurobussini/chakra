using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.Principal;
using System.Text.Json;
using ZenProgramming.Chakra.WebApi.Filters.Models;

namespace ZenProgramming.Chakra.WebApi.Filters.Helpers
{
    /// <summary>
    /// Utilities for trace action
    /// </summary>
    internal static class TraceUtils
    {
        /// <summary>
        /// Executes generation of request trace using provided data
        /// </summary>
        /// <param name="httpMethod">Http method</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="actionName">Action name</param>
        /// <param name="actionParameters">Action parameters</param>
        /// <returns>Returns trace</returns>
        public static RequestTrace GenerateRequest(IPrincipal principal, string httpMethod, string controllerName, string actionName, IDictionary<string, object> actionParameters)
        {
            //Arguments validation
            if (principal == null) throw new ArgumentNullException(nameof(principal));
            if (string.IsNullOrEmpty(httpMethod)) throw new ArgumentNullException(nameof(httpMethod));
            if (string.IsNullOrEmpty(controllerName)) throw new ArgumentNullException(nameof(controllerName));
            if (string.IsNullOrEmpty(actionName)) throw new ArgumentNullException(nameof(actionName));

            //Generate autentication trace holder
            AuthenticationTrace auth = new AuthenticationTrace();

            //With a valid authentication
            if (principal.Identity != null)
            {
                //Insert values on object
                auth.IsAuthenticated = principal.Identity.IsAuthenticated;
                auth.AuthenticationType = principal.Identity.AuthenticationType;
                auth.IdentityName = principal.Identity.Name;
            }

            //Initialize trace object
            return new RequestTrace
            {
                //Set controller properties
                UniqueId = Guid.NewGuid(),
                CreationDate = DateTime.Now,
                HttpMethod = httpMethod,
                ControllerName = controllerName,
                ActionName = actionName,
                ActionParameters = actionParameters,

                //Set authentication
                Authentication = auth
            };
        }

        /// <summary>
        /// Convert to string specified authentication trace
        /// </summary>
        /// <param name="authentication">Authentication</param>
        /// <returns>Returns string representing object</returns>
        public static string StringifyAuthentication(AuthenticationTrace authentication)
        {
            //Argumenta validation
            if (authentication == null) throw new ArgumentNullException(nameof(authentication));

            //Set authentication data or anonymous
            return !authentication.IsAuthenticated ?
                "authentication:Anonymous" : $"authentication:{authentication.AuthenticationType}->{authentication.IdentityName}";
        }

        /// <summary>
        /// Convert to string specified request trace
        /// </summary>
        /// <param name="request">Request trace</param>
        /// <returns>Returns string representing object</returns>
        public static string StringifyRequest(RequestTrace request)
        {
            //Arguments validation
            if (request == null) throw new ArgumentNullException(nameof(request));

            //List of parameters
            var cleanedParams = new Dictionary<string, object>();

            //Scorro tutti gli elementi presenti nelle action params
            foreach (var currentParam in request.ActionParameters)
            {
                //Predispongo la key e i valori
                string cleanKey = currentParam.Key;
                object cleanValue;

                //Poichè l'elemento è un object che potrebbe non essere
                //serializzabile (es. "HttpPostedFileWrapper") eseguo la
                //serializzazione di ogni singolo valore, impostando un
                //placeholder nel caso in cui la serializzazione fallisca
                try
                {
                    //Recupero il valore corrente
                    object value = request.ActionParameters[currentParam.Key];

                    //Eseguo la serializzazione per testare se l'elemento da 
                    //errore o meno; se non lo da il contenuto è immutato, 
                    //altrimenti verrà rimpiazzato con il messaggio di errore
                    JsonSerializer.Serialize(value);

                    //Se la serializzazione non ha dato problemi, accetto il value
                    cleanValue = value;
                }
                catch (Exception exc)
                {
                    //Imposto il messaggio di errore in serializzazione
                    cleanValue = $"[Serialization error: {exc.Message}]";
                }

                //Accodo i valori nella lista "clean"
                cleanedParams.Add(cleanKey, cleanValue);
            }

            //Imposto i parametri "puliti" nella request
            request.ActionParameters = cleanedParams;

            //Eseguo la serializzazione della struttura dei parametri e dell'autenticazione
            string jsonParam = JsonSerializer.Serialize(request.ActionParameters,new JsonSerializerOptions{WriteIndented = true});
            string authentication = StringifyAuthentication(request.Authentication);

            //Formatto i valori del verb, controller e azione
            return
                $"(uid: {request.UniqueId}) {authentication} - [{request.HttpMethod}] {request.ControllerName}/{request.ActionName}({jsonParam})";
        }

        /// <summary>
        /// Executes trace of response using provided data
        /// </summary>
        /// <param name="request">Related request</param>
        /// <param name="bodyType">Result type</param>
        /// <param name="bodyContent">Body content</param>
        /// <param name="bodyLenght">Body length</param>
        /// <param name="exception">Raised exception</param>
        /// <returns>Returns response trace</returns>
        public static ResponseTrace GenerateResponse(RequestTrace request, string bodyContent, string bodyType, int? bodyLenght, Exception exception)
        {
            //Validazione argomenti
            if (request == null) throw new ArgumentNullException(nameof(request));

            //Predispongo il messaggio di errore
            string exceptionMessage = null;
            string exceptionStackTrace = null;

            //Se ho un'eccezione
            if (exception != null)
            {
                //Recupero messaggio e eccezion
                exceptionMessage = exception.Message;
                exceptionStackTrace = exception.ToString();

                //Tento il casting a "ReflectionTypeLoadException"
                ReflectionTypeLoadException reflection = exception as ReflectionTypeLoadException;

                //Se il cast ha avuto successo
                if (reflection != null)
                {
                    //Scorro tutte le eccezioni di caricamento e le accodo
                    foreach (var current in reflection.LoaderExceptions)
                    {
                        //Accodo messaggio e stacktrace
                        exceptionMessage = $"{exceptionMessage}{Environment.NewLine}=>{current.Message}";
                        exceptionStackTrace = $"{exceptionStackTrace}{Environment.NewLine}=>{current}";
                    }
                }
            }

            //Inizializzo la response
            return new ResponseTrace
            {
                //Applico la data di creazione e la durata
                CreationDate = DateTime.Now,
                Duration = DateTime.Now.Subtract(request.CreationDate),

                //Inserisco la request relativa e il tipo di risultato
                Request = request,

                //Traccio il body
                BodyType = bodyType,
                BodyContent= bodyContent,
                BodyLenght = bodyLenght,

                //Inserisco l'evenentuale errore generato
                Error = new ErrorTrace
                {
                    HasError = exception != null,
                    Message = exceptionMessage,
                    StackTrace = exceptionStackTrace,
                }
            };
        }

        /// <summary>
        /// Convert to string specified error trace
        /// </summary>
        /// <param name="error">Error</param>
        /// <returns>Returns string representing object</returns>
        public static string StringifyError(ErrorTrace error)
        {
            //Validazione argomenti
            if (error == null) throw new ArgumentNullException(nameof(error));

            //Specifico l'errore, solo se presente
            return !error.HasError ? "OK" : $"ERROR: {error.Message}{Environment.NewLine}{error.StackTrace}";
        }

        /// <summary>
        /// Convert to string specified response trace
        /// </summary>
        /// <param name="response">Response trace</param>
        /// <returns>Returns string representing object</returns>
        public static string StringifyResponse(ResponseTrace response)
        {
            //Validazione argomenti
            if (response == null) throw new ArgumentNullException(nameof(response));

            //Se il risultato non è nullo, tento la serializzazione solo se non è già una stringa
            string resultValue = string.IsNullOrEmpty(response.BodyContent) ? "-" : response.BodyContent;

            //Eseguo la serializzazione della struttura di errore
            string result = string.IsNullOrEmpty(response.BodyType) ? "null" : $"{response.BodyType}:{resultValue}";
            string error = StringifyError(response.Error);

            //Formatto i valori della request
            return
                $"(uid: {response.Request.UniqueId}) duration:{Math.Round(response.Duration.TotalMilliseconds, 0)}ms - {result} [{error}]";
        }

        /// <summary>
        /// Writes an error log file on file system
        /// </summary>
        /// <param name="response">Response</param>       
        /// <param name="targetFolder">Target folder</param>
        public static void WriteErrorLogFile(ResponseTrace response, string targetFolder)
        {
            //Validazione argomenti
            if (response == null) throw new ArgumentNullException(nameof(response));

            //Se non ho errori, esco dalla funzione
            if (!response.Error.HasError)
                return;

            //Se la folder è nulla o vuota, uso come root quella di esecuzione
            if (string.IsNullOrEmpty(targetFolder))
                targetFolder = AppDomain.CurrentDomain.BaseDirectory;

            //Se il la folder non è "rooted", combino con il percorso base
            if (!Path.IsPathRooted(targetFolder))
                targetFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, targetFolder);

            //Compongo il percorso della root folder (la creo se non esiste)
            if (!Directory.Exists(targetFolder))
                Directory.CreateDirectory(targetFolder);

            //Creo la folder del giorno corrente se non esiste
            string dayFolder = Path.Combine(targetFolder, DateTime.Now.ToString("yyyy-MM-dd"));
            if (!Directory.Exists(dayFolder))
                Directory.CreateDirectory(dayFolder);

            //Compongo il nome del file con l'eccezione
            string errorFile = Path.Combine(dayFolder,
                $"Error_{DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss")}_{DateTime.Now.Millisecond.ToString(CultureInfo.InvariantCulture).PadLeft(3, '0')}.log");

            //Il contenuto è un elenco di stringhe con tutti i dettagli
            IList<string> content = new List<string>();
            content.Add($"REQUEST : {StringifyRequest(response.Request)}");
            content.Add($"RESPONSE : {StringifyResponse(response)}");

            //Scrivo il contenuto sul file
            File.WriteAllLines(errorFile, content);
        }
    }
}
