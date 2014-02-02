using System.Collections.Generic;
using EventLogCatcher.Sucesos;
using System;
//Desarrollado por:Emmanuel Lohora
//Contacto: punky_casinotomo@hotmail.com

namespace EventLogCatcher
{
      public interface INotificacionEmailService
    {
       bool EnviarNotificacion(NotificacionEmail pNemail);
       bool EnviaBitacora(List<SucesoInterfaseBuilder> IntefaseLogList);
       int Agregar(NotificacionEmail pNemail);
       bool Actualizar(NotificacionEmail pNemail);
       List<NotificacionEmail> Seleccionar();
       NotificacionEmail TraerDatosNotificacion(int pIdFormato);
       bool Eliminar(int pIdObjeto);
    }
}
