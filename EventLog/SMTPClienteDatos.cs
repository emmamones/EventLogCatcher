using System;
using System.Collections.Generic;
using System.Text;
//Desarrollado por:Emmanuel Lohora
//Contacto: punky_casinotomo@hotmail.com

namespace EventLogCatcher
    {
    [Serializable]
   public class SMTPClienteDatos
    {
        private int _puerto;

        public int Puerto
        {
            get { return _puerto; }
            set { _puerto = value; }
        }
        private string _nombre;

        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }


        public SMTPClienteDatos(int puerto, string nombre)
       {
           _puerto = puerto;
           _nombre = nombre;
       }
       public SMTPClienteDatos()
       {
          
       }

    }
}
