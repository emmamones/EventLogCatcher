using System;
using System.IO;
using EventLogCatcher.Core;
using EventLogCatcher.Entities;
//Desarrollado por:Emmanuel Lohora
//Contacto: punky_casinotomo@hotmail.com
//version: miercoles 29 enero 2014
namespace EventLogCatcher.MediaBuilders
    {
    [Serializable]
    public class MediaTextBuilder : MediaAbstract
        {

       

        #region Constructor

        public MediaTextBuilder(int pConfig, TemplateTextAbstract pTemplate, infoSuceso pInfo)
            : base(pConfig, pInfo.TypeEvent, pTemplate, pInfo)
            {
                    
            }

        #endregion

        }
    }
