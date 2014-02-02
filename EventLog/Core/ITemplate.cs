using EventLogCatcher.Entities;
using System;
//Desarrollado por:Emmanuel Lohora
//Contacto: punky_casinotomo@hotmail.com
//version: miercoles 29 enero 2014
namespace EventLogCatcher.Core
    { 
   public interface ITemplate
        {
       string CreateHeader(EventType HederType, int Conf, infoSuceso pInfo);
       string GetFileName();
       string GetPathName();
       bool WriteFile();
        }
    }
