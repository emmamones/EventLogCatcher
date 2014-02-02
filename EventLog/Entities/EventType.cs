using System;
//Desarrollado por:Emmanuel Lohora
//Contacto: punky_casinotomo@hotmail.com
//version: miercoles 29 enero 2014
namespace EventLogCatcher.Entities
{
    [Serializable]
    public enum EventType : int
        {
        Information = 1,
        Alert = 2,
        Error = 3,
        }
}
