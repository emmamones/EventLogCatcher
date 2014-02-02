using System;
//Desarrollado por:Emmanuel Lohora
//Contacto: punky_casinotomo@hotmail.com

namespace EventLogCatcher
    {
    [Serializable]
    public class ExceptionNotificacionEmail:Exception
    {
         /// <summary>
        /// Exception lanzada, cuando el proceso de Notificacion por email, fallo.
        /// </summary>
        public ExceptionNotificacionEmail(string message)
            : base(message)
        {
        }

        public ExceptionNotificacionEmail(string message,Exception e)
            : base(message,e)
        {
          
        }
    }
}
