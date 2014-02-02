using System;
using System.Configuration;
using EventLogCatcher.Templates;
using EventLogCatcher.MediaBuilders;
using EventLogCatcher.Core;
using EventLogCatcher.Entities;
//Desarrollado por:Emmanuel Lohora
//Contacto: punky_casinotomo@hotmail.com
//Fecha : 6/27/2011
namespace EventLogCatcher.Sucesos
{
   
    [Serializable]
    public class SucesoInterfaseBuilder : SucesoAbstract
    {
      
       public infoSuceso InformacionSuceso;
        #region Constructor
        public SucesoInterfaseBuilder(infoSuceso pInformacionSuceso)
            { 
            InformacionSuceso = pInformacionSuceso;
             }
        #endregion

        internal override void GetDestiny()
            { 
            base.LogDestino = Convert.ToInt32(ConfigurationManager.AppSettings["LogDestinoInter"].ToString());
            }

        internal override void GetConfiguration()
            {
            base.Config = Convert.ToInt32(ConfigurationManager.AppSettings["LogConfigInter"].ToString());
            
            switch (base.LogDestino)
                {
                case 0:
              
                    base.MediaExternal = new MediaTextBuilder(base.Config,new TemplateTextInterface(), InformacionSuceso);
                    break;
                //case 1:
                //    //WritteEventViewer(evento);
                //    break;
                //case 2:
                //    // WrritetoMemory(evento);
                //    break;
                //case 3:
                //    // WrittetoXML(evento);
                //    break;
                //case 4:
                //    //WrritetoMemory(evento);
                //    //WrittetoXML(evento);
                //    break; 
                //default:
                //    ITemp = new TemplateInterfaceText();
                //    externalMedia = new TextBuilder(base.Config, ITemp);
                //    break;
                }  

            }
    }
}
