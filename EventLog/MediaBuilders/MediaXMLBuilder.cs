using System;
using EventLogCatcher.Core;
using System.IO;
using System.Xml.Linq;
using EventLogCatcher.Entities;
//Desarrollado por:Emmanuel Lohora
//Contacto: punky_casinotomo@hotmail.com
//version: miercoles 29 enero 2014
namespace EventLogCatcher.MediaBuilders
    {
    [Serializable]
    public class MediaXMLBuilder : MediaAbstract
        {

        
        #region Constructor

        /// <summary>
        /// Constructor Para Mandar Errores de Notificacion Utilizada cuando son errores Del Usuario.
        /// </summary>
        /// <param name="asuntoMensaje"></param>
        /// <param name="exception"></param>
        /// <param name="tipoNotificacion"></param>
        public MediaXMLBuilder(int pConfig, TemplateXMLAbstract pTemplate, infoSuceso pInfo)
            : base(pConfig, pInfo.TypeEvent, pTemplate, pInfo)
            {
                   
            }

        #endregion
 
        }
    }
