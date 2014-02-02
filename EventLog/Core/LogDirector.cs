//Desarrollado por:Emmanuel Lohora
//Contacto: punky_casinotomo@hotmail.com
//version: miercoles 29 enero 2014
using System;
namespace EventLogCatcher.Core
    {
    [Serializable]
    public class LogDirector
        {
        /// <summary>
        /// Saves the event information in the appropiate media file, configured in the app.config 
        /// </summary>
        /// <param name="Suceso">Concrete Implementation object of the SuccesoAbstract class</param>
        /// <returns>variable answer ensures the process succes</returns>
        public bool Handles(SucesoAbstract Suceso)
            { 
            Suceso.GetDestiny();
            Suceso.GetConfiguration();
            return Suceso.Execute();
            }
        }
    }
