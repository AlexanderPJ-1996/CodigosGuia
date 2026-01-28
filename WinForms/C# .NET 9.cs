#region General
using System;
using System.Reflection;

namespace Proyecto
{
	public static class G
	{
		// Retornar valor almacenado en <Product></Product> dentro del archivo .csproj del proyecto
		public static string? AppName()
		{
			return Assembly.GetExecutingAssembly()?.
				GetCustomAttribute<AssemblyProductAttribute>()?.Product;
		}
	}
}
#endregion

#region Librería: QuestPDF
// Librerías clase para crear listado de datos
using System;

namespace CrearPDF_QuestPDF
{
	public class Personas
	{
		public Int32 ID { get; set; }
        public String? NDoc { get; set; }
        public String? Nomb { get; set; }
        public String? Sexo { get; set; }
        public DateTime FNac { get; set; }
        public String? Edad { get; set; }
	}
}

// Librerías clase para cargar datos desde la base de datos
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CrearPDF_QuestPDF
{
	// Cargar List<> desde base de datos
	public static class CRUD
	{
		readonly static String Cadena = @"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename=|DataDirectory|\[Database].mdf; Integrated Security=True";
		
		public static List<Personas> LoadRegs()
		{
			List<Personas> Output = [];
			try
			{
				using SqlConnection Conn = new(Cadena);
				Conn.Open();
				String SQL = "SELECT * FROM TPersonas ORDER BY Nomb ASC";
				using (SqlCommand CMD = new(SQL, Conn))
				{
					CMD.CommandType = CommandType.Text;
					using MySqlDataReader DR = CMD.ExecuteReader();
					while (DR.Read())
					{
						Output.Add(new VPerso()
						{
							ID = Convert.ToInt64(DR["ID"]),
							NDoc = DR["NDoc"].ToString(),
                            Nomb = DR["Nomb"].ToString(),
                            Sexo = DR["Sexo"].ToString(),
                            FNac = Convert.ToDateTime(DR["FNac"]),
                            Edad = DR["Edad"].ToString()
						});
					}
				}
				Conn.Close();
			}
			catch (Exception)
			{
				Output = [];
			}
			return Output;
		}
	}
}

