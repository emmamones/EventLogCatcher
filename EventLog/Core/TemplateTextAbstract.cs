using System.IO;
using EventLogCatcher.Entities;
using System.Configuration;
using System;
//Desarrollado por:Emmanuel Lohora
//Contacto: punky_casinotomo@hotmail.com
//version: miercoles 29 enero 2014
namespace EventLogCatcher.Core
    {
    [Serializable]
   public  abstract class TemplateTextAbstract:ITemplate
        {
       protected string header;
       protected int Config;
       protected string PathName;
       protected string filename;
       public infoSuceso InformacionSuceso = new infoSuceso();

       public abstract StreamWriter SetAllInfo(StreamWriter FW, string Encabezado, infoSuceso Suceso);
       public abstract StreamWriter SetAlertsErros(StreamWriter FW, string Encabezado, infoSuceso Suceso);
       public abstract StreamWriter SetOnlyErros(StreamWriter FW, string Encabezado, infoSuceso Suceso);

       public string CreateHeader(EventType HederType, int Conf,infoSuceso pInfo)
                 {
                 Config = Conf;
                 InformacionSuceso = pInfo;
                 switch (HederType)
                     {
                     case EventType.Information:
                       header= "(i) Information";
                         break;
                     case EventType.Alert:
                         header= "/A\\ Alert";
                         break;
                     case EventType.Error:
                         header= "[*] Error";
                         break;
                     default:
                         header= "";
                         break;
                     }
                 return header;
                 }


       public string GetFileName()
           {
           return filename=ConfigurationManager.AppSettings["LogFileText"].ToString();
           }

       public string GetPathName()
           {
           return PathName= ConfigurationManager.AppSettings["LogFilePath"].ToString();
           }

       public abstract bool WriteFile();
        }
    }
