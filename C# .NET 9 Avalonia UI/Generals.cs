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
	public static void OnlyDeci(TextBox TBx, KeyEventArgs e, Int32 Decimales)
    {
        Boolean isDigit = (e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9);

        Boolean isControlKey = e.Key == Key.Back || e.Key == Key.Delete || e.Key == Key.Tab || e.Key == Key.Enter || e.Key == Key.Left || e.Key == Key.Right;

        Boolean isDecimalSeparator = e.Key == Key.OemComma || e.Key == Key.OemPeriod || e.Key == Key.Decimal;

        if (!isDigit && !isControlKey && !isDecimalSeparator)
        {
            e.Handled = true; return;
        }
        
        if (isDecimalSeparator)
        {
            if (TBx.Text.Contains(',') || TBx.Text.Contains('.'))
            {
                e.Handled = true;
            }
        }
        else if (isDigit)
        {
            String Texto = TBx.Text;
            
            Int32 SeparatorIndex = Texto.IndexOfAny([',', '.']);
            
            if (SeparatorIndex >= 0)
            {
                Int32 DecimalsCount = Texto.Length - SeparatorIndex - 1;
                if (TBx.CaretIndex > SeparatorIndex && DecimalsCount >= Decimales)
                {
                    e.Handled = true;
                }
            }
        }
    }
}
