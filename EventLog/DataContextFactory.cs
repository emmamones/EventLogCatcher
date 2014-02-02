using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;

//Desarrollado por:Emmanuel Lohora
//Contacto: punky_casinotomo@hotmail.com

namespace EventLogCatcher
{  
        /// <summary>
        /// This class provides several static methods for loading dataContext objects 
        /// in a variety of ways. You can load the data context as normal one new instance
        /// at a time, or you can choose to use one of the scoped factory methods that
        /// can scope the dataContext to a WebRequest or a Thread context (in a WinForm app
        /// for example).
        /// 
        /// Using scoped variants can be more efficient in some scenarios and allows passing
        /// a dataContext across multiple otherwise unrelated components so that the change
        /// context can be shared. 
        /// </summary>
        [Serializable]
        internal static class DataContextFactory
        {
            //public static PPSDataContext GetPPSContext()
            //{
            //    return GetThreadScopeddataContext<PPSDataContext>("PROD", ConnnectionStringFactory(CinaContextType.Produccion));
            //}

            /// <summary>
            /// Regresa la cadena de conexion del app.config
            /// </summary>
            /// <returns></returns>
            public static SqlConnection GetSqlConnectionFactory()
            {
                return ConnnectionStringFactory();
            }
            /// <summary>
            /// Introduce la cadena de conexion
            /// </summary>
            /// <param name="con"></param>
            /// <returns></returns>
            public static SqlConnection SetSqlConnectionFactory(string con)
            {
                return ConnnectionStringProvided(con);
            }
            private static SqlConnection ConnnectionStringFactory()
            {
                //var connStr = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["dataAccessMode"]].ConnectionString;
                //string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                string connStr = string.Empty;
                string DataSource = ConfigurationManager.AppSettings["DataSource"].ToString();
                string Catalog = ConfigurationManager.AppSettings["Catalog"].ToString();
                string User = ConfigurationManager.AppSettings["User"].ToString();
                string Password = ConfigurationManager.AppSettings["Password"].ToString();
                connStr = (string.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}", DataSource, Catalog, User, Password));
                SqlConnection sqlcon = new SqlConnection(connStr);
                return sqlcon;
            }

            private static SqlConnection ConnnectionStringProvided(string con)
            {
                SqlConnection sqlcon = new SqlConnection(con);
                return sqlcon;
            }

            internal static SqlConnection GetSqlConnectionNoConfig()
            {
                throw new NotImplementedException();
            }
        }

        enum ConnectionContextType
        {
            Produccion,
            Desarrollo,
            Pruebas
        }
    }

