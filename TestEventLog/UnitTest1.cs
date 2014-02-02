using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EventLogCatcher.Core;
using EventLogCatcher.Sucesos;
using EventLogCatcher.Entities;

namespace TestEventLog
    {
    [TestClass]
    public class UnitTest1
        {
        [TestMethod]
        public void EjecutarLoger()
            {
            LogDirector LG = new LogDirector();
            SucesoAbstract SEventB;
            try
                {

                throw new Exception();

                }
            catch (Exception e)
                {
                #region adderrorLog

                SEventB = new SucesoEventBuilder(new infoSuceso()
                {
                    Application = "EventManager",
                    Event = "MainClass",
                    Method = "ReadEvents()",
                    Date = DateTime.Now,
                    Status = "Fail",
                    CustomMessage = "Error al Leer los Procesos del archivo fuente xml"
                    ,
                    SystemMessage = e.Message
                    ,
                    SystemError = e.StackTrace
                    ,
                    FileName = "MainClass.cs",
                    OpcionalField = "",
                    TypeEvent = EventLogCatcher.Entities.EventType.Error
                });

                //SEventB = new SucesoInterfaseBuilder(new infoSuceso()
                //{
                //    IdBitacora = "0",
                //    NombreInterfase = "InterfasePrueba",
                //    IdInterfase = "p1",
                //    Event = "Fallo al  Buscar el Archivo de Control",
                //    Method = "BuscarArchivoControlFTP()",
                //    Date = DateTime.Now,
                //    Resultado = "Fallo",
                //    Subject = "",
                //    SystemError = e.ToString(),
                //    SystemMessage = e.Message.ToString(),
                //    AControl = "ArchivoControl",
                //    ADatos = "archivoDatos",
                //    TypeEvent = EventLogCatcher.Entities.EventType.Error,                         
                //    OpcionalField = "Admin"
                //});


                Assert.AreEqual(true, LG.Handles(SEventB));

                #endregion

                }

            }
        }
    }
