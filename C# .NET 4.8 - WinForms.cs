#region Librerías
// Manejo de archivos y carpetas/directorios
using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
// Exportar datos desde DataGridView a archivo de Excel (.xlsx)
using System.Windows.Forms;
using Excel=Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
// Exportar datos desde DataGridView a archivo de Excel (.xlsx): Alternativa ClosedXML.Excel
using System;
using System.Data;
using System.Windows.Forms;
using ClosedXML.Excel;
// Encriptar y desencriptar texto con Base64 (C#)
using System;
using System.Text;
// Encriptar y desencriptar texto con el algoritmo AES (Advanced Encryption Standard) (C#)
using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;
// Encriptar texto con métodos MD5/SHA-1/SHA-256/SHA-512 con C#
using System.Text;
using System.Security.Cryptography;
// Copiar texto al portapapeles
using System;
using System.Windows;
// Eliminar espacios en blanco de un string
using System.Text;
using System.Text.RegularExpressions;
// Dar formato moneda al texto de un Label/TextBox
using System.Globalization;
// Captar texto como hipervínculo
using System;
using System.Windows.Forms;
using System.Diagnostics;
// Crear un List<string> con items en negrita(blod) y mostrarlos en un RichTextBox
using System;
using System.Windows.Forms;
using System.Collections.Generic;
// Procedimiento para permitir una única instancia de un proyecto Windows Forms .NET Framework
using System;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
#endregion

#region Manejo de archivos y carpetas/directorios
namespace ArchviosYCarpetas
{
    public static class Carpetas
    {
        // Captar directorio desde donde se ejecuta la aplicación
        public static String PathBase = AppDomain.CurrentDomain.BaseDirectory;

        // Crear/eliminar carpetas y subcarpetas
        public static void Directorios()
        {
            // Crear carpeta/directorio que se desea crear
            String NewPath1 = Path.Combine(PathBase, @"[Directorio]");
            if (!Directory.Exists(NewPath1))
            {
                Directory.CreateDirectory(NewPath1);
            }

            // Crear subcarpeta/subdirectorio
            String NewPath2 = Path.Combine(PathBase, NewPath1, @"[Subdirectorio]");
            if (!Directory.Exists(NewPath2))
            {
                Directory.CreateDirectory(NewPath2);
            }

            // Eliminar carpeta/directorio
            if (Directory.Exists(NewPath1))
            {
                Directory.Delete(NewPath1);
                // Usar esta linea si la carpeta/directorio posee archivos y otras carpetas en su interior
                Directory.Delete(NewPath1, recursive: true);
            }
        }

        // Crear/eliminar archivos
        public static void Archivos(String Extension)
        {
            // Ruta donde se creará/ubicará el archivo
            String NewFile = Path.Combine(PathBase, @"[Archivo]" + "." + Extension);

            // Hacer si un archivo existe
            if (File.Exists(NewFile))
            {
                // Borrar archivo
                File.Delete(NewFile);
            }

            // Hacer si archivo no existe
            if (!File.Exists(NewFile))
            {
            }
        }
    }

    // Manejo para leer texto almacenado en archivos .txt
    public static class ArchivosTexto
    {
        // Leer archivos de texto de una única línea y retornar contenido como variable
        public static String Texto()
        {
            String Output = String.Empty;
            String Ruta = @"[Archivo].txt";
            if (File.Exists(Ruta))
            {
                using (StreamReader SR = new StreamReader(Ruta))
                {
                    try
                    {
                        String Line = SR.ReadLine();
                        while (Line != null)
                        {
                            Output = Line;
                            Line = SR.ReadLine();
                        }
                    }
                    catch (Exception)
                    {
                        Output = String.Empty;
                    }
                }
            }
            return Output;
        }

        // Leer archivos de texto multi-línea y retornar contenido como variable
        public static String Texto()
        {
            String Output = String.Empty;
            String Ruta = @"[Archivo].txt";
            if (File.Exists(Ruta))
            {
                try
                {
                    Output = File.ReadAllText(Ruta);
                }
                catch (Exception)
                {
                    Output = String.Empty;
                }
            }
            return Output;
        }
    }

