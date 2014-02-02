using EventLogCatcher.Entities;
using System.Xml.Linq;
using System.Configuration;
using System;
//Desarrollado por:Emmanuel Lohora
//Contacto: punky_casinotomo@hotmail.com
//version: miercoles 29 enero 2014
namespace EventLogCatcher.Core
    {
    [Serializable]
    public abstract class TemplateXMLAbstract : ITemplate
        {
        protected string header;
        protected int Config;
        protected string PathName;
        protected string filename;
        public infoSuceso InformacionSuceso = new infoSuceso();

        public abstract XElement SetAllInfo(XElement xml, string Encabezado, infoSuceso Suceso);
        public abstract XElement SetAlertsErros(XElement xml, string Encabezado, infoSuceso Suceso);
        public abstract XElement SetOnlyErros(XElement xml, string Encabezado, infoSuceso Suceso);

        public string CreateHeader(EventType HederType, int Conf, infoSuceso pInfo)
                 {
                 Config = Conf;
                 InformacionSuceso = pInfo;
                 switch (HederType)
                     {
                     case EventType.Information:
                       header= "Information";
                         break;
                     case EventType.Alert:
                         header= "Alerts";
                         break;
                     case EventType.Error:
                         header= "Errors";
                         break;
                     default:
                         header= "";
                         break;
                     }
                 return header;
                 }


        public string GetFileName()
            {
            return filename = ConfigurationManager.AppSettings["LogFileXML"].ToString();
            }

        public string GetPathName()
            {
            return PathName = ConfigurationManager.AppSettings["LogFilePath"].ToString();
            }

        public abstract bool WriteFile();
           
        }
    }
