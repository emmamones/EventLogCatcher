using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using EventLogCatcher.Sucesos;
using EventLogCatcher.Entities;
//Desarrollado por:Emmanuel Lohora
//Contacto: punky_casinotomo@hotmail.com
//version: miercoles 29 enero 2014

namespace EventLogCatcher
{
    [Serializable]
    public static class EventWriter
    {
        #region Properties
        public static List<SucesoEventBuilder> EventosLogList;
        public static List<SucesoInterfaseBuilder> IntefaseLogList;
        static string ConGenral = string.Empty;
        #endregion

        #region Methods

        /// <summary>
        /// Log para el sistema general. tomando en cuenta el retorno del destino pre configurado en el app.config.
        /// </summary>
        /// <param name="evento"></param>
        public static void Log(SucesoEventBuilder evento)
        {
            switch (GetDestiny())
            {
                case 0:
                    WrittetoText(evento);
                    break;
                case 1:
                    WritteEventViewer(evento);
                    break;
                case 2:
                    WrritetoMemory(evento);
                    break;
                case 3:
                    WrittetoXML(evento);
                    break;
                case 4:
                    WrritetoMemory(evento);
                    WrittetoXML(evento);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Log para interfases tomando en cuenta el retorno del destino pre configurado en el app.config .
        /// </summary>
        /// <param name="InterfasEvento"></param>
        public static void Log(SucesoInterfaseBuilder InterfasEvento)
        {
            switch (GetDestinyInterfase())
            {
                case 0:
                    WrittetoText(InterfasEvento);
                    break;
                case 1:
                    WritteEventViewer(InterfasEvento);
                    break;
                case 2:
                    WrritetoMemory(InterfasEvento);
                    break;
                case 3:
                    WrittetoXML(InterfasEvento);
                    break;
                case 4:
                    WrritetoMemory(InterfasEvento);
                    WrittetoXML(InterfasEvento);
                    break;
                default:
                    break;
            }
        }

        #region Read and Configuration
        private static int GetDestiny()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["LogDestino"].ToString());
        }