    // Abrir/guardar archivos
    public static class OpenSaveFile
    {
        // OpenFileDialog para cargar una imagen en un PictureBox
        public static void OpenFile(PictureBox PBx)
        {
            // Configurar OpenFileDialog
            OpenFileDialog OFile = new OpenFileDialog
            {
                Filter = "Imagenes PNG|*.png|" +
                "Imagenes JPG/JPEG|*.jpg;*.jpeg|" +
                "Imagenes BMP|*.bmp|" +
                "Imagenes GIF|*.gif|" +
                "Todos los archivos|*.*",
                Title = "Seleccionar imagen",
                FilterIndex = 1,
                RestoreDirectory = true
            };
            // Abrir OpenFileDialog, y cargar imagen en PictureBox
            if (OFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (FileStream FS = new FileStream(OFile.FileName, FileMode.Open, FileAccess.Read))
                    {
                        using (MemoryStream MS = new MemoryStream())
                        {
                            FS.CopyTo(MS);
                            MS.Seek(0, SeekOrigin.Begin);
                            PBxFoto.Image = Image.FromStream(MS);
                        }
                    }
                }
                catch (Exception ex)
                {
                    PBx.Image = null;
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // SaveFileDialog para guardar imagen desde un PictureBox
        public static void SaveFile(PictureBox PBx)
        {
            // Configurar SaveFileDialog
            SaveFileDialog SaveDialog = new SaveFileDialog
            {
                Filter = "Imagenes PNG|*.png|" +
                "Imagenes JPG/JPEG|*.jpg;*.jpeg|" +
                "Imagenes BMP|*.bmp|" +
                "Imagenes GIF|*.gif|" +
                "Todos los archivos|*.*",
                Title = "Guardar imagen",
                FilterIndex = 1,
                RestoreDirectory = true
            };
            // Abrir SaveFileDialog y guardar archivo
            if (SaveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Image Img = PBx.Image;
                    Img.Save(SaveDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
#endregion

#region Exportar datos desde DataGridView a archivo de Excel (.xlsx)
// Agregar referencia COM Microsoft Excel xx.x Object Library
namespace ExportarDatos
{
    public static void ExprtDGV(DataGridView DGV)
    {
        try
        {
            Excel.Application ExcelApp = new Excel.Application();
            Excel.Workbook WorkBook = ExcelApp.Workbooks.Add(Type.Missing);
            Excel.Worksheet WorkSheet = WorkBook.Sheets[1];
            WorkSheet = WorkBook.ActiveSheet;
            WorkSheet.Name = "Datos Exportados";
            // Exportar los encabezados
            for (int i = 1; i < DGV.Columns.Count + 1; i++)
            {
                WorkSheet.Cells[1, i] = DGV.Columns[i - 1].HeaderText;
            }
            // Exportar las filas
            for (int i = 0; i < DGV.Rows.Count; i++)
            {
                for (int j = 0; j < DGV.Columns.Count; j++)
                {
                    Object Valor = DGV.Rows[i].Cells[j].Value;
                    WorkSheet.Cells[i + 2, j + 1] = Valor?.ToString() ?? String.Empty;
                }
            }
            // Guardar el archivo
            SaveFileDialog SaveDialog = new SaveFileDialog
            {
                Filter = "Archivos de Excel|*.xlsx",
                Title = "Guardar archivo de Excel",
                FileName = "Datos Exportados"
                // Esta última línea [FileName] puede ser removida, ya que solo es para dar un nombre automático al archivo que se desa guardar
            };
            if (SaveDialog.ShowDialog() == DialogResult.OK)
            {
                WorkBook.SaveAs(SaveDialog.FileName);
            }
            WorkBook.Close();
            ExcelApp.Quit();
            Marshal.ReleaseComObject(WorkSheet);
            Marshal.ReleaseComObject(WorkBook);
            Marshal.ReleaseComObject(ExcelApp);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    // Alternativa con el NuGet: ClosedXML.Excel
    public static void ExpCXMLE(DataGridView DGV)
    {
        try
        {
            using (SaveFileDialog SaveDialog = new SaveFileDialog())
            {
                SaveDialog.Filter = "Archivos de Excel|*.xlsx";
                SaveDialog.Title = "Guardar archivo de Excel";
                SaveDialog.DefaultExt = "xlsx";
                SaveDialog.AddExtension = true;
                // Mostrar el diálogo y verificar si el usuario seleccionó una ruta
                if (SaveDialog.ShowDialog() == DialogResult.OK)
                {
                    // Crear un DataTable a partir del DataGridView
                    DataTable DT = new DataTable();
                    foreach (DataGridViewColumn Columna in DGV.Columns)
                    {
                        DT.Columns.Add(Columna.HeaderText, typeof(String));
                    }
                    // Filas
                    foreach (DataGridViewRow Fila in DGV.Rows)
                    {
                        if (!Fila.IsNewRow) // Evitar filas vacías
                        {
                            DataRow DR = DT.NewRow();
                            foreach (DataGridViewCell Celda in Fila.Cells)
                            {
                                DR[Celda.ColumnIndex] = Celda.Value?.ToString() ?? String.Empty;
                            }
                            DT.Rows.Add(DR);
                        }
                    }
                    // Crear un archivo Excel con ClosedXML
                    using (XLWorkbook WB = new XLWorkbook())
                    {
                        WB.Worksheets.Add(DT, "Datos");
                        WB.SaveAs(SaveDialog.FileName); // Guardar en la ruta seleccionada
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
}
#endregion

#region Excriptar Texto
namespace EncriptarTexto
{
    public static class Base64
    {
        // Encriptar texto con Base64
        public static String Base64Encrypt(String Input)
        {
            Byte[] Bytes = Encoding.Unicode.GetBytes(Input);
            String Output = Convert.ToBase64String(Bytes);
            return Output;
        }
        // Desencriptar texto con cifrado Base64
        public static String Base64Decrypt(String Input)
        {
            Byte[] Bytes = Convert.FromBase64String(Input);
            String Output = Encoding.Unicode.GetString(Bytes);
            return Output;
        }
    }

    public static class AES
    {
        // Encryption Key (clave de cifrado)
        private static readonly string key = "0123456789012345"; // Debe ser de 16, 24 o 32 caracteres
        // Initialization Vector (vector de inicialización)
        private static readonly string iv = "5432109876543210";  // Debe ser de 16 caracteres

        // Encriptar texto con AES
        public static String AESEncrypt(String Input)
        {
            using (Aes AESAlg = Aes.Create())
            {
                AESAlg.Key = Encoding.UTF8.GetBytes(key);
                AESAlg.IV = Encoding.UTF8.GetBytes(iv);
                ICryptoTransform Encryptor = AESAlg.CreateEncryptor(AESAlg.Key, AESAlg.IV);
                using (MemoryStream MSe = new MemoryStream())
                {
                    using (CryptoStream CSe = new CryptoStream(MSe, Encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter SWe = new StreamWriter(CSe))
                        {
                            SWe.Write(Input);
                        }
                        return Convert.ToBase64String(MSe.ToArray());
                    }
                }
            }
        }

        // Desencriptar texto con cifrado AES
        public static String AESDecrypt(String Input)
        {
            using (Aes AESAlg = Aes.Create())
            {
                AESAlg.Key = Encoding.UTF8.GetBytes(key);
                AESAlg.IV = Encoding.UTF8.GetBytes(iv);
                ICryptoTransform Decryptor = AESAlg.CreateDecryptor(AESAlg.Key, AESAlg.IV);
                using (MemoryStream MSd = new MemoryStream(Convert.FromBase64String(Input)))
                {
                    using (CryptoStream CSd = new CryptoStream(MSd, Decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader SRd = new StreamReader(CSd))
                        {
                            return SRd.ReadToEnd();
                        }
                    }
                }
            }
        }
    }

    // Encriptar texto con métodos MD5/SHA-1/SHA-256/SHA-512 con C#
    public static class SHA
    {
        private static String HashEncrypt(String Input, HashAlgorithm Method)
        {
            Byte[] TextBytes = Encoding.UTF8.GetBytes(Input);
            Byte[] HashBytes = Method.ComputeHash(TextBytes);
            StringBuilder SB = new StringBuilder();
            foreach (Byte B in HashBytes)
            {
                SB.Append(B.ToString("X2"));
            }
            return SB.ToString();
        }

        // Encriptar texto con método SHA: MD5
        public static String MD5Encrypt(String Input)
        {
            MD5 Md5 = MD5.Create();
            return HashEncrypt(Input, Md5);
        }

        // Encriptar texto con método SHA: Sha-160
        public static String SHA1Encrypt(String Input)
        {
            SHA1 S160 = SHA1.Create();
            return HashEncrypt(Input, S160);
        }

        // Encriptar texto con método SHA: Sha-256
        public static String SHA256Encrypt(String Input)
        {
            SHA256 S256 = SHA256.Create();
            return HashEncrypt(Input, S256);
        }
		
        // Encriptar texto con método SHA: Sha-512
        public static String SHA512Encrypt(String Input)
        {
            SHA512 S512 = SHA512.Create();
            return HashEncrypt(Input, S512);
        }
    }
}
#endregion

#region Manejo de texto y cadenas String
namespace CadenasString
{
    public static class ManejarTexto
    {
        // Copiar texto al portapapeles
        public static void CopyText(String Input)
        {
            try
            {
                Clipboard.SetText(Input);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Eliminar espacios en blanco de un string
        public static String SinEspacios(String Input)
        {
            String Output = Regex.Replace(Input, @"\s", String.Empty);
            return Output;
        }

        // Captar un número y mostrar con formato moneda en un Label/TextBox
        public static void Formatear(String Input, Label Lab)
        {
            Int32 Numero = Convert.ToInt32(Input);
            Lab.Text = Numero.ToString("C", CultureInfo.CurrentCulture);  // Con decimales
            Lab.Text = Numero.ToString("C0", CultureInfo.CurrentCulture); // Sin decimales
            Lab.Text = Numero.ToString("C2", CultureInfo.CurrentCulture); // Con 2 decimales
            Lab.Text = Numero.ToString("C4", CultureInfo.CurrentCulture); // Con 4 decimales
        }

        // Captar texto como hipervínculo
        public static void AbrirUrl(String Url)
        {
            try
            {
                if (Uri.IsWellFormedUriString(Url, UriKind.Absolute))
                {
                    Process.Start(new ProcessStartInfo(UrlW)
                    {
                        UseShellExecute = true
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "URL vo válida");
            }
        }

        // Unir cadenas de texto como una sola sin separadores
        public static String CadenasUnidasSparador(params String[] Cadenas)
        {
            // Este método no es dinámico
            return String.Concat("Texto1", "Texto2", "Texto 3");
        }

        // Unir cadenas de texto como una sola con separadores
        public static String CadenasUnidasSparador(params String[] Cadenas)
        {
            return String.Join(" ", Cadenas);                 // Unir cadenas con un espacio en blanco
            return String.Join(Environment.NewLine, Cadenas); // Une con saltos de línea
        }
    }
}
#endregion

#region Controles Windows Forms
namespace CtrlWinForms
{
    // Procedimiento para permitir una única instancia de un proyecto Windows Forms .NET Framework
    static class Program
    {
        [STAThread]
        static void Main()
        {
            var ProjInfo = typeof(Program).Assembly;
            var Atributos = (GuidAttribute)ProjInfo.GetCustomAttributes(typeof(GuidAttribute), true)[0];
            string MyGUID = Atributos.Value.ToString();
            Mutex Mtx = new Mutex(true, "{" + MyGUID +"}");
			
			if(Mtx.WaitOne(TimeSpan.Zero, true))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form());
            }
            else
            {
                MessageBox.Show("Instancia abierta", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
    
    public partial class AbrirForms : Form
    {
        // Procedimiento para abrir un Form como elemento dentro de un Panel
        void OpenForm(Panel Pnl, Object Mod)
        {
            if (Pnl.Controls.Count > 0)
            {
                Pnl.Controls.RemoveAt(0);
            }
            Form Frm = Modulo as Form;
            Frm.TopLevel = false;
            Frm.FormBorderStyle = FormBorderStyle.None;
            Frm.Dock = DockStyle.Fill;
            Frm.BackColor = BackColor;
            Pnl.Controls.Add(Frm);
            Pnl.Tag = Frm;
            Frm.Show();
        }

        // Evento para abrir Form en Panel
        void Abrir(Object sender, EventArgs e)
        {
            OpenForms(Panel, new Form());
        }

        void DoItForm()
        {
            Form Frm = new Form();
            // Abrir ventana
            Frm.Show();
            // Abir ventana sin permitir perdida del foco
            Frm.ShowDialog();
            // Ocultar
            Hide();
            // Cerrar
            Close();
            // Cerrar aplicación/proyecto
            Application.Exit();
            // Cerrar la aplicación
            Environment.Exit(0);
            // Reiniciar aplicación/proyecto
            Application.Restart();
            // Llamar FormClosing += Frm_Closing;
            Frm.FormClosing += Frm_Closing;
        }

        void Frm_Closing(Object sender, FormClosingEventArgs e)
        {
        }

        // Mostrar un Form en instancia única
        Form Frm;
        void FormInstaciaUnica()
        {
            if (Frm == null)
            {
                Frm = new Form();
                Frm.FormClosed += (o, args) => Frm = null;
            }
            Frm.Show();
            Frm.BringToFront();
        }
    }

    public static class PropControls
    {
        // Solo aceptar entrada de números enteros en TextBox (Evento: KeyPress)
        public static void OnlyInts(KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        // Solo aceptar entrada de letras sin espacios vacios en TextBox (Evento: KeyPress)
        public static void OnlyText(KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar)) && (e.KeyChar != (char)Keys.Back);
        }

        // Asegurar que un TextBox (Evento: KeyPress) acepte números enteros, un solo punto decimal, y hasta 2 dígitos después del punto
        public static void TextDeci(object sender, KeyPressEventArgs e)
        {
            // Permitir solo números, un punto decimal y teclas de control
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            // Permitir solo un punto decimal
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
            // Permitir solo 2 dígitos después del punto decimal
            if ((sender as TextBox).Text.Contains("."))
            {
                String[] Parts = (sender as TextBox).Text.Split('.');
                if (Parts.Length > 1 && Parts[1].Length >= 2 && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
        }

        // Calcular edad, o diferencia de años entre 2 fechas
        public static Int32 Edad(DateTime FechaMayor, DateTime FechaMenor)
        {
            Int32 Output;
            try
            {
                // Calcular número de días entre 2 fechas
                Double Dias = Convert.ToDouble((FechaMayor - FechaMenor).TotalDays);
                // Tomar número de días, convertirlos a años y eliminar decimales
                Int32 Edad = Convert.ToInt32(Math.Truncate(Dias / 365.25));
                Output = Edad;
            }
            catch (Exception ex)
            {
                Output = new Int32();
                MessageBox.Show(ex.Message);
            }
            return Output;
        }
    }
    
    // Crear un List<string> con items en negrita(blod) y mostrarlos en un RichTextBox
    public static class FormatearRichTextBox
    {
        static void ListInRichTextBox()
        {
            List<String> Lista = new List<String>
            {
                @"{\b Item en negrita}",
                "Item en texto normal"
            };
            ShowItems(Lista, [RichTextBox]);
        }

        static void ShowItems(List<String> Lista, RichTextBox RTB)
        {
            RTB.Rtf = BuildRtf(Lista, 00);
        }
        
        static String BuildRtf(List<String> Lista, Int32 FontS)
        {
			String FontName = Font.Name; // Obtener el nombre de la fuente del formulario
            Int32 FontSize = FontS; // Tamaño de la fuente
            String RtfCabecera = $@"{{\rtf1\ansi\deff0 {{\fonttbl {{\f0 {FontName};}}}} ";
            string RtfContenido = "";
			
            foreach (String Item in Lista)
            {
				RtfContenido += $@"\f0\fs{FontSize * 2} {Item}\par ";
            }
			
            String RtfFinal = "}";
            return RtfCabecera + RtfContenido + RtfFinal;
        }
    }
}
#endregion