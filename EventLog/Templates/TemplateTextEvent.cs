using System;
using System.IO;
using EventLogCatcher.Core;
using EventLogCatcher.Entities;
//Desarrollado por:Emmanuel Lohora
//Contacto: punky_casinotomo@hotmail.com

namespace EventLogCatcher.Templates
    {
    [Serializable]
    public class TemplateTextEvent : TemplateTextAbstract
        {
        /// <summary>
        /// Constructor Vacio.
        /// </summary>
        public TemplateTextEvent() { }

        public override StreamWriter SetAllInfo(StreamWriter FW, string Encabezado, infoSuceso Suceso)
            {
            try
                {
                FW.WriteLine("======================================");
                FW.WriteLine(Encabezado);
                FW.WriteLine("Date: " + Suceso.Date.ToString());
                FW.WriteLine("Application: " + Suceso.Application);
                FW.WriteLine("Event: " + Suceso.Event);
                FW.WriteLine("Method: " + Suceso.Method);
                FW.WriteLine("Status: " + Suceso.Status);
                FW.WriteLine("Custom Message: " + Suceso.CustomMessage);
                FW.WriteLine("System Error Message: " + Suceso.SystemMessage);
                FW.WriteLine("File Name: " + Suceso.FileName);
                FW.WriteLine("File Name: " + Suceso.OpcionalField);
                FW.WriteLine("Opcional: " + Suceso.OpcionalField2);
                FW.WriteLine("======================================");
                if (Suceso.SystemError != null)
                    {
                    FW.WriteLine("Type of Errorr: " + Suceso.SystemError.GetType().ToString());
                    }
                FW.WriteLine("======================================");
                }
            catch (Exception)
                {

                throw;
                }

            return FW;
            }

        public override StreamWriter SetAlertsErros(StreamWriter FW, string Encabezado, infoSuceso Suceso)
            {
            if (Suceso.TypeEvent == EventType.Alert || Suceso.TypeEvent == EventType.Error)
                {
                FW.WriteLine("======================================");
                FW.WriteLine(Encabezado);
                FW.WriteLine("Date: " + Suceso.Date.ToString());
                FW.WriteLine("Application: " + Suceso.Application);
                FW.WriteLine("Event: " + Suceso.Event);
                FW.WriteLine("Method: " + Suceso.Method);
                FW.WriteLine("Status: " + Suceso.Status);
                FW.WriteLine("Custom Message: " + Suceso.CustomMessage);
                FW.WriteLine("System Error Message: " + Suceso.SystemMessage);
                FW.WriteLine("File Name: " + Suceso.FileName);
                FW.WriteLine("File Name: " + Suceso.OpcionalField);
                FW.WriteLine("Opcional: " + Suceso.OpcionalField2);
                FW.WriteLine("======================================");
                if (Suceso.SystemError != null)
                    {
                    FW.WriteLine("Type of Errorr: " + Suceso.SystemError.GetType().ToString());
                    }
                FW.WriteLine("======================================");

                }

            return FW;
            }

        public override StreamWriter SetOnlyErros(StreamWriter FW, string Encabezado, infoSuceso Suceso)
            {
            if (Suceso.TypeEvent == EventType.Error)
                {
                FW.WriteLine("======================================");
                FW.WriteLine(Encabezado);
                FW.WriteLine("Date: " + Suceso.Date.ToString());
                FW.WriteLine("Application: " + Suceso.Application);
                FW.WriteLine("Event: " + Suceso.Event);
                FW.WriteLine("Method: " + Suceso.Method);
                FW.WriteLine("Status: " + Suceso.Status);
                FW.WriteLine("Custom Message: " + Suceso.CustomMessage);
                FW.WriteLine("System Error Message: " + Suceso.SystemMessage);
                FW.WriteLine("File Name: " + Suceso.FileName);
                FW.WriteLine("File Name: " + Suceso.OpcionalField);
                FW.WriteLine("Opcional: " + Suceso.OpcionalField2);
                FW.WriteLine("======================================");
                if (Suceso.SystemError != null)
                    {
                    FW.WriteLine("Type of Errorr: " + Suceso.SystemError.GetType().ToString());
                    }
                FW.WriteLine("======================================");

                }

            return FW;
            }

        public override bool WriteFile()
            {

            #region File Setup

            base.GetFileName();
            base.GetPathName();
            StreamWriter filewritter;

            if (!Directory.Exists(base.filename))
                {
                Directory.CreateDirectory(base.PathName);
                }
            #endregion

            #region LLena Archivo
            if (!File.Exists(base.filename))
                {
                filewritter = File.CreateText(base.filename);
                switch (base.Config)
                    {
                    case 0:
                        SetAllInfo(filewritter, base.header, base.InformacionSuceso);
                        break;
                    case 1:
                        SetAlertsErros(filewritter, base.header, base.InformacionSuceso);
                        break;
                    default:
                        SetOnlyErros(filewritter, base.header, base.InformacionSuceso);
                        break;
                    }
                filewritter.Close();
                return true;
                }
            else
                {
                using (FileStream files = new FileStream(base.filename, FileMode.Append, FileAccess.Write))
                    {
                    filewritter = new StreamWriter(files);
                    switch (base.Config)
                        {
                        case 0:
                            SetAllInfo(filewritter, base.header, base.InformacionSuceso);
                            break;
                        case 1:
                            SetAlertsErros(filewritter, base.header, base.InformacionSuceso);
                            break;
                        default:
                            SetOnlyErros(filewritter, base.header, base.InformacionSuceso);
                            break;
                        }
                    filewritter.Close();
                    }
                return true;
                }
            
            #endregion

            }

        }
    }
