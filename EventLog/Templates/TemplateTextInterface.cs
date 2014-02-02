using System.IO;
using EventLogCatcher.Core;
using EventLogCatcher.Entities;
using System;
//Desarrollado por:Emmanuel Lohora
//Contacto: punky_casinotomo@hotmail.com

namespace EventLogCatcher.Templates
    {
    [Serializable]
    public class TemplateTextInterface : TemplateTextAbstract
        {
        /// <summary>
        /// Constructor Vacio.
        /// </summary>
        public TemplateTextInterface() { }


        public override StreamWriter SetAllInfo(StreamWriter FW, string Encabezado, infoSuceso Suceso)
            {
            FW.WriteLine("======================================");
            FW.WriteLine(Encabezado);
            FW.WriteLine("Date: " + Suceso.Date.ToString());
            FW.WriteLine("Interfas: " + Suceso.NombreInterfase);
            FW.WriteLine("IdProceso: " + Suceso.IdInterfase);
            FW.WriteLine("Event: " + Suceso.Event);
            FW.WriteLine("Method: " + Suceso.Method);
            FW.WriteLine("Resultado: " + Suceso.Resultado);
            FW.WriteLine("Subject: " + Suceso.Subject);
            FW.WriteLine("System Error Message: " + Suceso.SystemMessage);
            FW.WriteLine("Opcional: " + Suceso.OpcionalField);
            FW.WriteLine("Archivo Datos: " + Suceso.ADatos);
            FW.WriteLine("Archivo Control: " + Suceso.AControl);
            if (Suceso.SystemError != null)
                {
                FW.WriteLine("Type of Error: " + Suceso.SystemError.GetType().ToString());
                }
            FW.WriteLine("======================================");
            return FW;
            }

        public override StreamWriter SetAlertsErros(StreamWriter FW, string Encabezado, infoSuceso Suceso)
            {
            if (Suceso.TypeEvent == EventType.Alert || Suceso.TypeEvent == EventType.Error)
                {
                FW.WriteLine("======================================");
                FW.WriteLine(Encabezado);
                FW.WriteLine("Date: " + Suceso.Date.ToString());
                FW.WriteLine("Interfas: " + Suceso.NombreInterfase);
                FW.WriteLine("IdProceso: " + Suceso.IdInterfase);
                FW.WriteLine("Event: " + Suceso.Event);
                FW.WriteLine("Method: " + Suceso.Method);
                FW.WriteLine("Resultado: " + Suceso.Resultado);
                FW.WriteLine("Subject: " + Suceso.Subject);
                FW.WriteLine("System Error Message: " + Suceso.SystemMessage);
                FW.WriteLine("Opcional: " + Suceso.OpcionalField);
                FW.WriteLine("Archivo Datos: " + Suceso.ADatos);
                FW.WriteLine("Archivo Control: " + Suceso.AControl);
                if (Suceso.SystemError != null)
                    {
                    FW.WriteLine("Type of Error: " + Suceso.SystemError.GetType().ToString());
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
                FW.WriteLine("Interfas: " + Suceso.NombreInterfase);
                FW.WriteLine("IdProceso: " + Suceso.IdInterfase);
                FW.WriteLine("Event: " + Suceso.Event);
                FW.WriteLine("Method: " + Suceso.Method);
                FW.WriteLine("Resultado: " + Suceso.Resultado);
                FW.WriteLine("Subject: " + Suceso.Subject);
                FW.WriteLine("System Error Message: " + Suceso.SystemMessage);
                FW.WriteLine("Opcional: " + Suceso.OpcionalField);
                FW.WriteLine("Archivo Datos: " + Suceso.ADatos);
                FW.WriteLine("Archivo Control: " + Suceso.AControl);
                if (Suceso.SystemError != null)
                    {
                    FW.WriteLine("Type of Error: " + Suceso.SystemError.GetType().ToString());
                    }
                FW.WriteLine("======================================");
                }
            return FW;
            }

        public override bool WriteFile()
            {
            throw new NotImplementedException();
            }
        }
    }
