using System;
using System.IO;
using System.Reflection;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media.Imaging;

namespace Proyecto;

public class Generals
{
    public static String? ExMess; // String que almacena ex.Message de los trycatch

    // String para retornar el nombre de la app + versión de la app
    public static String AppName()
    {
        return Assembly.GetEntryAssembly()?.GetName().Name + " | " + Assembly.GetEntryAssembly()?.GetName().Version;
    }

	// Evento PointerPressed para mover ventanas
    public static void MoverWin(Object sender, PointerPressedEventArgs e, Window Win)
    {
        if (e.GetCurrentPoint(Win).Properties.IsLeftButtonPressed) { Win.BeginMoveDrag(e); }
    }

	// Abrir UserControl dentro de DockPanel (Equivalente WinForms a Form dentro de Panel)
    public static void OpenMods(DockPanel DPl, UserControl Mod)
    {
        try
        {
            if (DPl.Children.Count > 0) { DPl.Children.Clear(); }
            Mod.HorizontalAlignment = HorizontalAlignment.Stretch;
            Mod.VerticalAlignment = VerticalAlignment.Stretch;
            DPl.Children.Add(Mod);
        }
        catch (Exception ex)
        {
            ExMess = ex.Message;
        }
    }

	// Cargar imagen desde archivo para Image contenido dentro de Button
    public static void BtnImage(Button Btn, String? Ruta, Int32 W, Int32 H)
    {
        try
        {
            if (File.Exists(Ruta))
            {
                Btn.Content = new Image() { Source = new Bitmap(Ruta), Width = W, Height = H };
            }
        }
        catch (Exception ex)
        {
            ExMess = ex.Message;
        }
    }

	// Cargar imagen desde archivo para Image
    public static void ImgImage(Image Img, String? Ruta, Int32 W, Int32 H)
    {
        try
        {
            if (File.Exists(Ruta))
            {
                if (File.Exists(Ruta))
                {
                    Img.Source = new Bitmap(Ruta);
                    Img.Width = W;
                    Img.Height = H;
                }
            }
        }
        catch (Exception ex)
        {
            ExMess = ex.Message;
        }
    }

    // Solo permitir números enteros en TextBox
    public static void OnlyInts(KeyEventArgs e)
    {
        Boolean isDigit = (e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9);
        
        Boolean isControlKey = e.Key == Key.Back || e.Key == Key.Delete || e.Key == Key.Tab || e.Key == Key.Enter || e.Key == Key.Left || e.Key == Key.Right;

        if (!isDigit && !isControlKey) { e.Handled = true; }
    }

	// Solo permitir números enteros, junto a una coma (,) o punto (.) en un TextBox
	public static void OnlyDeci(Object sender, KeyEventArgs e, Int32 Decimales)
    {
		if (sender is not TextBox TBx) return;
		
		// Teclas de control permitidas (Backspace, Delete, flechas, Tab)
		if (e.Key == Key.Back || e.Key == Key.Delete || e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Tab)
		{
			return; // Permitir
		}
		
		// Obtener el carácter correspondiente a la tecla
		Char? IC = null;
		
		if (e.Key >= Key.D0 && e.Key <= Key.D9)
		{
			IC = (Char)('0' + (e.Key - Key.D0));
		}
		else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
		{
			IC = (Char)('0' + (e.Key - Key.NumPad0));
		}
		else if (e.Key == Key.OemComma || e.Key == Key.Decimal || e.Key == Key.OemPeriod)
		{
			IC = '.'; // Normalizamos a punto
		}
		
		// Bloquear cualquier otra tecla
		if (IC == null)
		{
			e.Handled = true;
			return;
		}
		
		// Simular el texto resultante si se acepta la tecla
		Int32 CI = TBx.CaretIndex;
		String? NT = TBx.Text?[..CI] + IC + TBx.Text?[CI..];
		
		// Validar: solo un punto/coma
		Int32 CS = NT.Split('.', ',').Length - 1;
		if (CS > 1)
		{
			e.Handled = true;
			return;
		}
		
		// Validar Número de decimales
		if (NT.Contains('.') || NT.Contains(','))
		{
			Char S = NT.Contains('.') ? '.' : ',';
			String[] P = NT.Split(S);
			if (P.Length > 1 && P[1].Length > Decimales)
			{
				e.Handled = true;
				return;
			}
		}
    }
    
    // Procedimiento para crear columnas en un control DataDrid
    public static void GColumns(DataGrid DG, String? TextoColumna, String? NombreColumna, Int32 Ancho, Boolean EsVisible)
    {
        try
        {
            DataGridTextColumn NewCol = new()
            {
                Header = Head,
                Binding = new Avalonia.Data.Binding(Columna),
                Width = new DataGridLength(Ancho),
                IsVisible = EsVisible
            };
            DG.Columns.Add(NewCol);
        }
        catch (Exception)
        {
            DG.Columns.Clear();
        }
    }
}
