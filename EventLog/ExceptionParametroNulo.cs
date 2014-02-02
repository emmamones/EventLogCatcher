using System;
//Desarrollado por:Emmanuel Lohora
//Contacto: punky_casinotomo@hotmail.com

namespace EventLogCatcher
    {
    [Serializable]
    public class ExceptionParametroNulo:Exception
    {
         /// <summary>
        /// Exception lanzada, cuando los parametros Vienen Nulos.
        /// </summary>
        public ExceptionParametroNulo(string message)
            : base(message)
        {
        }
    }
}
