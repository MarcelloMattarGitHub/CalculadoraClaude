using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace CalculadoraClaude;

public partial class MainWindow : Window
{
    private readonly Calculator _calc = new();

    public MainWindow()
    {
        InitializeComponent();
        KeyDown += OnKeyDown;
    }

    private void Atualizar() => Visor.Text = _calc.Display;

    private static string Texto(object? sender) => (string)((Button)sender!).Content!;

    private void OnDigito(object? s, RoutedEventArgs e) { _calc.Digito(Texto(s)[0]); Atualizar(); }
    private void OnVirgula(object? s, RoutedEventArgs e) { _calc.Virgula(); Atualizar(); }
    private void OnOperador(object? s, RoutedEventArgs e) { _calc.Operador(Texto(s)[0]); Atualizar(); }
    private void OnIgual(object? s, RoutedEventArgs e) { _calc.Igual(); Atualizar(); }
    private void OnLimpar(object? s, RoutedEventArgs e) { _calc.Limpar(); Atualizar(); }
    private void OnSinal(object? s, RoutedEventArgs e) { _calc.InverterSinal(); Atualizar(); }
    private void OnPorcento(object? s, RoutedEventArgs e) { _calc.Porcentagem(); Atualizar(); }
    private void OnApagar(object? s, RoutedEventArgs e) { _calc.Apagar(); Atualizar(); }

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        bool shift = e.KeyModifiers == KeyModifiers.Shift;
        switch (e.Key)
        {
            case >= Key.D0 and <= Key.D9 when !shift:
                _calc.Digito((char)('0' + (e.Key - Key.D0))); break;
            case Key.D5 when shift: _calc.Porcentagem(); break;
            case >= Key.NumPad0 and <= Key.NumPad9:
                _calc.Digito((char)('0' + (e.Key - Key.NumPad0))); break;
            case Key.OemComma or Key.OemPeriod or Key.Decimal: _calc.Virgula(); break;
            case Key.Add: _calc.Operador('+'); break;
            case Key.OemPlus when shift: _calc.Operador('+'); break;
            case Key.Subtract or Key.OemMinus: _calc.Operador('-'); break;
            case Key.Multiply: _calc.Operador('×'); break;
            case Key.D8 when shift: _calc.Operador('×'); break;
            case Key.Divide or Key.OemQuestion: _calc.Operador('÷'); break;
            case Key.Enter or Key.OemPlus: _calc.Igual(); break;
            case Key.Back: _calc.Apagar(); break;
            case Key.Escape or Key.Delete: _calc.Limpar(); break;
            default: return;
        }
        Atualizar();
        e.Handled = true;
    }
}
