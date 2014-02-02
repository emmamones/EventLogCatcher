
using System;
using System.Configuration;
using EventLogCatcher.Templates;
using EventLogCatcher.MediaBuilders;
using EventLogCatcher.Core;
using EventLogCatcher.Entities;
//Desarrollado por:Emmanuel Lohora
//Contacto: punky_casinotomo@hotmail.com
namespace EventLogCatcher.Sucesos
{

    [Serializable]
    public class SucesoEventBuilder : SucesoAbstract
    {
       public infoSuceso InformacionSuceso;
        #region Constructor

        /// <summary>
        /// Constructor Vacio
        /// </summary>
        public SucesoEventBuilder(infoSuceso pInformacionSuceso) {
        InformacionSuceso = pInformacionSuceso;
            }

        /// <summary>
       /// Constructor Para Mandar Errores de Notificacion Utilizada cuando son errores Del Usuario.
       /// </summary>
       /// <param name="asuntoMensaje"></param>
       /// <param name="exception"></param>
       /// <param name="tipoNotificacion"></param>
        public SucesoEventBuilder(string asuntoMensaje, Exception exception)
       {
     
       }

        #endregion

        internal override void GetDestiny()
            {           
            base.LogDestino = Convert.ToInt32(ConfigurationManager.AppSettings["LogDestino"].ToString());
            }

        internal override void GetConfiguration()
            {
            base.Config = Convert.ToInt32(ConfigurationManager.AppSettings["LogConfig"].ToString());
                  
            switch (base.LogDestino)
                {
                case 0:
                    //writte to text                      
                    base.MediaExternal = new MediaTextBuilder(base.Config, new TemplateTextEvent(), InformacionSuceso);
                    break;
                //case 1:
                //    //WritteEventViewer(evento);
                //    break;
                //case 2:
                //    // WrritetoMemory(evento);
                //    break;
                case 3:
                    base.MediaExternal = new MediaXMLBuilder(base.Config, new TemplateXMLEvent(), InformacionSuceso);
                    break;
                //case 4:
                //    //WrritetoMemory(evento);
                //    //WrittetoXML(evento);
                //    break; 
                //default:
                //    ITemp = new TemplateEventText();
                //    externalMedia = new TextBuilder(base.Config, ITemp);
                //    break;
                }         
          
            }
    }
}
