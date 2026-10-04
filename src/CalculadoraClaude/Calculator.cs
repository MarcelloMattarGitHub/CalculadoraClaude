using System;
using System.Globalization;

namespace CalculadoraClaude;

/// <summary>Lógica da calculadora, independente da interface.</summary>
public class Calculator
{
    private static readonly CultureInfo Culture = new("pt-BR");

    private double? _acumulado;
    private char? _operador;
    private string _entrada = "0";
    private bool _novaEntrada = true;

    public string Display => _entrada;

    public void Digito(char d)
    {
        if (_novaEntrada || _entrada == "0")
        {
            _entrada = d.ToString();
            _novaEntrada = false;
        }
        else if (_entrada.Length < 15)
        {
            _entrada += d;
        }
    }

    public void Virgula()
    {
        if (_novaEntrada)
        {
            _entrada = "0";
            _novaEntrada = false;
        }
        if (!_entrada.Contains(',')) _entrada += ",";
    }

    public void Operador(char op)
    {
        if (_operador != null && !_novaEntrada) Igual();
        _acumulado = Valor();
        _operador = op;
        _novaEntrada = true;
    }

    public void Igual()
    {
        if (_operador == null || _acumulado == null) return;

        double b = Valor();
        double a = _acumulado.Value;
        double r = _operador switch
        {
            '+' => a + b,
            '-' => a - b,
            '×' => a * b,
            '÷' => b == 0 ? double.NaN : a / b,
            _ => b
        };

        _entrada = double.IsNaN(r) || double.IsInfinity(r)
            ? "Erro"
            : r.ToString("0.##########", Culture);
        _acumulado = null;
        _operador = null;
        _novaEntrada = true;
    }

    public void Porcentagem()
    {
        _entrada = (Valor() / 100).ToString("0.##########", Culture);
        _novaEntrada = true;
    }

    public void InverterSinal()
    {
        if (_entrada is "0" or "Erro") return;
        _entrada = _entrada.StartsWith('-') ? _entrada[1..] : "-" + _entrada;
    }

    public void Apagar()
    {
        if (_novaEntrada || _entrada == "Erro") return;
        _entrada = _entrada.Length > 1 && !(_entrada.Length == 2 && _entrada[0] == '-')
            ? _entrada[..^1]
            : "0";
    }

    public void Limpar()
    {
        _acumulado = null;
        _operador = null;
        _entrada = "0";
        _novaEntrada = true;
    }

    private double Valor() =>
        double.TryParse(_entrada, NumberStyles.Float, Culture, out var v) ? v : 0;
}
