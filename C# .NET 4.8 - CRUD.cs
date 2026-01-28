#region Librerías
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using MySqlConnector;
using System.Data.SQLite;
using System.Data.OleDb;
#endregion

namespace CodigosGuia
{
    public static class CRUD
    {
        /*  ConnectionString: (https://www.connectionStrings.com/)
            ----------------------------------------------------------------------------------------------------
            
			SQL Server LocalDB:
            Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename=|DataDirectory|\[Database].mdf; Integrated Security=True
            
			----------------------------------------------------------------------------------------------------
            
			MySQL:
            Server=[ServerAddress]; Database=[Database]; Uid=[User]; Pwd=[Password]; -- Estandar
            Server=[ServerAddress]; Port=[#Port000]; Database=[Database]; Uid=[User]; Pwd=[Password]; -- Especificar n° puerto TCP
            Server=[ServerAddress], [ServerAddress], [ServerAddress]; Database=[Database]; Uid=[User]; Pwd=[Password; -- Servidores múltiples
            
			----------------------------------------------------------------------------------------------------
            
			SQLite:
            Data Source=(Ubicación)\[Database].db; Version=3;
            
			----------------------------------------------------------------------------------------------------
            Access 2003:
            Provider=Microsoft.Jet.OLEDB.4.0; Data Source=(Ubicación)\[Database].mdb
            Provider=Microsoft.Jet.OLEDB.4.0; Data Source=(Ubicación)\[Database].mdb; Jet OLEDB:Database Password=[Password]
			
            ----------------------------------------------------------------------------------------------------
            
			Access 2007-2013:
            Provider=Microsoft.ACE.OLEDB.12.0; Data Source=(Ubicación)\[Database].acCRUD
            Provider=Microsoft.ACE.OLEDB.12.0; Data Source=(Ubicación)\[Database].acCRUD; Jet OLEDB:Database Password=[Password]
			
            ----------------------------------------------------------------------------------------------------
        */
		
        #region Variables generales
        // Se puede utilizar este método para solo declarar la cadena/connectionStrings, pero en tal caso, deberá de declararse: using (var Conn = new SqlConnection(Cadena)){};
        private readonly static string Cadena = @"[ConnectionString]";
        #endregion
		
        #region Conectar con base de datos
        // Metodo que retorna una variable boolean para iniciar y probar conexión con base de datos.
        public static Boolean TryCon()
        {
            Boolean Salida = new Boolean();
			try
			{
				using (SqlConnection Conn = new SqlConnection(Cadena))
				{
					Conn.Open();
					Salida = true;
					Conn.Close();
				}
			}
			catch (Exception ex)
			{
				Salida = false;
				MessageBox.Show(ex.Message);
			}
            return Salida;
        }
        #endregion
		
        #region Procedimientos SELECT
        // Retornar variable boolean para verificar existencia de un registro en base de datos
        public static Boolean DataEx(String Tabla, String Columna, String Verificar)
        {
            Boolean Salida = new Boolean();
			try
			{
				String SQL = 
					"SELECT * FROM " + Tabla + " WHERE (" + Columna + " = @Verificar)";
				
				using (SqlConnection Conn = new SqlConnection(Cadena))
				{
					Conn.Open();
					using (SqlCommand CMD = new SqlCommand(SQL, Conn))
					{
						CMD.Parameters.AddWithValue("@Verificar", Verificar);
						
						using (SqlDataReader DR = CMD.ExecuteReader())
						{
							if (DR.HasRows)
							{
								Salida = true;
							}
							else
							{
								Salida = false;
							}
						}
					}
					Conn.Close();
				}
			}
			catch (Exception ex)
			{
				Salida = new Boolean();
				MessageBox.Show(ex.Message);
			}
            return Salida;
        }
		
