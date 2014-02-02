using System;
//Desarrollado por:Emmanuel Lohora
//Contacto: punky_casinotomo@hotmail.com
//version: miercoles 29 enero 2014
namespace EventLogCatcher.Entities
    {
    [Serializable]
  public  class infoSuceso
        {
        #region Fields
        public Exception _exception { get; set; }
        public string IdBitacora { get; set; }
        public string Application { get; set; }
        public string IdApplication { get; set; }
        public string Event { get; set; }
        public string Method { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public string CustomMessage { get; set; }
        public object SystemError { get; set; }
        public string SystemMessage { get; set; }
        public object FileName { get; set; }
        public object OpcionalField { get; set; }
        public object OpcionalField2 { get; set; }
        public EventType TypeEvent { get; set; }
        public int Config;
        public string NombreInterfase { get; set; }
        public string IdInterfase { get; set; }
        public string Resultado { get; set; }
        public string Subject;
        public object AControl { get; set; }
        public object ADatos { get; set; }

        #endregion

      public infoSuceso() { }
      
        }
    }