// Librerías para generar reporte PDF con libreria QuestPDF
using System;
using Sys.IO;
using System.Collections.ObjectModel;
using System.Diagnostics;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace CrearPDF_QuestPDF
{
	// Procedimiento que pide un ObservableCollection<> para alimentar los datos del reporte pdf
	public static class CrearPDF_QuestPDF
	{
		// Bytes[] para crear Png vacío
		static readonly Byte[] EmptyPng = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mP8Xw8AAoMBgVjA9xkAAAAASUVORK5CYII=");
		
		// Captar Bytes[] de archivo de imagen y convertirlo en un arreglo de Bytes[]
		static void Byte[] Imgs(String RutaImg)
		{
			Byte[] Salida = [];
			if (File.Exists(RutaImg))
			{
				Salida = File.ReadAllBytes(RutaImg);
			}
			else
			{
				Salida = EmptyPng;
			}
			return Salida;
		}
		
		public static void Generar(ObservableCollection<Personas> Personas, String Ruta)
		{
			String FilePdf = Path.Combine(Ruta, "[Documento].pdf");
			
			String[] Columnas = ["ID", "N° Documento", "Nombre completo", "Sexo", "Fecha de nacimiento", "Edad"];
			// Crear documento
			var Doc = Document.Create(Cont => 
			{
				// Nombre de la fuente usada en el documento
				// Fuente: Segoe UI
				String Font = "Segoe UI";
				// Contenido de la página
				Cont.Page(Pg => 
				{
					// Tamaño de hoja
					Pg.Size(PageSizes.Letter); // Tamaño carta vertical
					Pg.Size(new PageSize(792, 612)); // Tamaño de hoja: Carta horizontal
					Pg.Margin(2, Unit.Centimetre); // Margen: 2cm
					
					// Establecer encabezado
					// -> Color de fondo: Verde oscuro
					// -> Tamaño fuente: 15
					// -> Estilo: Negritas
					// -> Color de texto: Blanco
					// -> Alienado al centro
					String Encabezado = "Personas Registradas";
					Pg.Header().
						PaddingBottom(15).
						Background(Colors.Green.Darken4).
						Text(Encabezado).
						FontFamily(Font).FontSize(15).
						Bold().
						FontColor(Colors.White).
						AlignCenter();
						
					// Crear tabla que muestre los datos
					Pg.Content().Table(Tb => 
					{
						// Ancho de las columnas
						// -> RelativeColumn(1): Tamaño proporcional
						// -> ConstantColumn(100): Tamaño fijo de 100 puntos (Hoja carta con márgen 2 cm tiene max. 500-600 puntos)
						Tb.ColumnsDefinition(Col => 
						{
							Col.RelativeColumn(1); // ID
							Col.RelativeColumn(2); // N Documento
							Col.RelativeColumn(3); // Nombre completo
							Col.RelativeColumn(1); // Sexo
							Col.RelativeColumn(2); // Fecha de nacimiento
							Col.RelativeColumn(1); // Edad
						});
						// Establecer encabezados de la tabla
						// -> Tamaño de fuente: 12
						// -> Estilo: Negritas
						Tb.Header(Hd => 
						{
							Hd.Cell().Border(0).Text(Columnas[0]).FontFamily(Font).FontSize(12).Bold();
							Hd.Cell().Border(0).Text(Columnas[1]).FontFamily(Font).FontSize(12).Bold();
							Hd.Cell().Border(0).Text(Columnas[2]).FontFamily(Font).FontSize(12).Bold();
							Hd.Cell().Border(0).Text(Columnas[3]).FontFamily(Font).FontSize(12).Bold();
							Hd.Cell().Border(0).Text(Columnas[4]).FontFamily(Font).FontSize(12).Bold();
							Hd.Cell().Border(0).Text(Columnas[5]).FontFamily(Font).FontSize(12).Bold();
						});
						// 
						// Tamaño de fuente: 12
						// Fecha con formato 'dd/MM/yyyy'
						foreach (var P in Personas)
						{
							// Filas con valor texto
							Tb.Cell().
								PaddingVertical(1).
								Border(0).
								Text(P.ID.ToString()).
								FontFamily(Font).
								FontSize(10);
							
							Tb.Cell().
								PaddingVertical(1).
								Border(0).
								Text(P.NDoc).
								FontFamily(Font).
								FontSize(10);
							
							Tb.Cell().
								PaddingVertical(1).
								Border(0).
								Text(P.Nomb).
								FontFamily(Font).
								FontSize(10);
							
							Tb.Cell().
								PaddingVertical(1).
								Border(0).
								Text(P.Sexo).
								FontFamily(Font).
								FontSize(10);
							
							Tb.Cell().
								PaddingVertical(1).
								Border(0).
								Text(P.FNac.ToString("dd/MM/yyyy")).
								FontFamily(Font).
								FontSize(10);
							
							Tb.Cell().
								PaddingVertical(1).
								Border(0).
								Text(P.Edad).
								FontFamily(Font).
								FontSize(10);
							
							//Filas con imagenes
							String RutaPng = Path.Combine(Ruta, P.ID.ToString() + ".png");
							
							Tb.Cell().
								PaddingVertical(1).
								Image(Image.FromBinaryData(Imgs(RutaPng)));
						}
					});
					// Establecer pie de página
					Pg.Footer().AlignRight().Text(Txt => 
					{
						Txt.Span("Generado con QuestPDF | Página ").FontFamily(Font).FontSize(15);
						Txt.CurrentPageNumber().FontFamily(Font).FontSize(15); // N° actual de página
						Txt.Span(" - ").FontFamily(Font).FontSize(15);
						Txt.TotalPages().FontFamily(Font).FontSize(15); // N° total de páginas
					});
				});
			});
			// Crear documento pdf en la ruta asignada
			Doc.GeneratePdf(FilePdf);
			// Crear proceso para abrir archivo pdf con visor predeterminado establecido en el sistema
			// Requiere .NET moderno
			ProcessStartInfo PSi = new() { FileName = FilePath, UseShellExecute = true };
			// Iniciar proceso PSi
			Process.Start(PSi);
		}
	}

// Librerías para mostrar pdf en control panel (WinForms). Nuget: Microsoft.Web.WebView2
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;

namespace CrearPDF_QuestPDF
{
	// Mostrar documento PDF en Control: Panel (Windows Forms)
	public static class ShowPDFs
	{
		WebView2 WebV;
		public static async void LoadPDF_WinForms(Panel Pnl, String Ruta)
		{
			WarningHeaderValue = new() { Dock = DockStyle.Fill };
			// Agregar WebView2 dentro del Panel
			Pnl.Controls.Add(WebV);
			// Inicializar el entorno
			await WebV.EnsureCoreWebView2Async();
			// Cargar documento PDF
			WebV.CoreWebView2.Navigate(Ruta);
		}
	}
}
#endregion