        // Retornar un único valor como variable
        public static String Texto(String Tabla, String Columna, String Verificar)
        {
            String Salida = String.Empty;
			try
			{
				String SQL = 
					"SELECT * FROM " + Tabla + " WHERE (" + Columna + " = @Verificar)";
				
				using (SqlConnection Conn = new SqlConnection(Cadena))
				{
					Conn.Open();
					using (SqlCommand CMD = new SqlCommand(SQL, Conn))
					{
						CMD.Parameters.AddWithValue("@Verificar", Verificar);
						var Result = CMD.ExecuteScalar();
						
						if (Result != null)
						{
							Salida = Result.ToString();
						}
					}
					Conn.Close();
				}
			}
			catch (Exception ex)
			{
				Salida = String.Empty;
				MessageBox.Show(ex.Message);
			}
			return Salida;
        }
        #endregion
		
        #region Procedimientos SELECT: List<>
        // Retornar listado de registros almacenados en una tabla para usar como fuente para DataSource o BindingSource.
        public static List<ClaseTabla> CargarDatos()
        {
            List<ClaseTabla> Salida = new List<ClaseTabla>();
			try
			{
				String SQL = 
					"SELECT * FROM [Tabla]";
				
				using (SqlConnection Conn = new SqlConnection(Cadena))
				{
					Conn.Open();
					using (SqlCommand CMD = new SqlCommand(SQL, Conn))
					{
						CMD.CommandType = CommandType.Text;
						
						using (SqlDataReader DR = CMD.ExecuteReader())
						{
							while (DR.Read())
							{
								Salida.Add(new ClaseTabla()
								{
									// Nombre de la columna dentro de DR[""] y la variable deberá ser exactamente iguales
									Columna1 = Convert.ToInt64(DR["Columna1"]),
									Columna2 = DR["Columna2"].ToString(),
									Columna3 = Convert.ToDateTime(DR["Columna3"]),
									Columna4 = Convert.ToBoolean(DR["Columna4"]),
									Columna5 = Convert.ToDecimal(DR["Columna5"])
								});
							}
						}
					}
					Conn.Close();
				}
			}
			catch (Exception ex)
			{
				Salida = new List<ClaseTabla>();
				MessageBox.Show(ex.Message);
			}
            return Salida;
        }
		
        // Retornar un List<String> con de datos de una columna en base de datos
        public static List<String> CargarLista(String Tabla, String Columna, String Verificar)
        {
            List<String> Salida = new List<String>();
			try
			{
				String SQL = 
					"SELECT * FROM " + Tabla + " WHERE (" + Columna + " = @Verificar)";
				
				using (SqlConnection Conn = new SqlConnection(Cadena))
				{
					Conn.Open();
					using (SqlCommand CMD = new SqlCommand(SQL, Conn))
					{
						CMD.Parameters.AddWithValue("@Verificar", Verificar);
						CMD.CommandType = CommandType.Text;
						
						using (SqlDataReader DR = CMD.ExecuteReader())
						{
							while (DR.Read())
							{
								Salida.Add(DR[Columna].ToString());
							}
						}
					}
					Conn.Close();
				}
			}
			catch (Exception ex)
			{
				Salida = new List<String>();
			}
			return Salida;
        }
        #endregion
		