        private static int GetConfiguration()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["LogConfig"].ToString());
        }

        private static string GetFileName()
        {
            return ConfigurationManager.AppSettings["LogFileText"].ToString();
        }
        private static string GetPathName()
        {
            return ConfigurationManager.AppSettings["LogFilePath"].ToString();
        }

        private static string GetFileNameXml()
        {
            return ConfigurationManager.AppSettings["LogFileXML"].ToString();
        }

        private static string GetResourceName()
        {
            return ConfigurationManager.AppSettings["LogSourceName"].ToString();
        }

        #endregion

        #region Read and Configuration interfase
        private static int GetDestinyInterfase()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["LogDestinoInter"].ToString());
        }

        private static int GetConfigurationInterfase()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["LogConfigInter"].ToString());
        }

        private static string GetFileNameInterfase()
        {
            return ConfigurationManager.AppSettings["LogFileTextInter"].ToString();
        }
        private static string GetPathNameInterfase()
        {
            return ConfigurationManager.AppSettings["LogFilePathInter"].ToString();
        }

        private static string GetFileNameXmlInterfase()
        {
            return ConfigurationManager.AppSettings["LogFileXMLInter"].ToString();
        }

        private static string GetResourceNameInterfase()
        {
            return ConfigurationManager.AppSettings["LogSourceNameInter"].ToString();
        }

        #endregion

        #region XML
        private static void WrittetoXML(SucesoEventBuilder evento)
        {
            string filename = GetFileNameXml();
            XDocument fileXml = new XDocument();
            XElement xml;

            string path = GetPathName();

            if (!Directory.Exists(filename))
            {
                Directory.CreateDirectory(path);
            }

            if (File.Exists(filename))
            {
                fileXml = XDocument.Load(filename);
                xml = fileXml.Root;
            }
            else 
            {
                xml = new XElement("Log");
                xml.Add(new XElement("Information"));
                xml.Add(new XElement("Alerts"));
                xml.Add(new XElement("Errors"));
            }
            switch (GetConfiguration())
            {
                case 0:
                    InsertallInfo(xml,evento);
                    break;
                case 1:
                    INsertAlertsandErrors(xml, evento);
                    break;
                default:
                    InsertErrors(xml, evento);
                    break;
            }
            fileXml = new XDocument(xml);
            fileXml.Save(filename);
        }

        private static void InsertErrors(XElement xml, SucesoEventBuilder evento)
        {
            switch (evento.InformacionSuceso.TypeEvent)
            {
                case EventType.Information:
                    break;
                case EventType.Alert:
                    break;
                case EventType.Error:
                    xml.Element("Errors").Add(
                        new XElement("Event", new XAttribute("Aplication", evento.InformacionSuceso.Application)
                            , new XAttribute("EventName", evento.InformacionSuceso.Event)
                            , new XAttribute("MethodName", evento.InformacionSuceso.Method)
                            , new XAttribute("Date", evento.InformacionSuceso.Date.ToString())));
                    xml.Element("Errors").Descendants("Event").Last().Add(new XElement("CustomMessage", evento.InformacionSuceso.CustomMessage)
                        , new XElement("Status", evento.InformacionSuceso.Status)
                        , new XElement("SystemMessage", evento.InformacionSuceso.SystemMessage)
                        , new XElement("SystemError", evento.InformacionSuceso.SystemError)
                        , new XElement("FileName", evento.InformacionSuceso.FileName)
                        , new XElement("Opcional", evento.InformacionSuceso.OpcionalField)
                         , new XElement("Opcional", evento.InformacionSuceso.OpcionalField2));
                    break;
                default:
                    break;
            }
        }

        private static void INsertAlertsandErrors(XElement xml, SucesoEventBuilder evento)
        {
            switch (evento.InformacionSuceso.TypeEvent)
            {
                case EventType.Information:
                    break;
                case EventType.Alert:
                    xml.Element("Alerts").Add(
                     new XElement("Event", new XAttribute("Aplication", evento.InformacionSuceso.Application)
                         , new XAttribute("EventName", evento.InformacionSuceso.Event)
                         , new XAttribute("MethodName", evento.InformacionSuceso.Method)
                         , new XAttribute("Date", evento.InformacionSuceso.Date.ToString())));
                      xml.Element("Alerts").Descendants("Event").Last().Add(new XElement("CustomMessage", evento.InformacionSuceso.CustomMessage)
                        , new XElement("SystemMessage", evento.InformacionSuceso.SystemMessage)                       
                        , new XElement("FileName", evento.InformacionSuceso.FileName)
                        , new XElement("Status", evento.InformacionSuceso.Status)
                         , new XElement("Opcional", evento.InformacionSuceso.OpcionalField));
                    break;
                case EventType.Error:
                    xml.Element("Errors").Add(
                      new XElement("Event", new XAttribute("Aplication", evento.InformacionSuceso.Application)
                          , new XAttribute("EventName", evento.InformacionSuceso.Event)
                          , new XAttribute("MethodName", evento.InformacionSuceso.Method)
                          , new XAttribute("Date", evento.InformacionSuceso.Date.ToString())));
                    xml.Element("Errors").Descendants("Event").Last().Add(new XElement("CustomMessage", evento.InformacionSuceso.CustomMessage)
                        , new XElement("Status", evento.InformacionSuceso.Status)
                        , new XElement("SystemMessage", evento.InformacionSuceso.SystemMessage)
                        , new XElement("SystemError", evento.InformacionSuceso.SystemError)
                        , new XElement("FileName", evento.InformacionSuceso.FileName)
                        , new XElement("Opcional", evento.InformacionSuceso.OpcionalField)
                         , new XElement("Opcional", evento.InformacionSuceso.OpcionalField2));

                    break;
                default:
                    break;
            }
        }

        private static void InsertallInfo(XElement xml, SucesoEventBuilder evento)
        {
            switch (evento.InformacionSuceso.TypeEvent)
            {
                case EventType.Information:
                    xml.Element("Information").Add(
                new XElement("Event", new XAttribute("Aplication", evento.InformacionSuceso.Application)
                    , new XAttribute("EventName", evento.InformacionSuceso.Event)
                    , new XAttribute("MethodName", evento.InformacionSuceso.Method)
                    , new XAttribute("Date", evento.InformacionSuceso.Date.ToString())));
                    xml.Element("Information").Descendants("Event").Last().Add(new XElement("CustomMessage", evento.InformacionSuceso.CustomMessage)
                        , new XElement("SystemMessage", evento.InformacionSuceso.SystemMessage)                       
                        , new XElement("FileName", evento.InformacionSuceso.FileName)
                        , new XElement("Status", evento.InformacionSuceso.Status)
                         , new XElement("Opcional", evento.InformacionSuceso.OpcionalField));
                    break;
                case EventType.Alert:
                    xml.Element("Alerts").Add(
                  new XElement("Event", new XAttribute("Aplication", evento.InformacionSuceso.Application)
                      , new XAttribute("EventName", evento.InformacionSuceso.Event)
                      , new XAttribute("MethodName", evento.InformacionSuceso.Method)
                      , new XAttribute("Date", evento.InformacionSuceso.Date.ToString())));
                    xml.Element("Alerts").Descendants("Event").Last().Add(new XElement("CustomMessage", evento.InformacionSuceso.CustomMessage)
                      , new XElement("SystemMessage", evento.InformacionSuceso.SystemMessage)
                      , new XElement("FileName", evento.InformacionSuceso.FileName)
                      , new XElement("Status", evento.InformacionSuceso.Status)
                       , new XElement("Opcional", evento.InformacionSuceso.OpcionalField));
                    break;
                case EventType.Error:
                    xml.Element("Errors").Add(
                  new XElement("Event", new XAttribute("Aplication", evento.InformacionSuceso.Application)
                      , new XAttribute("EventName", evento.InformacionSuceso.Event)
                      , new XAttribute("MethodName", evento.InformacionSuceso.Method)
                      , new XAttribute("Date", evento.InformacionSuceso.Date.ToString())));
                    xml.Element("Errors").Descendants("Event").Last().Add(new XElement("CustomMessage", evento.InformacionSuceso.CustomMessage)
                        , new XElement("Status", evento.InformacionSuceso.Status)
                        , new XElement("SystemMessage", evento.InformacionSuceso.SystemMessage)
                        , new XElement("SystemError", evento.InformacionSuceso.SystemError)
                        , new XElement("FileName", evento.InformacionSuceso.FileName)
                        , new XElement("Opcional", evento.InformacionSuceso.OpcionalField)
                         , new XElement("Opcional", evento.InformacionSuceso.OpcionalField2));
                    break;
                default:
                    break;
            }
        }


        //region Interfases
        private static void WrittetoXML(SucesoInterfaseBuilder InterfasEvento)
        {
            string filename = GetFileNameXmlInterfase();
            XDocument fileXml = new XDocument();
            XElement xml;

            string path = GetPathNameInterfase();

            if (!Directory.Exists(filename))
            {
                Directory.CreateDirectory(path);
            }

            if (File.Exists(filename))
            {
                fileXml = XDocument.Load(filename);
                xml = fileXml.Root;
            }
            else
            {
                xml = new XElement("Log");
                xml.Add(new XElement("Information"));
                xml.Add(new XElement("Alerts"));
                xml.Add(new XElement("Errors"));
            }
            switch (GetConfiguration())
            {
                case 0:
                    InsertallInfo(xml, InterfasEvento);
                    break;
                case 1:
                    INsertAlertsandErrors(xml, InterfasEvento);
                    break;
                default:
                    InsertErrors(xml, InterfasEvento);
                    break;
            }
            fileXml = new XDocument(xml);
            fileXml.Save(filename);
        }

        private static void InsertErrors(XElement xml, SucesoInterfaseBuilder InterfasEvento)
        {
        switch (InterfasEvento.InformacionSuceso.TypeEvent)
            {
            case EventType.Information:
                    break;
                case EventType.Alert:
                    break;
                case EventType.Error:
                    xml.Element("Errors").Add(
                        new XElement("Interfase", new XAttribute("Aplication", InterfasEvento.InformacionSuceso.NombreInterfase)
                         , new XAttribute("IdProceso", InterfasEvento.InformacionSuceso.IdInterfase)
                            , new XAttribute("EventName", InterfasEvento.InformacionSuceso.Event)
                            , new XAttribute("MethodName", InterfasEvento.InformacionSuceso.Method)
                            , new XAttribute("Date", InterfasEvento.InformacionSuceso.Date.ToString())
                            ,new XAttribute("CustomMessage", InterfasEvento.InformacionSuceso.Subject)
                            , new XAttribute("Status", InterfasEvento.InformacionSuceso.Resultado)
                            , new XAttribute("SystemMessage", InterfasEvento.InformacionSuceso.SystemMessage)
                            , new XAttribute("SystemError", InterfasEvento.InformacionSuceso.SystemError)
                            , new XAttribute("AControl", InterfasEvento.InformacionSuceso.AControl)
                            , new XAttribute("ADatos", InterfasEvento.InformacionSuceso.ADatos)
                            , new XAttribute("Opcional", InterfasEvento.InformacionSuceso.OpcionalField)
                         ));
                    break;
                default:
                    break;
            }
        }

        private static void INsertAlertsandErrors(XElement xml, SucesoInterfaseBuilder InterfasEvento)
        {
            switch (InterfasEvento.InformacionSuceso.TypeEvent)
            {
                case EventType.Information:
                    break;
                case EventType.Alert:
                    xml.Element("Alerts").Add(
                    new XElement("Interfase", new XAttribute("Aplication", InterfasEvento.InformacionSuceso.NombreInterfase)
                     , new XAttribute("IdProceso", InterfasEvento.InformacionSuceso.IdInterfase)
                        , new XAttribute("EventName", InterfasEvento.InformacionSuceso.Event)
                        , new XAttribute("MethodName", InterfasEvento.InformacionSuceso.Method)
                      , new XAttribute("Date", InterfasEvento.InformacionSuceso.Date.ToString())
                              , new XAttribute("CustomMessage", InterfasEvento.InformacionSuceso.Subject)
                              , new XAttribute("Status", InterfasEvento.InformacionSuceso.Resultado)
                              , new XAttribute("SystemMessage", InterfasEvento.InformacionSuceso.SystemMessage)
                              , new XAttribute("SystemError", InterfasEvento.InformacionSuceso.SystemError)
                              , new XAttribute("AControl", InterfasEvento.InformacionSuceso.AControl)
                              , new XAttribute("ADatos", InterfasEvento.InformacionSuceso.ADatos)
                              , new XAttribute("Opcional", InterfasEvento.InformacionSuceso.OpcionalField)
                           ));
                    break;
                case EventType.Error:
                    xml.Element("Errors").Add(
                       new XElement("Interfase", new XAttribute("Aplication", InterfasEvento.InformacionSuceso.NombreInterfase)
                        , new XAttribute("IdProceso", InterfasEvento.InformacionSuceso.IdInterfase)
                           , new XAttribute("EventName", InterfasEvento.InformacionSuceso.Event)
                           , new XAttribute("MethodName", InterfasEvento.InformacionSuceso.Method)
                           , new XAttribute("Date", InterfasEvento.InformacionSuceso.Date.ToString())
                           , new XAttribute("CustomMessage", InterfasEvento.InformacionSuceso.Subject)
                           , new XAttribute("Status", InterfasEvento.InformacionSuceso.Resultado)
                           , new XAttribute("SystemMessage", InterfasEvento.InformacionSuceso.SystemMessage)
                           , new XAttribute("SystemError", InterfasEvento.InformacionSuceso.SystemError)
                           , new XAttribute("AControl", InterfasEvento.InformacionSuceso.AControl)
                           , new XAttribute("ADatos", InterfasEvento.InformacionSuceso.ADatos)
                           , new XAttribute("Opcional", InterfasEvento.InformacionSuceso.OpcionalField)
                        ));

                    break;
                default:
                    break;
            }
        }

        private static void InsertallInfo(XElement xml, SucesoInterfaseBuilder InterfasEvento)
        {
            switch (InterfasEvento.InformacionSuceso.TypeEvent)
            {
                case EventType.Information:
                    xml.Element("Information").Add(
                   new XElement("Interfase", new XAttribute("Aplication", InterfasEvento.InformacionSuceso.NombreInterfase)
                    , new XAttribute("IdProceso", InterfasEvento.InformacionSuceso.IdInterfase)
                       , new XAttribute("EventName", InterfasEvento.InformacionSuceso.Event)
                       , new XAttribute("MethodName", InterfasEvento.InformacionSuceso.Method)
                    , new XAttribute("Date", InterfasEvento.InformacionSuceso.Date.ToString())
                              , new XAttribute("CustomMessage", InterfasEvento.InformacionSuceso.Subject)
                              , new XAttribute("Status", InterfasEvento.InformacionSuceso.Resultado)
                              , new XAttribute("SystemMessage", InterfasEvento.InformacionSuceso.SystemMessage)
                              , new XAttribute("SystemError", InterfasEvento.InformacionSuceso.SystemError)
                              , new XAttribute("AControl", InterfasEvento.InformacionSuceso.AControl)
                              , new XAttribute("ADatos", InterfasEvento.InformacionSuceso.ADatos)
                              , new XAttribute("Opcional", InterfasEvento.InformacionSuceso.OpcionalField)
                           ));
                    break;
                case EventType.Alert:
                    xml.Element("Alerts").Add(
                    new XElement("Interfase", new XAttribute("Aplication", InterfasEvento.InformacionSuceso.NombreInterfase)
                     , new XAttribute("IdProceso", InterfasEvento.InformacionSuceso.IdInterfase)
                        , new XAttribute("EventName", InterfasEvento.InformacionSuceso.Event)
                        , new XAttribute("MethodName", InterfasEvento.InformacionSuceso.Method)
                        , new XAttribute("Date", InterfasEvento.InformacionSuceso.Date.ToString())
                              , new XAttribute("CustomMessage", InterfasEvento.InformacionSuceso.Subject)
                              , new XAttribute("Status", InterfasEvento.InformacionSuceso.Resultado)
                              , new XAttribute("SystemMessage", InterfasEvento.InformacionSuceso.SystemMessage)
                              , new XAttribute("SystemError", InterfasEvento.InformacionSuceso.SystemError)
                              , new XAttribute("AControl", InterfasEvento.InformacionSuceso.AControl)
                              , new XAttribute("ADatos", InterfasEvento.InformacionSuceso.ADatos)
                              , new XAttribute("Opcional", InterfasEvento.InformacionSuceso.OpcionalField)
                           ));
                    break;
                case EventType.Error:
                    xml.Element("Errors").Add(
                          new XElement("Interfase", new XAttribute("Aplication", InterfasEvento.InformacionSuceso.NombreInterfase)
                           , new XAttribute("IdProceso", InterfasEvento.InformacionSuceso.IdInterfase)
                              , new XAttribute("EventName", InterfasEvento.InformacionSuceso.Event)
                              , new XAttribute("MethodName", InterfasEvento.InformacionSuceso.Method)
                              , new XAttribute("Date", InterfasEvento.InformacionSuceso.Date.ToString())
                              , new XAttribute("CustomMessage", InterfasEvento.InformacionSuceso.Subject)
                              , new XAttribute("Status", InterfasEvento.InformacionSuceso.Resultado)
                              , new XAttribute("SystemMessage", InterfasEvento.InformacionSuceso.SystemMessage)
                              , new XAttribute("SystemError", InterfasEvento.InformacionSuceso.SystemError)
                              , new XAttribute("AControl", InterfasEvento.InformacionSuceso.AControl)
                              , new XAttribute("ADatos", InterfasEvento.InformacionSuceso.ADatos)
                              , new XAttribute("Opcional", InterfasEvento.InformacionSuceso.OpcionalField)
                           ));
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Memory
        private static void WrritetoMemory(SucesoEventBuilder evento)
        {
            if (EventosLogList == null)
            {
                EventosLogList = new List<SucesoEventBuilder>();
            }
            switch (GetConfiguration())
            {
                case 0:
                    {
                        EventosLogList.Add(evento);
                    }
                    break;
                case 1:
                    {
                        if (evento.InformacionSuceso.TypeEvent != EventType.Information)
                            EventosLogList.Add(evento);
                    }
                    break;
                default:
                    {
                        if (evento.InformacionSuceso.TypeEvent == EventType.Error)
                            EventosLogList.Add(evento);
                    }
                    break;
            }
        }
        //region Interfases
        private static void WrritetoMemory(SucesoInterfaseBuilder evento)
        {
            if (IntefaseLogList == null)
            {
                IntefaseLogList = new List<SucesoInterfaseBuilder>();
            }
            switch (GetConfigurationInterfase())
            {
                case 0:
                    {
                        IntefaseLogList.Add(evento);
                    }
                    break;
                case 1:
                    {
                        if (evento.InformacionSuceso.TypeEvent != EventType.Information)
                            IntefaseLogList.Add(evento);
                    }
                    break;
                default:
                    {
                        if (evento.InformacionSuceso.TypeEvent == EventType.Error)
                            IntefaseLogList.Add(evento);
                    }
                    break;
            }
        }
        #endregion

        #region EventViewer
        private static void WritteEventViewer(SucesoEventBuilder evento)
        {
            switch (GetConfiguration())
            {
                case 0:
                    WritteAllEventViewer(evento);
                    break;
                case 1:
                    WritteAlertsandErrosEventViewer(evento);
                    break;
                default:
                    WritteErrorsEventViewer(evento);
                    break;
            }
        }

        private static void WritteErrorsEventViewer(SucesoEventBuilder evento)
        {
            EventSourceCreationData SourceData = new EventSourceCreationData(GetResourceName(), evento.InformacionSuceso.Application);
            SourceData.MachineName = ".";
            if (!EventLog.SourceExists(GetResourceName(), SourceData.MachineName))
                EventLog.CreateEventSource(GetResourceName(), evento.InformacionSuceso.Application);
            EventLog mylog = new EventLog();
            mylog.MachineName = SourceData.MachineName;
            mylog.Source = SourceData.Source;
            mylog.Log =  evento.InformacionSuceso.Application;
            switch (evento.InformacionSuceso.TypeEvent)
            {
                case EventType.Information:
                   
                    break;
                case EventType.Alert:
                   
                    break;
                case EventType.Error:
                    mylog.WriteEntry(GetMessage(evento), EventLogEntryType.Error);
                    break;
                default:
                    break;
            }
        }

        private static void WritteAlertsandErrosEventViewer(SucesoEventBuilder evento)
        {
            EventSourceCreationData SourceData = new EventSourceCreationData(GetResourceName(), evento.InformacionSuceso.Application);
            SourceData.MachineName = ".";
            if (!EventLog.SourceExists(GetResourceName(), SourceData.MachineName))
                EventLog.CreateEventSource(GetResourceName(), evento.InformacionSuceso.Application);
            EventLog mylog = new EventLog();
            mylog.MachineName = SourceData.MachineName;
            mylog.Source = SourceData.Source;
            mylog.Log =  evento.InformacionSuceso.Application;
            switch (evento.InformacionSuceso.TypeEvent)
            {
                case EventType.Information:
                    break;
                case EventType.Alert:
                    mylog.WriteEntry(GetMessage(evento), EventLogEntryType.Warning);
                    break;
                case EventType.Error:
                    mylog.WriteEntry(GetMessage(evento), EventLogEntryType.Error);
                    break;
                default:
                    break;
            }
        }

        private static void WritteAllEventViewer(SucesoEventBuilder evento)
        {
            EventSourceCreationData SourceData = new EventSourceCreationData(GetResourceName(),evento.InformacionSuceso.Application);
            SourceData.MachineName = ".";
            if (!EventLog.SourceExists(GetResourceName(), SourceData.MachineName))
                EventLog.CreateEventSource(GetResourceName(), evento.InformacionSuceso.Application);
            EventLog mylog = new EventLog();
            mylog.MachineName = SourceData.MachineName;
            mylog.Source = SourceData.Source;
            mylog.Log =  evento.InformacionSuceso.Application;
            switch (evento.InformacionSuceso.TypeEvent)
            {
                case EventType.Information:
                    mylog.WriteEntry(GetMessage(evento), EventLogEntryType.Information);
                    break;
                case EventType.Alert:
                    mylog.WriteEntry(GetMessage(evento), EventLogEntryType.Warning);
                    break;
                case EventType.Error:
                    mylog.WriteEntry(GetMessage(evento), EventLogEntryType.Error);
                    break;
                default:
                    break;
            }
        }

        private static string GetMessage(SucesoEventBuilder evento)
        {
            string mensaje = string.Empty;
            mensaje = string.Concat("Application:", evento.InformacionSuceso.Application, " Event:", evento.InformacionSuceso.Event, " Method:", evento.InformacionSuceso.Method
                , " Date:", evento.InformacionSuceso.Date, " Custom Message:", evento.InformacionSuceso.CustomMessage, " System Message: "
                , evento.InformacionSuceso.SystemMessage," System Error: " , evento.InformacionSuceso.SystemError, "Status: ", evento.InformacionSuceso.Status, " File Name:", evento.InformacionSuceso.FileName, "Opcional: ", evento.InformacionSuceso.OpcionalField);
            return mensaje;
        }

        //region Interfases

        private static void WritteEventViewer(SucesoInterfaseBuilder InterfasEvento)
        {
            switch (GetConfigurationInterfase())
            {
                case 0:
                    WritteAllEventViewer(InterfasEvento);
                    break;
                case 1:
                    WritteAlertsandErrosEventViewer(InterfasEvento);
                    break;
                default:
                    WritteErrorsEventViewer(InterfasEvento);
                    break;
            }
        }

        private static void WritteErrorsEventViewer(SucesoInterfaseBuilder InterfasEvento)
        {
            EventSourceCreationData SourceData = new EventSourceCreationData(GetResourceNameInterfase(), InterfasEvento.InformacionSuceso.NombreInterfase);
            
            SourceData.MachineName = ".";
            if (!EventLog.SourceExists(GetResourceNameInterfase(), SourceData.MachineName))
                EventLog.CreateEventSource(GetResourceNameInterfase(), InterfasEvento.InformacionSuceso.NombreInterfase);
            EventLog mylog = new EventLog();
            mylog.MachineName = SourceData.MachineName;
            mylog.Source = SourceData.Source;
            mylog.Log =  InterfasEvento.InformacionSuceso.NombreInterfase;
            switch (InterfasEvento.InformacionSuceso.TypeEvent)
            {
                case EventType.Information:

                    break;
                case EventType.Alert:

                    break;
                case EventType.Error:
                    mylog.WriteEntry(GetMessage(InterfasEvento), EventLogEntryType.Error);
                    break;
                default:
                    break;
            }
        }

        private static void WritteAlertsandErrosEventViewer(SucesoInterfaseBuilder InterfasEvento)
        {
            EventSourceCreationData SourceData = new EventSourceCreationData(GetResourceNameInterfase(), InterfasEvento.InformacionSuceso.NombreInterfase);
            SourceData.MachineName = ".";
            if (!EventLog.SourceExists(GetResourceNameInterfase(), SourceData.MachineName))
                EventLog.CreateEventSource(GetResourceNameInterfase(), InterfasEvento.InformacionSuceso.NombreInterfase);
            EventLog mylog = new EventLog();
            mylog.MachineName = SourceData.MachineName;
            mylog.Source = SourceData.Source;
            mylog.Log =  InterfasEvento.InformacionSuceso.NombreInterfase;
            switch (InterfasEvento.InformacionSuceso.TypeEvent)
            {
                case EventType.Information:
                    break;
                case EventType.Alert:
                    mylog.WriteEntry(GetMessage(InterfasEvento), EventLogEntryType.Warning);
                    break;
                case EventType.Error:
                    mylog.WriteEntry(GetMessage(InterfasEvento), EventLogEntryType.Error);
                    break;
                default:
                    break;
            }
        }

        private static void WritteAllEventViewer(SucesoInterfaseBuilder InterfasEvento)
        {
            EventSourceCreationData SourceData = new EventSourceCreationData(GetResourceNameInterfase(), InterfasEvento.InformacionSuceso.NombreInterfase);
            SourceData.MachineName = ".";
            if (!EventLog.SourceExists(GetResourceNameInterfase(), SourceData.MachineName))
                EventLog.CreateEventSource(GetResourceNameInterfase(), InterfasEvento.InformacionSuceso.NombreInterfase);
            EventLog mylog = new EventLog();
            mylog.MachineName = SourceData.MachineName;
            mylog.Source = SourceData.Source;
            mylog.Log =  InterfasEvento.InformacionSuceso.NombreInterfase;
            switch (InterfasEvento.InformacionSuceso.TypeEvent)
            {
                case EventType.Information:
                    mylog.WriteEntry(GetMessage(InterfasEvento), EventLogEntryType.Information);
                    break;
                case EventType.Alert:
                    mylog.WriteEntry(GetMessage(InterfasEvento), EventLogEntryType.Warning);
                    break;
                case EventType.Error:
                    mylog.WriteEntry(GetMessage(InterfasEvento), EventLogEntryType.Error);
                    break;
                default:
                    break;
            }
        }

        private static string GetMessage(SucesoInterfaseBuilder InterfasEvento)
        {
            string mensaje = string.Empty;
            mensaje = string.Concat("Interfase:", InterfasEvento.InformacionSuceso.NombreInterfase, " IdProceso:",InterfasEvento.InformacionSuceso.IdInterfase, " Event:", InterfasEvento.InformacionSuceso.Event, " Method:", InterfasEvento.InformacionSuceso.Method
                , " Date:", InterfasEvento.InformacionSuceso.Date, " Custom Message:", InterfasEvento.InformacionSuceso.Subject, " System Message: "
                , InterfasEvento.InformacionSuceso.SystemMessage, " System Error: ", InterfasEvento.InformacionSuceso.SystemError, "Resultado: ", InterfasEvento.InformacionSuceso.Resultado, " Opcional:"
                , InterfasEvento.InformacionSuceso.OpcionalField, "AControl: ", InterfasEvento.InformacionSuceso.AControl, "ADatos :", InterfasEvento.InformacionSuceso.ADatos);
            return mensaje;
        }
        #endregion

        #region Text
        private static void WrittetoText(SucesoEventBuilder evento)
        {
            string filename = GetFileName();
            StreamWriter filewritter;

            string path = GetPathName();

            if (!Directory.Exists(filename))
            {
                Directory.CreateDirectory(path);
            }

            if (!File.Exists(filename))
            {
                filewritter = File.CreateText(filename);
            }
            else
            {
                //FileStream files = new FileStream(filename, FileMode.Append, FileAccess.Write);
                using (FileStream files = new FileStream(filename, FileMode.Append, FileAccess.Write))
                {
                    filewritter = new StreamWriter(files);
                }
            }
            
            switch (GetConfiguration())
            {
                case 0:
                    WrriteAll(filewritter, evento);
                    break;
                case 1:
                    WritteAllertsandErrors(filewritter, evento);
                    break;
                default:
                    WritteErrors(filewritter, evento);
                    break;
            }
            filewritter.Close();
        }

        private static void WritteErrors(StreamWriter filewriter, SucesoEventBuilder evento)
        {
            switch (evento.InformacionSuceso.TypeEvent)
            {
                case EventType.Information:
                    {
                    }
                    break;
                case EventType.Alert:
                    {
                    }
                    break;
                case EventType.Error:
                    {
                        filewriter.WriteLine("======================================");
                        filewriter.WriteLine("[*] Error");
                        filewriter.WriteLine("Date: " + evento.InformacionSuceso.Date.ToString());
                        filewriter.WriteLine("Application: " + evento.InformacionSuceso.Application);
                        filewriter.WriteLine("Event: " + evento.InformacionSuceso.Event);
                        filewriter.WriteLine("Method: " + evento.InformacionSuceso.Method);
                        filewriter.WriteLine("Status: " + evento.InformacionSuceso.Status);
                        filewriter.WriteLine("Custom Message: " + evento.InformacionSuceso.CustomMessage);
                        filewriter.WriteLine("System Error Message: " + evento.InformacionSuceso.SystemMessage);
                        filewriter.WriteLine("File Name: " + evento.InformacionSuceso.FileName);
                        filewriter.WriteLine("File Name: " + evento.InformacionSuceso.OpcionalField);
                        filewriter.WriteLine("Opcional: " + evento.InformacionSuceso.OpcionalField2);
                        if (evento.InformacionSuceso.SystemError != null)
                        {
                            filewriter.WriteLine("Type of Error: " + evento.InformacionSuceso.SystemError.GetType().ToString());
                        }
                        filewriter.WriteLine("======================================");
                    }
                    break;
                default:
                    break;
            }
        }

        private static void WritteAllertsandErrors(StreamWriter filewriter, SucesoEventBuilder evento)
        {
            switch (evento.InformacionSuceso.TypeEvent)
            {
                case EventType.Information:
                    {

                    }
                    break;
                case EventType.Alert:
                    {
                        filewriter.WriteLine("======================================");
                        filewriter.WriteLine("/A\\ Alerta");
                        filewriter.WriteLine("Date: " + evento.InformacionSuceso.Date.ToString());
                        filewriter.WriteLine("Application: " + evento.InformacionSuceso.Application);
                        filewriter.WriteLine("Event: " + evento.InformacionSuceso.Event);
                        filewriter.WriteLine("Method: " + evento.InformacionSuceso.Method);
                        filewriter.WriteLine("Custom Message: " + evento.InformacionSuceso.CustomMessage);
                        filewriter.WriteLine("File Name: " + evento.InformacionSuceso.FileName);
                        filewriter.WriteLine("Status : " + evento.InformacionSuceso.Status);
                        filewriter.WriteLine("File Name2: " + evento.InformacionSuceso.OpcionalField);
                        filewriter.WriteLine("Opcional: " + evento.InformacionSuceso.OpcionalField2);
                        if (evento.InformacionSuceso.SystemError != null)
                        {
                            filewriter.WriteLine("Type of Errorr: " + evento.InformacionSuceso.SystemError.GetType().ToString());
                        }
                        filewriter.WriteLine("======================================");

                    }
                    break;
                case EventType.Error:
                    {
                        filewriter.WriteLine("======================================");
                        filewriter.WriteLine("[*] Error");
                        filewriter.WriteLine("Date: " + evento.InformacionSuceso.Date.ToString());
                        filewriter.WriteLine("Application: " + evento.InformacionSuceso.Application);
                        filewriter.WriteLine("Event: " + evento.InformacionSuceso.Event);
                        filewriter.WriteLine("Method: " + evento.InformacionSuceso.Method);
                        filewriter.WriteLine("Status: " + evento.InformacionSuceso.Status);
                        filewriter.WriteLine("Custom Message: " + evento.InformacionSuceso.CustomMessage);
                        filewriter.WriteLine("System Error Message: " + evento.InformacionSuceso.SystemMessage);
                        filewriter.WriteLine("File Name: " + evento.InformacionSuceso.FileName);
                        filewriter.WriteLine("File Name: " + evento.InformacionSuceso.OpcionalField);
                        filewriter.WriteLine("Opcional: " + evento.InformacionSuceso.OpcionalField2);
                        if (evento.InformacionSuceso.SystemError != null)
                        {
                            filewriter.WriteLine("Type of Error: " + evento.InformacionSuceso.SystemError.GetType().ToString());
                        }
                        filewriter.WriteLine("======================================");
                    }
                    break;
                default:
                    break;
            }
        }

        private static void WrriteAll(StreamWriter filewriter, SucesoEventBuilder evento)
        {
            filewriter.WriteLine("======================================");
            switch (evento.InformacionSuceso.TypeEvent)
            {
                case EventType.Information:
                    filewriter.WriteLine("(i) Information");
                    break;
                case EventType.Alert:
                    filewriter.WriteLine("/A\\ Alert");
                    break;
                case EventType.Error:
                    filewriter.WriteLine("[*] Error");
                    break;
                default:
                    break;
            }
            filewriter.WriteLine("======================================");
            filewriter.WriteLine("Date: " + evento.InformacionSuceso.Date.ToString());
            filewriter.WriteLine("Application: " + evento.InformacionSuceso.Application);
            filewriter.WriteLine("Event: " + evento.InformacionSuceso.Event);
            filewriter.WriteLine("Method: " + evento.InformacionSuceso.Method);
            filewriter.WriteLine("Status: " + evento.InformacionSuceso.Status);
            filewriter.WriteLine("Custom Message: " + evento.InformacionSuceso.CustomMessage);
            filewriter.WriteLine("System Error Message: " + evento.InformacionSuceso.SystemMessage);
            filewriter.WriteLine("File Name: " + evento.InformacionSuceso.FileName);
            filewriter.WriteLine("File Name: " + evento.InformacionSuceso.OpcionalField);
            filewriter.WriteLine("Opcional: " + evento.InformacionSuceso.OpcionalField2);
            if (evento.InformacionSuceso.SystemError != null)
            {
                filewriter.WriteLine("Type of Error: " + evento.InformacionSuceso.SystemError.GetType().ToString());
            }
            filewriter.WriteLine("======================================");
        }
        
        //region Interfases
        private static void WrittetoText(SucesoInterfaseBuilder InterfasEvento)
        {
            string filename = GetFileNameInterfase();
            StreamWriter filewritter;

            string path = GetPathNameInterfase();

            if (!Directory.Exists(filename))
            {
                Directory.CreateDirectory(path);
            }

            if (!File.Exists(filename))
            {
                filewritter = File.CreateText(filename);
            }
            else
            {
                //FileStream files = new FileStream(filename, FileMode.Append, FileAccess.Write);
                using (FileStream files = new FileStream(filename, FileMode.Append, FileAccess.Write))
                {
                    filewritter = new StreamWriter(files);
                }
            }
            switch (GetConfigurationInterfase())
            {
                case 0:
                    WrriteAll(filewritter, InterfasEvento);
                    break;
                case 1:
                    WritteAllertsandErrors(filewritter, InterfasEvento);
                    break;
                default:
                    WritteErrors(filewritter, InterfasEvento);
                    break;
            }
            filewritter.Close();
        }

        private static void WritteErrors(StreamWriter filewriter, SucesoInterfaseBuilder InterfasEvento)
        {
            switch (InterfasEvento.InformacionSuceso.TypeEvent)
            {
                case EventType.Information:
                    {
                    }
                    break;
                case EventType.Alert:
                    {
                    }
                    break;
                case EventType.Error:
                    {
                        filewriter.WriteLine("======================================");
                        filewriter.WriteLine("[*] Error");
                        filewriter.WriteLine("Date: " + InterfasEvento.InformacionSuceso.Date.ToString());
                        filewriter.WriteLine("Interfas: " + InterfasEvento.InformacionSuceso.NombreInterfase);
                        filewriter.WriteLine("IdProceso: " + InterfasEvento.InformacionSuceso.IdInterfase);
                        filewriter.WriteLine("Event: " + InterfasEvento.InformacionSuceso.Event);
                        filewriter.WriteLine("Method: " + InterfasEvento.InformacionSuceso.Method);
                        filewriter.WriteLine("Resultado: " + InterfasEvento.InformacionSuceso.Resultado);
                        filewriter.WriteLine("Subject: " + InterfasEvento.InformacionSuceso.Subject);
                        filewriter.WriteLine("System Error Message: " + InterfasEvento.InformacionSuceso.SystemMessage);
                        filewriter.WriteLine("Opcional: " + InterfasEvento.InformacionSuceso.OpcionalField);
                        filewriter.WriteLine("Archivo Datos: " + InterfasEvento.InformacionSuceso.ADatos);
                        filewriter.WriteLine("Archivo Control: " + InterfasEvento.InformacionSuceso.AControl);
                        if (InterfasEvento.InformacionSuceso.SystemError != null)
                        {
                            filewriter.WriteLine("Type of Error: " + InterfasEvento.InformacionSuceso.SystemError.GetType().ToString());
                        }
                        filewriter.WriteLine("======================================");
                    }
                    break;
                default:
                    break;
            }
        }

        private static void WritteAllertsandErrors(StreamWriter filewriter, SucesoInterfaseBuilder InterfasEvento)
        {
            switch (InterfasEvento.InformacionSuceso.TypeEvent)
            {
                case EventType.Information:
                    {

                    }
                    break;
                case EventType.Alert:
                    {
                        filewriter.WriteLine("======================================");
                        filewriter.WriteLine("/A\\ Alerta");
                        filewriter.WriteLine("Date: " + InterfasEvento.InformacionSuceso.Date.ToString());
                        filewriter.WriteLine("Interfas: " + InterfasEvento.InformacionSuceso.NombreInterfase);
                        filewriter.WriteLine("IdProceso: " + InterfasEvento.InformacionSuceso.IdInterfase);
                        filewriter.WriteLine("Event: " + InterfasEvento.InformacionSuceso.Event);
                        filewriter.WriteLine("Method: " + InterfasEvento.InformacionSuceso.Method);
                        filewriter.WriteLine("Resultado: " + InterfasEvento.InformacionSuceso.Resultado);
                        filewriter.WriteLine("Subject: " + InterfasEvento.InformacionSuceso.Subject);
                        filewriter.WriteLine("System Error Message: " + InterfasEvento.InformacionSuceso.SystemMessage);
                        filewriter.WriteLine("Opcional: " + InterfasEvento.InformacionSuceso.OpcionalField);
                        filewriter.WriteLine("Archivo Datos: " + InterfasEvento.InformacionSuceso.ADatos);
                        filewriter.WriteLine("Archivo Control: " + InterfasEvento.InformacionSuceso.AControl);
                        if (InterfasEvento.InformacionSuceso.SystemError != null)
                        {
                            filewriter.WriteLine("Type of Errorr: " + InterfasEvento.InformacionSuceso.SystemError.GetType().ToString());
                        }
                        filewriter.WriteLine("======================================");

                    }
                    break;
                case EventType.Error:
                    {
                        filewriter.WriteLine("======================================");
                        filewriter.WriteLine("[*] Error");
                        filewriter.WriteLine("Date: " + InterfasEvento.InformacionSuceso.Date.ToString());
                        filewriter.WriteLine("Interfas: " + InterfasEvento.InformacionSuceso.NombreInterfase);
                        filewriter.WriteLine("IdProceso: " + InterfasEvento.InformacionSuceso.IdInterfase);
                        filewriter.WriteLine("Event: " + InterfasEvento.InformacionSuceso.Event);
                        filewriter.WriteLine("Method: " + InterfasEvento.InformacionSuceso.Method);
                        filewriter.WriteLine("Resultado: " + InterfasEvento.InformacionSuceso.Resultado);
                        filewriter.WriteLine("Subject: " + InterfasEvento.InformacionSuceso.Subject);
                        filewriter.WriteLine("System Error Message: " + InterfasEvento.InformacionSuceso.SystemMessage);
                        filewriter.WriteLine("Opcional: " + InterfasEvento.InformacionSuceso.OpcionalField);
                        filewriter.WriteLine("Archivo Datos: " + InterfasEvento.InformacionSuceso.ADatos);
                        filewriter.WriteLine("Archivo Control: " + InterfasEvento.InformacionSuceso.AControl);
                        if (InterfasEvento.InformacionSuceso.SystemError != null)
                        {
                            filewriter.WriteLine("Type of Error: " + InterfasEvento.InformacionSuceso.SystemError.GetType().ToString());
                        }
                        filewriter.WriteLine("======================================");
                    }
                    break;
                default:
                    break;
            }
        }

        private static void WrriteAll(StreamWriter filewriter, SucesoInterfaseBuilder InterfasEvento)
        {
            filewriter.WriteLine("======================================");
            switch (InterfasEvento.InformacionSuceso.TypeEvent)
            {
                case EventType.Information:
                    filewriter.WriteLine("(i) Information");
                    break;
                case EventType.Alert:
                    filewriter.WriteLine("/A\\ Alert");
                    break;
                case EventType.Error:
                    filewriter.WriteLine("[*] Error");
                    break;
                default:
                    break;
            }
            filewriter.WriteLine("======================================");
            filewriter.WriteLine("Date: " + InterfasEvento.InformacionSuceso.Date.ToString());
            filewriter.WriteLine("Interfas: " + InterfasEvento.InformacionSuceso.NombreInterfase);
            filewriter.WriteLine("IdProceso: " + InterfasEvento.InformacionSuceso.IdInterfase);
            filewriter.WriteLine("Event: " + InterfasEvento.InformacionSuceso.Event);
            filewriter.WriteLine("Method: " + InterfasEvento.InformacionSuceso.Method);
            filewriter.WriteLine("Resultado: " + InterfasEvento.InformacionSuceso.Resultado);
            filewriter.WriteLine("Subject: " + InterfasEvento.InformacionSuceso.Subject);
            filewriter.WriteLine("System Error Message: " + InterfasEvento.InformacionSuceso.SystemMessage);
            filewriter.WriteLine("Opcional: " + InterfasEvento.InformacionSuceso.OpcionalField);
            filewriter.WriteLine("Archivo Datos: " + InterfasEvento.InformacionSuceso.ADatos);
            filewriter.WriteLine("Archivo Control: " + InterfasEvento.InformacionSuceso.AControl);
            if (InterfasEvento.InformacionSuceso.SystemError != null)
            {
                filewriter.WriteLine("Type of Error: " + InterfasEvento.InformacionSuceso.SystemError.GetType().ToString());
            }
            filewriter.WriteLine("======================================");
        }


        #endregion

        #region grabaBD
       /// <summary>
       /// Guarda valores en la BD, crea la tabla y el SP.
       /// Toma lso datos de la conexion desde el app.Config
       /// </summary>
       /// <returns>regresa si guardo o no guardo los datos.</returns>
        public static bool GrabaBD()
        {
            bool pass = false;
           int confi = Convert.ToInt32(ConfigurationManager.AppSettings["LogDestino"].ToString());

            if (CreaTablaySP()==true)
            {
                pass = true;
                    switch (confi)
                    {
                            //texto
                        case 0:
                            break;
                            //eventViewer
                        case 1:
                            break;                          
                        case 2:
                            ListaInsertaToDB();
                            break;
                            //xml
                        case 3:
                            xmlInsertaToDB(ConfigurationManager.AppSettings["LogFileXMLInter"].ToString());
                            MantanimientoTablaandXML();
                            break;
                            //xml y lista de objetos                          
                        case 4:
                            xmlInsertaToDB(ConfigurationManager.AppSettings["LogFileXMLInter"].ToString());
                            MantanimientoTablaandXML();
                            break;
                        default:
                            break;
                    }
                }
            return pass;
        }
        /// <summary>
        /// Guarda valores en la BD, crea la tabla y el SP.
        /// Toma Recibe los datos de Conexion.
        /// </summary>
        /// <returns>regresa si guardo o no guardo los datos.</returns>
        public static bool GrabaBD(string ConecctionStringProvided)
        {
            bool pass = false;
            ConGenral = ConecctionStringProvided;

            int confi = Convert.ToInt32(ConfigurationManager.AppSettings["LogDestino"].ToString());

            if (CreaTablaySP() == true)
            {
                pass = true;
                switch (confi)
                {
                    //texto
                    case 0:
                        break;
                    //eventViewer
                    case 1:
                        break;
                    case 2:
                        ListaInsertaToDB();
                        break;
                    //xml
                    case 3:
                        xmlInsertaToDB(ConfigurationManager.AppSettings["LogFileXMLInter"].ToString());
                        MantanimientoTablaandXML();
                        break;
                    //xml y lista de objetos                          
                    case 4:
                        xmlInsertaToDB(ConfigurationManager.AppSettings["LogFileXMLInter"].ToString());
                        MantanimientoTablaandXML();
                        break;
                    default:
                        break;
                }
            }
            return pass;
        }

        private static SqlConnection RegresaConnexionstring()         
        {
            if (string.IsNullOrEmpty(ConGenral))
            {
                return DataContextFactory.GetSqlConnectionFactory();
            }
            else 
            {
                return DataContextFactory.SetSqlConnectionFactory(ConGenral);
            }
        
        }
        private static bool CreaTablaySP()
        {
            bool pass = false;
            int ErrorSP = 0;
            int ErrorT = 0;
            int ErrorSelectTabla = 0;
            int errorExisteSP = 0;
           #region tabla
            string Tabla = "SET ANSI_NULLS ON "
                            + "\n"
                //  + " GO"
                             + "\n"
                            + " SET QUOTED_IDENTIFIER ON "
                             + "\n"
                //   + " GO "
                             + "\n"
                            + " SET ANSI_PADDING ON "
                             + "\n"
                //  + " GO "
                             + "\n"
                            + " CREATE TABLE [dbo].[utbBitacoralog]( "
                            + "	[IdBitacora] [int] IDENTITY(1,1) NOT NULL, "
                            + "	[NombreProceso] [nvarchar](max) NULL, "
                            + "	[IdProceso] [int] NULL, "
                            + "	[Evento] [nvarchar](max) NULL, "
                            + "	[Metodo] [nvarchar](max) NULL, "
                            + "	[FechaEjecucion] [datetime] NULL, "
                            + "	[Resultado] [nvarchar](max) NULL, "
                            + "	[Subject] [nvarchar](max) NULL, "
                            + "	[ErrorSistema] [nvarchar](max) NULL, "
                            + "	[MensajeSistema] [nvarchar](max) NULL, "
                            + "	[NombreArchivoDatos] [nvarchar](max) NULL, "
                            + "	[NombreArchivoControl] [nvarchar](max) NULL, "
                            + "	[Opcional] [nvarchar](max) NULL, "
                              + "\n"
                            + " PRIMARY KEY CLUSTERED "
                             + "\n"
                            + " ( "
                             + "\n"
                            + "	[IdBitacora] ASC "
                             + "\n"
                            + " )"
                             + "\n"
                            + " ) ON [PRIMARY]"
                             + "\n"
                //  + " GO "
                             + "\n"
                            + " SET ANSI_PADDING OFF "
                             + "\n";
            //   + " GO ";
#endregion
            string ExisteTabla = "select top 1 * from utbBitacoralog";
   

            using (SqlDataAdapter adp = new SqlDataAdapter(ExisteTabla, RegresaConnexionstring()))
            {          
                try
                {
                  
                   DataSet ds = new DataSet();
                   adp.Fill(ds);
                   ErrorSelectTabla = ds.Tables[0].Rows.Count;
                   pass = true;
                }
                catch (SqlException)
                {
                    ErrorSelectTabla = -1;
                    pass = false;
                }
            }
            //no existe creala
            if (ErrorSelectTabla == -1)
            {
                using (SqlCommand cmd = new SqlCommand(Tabla, RegresaConnexionstring()))
                {
                    try
                    {
                        cmd.Connection.Open();
                        ErrorT = cmd.ExecuteNonQuery();
                        pass = true;
                    }
                    catch (SqlException)
                    {
                        ErrorSP = -1;
                        pass = false;
                    }
                }
            }

            //ejecuto SP solo si existe tabla
            if (pass == true && ErrorT==0 && ErrorSelectTabla>=0)
            {
                #region Crea Procedure
                string ExisteSP = " create PROCEDURE [dbo].[uspInsertaBitacora]"
                                   + "\n"
                                   + " @doc text, "
                                    + "\n"
                                    +" @opcion int"
                                    + "\n"
                                    + " AS "
                                    + "\n"
                                    + " BEGIN "
                                    + "\n"
                                    + " BEGIN TRANSACTION 	 "
                                     + "\n"
                                    + " 	DECLARE @idoc INT	 "
                                    + "\n"
                                    + " declare @Subelemento nvarchar(250)"
                                    + "\n"
                                    + " 	DECLARE @importId uniqueidentifier "
                                    + "\n"
                                    + "  SET @importId = newid() "
                                    + "\n"
                                    + " EXEC sp_xml_preparedocument @idoc OUTPUT, @doc "
                                         + "\n"

                                         + "if @opcion='0'"
                                          + "\n"
                                         + "Begin"
                                                  + "\n"
                                                
                                                  + "\n"
                                                 + "set @Subelemento='Log/Information/Interfase'"
                                                 + "\n"
                                                + " insert into [dbo].[utbBitacoralog] "
                                                   + " ( "
                                                  + " [NombreProceso] "
                                               + "    ,[IdProceso] "
                                                 + "  ,[Evento] "
                                                + "   ,[Metodo] "
                                                 + "  ,[FechaEjecucion] "
                                                  + " ,[Resultado] "
                                                  + " ,[Subject] "
                                                  + " ,[ErrorSistema] "
                                                  + " ,[MensajeSistema] "
                                                  + " ,[NombreArchivoDatos] "
                                                  + " ,[NombreArchivoControl] "
                                                  + " ,[Opcional]) " 
                                    						  
		                                            +" 			Select						 " 
		                                            +" 		  Xd.NombreProceso " 
		                                            +" 		  ,Xd.IdProceso " 
		                                            +" 		  ,Xd.Evento " 
		                                            +" 		  ,Xd.Metodo " 
		                                            +" 		  ,Xd.FechaEjecucion " 
		                                            +" 		  ,Xd.Resultado " 
		                                            +" 		  ,Xd.Sub " 
		                                            +" 		  ,Xd.ErrorSistema " 
		                                            +" 		  ,Xd.MensajeSistema  " 
		                                            +" 		  ,Xd.NombreArchivoDatos " 
		                                            +" 		  ,Xd.NombreArchivoControl " 
		                                            +" 		  ,Xd.Opcional	   "
                                                    + " 			From OPENXML (@idoc, @Subelemento, 1) " 
		                                            +" 			WITH ( " 
		                                            +" 			NombreProceso      nvarchar(max) '@Aplication', " 
		                                            +" 			IdProceso          int          '@IdProceso', " 
		                                            +" 			Evento             nvarchar(max) '@EventName', " 
		                                            +" 			Metodo             nvarchar(max) '@MethodName', " 
		                                            +" 			FechaEjecucion     datetime     '@Date', " 
		                                            +" 			Resultado          nvarchar(max) '@Status', " 
		                                            +" 			Sub                nvarchar(max) '@CustomMessage', " 
		                                            +" 			ErrorSistema       nvarchar(max) '@SystemError', " 
		                                            +" 			MensajeSistema     nvarchar(max) '@SystemMessage', " 
		                                            +" 			NombreArchivoDatos nvarchar(max) '@ADatos', " 
		                                            +" 			NombreArchivoControl  nvarchar(max) '@AControl', " 
		                                            +" 			Opcional              nvarchar(max) '@Opcional', " 								
		                                            +" 			importXdrentId int '@mp:id' " 
		                                            +" 			) as Xd "
                                                                 + "\n"
                                                                 + " where not exists(select idBitacora from utbBitacoralog "
                                                                        + " where  Xd.NombreProceso=NombreProceso and "
                                                                         + " Xd.Metodo=Metodo and"
                                                                          + " Xd.FechaEjecucion=FechaEjecucion and "
                                                                          + " Xd.Resultado=Resultado and Xd.IdProceso=IdProceso)"
                                                                          + "\n"

                                                   + "\n"
                                                 + "set @Subelemento='Log/Alerts/Interfase'"
                                                 + "\n"
                                                + " insert into [dbo].[utbBitacoralog] "
                                                   + " ( "
                                                  + " [NombreProceso] "
                                               + "    ,[IdProceso] "
                                                 + "  ,[Evento] "
                                                + "   ,[Metodo] "
                                                 + "  ,[FechaEjecucion] "
                                                  + " ,[Resultado] "
                                                  + " ,[Subject] "
                                                  + " ,[ErrorSistema] "
                                                  + " ,[MensajeSistema] "
                                                  + " ,[NombreArchivoDatos] "
                                                  + " ,[NombreArchivoControl] "
                                                  + " ,[Opcional]) "

                                                    + " 			Select						 "
                                                    + " 		  Xd.NombreProceso "
                                                    + " 		  ,Xd.IdProceso "
                                                    + " 		  ,Xd.Evento "
                                                    + " 		  ,Xd.Metodo "
                                                    + " 		  ,Xd.FechaEjecucion "
                                                    + " 		  ,Xd.Resultado "
                                                    + " 		  ,Xd.Sub "
                                                    + " 		  ,Xd.ErrorSistema "
                                                    + " 		  ,Xd.MensajeSistema  "
                                                    + " 		  ,Xd.NombreArchivoDatos "
                                                    + " 		  ,Xd.NombreArchivoControl "
                                                    + " 		  ,Xd.Opcional	   "
                                                    + " 			From OPENXML (@idoc, @Subelemento, 1) "
                                                    + " 			WITH ( "
                                                    + " 			NombreProceso      nvarchar(max) '@Aplication', "
                                                    + " 			IdProceso          int          '@IdProceso', "
                                                    + " 			Evento             nvarchar(max) '@EventName', "
                                                    + " 			Metodo             nvarchar(max) '@MethodName', "
                                                    + " 			FechaEjecucion     datetime     '@Date', "
                                                    + " 			Resultado          nvarchar(max) '@Status', "
                                                    + " 			Sub                nvarchar(max) '@CustomMessage', "
                                                    + " 			ErrorSistema       nvarchar(max) '@SystemError', "
                                                    + " 			MensajeSistema     nvarchar(max) '@SystemMessage', "
                                                    + " 			NombreArchivoDatos nvarchar(max) '@ADatos', "
                                                    + " 			NombreArchivoControl  nvarchar(max) '@AControl', "
                                                    + " 			Opcional              nvarchar(max) '@Opcional', "
                                                    + " 			importXdrentId int '@mp:id' "
                                                    + " 			) as Xd "
                                                                 + "\n"
                                                                 + " where not exists(select idBitacora from utbBitacoralog "
                                                                        + " where  Xd.NombreProceso=NombreProceso and "
                                                                         + " Xd.Metodo=Metodo and"
                                                                          + " Xd.FechaEjecucion=FechaEjecucion and "
                                                                          + " Xd.Resultado=Resultado and Xd.IdProceso=IdProceso)"
                                                                          + "\n"

                                              + "\n"
                                                 + "set @Subelemento='Log/Errors/Interfase'"
                                                 + "\n"
                                                + " insert into [dbo].[utbBitacoralog] "
                                                   + " ( "
                                                  + " [NombreProceso] "
                                               + "    ,[IdProceso] "
                                                 + "  ,[Evento] "
                                                + "   ,[Metodo] "
                                                 + "  ,[FechaEjecucion] "
                                                  + " ,[Resultado] "
                                                  + " ,[Subject] "
                                                  + " ,[ErrorSistema] "
                                                  + " ,[MensajeSistema] "
                                                  + " ,[NombreArchivoDatos] "
                                                  + " ,[NombreArchivoControl] "
                                                  + " ,[Opcional]) "

                                                    + " 			Select						 "
                                                    + " 		  Xd.NombreProceso "
                                                    + " 		  ,Xd.IdProceso "
                                                    + " 		  ,Xd.Evento "
                                                    + " 		  ,Xd.Metodo "
                                                    + " 		  ,Xd.FechaEjecucion "
                                                    + " 		  ,Xd.Resultado "
                                                    + " 		  ,Xd.Sub "
                                                    + " 		  ,Xd.ErrorSistema "
                                                    + " 		  ,Xd.MensajeSistema  "
                                                    + " 		  ,Xd.NombreArchivoDatos "
                                                    + " 		  ,Xd.NombreArchivoControl "
                                                    + " 		  ,Xd.Opcional	   "
                                                    + " 			From OPENXML (@idoc, @Subelemento, 1) "
                                                    + " 			WITH ( "
                                                    + " 			NombreProceso      nvarchar(max) '@Aplication', "
                                                    + " 			IdProceso          int          '@IdProceso', "
                                                    + " 			Evento             nvarchar(max) '@EventName', "
                                                    + " 			Metodo             nvarchar(max) '@MethodName', "
                                                    + " 			FechaEjecucion     datetime     '@Date', "
                                                    + " 			Resultado          nvarchar(max) '@Status', "
                                                    + " 			Sub                nvarchar(max) '@CustomMessage', "
                                                    + " 			ErrorSistema       nvarchar(max) '@SystemError', "
                                                    + " 			MensajeSistema     nvarchar(max) '@SystemMessage', "
                                                    + " 			NombreArchivoDatos nvarchar(max) '@ADatos', "
                                                    + " 			NombreArchivoControl  nvarchar(max) '@AControl', "
                                                    + " 			Opcional              nvarchar(max) '@Opcional', "
                                                    + " 			importXdrentId int '@mp:id' "
                                                    + " 			) as Xd "
                                                                 + "\n"
                                                                 + " where not exists(select idBitacora from utbBitacoralog "
                                                                        + " where  Xd.NombreProceso=NombreProceso and "
                                                                         + " Xd.Metodo=Metodo and"
                                                                          + " Xd.FechaEjecucion=FechaEjecucion and "
                                                                          + " Xd.Resultado=Resultado and Xd.IdProceso=IdProceso)"
                                                                          + "\n"
                                        + "end"
                                                 + "\n"

                                         + "if @opcion='1'"
                                          + "\n"
                                         + "Begin"
                                                 + "\n"
                                                 + "set @Subelemento='Log/Alerts/Interfase'"
                                                 + "\n"
                                                + " insert into [dbo].[utbBitacoralog] "
                                                   + " ( "
                                                  + " [NombreProceso] "
                                               + "    ,[IdProceso] "
                                                 + "  ,[Evento] "
                                                + "   ,[Metodo] "
                                                 + "  ,[FechaEjecucion] "
                                                  + " ,[Resultado] "
                                                  + " ,[Subject] "
                                                  + " ,[ErrorSistema] "
                                                  + " ,[MensajeSistema] "
                                                  + " ,[NombreArchivoDatos] "
                                                  + " ,[NombreArchivoControl] "
                                                  + " ,[Opcional]) "

                                                    + " 			Select						 "
                                                    + " 		  Xd.NombreProceso "
                                                    + " 		  ,Xd.IdProceso "
                                                    + " 		  ,Xd.Evento "
                                                    + " 		  ,Xd.Metodo "
                                                    + " 		  ,Xd.FechaEjecucion "
                                                    + " 		  ,Xd.Resultado "
                                                    + " 		  ,Xd.Sub "
                                                    + " 		  ,Xd.ErrorSistema "
                                                    + " 		  ,Xd.MensajeSistema  "
                                                    + " 		  ,Xd.NombreArchivoDatos "
                                                    + " 		  ,Xd.NombreArchivoControl "
                                                    + " 		  ,Xd.Opcional	   "
                                                    + " 			From OPENXML (@idoc, @Subelemento, 1) "
                                                    + " 			WITH ( "
                                                    + " 			NombreProceso      nvarchar(max) '@Aplication', "
                                                    + " 			IdProceso          int          '@IdProceso', "
                                                    + " 			Evento             nvarchar(max) '@EventName', "
                                                    + " 			Metodo             nvarchar(max) '@MethodName', "
                                                    + " 			FechaEjecucion     datetime     '@Date', "
                                                    + " 			Resultado          nvarchar(max) '@Status', "
                                                    + " 			Sub                nvarchar(max) '@CustomMessage', "
                                                    + " 			ErrorSistema       nvarchar(max) '@SystemError', "
                                                    + " 			MensajeSistema     nvarchar(max) '@SystemMessage', "
                                                    + " 			NombreArchivoDatos nvarchar(max) '@ADatos', "
                                                    + " 			NombreArchivoControl  nvarchar(max) '@AControl', "
                                                    + " 			Opcional              nvarchar(max) '@Opcional', "
                                                    + " 			importXdrentId int '@mp:id' "
                                                    + " 			) as Xd "
                                                                 + "\n"
                                                                 + " where not exists(select idBitacora from utbBitacoralog "
                                                                        + " where  Xd.NombreProceso=NombreProceso and "
                                                                         + " Xd.Metodo=Metodo and"
                                                                          + " Xd.FechaEjecucion=FechaEjecucion and "
                                                                          + " Xd.Resultado=Resultado and Xd.IdProceso=IdProceso)"
                                                                          + "\n"
                                              + "\n"
                                                 + "set @Subelemento='Log/Errors/Interfase'"
                                                 + "\n"
                                                + " insert into [dbo].[utbBitacoralog] "
                                                   + " ( "
                                                  + " [NombreProceso] "
                                               + "    ,[IdProceso] "
                                                 + "  ,[Evento] "
                                                + "   ,[Metodo] "
                                                 + "  ,[FechaEjecucion] "
                                                  + " ,[Resultado] "
                                                  + " ,[Subject] "
                                                  + " ,[ErrorSistema] "
                                                  + " ,[MensajeSistema] "
                                                  + " ,[NombreArchivoDatos] "
                                                  + " ,[NombreArchivoControl] "
                                                  + " ,[Opcional]) "

                                                    + " 			Select						 "
                                                    + " 		  Xd.NombreProceso "
                                                    + " 		  ,Xd.IdProceso "
                                                    + " 		  ,Xd.Evento "
                                                    + " 		  ,Xd.Metodo "
                                                    + " 		  ,Xd.FechaEjecucion "
                                                    + " 		  ,Xd.Resultado "
                                                    + " 		  ,Xd.Sub "
                                                    + " 		  ,Xd.ErrorSistema "
                                                    + " 		  ,Xd.MensajeSistema  "
                                                    + " 		  ,Xd.NombreArchivoDatos "
                                                    + " 		  ,Xd.NombreArchivoControl "
                                                    + " 		  ,Xd.Opcional	   "
                                                    + " 			From OPENXML (@idoc, @Subelemento, 1) "
                                                    + " 			WITH ( "
                                                    + " 			NombreProceso      nvarchar(max) '@Aplication', "
                                                    + " 			IdProceso          int          '@IdProceso', "
                                                    + " 			Evento             nvarchar(max) '@EventName', "
                                                    + " 			Metodo             nvarchar(max) '@MethodName', "
                                                    + " 			FechaEjecucion     datetime     '@Date', "
                                                    + " 			Resultado          nvarchar(max) '@Status', "
                                                    + " 			Sub                nvarchar(max) '@CustomMessage', "
                                                    + " 			ErrorSistema       nvarchar(max) '@SystemError', "
                                                    + " 			MensajeSistema     nvarchar(max) '@SystemMessage', "
                                                    + " 			NombreArchivoDatos nvarchar(max) '@ADatos', "
                                                    + " 			NombreArchivoControl  nvarchar(max) '@AControl', "
                                                    + " 			Opcional              nvarchar(max) '@Opcional', "
                                                    + " 			importXdrentId int '@mp:id' "
                                                    + " 			) as Xd "
                                                                 + "\n"
                                                                 + " where not exists(select idBitacora from utbBitacoralog "
                                                                        + " where  Xd.NombreProceso=NombreProceso and "
                                                                         + " Xd.Metodo=Metodo and"
                                                                          + " Xd.FechaEjecucion=FechaEjecucion and "
                                                                          + " Xd.Resultado=Resultado and Xd.IdProceso=IdProceso)"
                                                                          + "\n"


                                        + "end"
                                         + "\n"


                                             +" IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -100 END			    "
                                              + "\n"
                                            + " EXEC sp_xml_removedocument @idoc "
                                           + "\n"
                                        +" COMMIT TRANSACTION "
                                                 + "\n"	  
                                        +" return @@RowCount	 "
                                         + "\n"
                                        +" END ";
                #endregion
                //Creo SP
                using (SqlCommand cmd = new SqlCommand(ExisteSP, RegresaConnexionstring()))
                {
                    try
                    {
                      
                        cmd.Connection.Open();
                        errorExisteSP = cmd.ExecuteNonQuery();
                        pass = true;
                    }
                    catch (SqlException)
                    {
                        errorExisteSP = -1;
                        pass = false;
                    }
                }
             
                //si existe modificalo
                if (errorExisteSP == -1 && ErrorT == 0 && ErrorSelectTabla >= 0)
                {
                    #region MODIFICA sp
                    string ModificaSP = " Alter PROCEDURE [dbo].[uspInsertaBitacora]"
                                   + "\n"
                                   + " @doc text, "
                                    + "\n"
                                    + " @opcion int"
                                    + "\n"
                                    + " AS "
                                    + "\n"
                                    + " BEGIN "
                                    + "\n"
                                    + " BEGIN TRANSACTION 	 "
                                     + "\n"
                                    + " 	DECLARE @idoc INT	 "
                                    + "\n"
                                    + " declare @Subelemento nvarchar(250)"
                                    + "\n"
                                    + " 	DECLARE @importId uniqueidentifier "
                                    + "\n"
                                    + "  SET @importId = newid() "
                                    + "\n"
                                    + " EXEC sp_xml_preparedocument @idoc OUTPUT, @doc "
                                         + "\n"

                                         + "if @opcion='0'"
                                          + "\n"
                                         + "Begin"
                                                  + "\n"

                                                  + "\n"
                                                 + "set @Subelemento='Log/Information/Interfase'"
                                                 + "\n"
                                                + " insert into [dbo].[utbBitacoralog] "
                                                   + " ( "
                                                  + " [NombreProceso] "
                                               + "    ,[IdProceso] "
                                                 + "  ,[Evento] "
                                                + "   ,[Metodo] "
                                                 + "  ,[FechaEjecucion] "
                                                  + " ,[Resultado] "
                                                  + " ,[Subject] "
                                                  + " ,[ErrorSistema] "
                                                  + " ,[MensajeSistema] "
                                                  + " ,[NombreArchivoDatos] "
                                                  + " ,[NombreArchivoControl] "
                                                  + " ,[Opcional]) "

                                                    + " 			Select						 "
                                                    + " 		  Xd.NombreProceso "
                                                    + " 		  ,Xd.IdProceso "
                                                    + " 		  ,Xd.Evento "
                                                    + " 		  ,Xd.Metodo "
                                                    + " 		  ,Xd.FechaEjecucion "
                                                    + " 		  ,Xd.Resultado "
                                                    + " 		  ,Xd.Sub "
                                                    + " 		  ,Xd.ErrorSistema "
                                                    + " 		  ,Xd.MensajeSistema  "
                                                    + " 		  ,Xd.NombreArchivoDatos "
                                                    + " 		  ,Xd.NombreArchivoControl "
                                                    + " 		  ,Xd.Opcional	   "
                                                    + " 			From OPENXML (@idoc, @Subelemento, 1) "
                                                    + " 			WITH ( "
                                                    + " 			NombreProceso      nvarchar(max) '@Aplication', "
                                                    + " 			IdProceso          int          '@IdProceso', "
                                                    + " 			Evento             nvarchar(max) '@EventName', "
                                                    + " 			Metodo             nvarchar(max) '@MethodName', "
                                                    + " 			FechaEjecucion     datetime     '@Date', "
                                                    + " 			Resultado          nvarchar(max) '@Status', "
                                                    + " 			Sub                nvarchar(max) '@CustomMessage', "
                                                    + " 			ErrorSistema       nvarchar(max) '@SystemError', "
                                                    + " 			MensajeSistema     nvarchar(max) '@SystemMessage', "
                                                    + " 			NombreArchivoDatos nvarchar(max) '@ADatos', "
                                                    + " 			NombreArchivoControl  nvarchar(max) '@AControl', "
                                                    + " 			Opcional              nvarchar(max) '@Opcional', "
                                                    + " 			importXdrentId int '@mp:id' "
                                                    + " 			) as Xd "
                                                                 + "\n"
                                                                 + " where not exists(select idBitacora from utbBitacoralog "
                                                                        + " where  Xd.NombreProceso=NombreProceso and "
                                                                         + " Xd.Metodo=Metodo and"
                                                                          + " Xd.FechaEjecucion=FechaEjecucion and "
                                                                          + " Xd.Resultado=Resultado and Xd.IdProceso=IdProceso)"
                                                                          + "\n"

                                                   + "\n"
                                                 + "set @Subelemento='Log/Alerts/Interfase'"
                                                 + "\n"
                                                + " insert into [dbo].[utbBitacoralog] "
                                                   + " ( "
                                                  + " [NombreProceso] "
                                               + "    ,[IdProceso] "
                                                 + "  ,[Evento] "
                                                + "   ,[Metodo] "
                                                 + "  ,[FechaEjecucion] "
                                                  + " ,[Resultado] "
                                                  + " ,[Subject] "
                                                  + " ,[ErrorSistema] "
                                                  + " ,[MensajeSistema] "
                                                  + " ,[NombreArchivoDatos] "
                                                  + " ,[NombreArchivoControl] "
                                                  + " ,[Opcional]) "

                                                    + " 			Select						 "
                                                    + " 		  Xd.NombreProceso "
                                                    + " 		  ,Xd.IdProceso "
                                                    + " 		  ,Xd.Evento "
                                                    + " 		  ,Xd.Metodo "
                                                    + " 		  ,Xd.FechaEjecucion "
                                                    + " 		  ,Xd.Resultado "
                                                    + " 		  ,Xd.Sub "
                                                    + " 		  ,Xd.ErrorSistema "
                                                    + " 		  ,Xd.MensajeSistema  "
                                                    + " 		  ,Xd.NombreArchivoDatos "
                                                    + " 		  ,Xd.NombreArchivoControl "
                                                    + " 		  ,Xd.Opcional	   "
                                                    + " 			From OPENXML (@idoc, @Subelemento, 1) "
                                                    + " 			WITH ( "
                                                    + " 			NombreProceso      nvarchar(max) '@Aplication', "
                                                    + " 			IdProceso          int          '@IdProceso', "
                                                    + " 			Evento             nvarchar(max) '@EventName', "
                                                    + " 			Metodo             nvarchar(max) '@MethodName', "
                                                    + " 			FechaEjecucion     datetime     '@Date', "
                                                    + " 			Resultado          nvarchar(max) '@Status', "
                                                    + " 			Sub                nvarchar(max) '@CustomMessage', "
                                                    + " 			ErrorSistema       nvarchar(max) '@SystemError', "
                                                    + " 			MensajeSistema     nvarchar(max) '@SystemMessage', "
                                                    + " 			NombreArchivoDatos nvarchar(max) '@ADatos', "
                                                    + " 			NombreArchivoControl  nvarchar(max) '@AControl', "
                                                    + " 			Opcional              nvarchar(max) '@Opcional', "
                                                    + " 			importXdrentId int '@mp:id' "
                                                    + " 			) as Xd "
                                                                 + "\n"
                                                                 + " where not exists(select idBitacora from utbBitacoralog "
                                                                        + " where  Xd.NombreProceso=NombreProceso and "
                                                                         + " Xd.Metodo=Metodo and"
                                                                          + " Xd.FechaEjecucion=FechaEjecucion and "
                                                                          + " Xd.Resultado=Resultado and Xd.IdProceso=IdProceso)"
                                                                          + "\n"

                                              + "\n"
                                                 + "set @Subelemento='Log/Errors/Interfase'"
                                                 + "\n"
                                                + " insert into [dbo].[utbBitacoralog] "
                                                   + " ( "
                                                  + " [NombreProceso] "
                                               + "    ,[IdProceso] "
                                                 + "  ,[Evento] "
                                                + "   ,[Metodo] "
                                                 + "  ,[FechaEjecucion] "
                                                  + " ,[Resultado] "
                                                  + " ,[Subject] "
                                                  + " ,[ErrorSistema] "
                                                  + " ,[MensajeSistema] "
                                                  + " ,[NombreArchivoDatos] "
                                                  + " ,[NombreArchivoControl] "
                                                  + " ,[Opcional]) "

                                                    + " 			Select						 "
                                                    + " 		  Xd.NombreProceso "
                                                    + " 		  ,Xd.IdProceso "
                                                    + " 		  ,Xd.Evento "
                                                    + " 		  ,Xd.Metodo "
                                                    + " 		  ,Xd.FechaEjecucion "
                                                    + " 		  ,Xd.Resultado "
                                                    + " 		  ,Xd.Sub "
                                                    + " 		  ,Xd.ErrorSistema "
                                                    + " 		  ,Xd.MensajeSistema  "
                                                    + " 		  ,Xd.NombreArchivoDatos "
                                                    + " 		  ,Xd.NombreArchivoControl "
                                                    + " 		  ,Xd.Opcional	   "
                                                    + " 			From OPENXML (@idoc, @Subelemento, 1) "
                                                    + " 			WITH ( "
                                                    + " 			NombreProceso      nvarchar(max) '@Aplication', "
                                                    + " 			IdProceso          int          '@IdProceso', "
                                                    + " 			Evento             nvarchar(max) '@EventName', "
                                                    + " 			Metodo             nvarchar(max) '@MethodName', "
                                                    + " 			FechaEjecucion     datetime     '@Date', "
                                                    + " 			Resultado          nvarchar(max) '@Status', "
                                                    + " 			Sub                nvarchar(max) '@CustomMessage', "
                                                    + " 			ErrorSistema       nvarchar(max) '@SystemError', "
                                                    + " 			MensajeSistema     nvarchar(max) '@SystemMessage', "
                                                    + " 			NombreArchivoDatos nvarchar(max) '@ADatos', "
                                                    + " 			NombreArchivoControl  nvarchar(max) '@AControl', "
                                                    + " 			Opcional              nvarchar(max) '@Opcional', "
                                                    + " 			importXdrentId int '@mp:id' "
                                                    + " 			) as Xd "
                                                                 + "\n"
                                                                 + " where not exists(select idBitacora from utbBitacoralog "
                                                                        + " where  Xd.NombreProceso=NombreProceso and "
                                                                         + " Xd.Metodo=Metodo and"
                                                                          + " Xd.FechaEjecucion=FechaEjecucion and "
                                                                          + " Xd.Resultado=Resultado and Xd.IdProceso=IdProceso)"
                                                                          + "\n"
                                        + "end"
                                                 + "\n"

                                         + "if @opcion='1'"
                                          + "\n"
                                         + "Begin"
                                          + "\n"
                                                 + "set @Subelemento='Log/Alerts/Interfase'"
                                                 + "\n"
                                                + " insert into [dbo].[utbBitacoralog] "
                                                   + " ( "
                                                  + " [NombreProceso] "
                                               + "    ,[IdProceso] "
                                                 + "  ,[Evento] "
                                                + "   ,[Metodo] "
                                                 + "  ,[FechaEjecucion] "
                                                  + " ,[Resultado] "
                                                  + " ,[Subject] "
                                                  + " ,[ErrorSistema] "
                                                  + " ,[MensajeSistema] "
                                                  + " ,[NombreArchivoDatos] "
                                                  + " ,[NombreArchivoControl] "
                                                  + " ,[Opcional]) "

                                                    + " 			Select						 "
                                                    + " 		  Xd.NombreProceso "
                                                    + " 		  ,Xd.IdProceso "
                                                    + " 		  ,Xd.Evento "
                                                    + " 		  ,Xd.Metodo "
                                                    + " 		  ,Xd.FechaEjecucion "
                                                    + " 		  ,Xd.Resultado "
                                                    + " 		  ,Xd.Sub "
                                                    + " 		  ,Xd.ErrorSistema "
                                                    + " 		  ,Xd.MensajeSistema  "
                                                    + " 		  ,Xd.NombreArchivoDatos "
                                                    + " 		  ,Xd.NombreArchivoControl "
                                                    + " 		  ,Xd.Opcional	   "
                                                    + " 			From OPENXML (@idoc, @Subelemento, 1) "
                                                    + " 			WITH ( "
                                                    + " 			NombreProceso      nvarchar(max) '@Aplication', "
                                                    + " 			IdProceso          int          '@IdProceso', "
                                                    + " 			Evento             nvarchar(max) '@EventName', "
                                                    + " 			Metodo             nvarchar(max) '@MethodName', "
                                                    + " 			FechaEjecucion     datetime     '@Date', "
                                                    + " 			Resultado          nvarchar(max) '@Status', "
                                                    + " 			Sub                nvarchar(max) '@CustomMessage', "
                                                    + " 			ErrorSistema       nvarchar(max) '@SystemError', "
                                                    + " 			MensajeSistema     nvarchar(max) '@SystemMessage', "
                                                    + " 			NombreArchivoDatos nvarchar(max) '@ADatos', "
                                                    + " 			NombreArchivoControl  nvarchar(max) '@AControl', "
                                                    + " 			Opcional              nvarchar(max) '@Opcional', "
                                                    + " 			importXdrentId int '@mp:id' "
                                                    + " 			) as Xd "
                                                                 + "\n"
                                                                 + " where not exists(select idBitacora from utbBitacoralog "
                                                                        + " where  Xd.NombreProceso=NombreProceso and "
                                                                         + " Xd.Metodo=Metodo and"
                                                                          + " Xd.FechaEjecucion=FechaEjecucion and "
                                                                          + " Xd.Resultado=Resultado and Xd.IdProceso=IdProceso)"
                                                                          + "\n"
                                              + "\n"
                                                 + "set @Subelemento='Log/Errors/Interfase'"
                                                 + "\n"
                                                + " insert into [dbo].[utbBitacoralog] "
                                                   + " ( "
                                                  + " [NombreProceso] "
                                               + "    ,[IdProceso] "
                                                 + "  ,[Evento] "
                                                + "   ,[Metodo] "
                                                 + "  ,[FechaEjecucion] "
                                                  + " ,[Resultado] "
                                                  + " ,[Subject] "
                                                  + " ,[ErrorSistema] "
                                                  + " ,[MensajeSistema] "
                                                  + " ,[NombreArchivoDatos] "
                                                  + " ,[NombreArchivoControl] "
                                                  + " ,[Opcional]) "

                                                    + " 			Select						 "
                                                    + " 		  Xd.NombreProceso "
                                                    + " 		  ,Xd.IdProceso "
                                                    + " 		  ,Xd.Evento "
                                                    + " 		  ,Xd.Metodo "
                                                    + " 		  ,Xd.FechaEjecucion "
                                                    + " 		  ,Xd.Resultado "
                                                    + " 		  ,Xd.Sub "
                                                    + " 		  ,Xd.ErrorSistema "
                                                    + " 		  ,Xd.MensajeSistema  "
                                                    + " 		  ,Xd.NombreArchivoDatos "
                                                    + " 		  ,Xd.NombreArchivoControl "
                                                    + " 		  ,Xd.Opcional	   "
                                                    + " 			From OPENXML (@idoc, @Subelemento, 1) "
                                                    + " 			WITH ( "
                                                    + " 			NombreProceso      nvarchar(max) '@Aplication', "
                                                    + " 			IdProceso          int          '@IdProceso', "
                                                    + " 			Evento             nvarchar(max) '@EventName', "
                                                    + " 			Metodo             nvarchar(max) '@MethodName', "
                                                    + " 			FechaEjecucion     datetime     '@Date', "
                                                    + " 			Resultado          nvarchar(max) '@Status', "
                                                    + " 			Sub                nvarchar(max) '@CustomMessage', "
                                                    + " 			ErrorSistema       nvarchar(max) '@SystemError', "
                                                    + " 			MensajeSistema     nvarchar(max) '@SystemMessage', "
                                                    + " 			NombreArchivoDatos nvarchar(max) '@ADatos', "
                                                    + " 			NombreArchivoControl  nvarchar(max) '@AControl', "
                                                    + " 			Opcional              nvarchar(max) '@Opcional', "
                                                    + " 			importXdrentId int '@mp:id' "
                                                    + " 			) as Xd "
                                                                 + "\n"
                                                                 + " where not exists(select idBitacora from utbBitacoralog "
                                                                        + " where  Xd.NombreProceso=NombreProceso and "
                                                                         + " Xd.Metodo=Metodo and"
                                                                          + " Xd.FechaEjecucion=FechaEjecucion and "
                                                                          + " Xd.Resultado=Resultado and Xd.IdProceso=IdProceso)"
                                                                          + "\n"


                                        + "end"
                                         + "\n"


                                             + " IF @@ERROR<>0 BEGIN ROLLBACK TRANSACTION RETURN -100 END			    "
                                              + "\n"
                                            + " EXEC sp_xml_removedocument @idoc "
                                           + "\n"
                                        + " COMMIT TRANSACTION "
                                                 + "\n"
                                        + " return @@RowCount	 "
                                         + "\n"
                                        + " END ";

                    #endregion
                    //modifico SP siempre
                    using (SqlCommand cmd = new SqlCommand(ModificaSP, RegresaConnexionstring()))
                    {
                        try
                        {
                            cmd.Connection.Open();
                            ErrorSP = cmd.ExecuteNonQuery();
                            pass = true;
                        }
                        catch (SqlException)
                        {
                            ErrorSP = -1;
                            pass = false;
                        }
                    }                  
                }            

               
            }

            return pass;
        
        }

    /// <summary>
    /// graba el XML hacia a tabla
    /// </summary>
    /// <param name="PathXML"></param>
    /// <returns></returns>
        private static int xmlInsertaToDB(string PathXML)
        {
            int insrtados = 0;
            using (SqlCommand command = new SqlCommand("uspInsertaBitacora"
                             , RegresaConnexionstring()))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 10000000;
                XDocument fileXml;
              

                try
                {
                    fileXml = XDocument.Load(PathXML);
                    command.Parameters.Add("@doc", SqlDbType.Text).Value = fileXml.ToString();
                    command.Parameters.Add("@opcion", SqlDbType.Int).Value =Convert.ToInt32(ConfigurationManager.AppSettings["LogConfig"].ToString());
                }
                catch (Exception)
                {
                }

               
              

                SqlParameter ret = new SqlParameter("@Return_Value", SqlDbType.Int);
                ret.Direction = ParameterDirection.ReturnValue;
                command.Parameters.Add(ret);

                try
                {
                    command.Connection.Open();
                    insrtados = Convert.ToInt32(command.ExecuteNonQuery());
                }
                catch (Exception)
                {

                }

            }
            return insrtados;

        }
        /// <summary>
        /// Convierte la lista en XML y la graba a la tabla.
        /// </summary>
        /// <returns></returns>
        private static int ListaInsertaToDB() 
        {
            int insrtados = 0;
            using (SqlCommand command = new SqlCommand("uspInsertaBitacora"
                               , RegresaConnexionstring()))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 10000000;
                XDocument fileXml = new XDocument();
                XElement xml;
                xml = fileXml.Root;

                xml = new XElement("Log");
                xml.Add(new XElement("Information"));
                xml.Add(new XElement("Alerts"));
                xml.Add(new XElement("Errors"));

                try
                {
                    if (IntefaseLogList!=null || IntefaseLogList.Count>=1  )
                    {
                    foreach (SucesoInterfaseBuilder iInt in IntefaseLogList)
                    {
                        InsertallInfo(xml, iInt);
                    }
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                fileXml = new XDocument(xml);

                command.Parameters.Add("@doc", SqlDbType.Text).Value = fileXml.ToString();
                command.Parameters.Add("@opcion", SqlDbType.Int).Value = Convert.ToInt32(ConfigurationManager.AppSettings["LogConfig"].ToString());
                SqlParameter ret = new SqlParameter("@Return_Value", SqlDbType.Int);
                ret.Direction = ParameterDirection.ReturnValue;
                command.Parameters.Add(ret);

                try
                {
                    command.Connection.Open();
                    insrtados = Convert.ToInt32(command.ExecuteNonQuery());
                }
                catch (Exception)
                {

                }

            }

            return insrtados;
        
        }
        #endregion

        #region Envio de Notificaciones
        /// <summary>
        /// envia Notificacion con una conexion a BD Tomada del APP.config
        /// </summary>
        /// <param name="eNotificacion"></param>
        /// <returns></returns>
        public static bool EnviaNotificacion(NotificacionEmail eNotificacion)
        {
            bool pass = false;
           
            NotificacionEmailService NES = new NotificacionEmailService();
            pass = NES.EnviarNotificacion(eNotificacion);
            return pass;
        }

        /// <summary>
        /// envia Notificacion con una conexion a BD especificada.
        /// </summary>
        /// <param name="eNotificacion"></param>
        /// <returns></returns>
        public static bool EnviaNotificacion(NotificacionEmail eNotificacion,string ConecctionStringProvided)
        {
            bool pass = false;
            ConGenral = ConecctionStringProvided;
            NotificacionEmailService NES = new NotificacionEmailService(ConecctionStringProvided);
            pass = NES.EnviarNotificacion(eNotificacion);
            return pass;
        }
        /// <summary>
        /// Envia Bitacora de los procesos con una conexion a BD Tomada del APP.config
        /// </summary>
        /// <param name="ConecctionStringProvided"></param>
        /// <returns></returns>
        public static bool EnviaBitacora()
        {
            bool pass = false;

            NotificacionEmailService NES = new NotificacionEmailService();
            pass = NES.EnviaBitacora(IntefaseLogList);
            return pass;
        }
        /// <summary>
        /// Envia Bitacora de los procesos con una conexion a BD especificada.
        /// </summary>
        /// <param name="ConecctionStringProvided"></param>
        /// <returns></returns>
        public static bool EnviaBitacora(string ConecctionStringProvided)
        {
            bool pass = false;
            ConGenral = ConecctionStringProvided;
            NotificacionEmailService NES = new NotificacionEmailService(ConecctionStringProvided);
            pass = NES.EnviaBitacora(IntefaseLogList);
            return pass;
        }
        #endregion Envio de Notificaciones


        #region Mantenimiento
        /// <summary>
        /// Limpia registros en la tabla y en el archivo XML.
        /// </summary>
        /// <returns></returns>
        private static bool MantanimientoTablaandXML()
        {
            bool pass = false;
            string Borra = "delete from utbBitacoralog"
                + "\n"
               + " where fechaEjecucion <= (convert(datetime, getdate(),103)-" + Convert.ToInt32(ConfigurationManager.AppSettings["Mantenimiento"].ToString()) + ")";
            ;
            using (SqlCommand cmd = new SqlCommand(Borra, RegresaConnexionstring()))
            {
                try
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    pass = true;
                }
                catch (Exception)
                {
                    pass = false;
                }
            }

            string filename = GetFileNameXmlInterfase();
            XDocument fileXml = new XDocument();
            XElement xml;
            try
            {
                fileXml = XDocument.Load(filename);
                xml = fileXml.Root;
                var iprocElement = (from e in xml.Descendants("Interfase")
                                    where DateTime.ParseExact(e.Attribute("Date").Value, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture) <= (DateTime.Now.Subtract(new TimeSpan(Convert.ToInt32(ConfigurationManager.AppSettings["Mantenimiento"].ToString()), 0, 0, 0)))
                                    select e).ToList();
                iprocElement.Remove();
                fileXml.Save(filename);
            }
            catch (Exception)
            {
                pass = false;
            }

            return pass;

        }
        #endregion
        #endregion
    }
}
