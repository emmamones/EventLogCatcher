using System.Net.Mime;
using System;
//Desarrollado por:Emmanuel Lohora
//Contacto: punky_casinotomo@hotmail.com
//version: miercoles 29 enero 2014

namespace EventLogCatcher.Entities
    {
    [Serializable]
   public class AttachedFile
    {
       #region Declarations
        private byte[] _arraydeArchivo;
        private string _nombreArchivo;
        private ContentType _tipoArchivo;        
       #endregion
       #region Properties
        public byte[] ArraydeArchivo
        {
            get { return _arraydeArchivo; }
            set { _arraydeArchivo = value; }
        }


        public string NombreArchivo
        {
            get { return _nombreArchivo; }
            set { _nombreArchivo = value; }
        }


       public ContentType TipoArchivo
        {
            get { return _tipoArchivo; }
            set { _tipoArchivo = value; }
        }
       #endregion
       #region Constructors

       public AttachedFile(byte[] arraydeArchivo, string nombreArchivo, ContentType tipoArchivo)
       {
           _arraydeArchivo = arraydeArchivo;
           _nombreArchivo = nombreArchivo;
           _tipoArchivo = tipoArchivo;
       
       }

       #endregion
    }
}
