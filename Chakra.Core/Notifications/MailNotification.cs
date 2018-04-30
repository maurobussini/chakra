using System;
using System.Collections.Generic;
using System.Net.Mail;
using ZenProgramming.Chakra.Core.Diagnostic;
using ZenProgramming.Chakra.Core.Notifications.Common;

namespace ZenProgramming.Chakra.Core.Notifications
{
    /// <summary>
    /// Represents class for e-mail notification messages
    /// </summary>
    public class MailNotification : NotificationBase
    {
        #region Public properties
        /// <summary>
        /// Subject of message
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Get and set a flag that specify if body has HTML content
        /// </summary>
        public bool IsBodyHtml { get; set; }

        /// <summary>
        /// Get and set priority of the message
        /// </summary>
        public MailPriority Priority { get; set; }

        /// <summary>
        /// Get and set list of attachments
        /// </summary>
        public IList<Attachment> Attachments { get; set; }

        /// <summary>
        /// Get and set list of copy carbon recipients
        /// </summary>
        public IList<string> CopyCarbons { get; set; }

        /// <summary>
        /// Get and set list of blind copy carbon recipients
        /// </summary>
        public IList<string> BlindCopyCarbons { get; set; }
        #endregion

        /// <summary>
        /// Execute sending of notification
        /// </summary>
        /// <param name="throwOnError">Flag used to contron exception throwing</param>
        public override void Send(bool throwOnError)
        {
            try
            {
                //Istanzio un nuovo messaggio e-mail da inviare
                using (MailMessage message = new MailMessage())
                {
                    //Compongo l'indirizzo del mittente del messaggio e-mail
                    MailAddress sender = new MailAddress(Sender, Sender);

                    //Scorro l'elenco dei destinatari della e-mail e li aggiungo
                    if (Recipients != null)
                        foreach (string currentRecipient in Recipients)
                            message.To.Add(new MailAddress(currentRecipient));

                    //Scorro l'elenco dei destinatari in copia e li aggiungo (se necessario)
                    if (CopyCarbons != null)
                        foreach (string currentCopyCarbon in CopyCarbons)
                            message.CC.Add(new MailAddress(currentCopyCarbon));

                    //Scorro l'elenco dei destinatari in copia nascosta e li aggiungo (se necessario)
                    if (BlindCopyCarbons != null)
                        foreach (string currentBlindCopyCarbon in BlindCopyCarbons)
                            message.Bcc.Add(new MailAddress(currentBlindCopyCarbon));

                    //Scorro tutti gli allegati presenti e li aggiungo (se necessario)
                    if (Attachments != null)
                        foreach (var current in Attachments)
                            message.Attachments.Add(current);

                    //Inserisco le informazioni necessarie
                    message.Body = Content;
                    message.IsBodyHtml = IsBodyHtml;
                    message.Subject = Subject;
                    message.Priority = Priority;
                    message.From = sender;

                    //Istanzio la classe SMTP per l'invio delle informazioni
                    using (SmtpClient smtpClient = new SmtpClient())
                    {
                        //Eseguo l'invio effettivo del messaggio
                        smtpClient.Send(message);
                    }
                }
            }
            catch (ArgumentNullException exc)
            {
                //Traccio l'errore di invio
                Tracer.Error("Error sending email: {0}", exc);

                //Se ho impostato il flag, emetto l'eccezione
                if (throwOnError)
                    throw;
            }
            catch (ObjectDisposedException exc)
            {
                //Traccio l'errore di invio
                Tracer.Error("Error sending email: {0}", exc);

                //Se ho impostato il flag, emetto l'eccezione
                if (throwOnError)
                    throw;
            }
            catch (InvalidOperationException exc)
            {
                //Traccio l'errore di invio
                Tracer.Error("Error sending email: {0}", exc);

                //Se ho impostato il flag, emetto l'eccezione
                if (throwOnError)
                    throw;
            }
            catch (SmtpFailedRecipientsException exc)
            {
                //Traccio l'errore di invio
                Tracer.Error("Error sending email: {0}", exc);

                //Se ho impostato il flag, emetto l'eccezione
                if (throwOnError)
                    throw;
            }
            catch (SmtpException exc)
            {
                //Traccio l'errore di invio
                Tracer.Error("Error sending email: {0}", exc);

                //Se ho impostato il flag, emetto l'eccezione
                if (throwOnError)
                    throw;
            }
            catch (Exception exc)
            {
                //Traccio l'errore di invio
                Tracer.Error("Error sending email: {0}", exc);

                //Se ho impostato il flag, emetto l'eccezione
                if (throwOnError)
                    throw;
            }
        }
    }
}
