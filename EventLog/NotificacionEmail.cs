using System;
using System.Collections.Generic;
using System.Net.Mail;
using EventLogCatcher.Entities;
//Desarrollado por:Emmanuel Lohora
//Contacto: punky_casinotomo@hotmail.com


namespace EventLogCatcher
    {
    [Serializable]
   public class NotificacionEmail
    {
       #region Declarations

       private int _idFormato;
       private string _nombreNotificacion;
       private string _cuentaOrigen;
       private string _cuentaDestino;
       private List<string> _copiaPara;
       private string _cuerpoMensaje;
       private string _asuntoMensaje;
       private List<AttachedFile> _attachments;
       private MailPriority  _prioridad;
       private Exception _exception;
       private EnumTipoNotificacion _tipoNotificacion;
       public List<System.Net.Mail.Attachment> Adjuntos = new List<Attachment>();
       #endregion

       #region Properties

       public int IdFormato
       {
           get { return _idFormato; }
           set { _idFormato = value; }
       }


       public string NombreNotificacion
       {
           get { return _nombreNotificacion; }
           set { _nombreNotificacion = value; }
       }


       public string CuentaOrigen
       {
           get { return _cuentaOrigen; }
           set { _cuentaOrigen = value; }
       }


       public string CuentaDestino
       {
           get { return _cuentaDestino; }
           set { _cuentaDestino = value; }
       }


       public List<string> CopiaPara
       {
           get { return _copiaPara; }
           set { _copiaPara = value; }
       }


       public string CuerpoMensaje
       {
           get { return _cuerpoMensaje; }
           set { _cuerpoMensaje = value; }
       }


       public string AsuntoMensaje
       {
           get { return _asuntoMensaje; }
           set { _asuntoMensaje = value; }
       }


       public List<AttachedFile> Attachments
       {
           get { return _attachments; }
           set { _attachments = value; }
       }


       public MailPriority Prioridad
       {
           get { return _prioridad; }
           set { _prioridad = value; }
       }

       public Exception Exception
       {
           get { return _exception; }
           set { _exception = value; }
       }


       public EnumTipoNotificacion TipoNotificacion
       {
           get { return _tipoNotificacion; }
           set { _tipoNotificacion = value; }
       }
       #endregion

       #region Constructors


       /// <summary>
       /// Constructor para seleccionar los registros.Utilizado para Llenar un Formato de Mensaje.
       /// </summary>
       /// <param name="idFormato"></param>
       /// <param name="cuentaOrigen"></param>
       /// <param name="cuentaDestino"></param>
       /// <param name="asuntoMensaje"></param>
       /// <param name="cuerpo"></param>
       /// <param name="nombreFormato"></param>
       public NotificacionEmail(int idFormato, string cuentaOrigen
          , string cuentaDestino, string asuntoMensaje,string cuerpo,string nombreFormato
          )
       {
           _idFormato = idFormato;
           _cuentaOrigen = cuentaOrigen;
           _cuentaDestino = cuentaDestino;
           _asuntoMensaje = asuntoMensaje;
           _cuerpoMensaje = cuerpo;
           _nombreNotificacion = nombreFormato;
           _copiaPara = new List<string>();         
       }

       /// <summary>
       /// Constructor para Crear una notificacion sin Formato de Mensaje Especificado
       /// </summary>
       /// <param name="idFormato"></param>
       /// <param name="cuentaOrigen"></param>
       /// <param name="cuentaDestino"></param>
       /// <param name="asuntoMensaje"></param>
       /// <param name="cuerpo"></param>
       /// <param name="nombreFormato"></param>
       public NotificacionEmail(string cuentaOrigen
         , string cuentaDestino, List<string> copiaPara, string asuntoMensaje, string cuerpo
           , EnumTipoNotificacion tipoNotificacion)
       {
           _cuentaOrigen = cuentaOrigen;
           _cuentaDestino = cuentaDestino;
           _asuntoMensaje = asuntoMensaje;
           _cuerpoMensaje = cuerpo;
           _tipoNotificacion = tipoNotificacion;
           _copiaPara = new List<string>();
           _copiaPara = copiaPara;           
       }


       /// <summary>
       /// Constructor Para Mandar Errores de Notificacion Utilizada cuando son errores Del Usuario.
       /// 
       /// </summary>
       /// <param name="asuntoMensaje"></param>
       /// <param name="exception"></param>
       /// <param name="tipoNotificacion"></param>
       public NotificacionEmail(string asuntoMensaje
          , Exception exception)
       {
           _tipoNotificacion= EnumTipoNotificacion.Error;
           _asuntoMensaje = asuntoMensaje;
           _exception = exception;
       }
       public NotificacionEmail()
       {

       }
       #endregion
    }
}
