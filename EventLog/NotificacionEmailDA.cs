using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Data.SqlClient;
using System.Data;

//Desarrollado por:Emmanuel Lohora
//Contacto: punky_casinotomo@hotmail.com


namespace EventLogCatcher
    {
    [Serializable]
   public class NotificacionEmailDA
          {


       static string ConGenral = string.Empty;
       public NotificacionEmailDA()
       {
         
       }
       public NotificacionEmailDA(string ConecctionStringProvided) 
       {
           ConGenral = ConecctionStringProvided;
       }

       #region Declarations
       public static MailMessage MyMail = new MailMessage();
       public NotificacionEmail MyNotificacionEmail = new NotificacionEmail();
       #endregion


       /// <summary>
       /// Actualiza un Formato de notificacion en la BD del catalogo
       /// </summary>
       /// <param name="pNemail">entidad de NotificacionEmail</param>
       public int Agregar(NotificacionEmail pNemail)
       {
           int id = 0;
           using (SqlCommand command = new SqlCommand("uspSmehl_CrearFormatoNotificacion"
                                            , RegresaConnexionstring()))
           {
               command.CommandType = CommandType.StoredProcedure;
               command.Parameters.AddWithValue("@NombreFormato",pNemail.NombreNotificacion);
               command.Parameters.AddWithValue("@Cuerpo", pNemail.CuerpoMensaje);
               command.Parameters.AddWithValue("@De", pNemail.CuentaOrigen);
               command.Parameters.AddWithValue("@Para", pNemail.CuentaDestino);
               command.Parameters.AddWithValue("@Asunto", pNemail.AsuntoMensaje);

               SqlParameter ret = new SqlParameter("@Return_Value", SqlDbType.Int);
               ret.Direction = ParameterDirection.ReturnValue;
               command.Parameters.Add(ret);

               command.Connection.Open();
               command.ExecuteNonQuery();

               id = Convert.ToInt32(ret.Value);
           }


           return id;
       }

       /// <summary>
       /// Actualiza un Formato de notificacion en la BD del catalogo
       /// </summary>
       /// <param name="pNemail">entidad Formato NotificacionEmail</param>
       public bool Actualizar(NotificacionEmail pNemail)
       {
           bool CambioExitoso = false;

           using (SqlCommand command = new SqlCommand("uspSmehl_ActualizaFormatoNotificacion"
                                  , RegresaConnexionstring()))
           {
               command.CommandType = CommandType.StoredProcedure;
               command.Parameters.AddWithValue("@IdFormato", pNemail.IdFormato);
               command.Parameters.AddWithValue("@NombreFormato", pNemail.NombreNotificacion);
               command.Parameters.AddWithValue("@Cuerpo", pNemail.CuerpoMensaje);
               command.Parameters.AddWithValue("@De", pNemail.CuentaOrigen);
               command.Parameters.AddWithValue("@Para", pNemail.CuentaDestino);
               command.Parameters.AddWithValue("@Asunto", pNemail.AsuntoMensaje);

               SqlParameter ret = new SqlParameter("@Return_Value", SqlDbType.Int);
               ret.Direction = ParameterDirection.ReturnValue;
               command.Parameters.Add(ret);

               command.Connection.Open();
               command.ExecuteNonQuery();

               CambioExitoso = Convert.ToBoolean(ret.Value);

           }

           return CambioExitoso;
       }

       /// <summary>
       /// Regresa una Lista de todos los Formatos de Notificacion
       /// </summary>
       public List<NotificacionEmail> Seleccionar()
       {
           List<NotificacionEmail> lista = new List<NotificacionEmail>();
           using (SqlCommand command = new SqlCommand("uspSmehl_SeleccionarFormatosNorificacion"
                                   , RegresaConnexionstring()))
           {
               SqlDataReader Dr;


               command.CommandType = CommandType.StoredProcedure;

               command.Connection.Open();

               Dr = command.ExecuteReader();

               while (Dr.Read())
               {
                   lista.Add(new NotificacionEmail(
                       Convert.ToInt32(Dr["idFormato"]), Convert.ToString(Dr["De"]), Convert.ToString(Dr["Para"]), Convert.ToString(Dr["Asunto"]), Convert.ToString(Dr["Cuerpo"]), Convert.ToString(Dr["NombreFormato"])));

               }
           }

           return lista;
       }

       /// <summary>
       /// Trae el Regristo de la BD de un Formato de Notificacion
       /// </summary>
       /// <param name="pIdFormato">id Formato de Notificacion</param>
       public NotificacionEmail TraerDatosNotificacion(int pIdFormato)
       {

                   using (SqlCommand cm = new SqlCommand("uspSeleccionarFormatoCorreo", RegresaConnexionstring()))
                   {

                       cm.CommandType = CommandType.StoredProcedure;

                       cm.Parameters.Add("@idFormato", SqlDbType.Int).Value = pIdFormato;

                       cm.Connection.Open();
                       SqlDataReader rd = cm.ExecuteReader();

                       while (rd.Read())
                       {
                           NotificacionEmail Nemail = new NotificacionEmail(pIdFormato, rd["De"].ToString(), rd["Para"].ToString(), rd["Asunto"].ToString(), rd["Cuerpo"].ToString(), rd["NombreFormato"].ToString());
                           this.MyNotificacionEmail = Nemail;
                       }

                   }        
         

            return this.MyNotificacionEmail;
       }
       /// <summary>
       /// Actualiza un Formato de notificacion en la BD del catalogo
       /// </summary>
       /// <param name="pIdObjeto">id Formato de Notificacion</param>
       public bool Eliminar(int pIdObjeto)
       {
           bool CambioExitoso = false;

           using (SqlCommand command = new SqlCommand("uspSmehl_DeleteFormatoNotificaciones"
                                  , RegresaConnexionstring()))
           {

               command.CommandType = CommandType.StoredProcedure;
               command.Parameters.AddWithValue("@IdFormato",pIdObjeto);

               SqlParameter ret = new SqlParameter("@Return_Value", SqlDbType.Int);
               ret.Direction = ParameterDirection.ReturnValue;
               command.Parameters.Add(ret);

               command.Connection.Open();
               command.ExecuteNonQuery();

               CambioExitoso = Convert.ToBoolean(ret.Value);

           }

           return CambioExitoso;
       }


       #region INotificacionEmailService Members


       /// <summary>
       /// Regresa los datos del Servidor SMPT
       /// </summary>
       public SMTPClienteDatos TraerServidorSMPT()
       {
           SMTPClienteDatos smtpRegreso = new SMTPClienteDatos(0, "");
      
                   using (SqlCommand cm = new SqlCommand("sp_Smehl_TraeSMPTinfo", RegresaConnexionstring()))
                   {

                       cm.CommandType = CommandType.StoredProcedure;

                       //cm.Parameters.Add("@IP", SqlDbType.NVarChar).Value = "pIP";
                       //cm.Parameters.Add("@Puerto", SqlDbType.NVarChar).Value = "pPuerto";

                       cm.Connection.Open();
                       SqlDataReader rd = cm.ExecuteReader();

                       while (rd.Read())
                       {
                           smtpRegreso = new SMTPClienteDatos(Convert.ToInt32(rd["Puerto"].ToString()), rd["IP"].ToString());

                       }
                   }

           return smtpRegreso;
       }    

      

    
       #endregion

       private static SqlConnection RegresaConnexionstring()
       {
           if (string.IsNullOrEmpty(ConGenral))
           {
               return DataContextFactory.GetSqlConnectionFactory();
           }
           else
           {
               return DataContextFactory.SetSqlConnectionFactory(ConGenral);
           }

       }
   }
}
