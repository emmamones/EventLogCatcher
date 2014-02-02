using EventLogCatcher.Entities;
using System;
//Desarrollado por:Emmanuel Lohora
//Contacto: punky_casinotomo@hotmail.com
//version: miercoles 29 enero 2014
namespace EventLogCatcher.Core
    {
    [Serializable]
    public class MediaAbstract
        {
        public MediaAbstract() { }
 
        protected bool Succes;

        public ITemplate Template;


        public MediaAbstract(int pConfig, EventType pTypeEvent, ITemplate pTemplate, infoSuceso pInfo)
            { 
            Template = pTemplate;
            Template.CreateHeader(pTypeEvent, pConfig, pInfo);
            }

        /// <summary>
        /// Ejecucion del Proceso
        /// </summary>
        public bool Start()
            {
            try
                {
                Succes = Template.WriteFile();
                }
            catch (Exception)
                {
                Succes = false;
                }
            return Succes;

            }


        }
    }
