using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.IO;
using System.Linq;
using System.ComponentModel;
using EventLogCatcher.Entities;
using EventLogCatcher.Sucesos;
//Desarrollado por:Emmanuel Lohora
//Contacto: punky_casinotomo@hotmail.com

namespace EventLogCatcher
    {
    [Serializable]
   public class NotificacionEmailService:INotificacionEmailService
   {
       #region Declarations
       public static MailMessage MyMail = new MailMessage();
       public NotificacionEmail MyNotificacionEmail = new NotificacionEmail();
       static string ConGenral = string.Empty;
       public int Tipo = 0;
       static bool enviado = false;
       #endregion

    
       public NotificacionEmailService()
       {
         
       }
       /// <summary>
       /// si se usa externo o en otra capa se le puede mandar la conexion.
       /// </summary>
       /// <param name="ConecctionStringProvided"></param>
       public NotificacionEmailService(string ConecctionStringProvided) 
       {
           ConGenral = ConecctionStringProvided;
       }
      /// <summary>
       /// 1 para no ir a la BD (crea un formato default y manda errores a cuenta default
       /// 0 Busca el formato y las cuentas en BD.
      /// </summary>
      /// <param name="tipo"></param>
       public NotificacionEmailService(int tipo)
       {         
           Tipo = tipo;
       }
       #region INotificacionEmailService Members


       /// <summary>
       /// Envia Notificacion.
       /// </summary>
       /// <param name="pNemail"></param>
       /// <returns></returns>
       public bool EnviarNotificacion(NotificacionEmail pNemail)
       {
          
           MailMessage MyMail = new MailMessage();
           switch (pNemail.TipoNotificacion)
           {
               case EnumTipoNotificacion.Mensajes:

               #region Notificacion Mensaje

                   #region From
                   if (string.IsNullOrEmpty(pNemail.CuentaOrigen))
                       throw new ExceptionNotificacionEmail("No se ingreso Cuenta de origen");
                   else
                   {

                       MyMail.From = new MailAddress(pNemail.CuentaOrigen.ToString(), pNemail.CuentaOrigen.ToString());
                   }



                   #endregion

                   #region To

                   if (string.IsNullOrEmpty(pNemail.CuentaDestino))
                       throw new ExceptionNotificacionEmail("No se ingreso Cuenta de Destino");
                   else
                   {
                       MyMail.To.Add(new MailAddress(pNemail.CuentaDestino.ToString()));
                   }


                   #endregion

                   #region BCC

                   //MyMail.Bcc.Add(new MailAddress(pNemail.CopiaPara.ToString()));
                   #endregion

                   #region CC

                   if (pNemail.CopiaPara.Count() >= 1 && pNemail.CopiaPara != null)
                   {
                       foreach (string inAdress in pNemail.CopiaPara)
                       {
                           MyMail.CC.Add(new MailAddress(inAdress));
                       }
                   }

                   #endregion

                   #region Subject
                   if (string.IsNullOrEmpty(pNemail.AsuntoMensaje))
                       throw new ExceptionNotificacionEmail("No se ingreso El Asunto del Mensaje");
                   else
                   {
                       MyMail.Subject = pNemail.AsuntoMensaje.ToString();
                   }
                   #endregion

                   #region Attachments
                   if(pNemail.Adjuntos!=null && pNemail.Adjuntos.Count > 0)
                   {
                       foreach (var iFile in pNemail.Adjuntos)
                       {
                           MyMail.Attachments.Add(iFile);
                       }
                   }


                   if (pNemail.Attachments != null && pNemail.Attachments.Count > 0)
                   { SetAttachments(pNemail.Attachments); }

                   #endregion

                   #region Body
                   if (string.IsNullOrEmpty(pNemail.CuerpoMensaje))
                       throw new ExceptionNotificacionEmail("No se ingreso el Cuerpo del Mensaje");
                   else
                   {
                       MyMail.Body = pNemail.CuerpoMensaje;
                   }
                   #endregion

                   #region Deployment
                   // Especifica el formato del cuerpo del mensaje
                   MyMail.IsBodyHtml = true;
                   // establece la prioridad del mensaje
                   MyMail.Priority = pNemail.Prioridad;
                   #endregion


                   SMTPClienteDatos smtpServer = TraerServidorSMPT();
                   if (string.IsNullOrEmpty(smtpServer.Nombre))
                       throw new ExceptionNotificacionEmail("No existe registro del Nombre de servidor SMPT");

                   if (string.IsNullOrEmpty(smtpServer.Puerto.ToString()))
                       throw new ExceptionNotificacionEmail("No existe registro del Puerto de servidor SMPT");

                   if (!(string.IsNullOrEmpty(smtpServer.Nombre) || string.IsNullOrEmpty(smtpServer.Puerto.ToString())))
                   {

                       SmtpClient clienteSmtp = new SmtpClient(smtpServer.Nombre, smtpServer.Puerto);                   
                       try
                       {                         

                           clienteSmtp.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);                            
                           string userState = "envio de notificacion ||" + MyMail.Subject;
                           clienteSmtp.SendAsync(MyMail, userState);
                           enviado = true;
                       }
                       catch (Exception e)
                       {
                           enviado = false;

                           ExceptionNotificacionEmail NotificacionException = new ExceptionNotificacionEmail("El Metodo EnviarNotificacion fallo" + " || ", e);
                           NotificacionEmail notificacion = new NotificacionEmail("Error en DLOG", NotificacionException);
                           notificacion.TipoNotificacion = EnumTipoNotificacion.Error;
                           NotificacionEmailService NS = new NotificacionEmailService(ConGenral);
                           NS.EnviarNotificacion(notificacion);

                           throw NotificacionException;
                       }
                      

                   }


                   #endregion
                   break;
               #region Error
               case EnumTipoNotificacion.Error:


                   NotificacionEmail NotificacionError = TraerDatosNotificacionError(pNemail, 1);


                   #region From
                   if (string.IsNullOrEmpty(NotificacionError.CuentaOrigen.ToString()))
                       throw new ExceptionNotificacionEmail("No se ingreso Cuenta de origen");
                   else
                   {
                       MyMail.From = new MailAddress(NotificacionError.CuentaOrigen.ToString(), NotificacionError.CuentaOrigen.ToString());
                   }

                   #endregion

                   #region To
                   if (string.IsNullOrEmpty(NotificacionError.CuentaDestino.ToString()))
                       throw new ExceptionNotificacionEmail("No se ingreso Cuenta Destino");
                   else
                   {
                     string [] lCorreos=  NotificacionError.CuentaDestino.ToString().Split(';');

                     foreach (var icorreo in lCorreos)
                     {
                         MyMail.To.Add(new MailAddress(icorreo));
                     }
                      
                   }

                   #endregion

                   #region BCC

                  
                   #endregion

                   #region CC

                   if (NotificacionError.CopiaPara.Count() >= 1 && NotificacionError.CopiaPara != null)
                   {
                       foreach (string inAdress in NotificacionError.CopiaPara)
                       {
                           MyMail.CC.Add(new MailAddress(inAdress));
                       }
                   }

                   #endregion

                   #region Subject
                   if (string.IsNullOrEmpty(NotificacionError.AsuntoMensaje.ToString()))
                       throw new ExceptionNotificacionEmail("No se ingreso Asunto del Mensaje");
                   else
                   {
                       MyMail.Subject = NotificacionError.AsuntoMensaje.ToString();
                   }
                   #endregion

                   #region Attachments
                   if (NotificacionError.Attachments != null && NotificacionError.Attachments.Count > 0)
                   { SetAttachments(NotificacionError.Attachments); }

                   #endregion

                   #region Body
                   if (string.IsNullOrEmpty(NotificacionError.CuerpoMensaje.ToString()))
                       throw new ExceptionNotificacionEmail("No se ingreso El Cuerpo del Mensaje");
                   else
                   {
                       MyMail.Body = NotificacionError.CuerpoMensaje;
                   }
                   #endregion

                   #region Deployment
                   // Especifica el formato del cuerpo del mensaje
                   MyMail.IsBodyHtml = true;
                   // establece la prioridad del mensaje
                   MyMail.Priority = pNemail.Prioridad;
                   #endregion


                   SMTPClienteDatos smtpServer2 = TraerServidorSMPT();
                   if (string.IsNullOrEmpty(smtpServer2.Nombre))
                       throw new ExceptionNotificacionEmail("No existe registro del Nombre de servidor SMPT");

                   if (string.IsNullOrEmpty(smtpServer2.Puerto.ToString()))
                       throw new ExceptionNotificacionEmail("No existe registro del Puerto de servidor SMPT");

                   if (!(string.IsNullOrEmpty(smtpServer2.Nombre) || string.IsNullOrEmpty(smtpServer2.Puerto.ToString())))
                   {


                       try
                       {
                           SmtpClient clienteSmtp = new SmtpClient(smtpServer2.Nombre, smtpServer2.Puerto);
                           //envia el correo
                           clienteSmtp.Send(MyMail);
                           enviado = true;

                       }
                       catch (Exception e)
                       {
                           enviado = false;

                           ExceptionNotificacionEmail NotificacionException = new ExceptionNotificacionEmail("El Metodo EnviarNotificacion fallo" + " || ", e);
                           throw NotificacionException;
                       }

                   }


                   break;
               #endregion
               default:
                   break;
           }

           return enviado;

       }

       private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
       {
           // Get the unique identifier for this asynchronous operation.
           string token = (string)e.UserState;

           if (e.Cancelled)
           {
               //Console.WriteLine("[{0}] Send canceled.", token);
           }
           if (e.Error != null)
           {
               //Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
           }
           else
           {
               //Console.WriteLine("Message sent.");
           }
           enviado = true;

       }

       public bool EnviaBitacora(List<SucesoInterfaseBuilder> IntefaseLogList)
       {
           bool pass = false;
           NotificacionEmail Notificacion = TraerDatosNotificacion(11);
           Notificacion.TipoNotificacion = EnumTipoNotificacion.Mensajes;
           try
           {


               Notificacion.CuerpoMensaje = string.Format(Notificacion.CuerpoMensaje
                   , ""
                   , ""
                   , ""
                   , "");

               #region arma cuerpo


               StringBuilder detalle = new StringBuilder();

               foreach (SucesoInterfaseBuilder item in EventWriter.IntefaseLogList)
               {
               string FormatRenglon = " <tr> <td class='style1'>   <b>" + item.InformacionSuceso.Event.ToString() + "</b>  </td> <td class='style1'>  <b>" + item.InformacionSuceso.Method.ToString() + "</b></td>  <td class='style1'><b>" + item.InformacionSuceso.Resultado.ToString() + "</b> </td> </tr>";
                   detalle.Append(FormatRenglon);
               }


               string bottomMail = @"</table>   
          </td>                
          </tr>                  
          <tr>
          <td></td>
          </tr>
          <tr><td></td>
          </tr>
          <tr><td><p class='Italic'>'******************************FIN DE MENSAJE***************************'</p></td></tr>
          <tr><td></td></tr>
          <tr><td></td></tr>
          <tr><td></td></tr>
          <tr class='Italic Centrado'><td><b >'Favor de No Responder a este Correo'</b></td></tr></table></body></html>  ";


               #endregion

               Notificacion.CuerpoMensaje = Notificacion.CuerpoMensaje + detalle + bottomMail;
               //Notificacion.CuentaDestino = pinterfaseLocal.EmailResponsable;


               pass = EnviarNotificacion(Notificacion);

            
           }
           catch (Exception ex)
           {
             
           }
           return pass;
       }



       /// <summary>
       /// agrega un nuevo formato de notificacion
       /// </summary>
       /// <param name="pNemail"></param>
       /// <returns></returns>
       public int Agregar(NotificacionEmail pNemail)
       {
           int ID=0;

           try
           {
               NotificacionEmailDA NDA = new NotificacionEmailDA(ConGenral);
               ID = NDA.Agregar(pNemail);
           }
           catch (Exception ex)
           {
               ID = -1;
               ExceptionNotificacionEmail NotificacionException = new ExceptionNotificacionEmail("El Metodo Agregar(NotificacionEmail pNemail) ) fallo" + " || " + ex);
               NotificacionEmail notificacion = new NotificacionEmail("Error en SMEHL", NotificacionException);
               notificacion.TipoNotificacion = EnumTipoNotificacion.Error;
               NotificacionEmailService NS = new NotificacionEmailService(ConGenral);
               NS.EnviarNotificacion(notificacion);
           }
        
               return ID;
       }

       /// <summary>
       /// actualiza un Nuevo Formato de notificacion
       /// </summary>
       /// <param name="pNemail"></param>
       /// <returns></returns>
       public bool Actualizar(NotificacionEmail pNemail)
       {
           bool ID = false;

           try
           {
               NotificacionEmailDA NDA = new NotificacionEmailDA(ConGenral);
               ID = NDA.Actualizar(pNemail);
           }
           catch (Exception ex)
           {

               ExceptionNotificacionEmail NotificacionException = new ExceptionNotificacionEmail("El Metodo Actualizar(NotificacionEmail pNemail) ) fallo" + " || " + ex);
               NotificacionEmail notificacion = new NotificacionEmail("Error en SMEHL", NotificacionException);
               notificacion.TipoNotificacion = EnumTipoNotificacion.Error;
               NotificacionEmailService NS = new NotificacionEmailService(ConGenral);
               NS.EnviarNotificacion(notificacion);
           }
          
           return ID;
       }


       /// <summary>
       /// Regreasa todos los formatos de notificacion.
       /// </summary>
       /// <returns></returns>
       public List<NotificacionEmail> Seleccionar()
       {
           List<NotificacionEmail> MyNotificacion = new List<NotificacionEmail>();
           try
           {
               NotificacionEmailDA NDA = new NotificacionEmailDA(ConGenral);
               MyNotificacion = NDA.Seleccionar();
           }
           catch (Exception ex)
           {
               ExceptionNotificacionEmail NotificacionException = new ExceptionNotificacionEmail("El Metodo Seleccionar() ) fallo" + " || " + ex);
               NotificacionEmail notificacion = new NotificacionEmail("Error en SMEHL", NotificacionException);
               notificacion.TipoNotificacion = EnumTipoNotificacion.Error;
               NotificacionEmailService NS = new NotificacionEmailService(ConGenral);
               NS.EnviarNotificacion(notificacion);
           }
      
           return MyNotificacion;
       }

      
       /// <summary>
       /// metodo utilizado para traer un formato de notificacion deseado.
       /// </summary>
       /// <param name="pIdParametroSistemaDetalle"></param>
       /// <returns></returns>
       public NotificacionEmail TraerDatosNotificacion(int pIdParametroSistemaDetalle)
       {
           try
           {
               NotificacionEmailDA NDA = new NotificacionEmailDA(ConGenral);
               this.MyNotificacionEmail = NDA.TraerDatosNotificacion(pIdParametroSistemaDetalle);
             
           }
           catch (Exception e)
           {
               ExceptionNotificacionEmail NotificacionException = new ExceptionNotificacionEmail("El Metodo TraerDatosNotificacion fallo" + " || " + e);
               NotificacionEmail notificacion = new NotificacionEmail("Error en SMEHL", NotificacionException);
               notificacion.TipoNotificacion = EnumTipoNotificacion.Error;
               NotificacionEmailService NS = new NotificacionEmailService(ConGenral);
               NS.EnviarNotificacion(notificacion);

               throw NotificacionException;
           }

           return this.MyNotificacionEmail;

       }
     

       /// <summary>
       /// Elimina un Formato de notificacion
       /// </summary>
       /// <param name="pIdObjeto"></param>
       /// <returns></returns>
       public bool Eliminar(int pIdObjeto)
       {
           bool ID = false;
           try
           {
               NotificacionEmailDA NDA = new NotificacionEmailDA(ConGenral);
               ID = NDA.Eliminar(pIdObjeto);
           }
           catch (Exception ex)
           {
               ExceptionNotificacionEmail NotificacionException = new ExceptionNotificacionEmail("El Metodo Eliminar(int pIdObjeto) fallo" + " || " + ex);
               NotificacionEmail notificacion = new NotificacionEmail("Error en SMEHL", NotificacionException);
               notificacion.TipoNotificacion = EnumTipoNotificacion.Error;
               NotificacionEmailService NS = new NotificacionEmailService(ConGenral);
               NS.EnviarNotificacion(notificacion);
           }

          
           return ID;
       }

       #endregion

       #region Metodos Propios de El Servicio
       protected bool ValidarCuentaCorreo(string pCorreo)
       {
           throw new NotImplementedException();
       }

       /// <summary>
       /// Trae datos de Servidor SMPT
       /// </summary>
       /// <returns></returns>
       protected SMTPClienteDatos TraerServidorSMPT()
       {
           SMTPClienteDatos smtpRegreso = new SMTPClienteDatos();
           try
           {
               //NotificacionEmailDA NDA = new NotificacionEmailDA(TipoApp);
               //smtpRegreso = NDA.TraerServidorSMPT();

               //ParametrosDA parametros = new ParametrosDA(TipoApp);
               //List<ParametrosDelSistemaDetalle> listaParametros = parametros.SeleccionarGrupoDeParametros(eGruposDeParametros.FormatosDeNotificaciones, eSubGruposDeParametros.SMTP);

               //smtpRegreso.Nombre = (from c in listaParametros
               //                      where c.Clave.Equals("IP")
               //                      select c.Valor).FirstOrDefault().ToString();

               //smtpRegreso.Puerto = Convert.ToInt32((from c in listaParametros
               //                                      where c.Clave.Equals("Puerto")
               //                                      select c.Valor).FirstOrDefault());

               smtpRegreso.Nombre = "relaysmtp.ndc.nna";
               smtpRegreso.Puerto = Convert.ToInt32("25");

           }
           catch (Exception ex)
           {
               ExceptionNotificacionEmail NotificacionException = new ExceptionNotificacionEmail("El Metodo TraerServidorSMPT fallo" + " || " + ex);
               NotificacionEmail notificacion = new NotificacionEmail("Error en SMEHL", NotificacionException);
               NotificacionEmailService NS = new NotificacionEmailService(ConGenral);
               NS.EnviarNotificacion(notificacion);

               throw NotificacionException;
           }

           return smtpRegreso;
       }

       /// <summary>
       /// Adjunta los archivos para enviar
       /// </summary>
       /// <param name="pListaArchivos"></param>
       protected void SetAttachments(List<AttachedFile> pListaArchivos)
       {
           if (pListaArchivos.Count > 0)
           {
               foreach (AttachedFile inArchivo in pListaArchivos)
               {
                   //save the data to a memory stream
                   MemoryStream ms = new MemoryStream(inArchivo.ArraydeArchivo);

                   //create the attachment from a stream. Be sure to name the data with a file and 
                   //media type that is respective of the data
                   Attachment adjunto = new Attachment(ms, inArchivo.TipoArchivo);
                   adjunto.Name = inArchivo.NombreArchivo;
                   MyMail.Attachments.Add(adjunto);
                   ms.Close();

               }

           }
       }

       /// <summary>
       /// Metodo utilizado para mandar un error a el departamento de sistemas
       /// </summary>
       /// <param name="ex"></param>
       /// <param name="pIdParametroSistemaDetalle"></param>
       /// <returns></returns>
       protected NotificacionEmail TraerDatosNotificacionError(NotificacionEmail pNotiex, int pIdParametroSistemaDetalle)
       {

           NotificacionEmail NE = new NotificacionEmail();
           try 
           {
               switch (Tipo)
               {
                   case 0:
                       NotificacionEmailDA NDA = new NotificacionEmailDA(ConGenral);
                       this.MyNotificacionEmail = NDA.TraerDatosNotificacion(pIdParametroSistemaDetalle);
                       this.MyNotificacionEmail.AsuntoMensaje = "Error de Sistema : " +pNotiex.AsuntoMensaje;
                       break;
                   case 1:
                      
                       this.MyNotificacionEmail = new NotificacionEmail(1, "DLOG.Sistema@nissan.com.mx", "punky_casinotomo@hotmail.com", "Error de Sistema : " + pNotiex.AsuntoMensaje
                           ,"<table>  <tbody>  <tr>  <td>Este es un Mensaje Generado Automaticamente <b>DLOG</b></td></tr>  <tr>  <td><br />  </td></tr>  <tr>  <td>Tienes un Nuevo Caso Con las Siguientes Caracteristicas:</td></tr>  <tr>  <td><br />  </td></tr>  <tr>  <td><br />  </td></tr>  <tr>  <td>  <table>  <tbody>  <tr>  <td><b>Mensaje de Excepcion:</b></td>  <td>{0}</td></tr>  <tr>  <td><b>Fuente de Excepcion:</b></td>  <td>{1}</td></tr>  <tr>  <td><b>Excepción Interna:</b></td>  <td>{2}</td></tr>  <tr>  <td><b>Ocurrió: </b></td>  <td>{3}</td></tr>  <tr>  <td><b>Procedimiento que la genero: </b></td>  <td>{4}</td></tr>  <tr>  <td><b>Fecha en que ocurrió:</b></td>  <td>{5}</td></tr>  <tr>  <td><b>Hora en que ocurrió:</b></td>  <td>{6}</td></tr>  <tr>  <td><b>Maquina:</b></td>  <td>{7}</td></tr>  <tr>  <td><b>IP:</b></td>  <td>{8}</td></tr></tbody></table></td></tr>  <tr>  <td><br />  </td></tr>  <tr>  <td><br />  </td></tr>  <tr>  <td>  <p>******************************FIN DE MENSAJE***************************</p></td></tr>  <tr>  <td><br />  </td></tr>  <tr>  <td><br />  </td></tr>  <tr>  <td><br />  </td></tr>  <tr>  <td><b>Favor de No Responder a este Correo</b></td></tr></tbody></table>"
                           ,"Error Sistema");
                       this.MyNotificacionEmail.CopiaPara = new List<string>();
                       this.MyNotificacionEmail.CopiaPara.Add("Andres.Martinez@nissan.com.mx");
                       this.MyNotificacionEmail.CopiaPara.Add("Eduardo.pacheco@nissan.com.mx");
                     break;
                   default:
                       break;
               }
                          
           }
           catch (Exception e)
           {
               throw new ExceptionNotificacionEmail("No Fue posible Traer Los datos para Notificacion de Error" +" || " + e);
           }

          ///Lleno el formato del correo con las variables ya establecidas en el formato de la BD.
           if (pNotiex.Exception != null)
           {
               try
               {
                     string NombreDeMaquina = System.Net.Dns.GetHostName();
                     System.Net.IPAddress[] Ip = System.Net.Dns.GetHostAddresses(NombreDeMaquina);

                   this.MyNotificacionEmail.CuerpoMensaje = string.Format(this.MyNotificacionEmail.CuerpoMensaje
                       , string.IsNullOrEmpty(pNotiex.Exception.Message.ToString()) ? "" : pNotiex.Exception.Message.ToString()
                  , pNotiex.Exception.Source == null ? "" : pNotiex.Exception.Source.ToString()
                  , pNotiex.Exception.InnerException == null ? "" : pNotiex.Exception.InnerException.ToString()
                  , pNotiex.Exception.StackTrace == null ? "" : pNotiex.Exception.StackTrace.ToString()
                  , pNotiex.Exception.TargetSite == null ? "" : pNotiex.Exception.TargetSite.ToString()
                  , System.DateTime.Now.ToShortDateString()
                  , System.DateTime.Now.ToShortTimeString()
                  , NombreDeMaquina,Ip[0].ToString());
               }
               catch (Exception e)
               {                   
                   throw;
               }
           }
           else
           {
               throw new ExceptionParametroNulo("el Parametro no puede ser nulo ," + "Exception ex");
           }                  

           return this.MyNotificacionEmail;

       }


     
       #endregion



     
   }
}
