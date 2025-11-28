using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Proyecto;

public partial class MsgBox : Window
{
    public MsgBoxResult Result {get; private set; } = MsgBoxResult.None;

    public MsgBox(String? Message, MsgBoxOpcion Opcion)
    {
        InitializeComponent();
        WinProps(Opcion, Message);
    }

    void WinProps(MsgBoxOpcion Opcion, String Message)
    {
        Title = String.Empty;
        switch (Opcion)
        {
            case MsgBoxOpcion.OK: // OK
                BtnA.IsVisible = false;
                BtnB.IsVisible = true;
                BtnB.Content = "OK";
                break;
            case MsgBoxOpcion.OKCancel: // Ok/Cancel
                BtnA.IsVisible = true;
                BtnB.IsVisible = true;
                BtnA.Content = "OK";
                BtnB.Content = "Cancel";
                break;
            case MsgBoxOpcion.YesNo: // Yes/No
                BtnA.IsVisible = true;
                BtnB.IsVisible = true;
                BtnA.Content = "Si";
                BtnB.Content = "No";
                break;
            case MsgBoxOpcion.None: // None
                BtnA.IsVisible = false;
                BtnB.IsVisible = false;
                break;
        }
        TBxConte.Text = Message;
    }

    void ResultsA(Object sender, RoutedEventArgs e)
    {
        switch (BtnA.Content)
        {
            case "Ok":
                Result = MsgBoxResult.OK;
                break;
            case "Si":
                Result = MsgBoxResult.Yes;
                break;
            default:
                Result = MsgBoxResult.None;
                break;
        }
        Close();
    }

    void ResultsB(Object sender, RoutedEventArgs e)
    {
        switch (BtnB.Content)
        {
            case "Ok":
                Result = MsgBoxResult.OK;
                break;
            case "Cancel":
                Result = MsgBoxResult.Cancel;
                break;
            case "No":
                Result = MsgBoxResult.No;
                break;
            default:
                Result = MsgBoxResult.None;
                break;
        }
        Close();
    }

    public void ResetR()
    {
        Result = MsgBoxResult.None;
    }
}