        #region Procedimientos que no retornan datos
        // Registrar datos en una tabla
        public static void InseRegs(Int64 Columna1, String Columna2, String Columna3, String Columna4, String Columna5)
        {
            try
			{
				String SQL = 
					"INSERT INTO [Tabla] (Columna1, Columna2, Columna3, Columna4, Columna5) VALUES (@Columna1, @Columna2, @Columna3, @Columna4, @Columna5)";
				
				using (SqlConnection Conn = new SqlConnection(Cadena))
				{
					Conn.Open();
					using (SqlCommand CMD = new SqlCommand(SQL, Conn))
					{
						CMD.Parameters.AddWithValue("@Columna1", Columna1);
						CMD.Parameters.AddWithValue("@Columna2", Columna2);
						CMD.Parameters.AddWithValue("@Columna3", Columna3);
						CMD.Parameters.AddWithValue("@Columna4", Columna4);
						CMD.Parameters.AddWithValue("@Columna5", Columna5);
					}
					Conn.Close();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
        }
		
        // Editar datos existentes de una tabla
        public static void UpdaRegs(Int64 Columna1, String Columna2, String Columna3, String Columna4, String Columna5, Objetc ColumnaX)
        {
            try
			{
				String SQL = 
					"UPDATE [Tabla] SET " + 
					"Columna1 = @Columna1, " + 
					"Columna2 = @Columna2, " + 
					"Columna3 = @Columna3, " + 
					"Columna4 = @Columna4, " + 
					"Columna5 = @Columna5 " + 
					"WHERE (ColumnaX = @ColumnaX)";
				
				using (SqlConnection Conn = new SqlConnection(Cadena))
				{
					Conn.Open();
					using (SqlCommand CMD = new SqlCommand(SQL, Conn))
					{
						CMD.Parameters.AddWithValue("@Columna1", Columna1);
						CMD.Parameters.AddWithValue("@Columna2", Columna2);
						CMD.Parameters.AddWithValue("@Columna3", Columna3);
						CMD.Parameters.AddWithValue("@Columna4", Columna4);
						CMD.Parameters.AddWithValue("@Columna5", Columna5);
						CMD.Parameters.AddWithValue("@ColumnaX", ColumnaX);
						CMD.ExecuteNonQuery();
					}
					Conn.Close();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
        }
		
        // Eliminar datos existentes de una tabla
        public static void DeleRegs(String Tabla, Int64 ID)
        {
            try
			{
				String SQL = 
					"DELETE FROM " + Tabla + " WHERE (ID = @ID)";
				
				using (SqlConnection Conn = new SqlConnection(Cadena))
				{
					Conn.Open();
					using (SqlCommand CMD = new SqlCommand(SQL, Conn))
					{
						CMD.Parameters.AddWithValue("@ID", ID);
						CMD.ExecuteNonQuery();
					}
					Conn.Close();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
        }
		
        // Procedimiento para registrar datos en una tablas desde un archivo .csv UTF-8 Uniconde (separado por comas).
        public static void Importar()
        {
            try
			{
				String Ruta = 
					@"[Ruta de archivo].csv";
				
				if (File.Exists(Ruta))
				{
					using (StreamReader Read = new StreamReader(Ruta))
					{
						String Head = Read.ReadLine();
						
						using (SqlConnection Conn = new MySqlConnection(Cadena))
						{
							Conn.Open();
							while (!Read.EndOfStream)
							{
								String Line = Read.ReadLine();
								String[] Data = Line.Split(';');
								
								Int64 Columna1 = Convert.ToInt64(Data[0]);
								String Columna2 = Data[1];
								String Columna3 = Data[2];
								Boolean Columna4 = Convert.ToBoolean(Data[3]);
								Decimal Columna5 = Convert.ToDecimal(Data[4]);
								
								InseRegs(Columna1, Columna2, Columna3, Columna4, Columna5);
							}
							Conn.Close();
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
        }
		
        // Recuperar texto de los archivos .sql y ejecutar consultas sql desde archivos
        public static void ExecProc()
        {
            try
			{
				String Ruta = 
					@"[Ruta de archivo].sql";
				
				if (File.Exists(Ruta))
				{
					String SQL = File.ReadAllText(Ruta);
					
					using (SqlConnection Conn = new MySqlConnection(Cadena))
					{
						Conn.Open();
						using (SqlCommand CMD = new SqlCommand(SQL, Conn))
						{
							CMD.ExecuteNonQuery();
						}
						Conn.Close();
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
        }
        #endregion
    }
	
    #region Clases y funciones complementarias
    // Estructura base para las "ClaseTabla" para los procedimientos: CargarDatos.
    public class ClaseTabla
    {
        public Int64 Columna1 { get; set; }
        public String Columna2 { get; set; }
        public DateTime Columna3 { get; set; }
        public Boolean Columna4 { get; set; }
        public Decimal Columna5 { get; set; }
    }
    #endregion
}