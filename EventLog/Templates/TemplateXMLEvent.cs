using System.Linq;
using EventLogCatcher.Core;
using EventLogCatcher.Entities;
using System.Xml.Linq;
using System;
using System.IO;
//Desarrollado por:Emmanuel Lohora
//Contacto: punky_casinotomo@hotmail.com

namespace EventLogCatcher.Templates
    {
    [Serializable]
    public class TemplateXMLEvent : TemplateXMLAbstract
        {
        /// <summary>
        /// Constructor Vacio.
        /// </summary>
        public TemplateXMLEvent() { }

        public override XElement SetAllInfo(XElement xml, string Encabezado, infoSuceso Suceso)
            {
            switch (Suceso.TypeEvent)
                {
                case EventType.Information:
                    xml.Element(Encabezado).Add(
                new XElement("Event", new XAttribute("Aplication", Suceso.Application)
                    , new XAttribute("EventName", Suceso.Event)
                    , new XAttribute("MethodName", Suceso.Method)
                    , new XAttribute("Date", Suceso.Date.ToString())));
                    xml.Element("Information").Descendants("Event").Last().Add(new XElement("CustomMessage", Suceso.CustomMessage)
                        , new XElement("SystemMessage", Suceso.SystemMessage)
                        , new XElement("FileName", Suceso.FileName)
                        , new XElement("Status", Suceso.Status)
                         , new XElement("Opcional", Suceso.OpcionalField));
                    break;
                case EventType.Alert:
                    xml.Element(Encabezado).Add(
                  new XElement("Event", new XAttribute("Aplication", Suceso.Application)
                      , new XAttribute("EventName", Suceso.Event)
                      , new XAttribute("MethodName", Suceso.Method)
                      , new XAttribute("Date", Suceso.Date.ToString())));
                    xml.Element("Alerts").Descendants("Event").Last().Add(new XElement("CustomMessage", Suceso.CustomMessage)
                      , new XElement("SystemMessage", Suceso.SystemMessage)
                      , new XElement("FileName", Suceso.FileName)
                      , new XElement("Status", Suceso.Status)
                       , new XElement("Opcional", Suceso.OpcionalField));
                    break;
                case EventType.Error:
                    xml.Element(Encabezado).Add(
                  new XElement("Event", new XAttribute("Aplication", Suceso.Application)
                      , new XAttribute("EventName", Suceso.Event)
                      , new XAttribute("MethodName", Suceso.Method)
                      , new XAttribute("Date", Suceso.Date.ToString())));
                    xml.Element("Errors").Descendants("Event").Last().Add(new XElement("CustomMessage", Suceso.CustomMessage)
                        , new XElement("Status", Suceso.Status)
                        , new XElement("SystemMessage", Suceso.SystemMessage)
                        , new XElement("SystemError", Suceso.SystemError)
                        , new XElement("FileName", Suceso.FileName)
                        , new XElement("Opcional", Suceso.OpcionalField)
                         , new XElement("Opcional", Suceso.OpcionalField2));
                    break;
                default:
                    break;
                }

            return xml;
            }

        public override XElement SetAlertsErros(XElement xml, string Encabezado, infoSuceso Suceso)
            {
            switch (Suceso.TypeEvent)
                {                
                case EventType.Alert:
                    xml.Element(Encabezado).Add(
                  new XElement("Event", new XAttribute("Aplication", Suceso.Application)
                      , new XAttribute("EventName", Suceso.Event)
                      , new XAttribute("MethodName", Suceso.Method)
                      , new XAttribute("Date", Suceso.Date.ToString())));
                    xml.Element("Alerts").Descendants("Event").Last().Add(new XElement("CustomMessage", Suceso.CustomMessage)
                      , new XElement("SystemMessage", Suceso.SystemMessage)
                      , new XElement("FileName", Suceso.FileName)
                      , new XElement("Status", Suceso.Status)
                       , new XElement("Opcional", Suceso.OpcionalField));
                    break;
                case EventType.Error:
                    xml.Element(Encabezado).Add(
                  new XElement("Event", new XAttribute("Aplication", Suceso.Application)
                      , new XAttribute("EventName", Suceso.Event)
                      , new XAttribute("MethodName", Suceso.Method)
                      , new XAttribute("Date", Suceso.Date.ToString())));
                    xml.Element("Errors").Descendants("Event").Last().Add(new XElement("CustomMessage", Suceso.CustomMessage)
                        , new XElement("Status", Suceso.Status)
                        , new XElement("SystemMessage", Suceso.SystemMessage)
                        , new XElement("SystemError", Suceso.SystemError)
                        , new XElement("FileName", Suceso.FileName)
                        , new XElement("Opcional", Suceso.OpcionalField)
                         , new XElement("Opcional", Suceso.OpcionalField2));
                    break;
                default:
                    break;
                }

            return xml;
            }

        public override XElement SetOnlyErros(XElement xml, string Encabezado, infoSuceso Suceso)
            {
            switch (Suceso.TypeEvent)
                {                
                case EventType.Error:
                    xml.Element(Encabezado).Add(
                  new XElement("Event", new XAttribute("Aplication", Suceso.Application)
                      , new XAttribute("EventName", Suceso.Event)
                      , new XAttribute("MethodName", Suceso.Method)
                      , new XAttribute("Date", Suceso.Date.ToString())));
                    xml.Element("Errors").Descendants("Event").Last().Add(new XElement("CustomMessage", Suceso.CustomMessage)
                        , new XElement("Status", Suceso.Status)
                        , new XElement("SystemMessage", Suceso.SystemMessage)
                        , new XElement("SystemError", Suceso.SystemError)
                        , new XElement("FileName", Suceso.FileName)
                        , new XElement("Opcional", Suceso.OpcionalField)
                         , new XElement("Opcional", Suceso.OpcionalField2));
                    break;
                default:
                    break;
                }

            return xml;
            }

        public override bool WriteFile()
            {

            base.GetFileName();
            base.GetPathName();
            
            XDocument fileXml = new XDocument();
            XElement xml; 
            if (!Directory.Exists(base.filename))
                {
                Directory.CreateDirectory(base.PathName);
                }

            if (File.Exists(base.filename))
                {
                fileXml = XDocument.Load(base.filename);
                xml = fileXml.Root;
                }
            else
                {
                xml = new XElement("Log");
                xml.Add(new XElement("Information"));
                xml.Add(new XElement("Alerts"));
                xml.Add(new XElement("Errors"));
                }
            switch (base.Config)
                {
                case 0:
                    SetAllInfo(xml, base.header, base.InformacionSuceso);
                    break;
                case 1:
                    SetAlertsErros(xml, base.header, base.InformacionSuceso);
                    break;
                default:
                    SetOnlyErros(xml, base.header, base.InformacionSuceso);
                    break;
                }
            fileXml = new XDocument(xml);
            fileXml.Save(filename);
            return true;
            }
        }
    }
