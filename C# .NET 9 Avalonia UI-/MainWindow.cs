using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading; // Librería para control Timer
using Avalonia.Interactivity; // Librería para RoutedEvent
using Avalonia.Input; // Librería para PointerPressed

namespace Proyecto;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    #region Control: Timer
    DispatcherTimer? Timer; // Control Timer
    Int32 Counter = 0; // Contador para Timer

    // Definir propiedades y acciones a control Timer
    void Tempor()
    {
        // Hacer que el Timer ejecute cada 100 milisegundos (10 veces por segundo)
        Timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
        // Llamar Evento: Timer_Tick
        Timer.Tick += Timer_Tick;
        // Iniciar Timer
        Timer.Start();
    }

    // Evento: Timer_Tick
    void Timer_Tick(Object sender, EventArgs e)
    {
        Counter++; // Contar intervalos
        // Procedimientos a ejecutar en Timer_Tick
    }
    #endregion

    #region Eventos
    // Eventos EventArgs
    void EventoRouted(Object sender, RoutedEventArgs e)
    {
        // Evento: Click
        // Evento: 
    }

    // Eventos PointerPressed
    void EventoPointerPressed(Object sender, PointerPressedEventArgs e)
    {
    }

    // Abrir Window desde Window
    async void WinByWin(Object sender, RoutedEventArgs e)
    {
        Window Win = new();
        await Win.ShowDialog(this); // ShowDialog()
        Win.Show(this); // Show()
    }

    // Abrir Window desde Window
    async Task WinByUserControl()
    {
        Window Win = new();
        MainWindow? MW = this.FindAncestorOfType<Window>();
        if (MW != null)
        {
            await Win.ShowDialog(MW); // ShowDialog()
            // Código a ejecutar despues de cerrar Window
        }
    }
    
    // Expandir/Contraer barra lateral (<SplitView> <SplitView.Pane></SplitView.Pane> </SplitView>)
    void ExpandirContraerPane(Object sender, RoutedEventArgs e)
    {
        SplitView SV = new();
        SV.IsPaneOpen = !SV.IsPaneOpen;
        // Hacer cuando IsPaneOpen = true/false
    }
    #endregion
}