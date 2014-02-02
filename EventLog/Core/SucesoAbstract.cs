
//Desarrollado por:Emmanuel Lohora
//Contacto: punky_casinotomo@hotmail.com
//version: miercoles 29 enero 2014
using System;
namespace EventLogCatcher.Core
    {
    [Serializable]
    public abstract class SucesoAbstract
        {
        #region Propiedades

        protected int LogDestino;
        protected int Config;
        protected MediaAbstract MediaExternal;     
        #endregion

        internal bool Execute()
            {
             return  MediaExternal.Start();           
            }
        internal abstract void GetDestiny();
        internal abstract void GetConfiguration();      
        }
    }
