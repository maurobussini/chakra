using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using ZenProgramming.Chakra.Core.WebApi.Filters.Helpers;
using ZenProgramming.Chakra.Core.WebApi.Filters.Models;

namespace ZenProgramming.Chakra.Core.WebApi.Filters
{
    /// <summary>
    /// Traces requests and responses on action or controller API
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class TraceAttribute : ActionFilterAttribute
    {
        #region Private fields
        private RequestTrace _Request;
        private ResponseTrace _Response;
        #endregion        

        #region Properties
        /// <summary>
        /// Enable request trace
        /// </summary>
        public bool EnableRequestTrace { get; set; }

        /// <summary>
        /// Enable response trace
        /// </summary>
        public bool EnableResponseTrace { get; set; }
        #endregion        

        /// <summary>
        /// Constructor
        /// </summary>
        public TraceAttribute()
        {
            //Attivo tutti i trace di default
            EnableRequestTrace = true;
            EnableResponseTrace = true;
        }

        /// <summary>
        /// Trace request of action
        /// </summary>
        /// <param name="request">Request trace</param>
        protected virtual void TraceRequest(RequestTrace request)
        {
            //Eseguo la stringhificazione della request
            string value = TraceUtils.StringifyRequest(request);

            //Di base eseguo il tracciamento sul tracer impostato
            Debug.Write($"[{GetType().Name}] REQUEST: {value}");
        }
        
        /// <summary>
        /// Trace response of action
        /// </summary>
        /// <param name="response">Response trace</param>
        protected virtual void TraceResponse(ResponseTrace response)
        {
            //Eseguo la stringhificazione della response
            string value = TraceUtils.StringifyResponse(response);

            //Di base eseguo il tracciamento sul tracer impostato
            Debug.Write($"[{GetType().Name}] RESPONSE: {value}");
        }

        /// <summary>
        /// Occurs before the action method is invoked.
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            //Validazione argomenti
            if (actionContext == null) throw new ArgumentNullException(nameof(actionContext));

            //Esecuzione delle funzioni base
            base.OnActionExecuting(actionContext);

            //Nome del controller, action e http method (default)
            var controllerName = "<unknown>";
            var actionName = "<unknown>";
            var httpMethodName = "<unknown>";

            //Cast della action al descrittore del controller
            ControllerActionDescriptor controllerDescriptor = actionContext.ActionDescriptor as ControllerActionDescriptor;

            //Se il cast va a buon fine
            if (controllerDescriptor != null)
            {
                //Imposto il nome del controller e la action
                controllerName = controllerDescriptor.ControllerName;
                actionName = controllerDescriptor.ActionName;

                //Se non ho constraints, esco
                if (controllerDescriptor.ActionConstraints != null)
                {
                    //Tento il recupero del primo constraint su HTTP
                    var single = controllerDescriptor.ActionConstraints
                        .FirstOrDefault(c => c.GetType() == typeof(HttpMethodActionConstraint));

                    //Se ho trovato l'elemento
                    if (single != null)
                    {
                        //Cast e recupero il valore
                        var castedConstraint = (HttpMethodActionConstraint)single;
                        httpMethodName = castedConstraint.HttpMethods.SingleOrDefault();
                    }
                }
            }

            //Eseguo la generazione della trace request
            _Request = TraceUtils.GenerateRequest(actionContext.HttpContext.User,
                httpMethodName, 
                controllerDescriptor.ControllerName, 
                controllerDescriptor.ActionName, actionContext.ActionArguments);

            //Traccio la request (se richiesto)
            if (EnableRequestTrace)
                TraceRequest(_Request);
        }

        /// <summary>
        /// Occurs after the action method is invoked.
        /// </summary>
        /// <param name="actionExecutedContext">The action executed context.</param>
        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            //Validazione argomenti
            if (actionExecutedContext == null) throw new ArgumentNullException(nameof(actionExecutedContext));

            //Leggo il content e il relativo type
            BodyContent body = GetResponseBody(actionExecutedContext.Result);
            
            //Inizializzo la response
            _Response = TraceUtils.GenerateResponse(_Request, 
                body.Value, body.Type, body.Length, actionExecutedContext.Exception);

            //Traccio il response (se richiesto)
            if (EnableResponseTrace)
                TraceResponse(_Response);

            //Esecuzione delle funzioni base
            base.OnActionExecuted(actionExecutedContext);
        }
        
        /// <summary>
        /// Get response data using action result
        /// </summary>
        /// <param name="result">Action result</param>
        /// <returns>Returns structure to hold data</returns>
        private BodyContent GetResponseBody(IActionResult result)
        {
            //Default values
            var content = new BodyContent();

            //With invalid result, return empty
            if (result == null)
                return content;

            //If content result
            if (result is ContentResult or)
            {
                //Set values
                content.Length = or.Content?.Length;
                content.Type = or.ContentType;
                content.Value = or.Content;
                return content;
            }

            //If ok result
            if (result is OkResult ok)
            {
                //Set values
                content.Value = ok.StatusCode.ToString();
                content.Type = nameof(OkResult);                
                return content;
            }

            //If ok object result
            if (result is OkObjectResult okobj)
            {
                //Set values
                content.Value = okobj.Value == null ? "null" : JsonSerializer.Serialize(okobj.Value,new JsonSerializerOptions{WriteIndented = true});
                content.Type = okobj.Value == null ? null : okobj.Value.GetType().Name;
                return content;
            }

            //If empty
            if (result is EmptyResult er)
            {
                //Set values
                content.Value = "";
                content.Type = nameof(EmptyResult);
                return content;
            }

            //If file
            if (result is FileResult fr)
            {
                //Set name of file
                content.Value = fr.FileDownloadName;
                content.Type = nameof(FileResult);
                return content;
            }

            //If redirect
            else if (result is RedirectResult rr)
            {
                //Set url 
                content.Value = rr.Url;
                content.Type = nameof(RedirectResult);
                return content;
            }

            //If redirect to route 
            else if (result is RedirectToRouteResult rtr)
            {
                //Set route name
                content.Value = rtr.RouteName;
                content.Type = nameof(RedirectToRouteResult);
                return content;
            }

            //Otherwise, set value as string
            content.Value = result.ToString();
            content.Type = result.GetType().Name;
            return content;
        }

        /// <summary>
        /// Private class for hold body content
        /// </summary>
        private class BodyContent
        {
            /// <summary>
            /// Content
            /// </summary>
            public string Value { get; set; }

            /// <summary>
            /// Type
            /// </summary>
            public string Type { get; set; }

            /// <summary>
            /// Lenght
            /// </summary>
            public int? Length { get; set; }
        }
    }
}
