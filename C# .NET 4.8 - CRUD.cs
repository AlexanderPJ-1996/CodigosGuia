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
            ------------------------------------------------------------------------------------------------------------------------
            SQL Server LocalDB:
            Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename=|DataDirectory|\[Database].mdf; Integrated Security=True
            ------------------------------------------------------------------------------------------------------------------------
            MySQL:
            Server=[ServerAddress]; Database=[Database]; Uid=[User]; Pwd=[Password]; -- Estandar
            Server=[ServerAddress]; Port=[#Port000]; Database=[Database]; Uid=[User]; Pwd=[Password]; -- Especificar n° puerto TCP
            Server=[ServerAddress], [ServerAddress], [ServerAddress]; Database=[Database]; Uid=[User]; Pwd=[Password; -- Servidores múltiples
            ------------------------------------------------------------------------------------------------------------------------
            SQLite:
            Data Source=(Ubicación)\[Database].db; Version=3;
            ------------------------------------------------------------------------------------------------------------------------
            Access 2003:
            Provider=Microsoft.Jet.OLEDB.4.0; Data Source=(Ubicación)\[Database].mdb
            Provider=Microsoft.Jet.OLEDB.4.0; Data Source=(Ubicación)\[Database].mdb; Jet OLEDB:Database Password=[Password]
            ------------------------------------------------------------------------------------------------------------------------
            Access 2007-2013:
            Provider=Microsoft.ACE.OLEDB.12.0; Data Source=(Ubicación)\[Database].acCRUD
            Provider=Microsoft.ACE.OLEDB.12.0; Data Source=(Ubicación)\[Database].acCRUD; Jet OLEDB:Database Password=[Password]
            ------------------------------------------------------------------------------------------------------------------------
        */

        #region Variables generales
        // Iniciar la conexión y almacenar la cadena de conexión (ConnectionString).
        // C# no admite la opción de usar variables para almacenar ConnectionString con el metodo actual.
        private readonly static SqlConnection Conexion = new SqlConnection(@"[ConnectionString]");

        // Se puede utilizar este método para solo declarar la cadena/connectionStrings, pero en tal caso, deberá de declararse: using (var Conexion = new SqlConnection(CoString)) {};
        private readonly static string CoString = @"[ConnectionString]";

        // Variable para almacenar si los procedimientos: UPDATE y DELETE, se realizaron con exito o no.
        public static Boolean Done;
        // Variables para almacenar Exception: ex.Message 
        public static String ExMess, XTitle;
        #endregion

        #region Conectar con base de datos
        // Metodo que retorna una variable boolean para iniciar y probar conexión con base de datos.
        public static Boolean TryCon()
        {
            Boolean Output = new Boolean();
            while (Output == false)
            {
                using (var Conexion = new SqlConnection(CoString))
                {
                    try
                    {
                        Conexion.Open();
                        Output = true;
                        Conexion.Close();
                    }
                    catch (Exception ex)
                    {
                        Output = false;
                        if (MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.RetryCancel) == DialogResult.Cancel)
                        {
                            Environment.Exit(0);
                        }
                    }
                }
            }
            return Output;
        }
        #endregion

        #region Procedimientos SELECT
        // Metodo que retorna una variable boolean para verificar la existencia de un archivo en base a condicionales WHERE.
        public static Boolean DataEx(String Tabla, String Columna, String Verificar)
        {
            Boolean Output = new Boolean();
            using (var Conexion = new SqlConnection(CoString))
            {
                Conexion.Open();
                try
                {
                    String SQL = "SELECT * FROM " + Tabla + " WHERE (" + Columna + " = @Verificar)";
                    using (SqlCommand CMD = new SqlCommand(SQL, Conexion))
                    {
                        CMD.Parameters.AddWithValue("@Verificar", Verificar);
                        using (SqlDataReader DR = CMD.ExecuteReader())
                        {
                            if (DR.HasRows)
                            {
                                Output = true;
                            }
                            else
                            {
                                Output = false;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Output = false;
                    MessageBox.Show(ex.Message);
                }
                Conexion.Close();
            }
            return Output;
        }

        // Procedimiento para retornar un único valor como variable en base a condicionales WHERE.
        public static String Texto(String SQL)
        {
            String Output = String.Empty;
            using (var Conexion = new SqlConnection(CoString))
            {
                Conexion.Open();
                try
                {
                    using (SqlCommand CMD = new SqlCommand(SQL, Conexion))
                    {
                        var Result = CMD.ExecuteScalar();
                        if (Result != null)
                        {
                            Output = Result.ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Output = String.Empty;
                    MessageBox.Show(ex.Message);
                }
                Conexion.Close();
            }
            return Output;
        }
        #endregion

        #region Procedimientos SELECT: List<>
        // Procedimiento para retornar listado de registros almacenados en una tabla para usar como fuente para DataSource o BindingSource.
        public static List<ClaseTabla> CargarDatos()
        {
            List<ClaseTabla> Output = new List<ClaseTabla>();
            using (var Conexion = new SqlConnection(CoString))
            {
                Conexion.Open();
                try
                {
                    String SQL = "SELECT * FROM [Tabla]";
                    using (SqlCommand CMD = new SqlCommand(SQL, Conexion))
                    {
                        CMD.CommandType = CommandType.Text;
                        using (SqlDataReader DR = CMD.ExecuteReader())
                        {
                            while (DR.Read())
                            {
                                Output.Add(new ClaseTabla()
                                {
                                    // Acá el nombre de la columna dentro de DR[""] y la variable deberá ser exactamente iguales.
                                    Columna1 = Convert.ToInt64(DR["Columna1"]),
                                    Columna2 = DR["Columna2"].ToString(),
                                    Columna3 = Convert.ToDateTime(DR["Columna3"]),
                                    Columna4 = Convert.ToBoolean(DR["Columna4"]),
                                    Columna5 = Convert.ToDecimal(DR["Columna5"])
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Output = new List<ClaseTabla>();
                    MessageBox.Show(ex.Message);
                }
                Conexion.Close();
            }
            return Output;
        }

        /*
            Procedimiento para retornar un List>String> con de datos de una columna en base a condicionales WHERE
            La variable: String SQL deberá ser del estilo: SELECT [Columna] FROM [Tabla] WHERE ([Columna]=@Columna) ORDER BY [Columna] ASC/DESC.
        */
        public static List<String> CargarLista(String SQL, String Columna)
        {
            List<String> Output = new List<String>();
            using (var Conexion = new SqlConnection(CoString))
            {
                Conexion.Open();
                try
                {
                    using (SqlCommand CMD = new SqlCommand(SQL, Conexion))
                    {
                        CMD.CommandType = CommandType.Text;
                        using (SqlDataReader DR = CMD.ExecuteReader())
                        {
                            while (DR.Read())
                            {
                                Output.Add(DR[Columna].ToString());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Output = new List<String>();
                }
                Conexion.Close();
            }
            return Output;
        }
        #endregion

        #region Procedimientos que no retornan datos
        // Procedimiento para registrar datos en una tabla.
        public static void InseRegs(Int64 Columna1, String Columna2, String Columna3, String Columna4, String Columna5)
        {
            using (var Conexion = new SqlConnection(CoString))
            {
                Conexion.Open();
                try
                {
                    String SQL = "INSERT INTO [Tabla] (Columna1, Columna2, Columna3, Columna4, Columna5) VALUES (@Columna1, @Columna2, @Columna3, @Columna4, @Columna5)";
                    using (SqlCommand CMD = new SqlCommand(SQL, Conexion))
                    {
                        CMD.Parameters.AddWithValue("@Columna1", Columna1);
                        CMD.Parameters.AddWithValue("@Columna2", Columna2);
                        CMD.Parameters.AddWithValue("@Columna3", Columna3);
                        CMD.Parameters.AddWithValue("@Columna4", Columna4);
                        CMD.Parameters.AddWithValue("@Columna5", Columna5);
                        CMD.ExecuteNonQuery();
                        Done = true;
                    }
                }
                catch (Exception ex)
                {
                    Done = false;
                    MessageBox.Show(ex.Message);
                }
                Conexion.Close();
            }
        }

        // Procedimiento para editar datos existentes de una tabla en base a condicionales WHERE.
        public static void UpdaRegs(Int64 Columna1, String Columna2, String Columna3, String Columna4, String Columna5, Objetc ColumnaX)
        {
            using (var Conexion = new SqlConnection(CoString))
            {
                Conexion.Open();
                try
                {
                    String SQL = "UPDATE [Tabla] SET " +
                    "Columna1 = @Columna1, " +
                    "Columna2 = @Columna2, " +
                    "Columna3 = @Columna3, " +
                    "Columna4 = @Columna4, " +
                    "Columna5 = @Columna5 " +
                    "WHERE (ColumnaX = @ColumnaX)";
                    using (SqlCommand CMD = new SqlCommand(SQL, Conexion))
                    {
                        CMD.Parameters.AddWithValue("@Columna1", Columna1);
                        CMD.Parameters.AddWithValue("@Columna2", Columna2);
                        CMD.Parameters.AddWithValue("@Columna3", Columna3);
                        CMD.Parameters.AddWithValue("@Columna4", Columna4);
                        CMD.Parameters.AddWithValue("@Columna5", Columna5);
                        CMD.Parameters.AddWithValue("@ColumnaX", ColumnaX);
                        CMD.ExecuteNonQuery();
                        Done = true;
                    }
                }
                catch (Exception ex)
                {
                    Done = false;
                    MessageBox.Show(ex.Message);
                }
                Conexion.Close();
            }
        }

        // Procedimiento para eliminar datos existentes de una tabla en base a condicionales WHERE.
        public static void DeleRegs(String Tabla, Objetc ColumnaX)
        {
            using (var Conexion = new SqlConnection(CoString))
            {
                Conexion.Open();
                try
                {
                    String SQL = "DELETE FROM " + Tabla + " WHERE (ColumnaX = @ColumnaX)";
                    using (SqlCommand CMD = new SqlCommand(SQL, Conexion))
                    {
                        CMD.Parameters.AddWithValue("@ColumnaX", ColumnaX);
                        CMD.ExecuteNonQuery();
                        Done = true;
                    }
                }
                catch (Exception ex)
                {
                    Done = false;
                    MessageBox.Show(ex.Message);
                }
                Conexion.Close();
            }
        }

        // Procedimiento para registrar datos en una tablas desde un archivo .csv UTF-8 Uniconde (separado por comas).
        public static void Importar()
        {
            String Ruta = @"[Ruta de archivo].csv";
            if (File.Exists(Ruta))
            {
                try
                {
                    using (var Read = new StreamReader(Ruta))
                    {
                        String Head = Read.ReadLine();
                        using (var Conexion = new MySqlConnection(CoString))
                        {
                            Conexion.Open();
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
                            Conexion.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // Recuperar texto de los archivos .sql y ejecutar consultas sql desde archivos
        public static void ExecProc()
        {
            using (var Conexion = new MySqlConnection(CoString))
            {
                Conexion.Open();
                try
                {
                    String Ruta = @"[Ruta de archivo].sql";
                    String SQL = File.ReadAllText(Ruta);
                    using (MySqlCommand CMD = new MySqlCommand(SQL, Conexion))
                    {
                        CMD.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                Conexion.Close();
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