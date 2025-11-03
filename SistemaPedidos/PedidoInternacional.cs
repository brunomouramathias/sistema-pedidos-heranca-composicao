namespace SistemaPedidos;

public sealed class PedidoInternacional : Pedido
{
    private Func<decimal, decimal>? _calcularFrete;
    private Func<decimal, decimal>? _calcularPromocao;
    private decimal _taxaImportacao;
    private decimal _custosAduaneiros;
    private decimal _cambio;

    public PedidoInternacional(
        decimal taxaImportacao = 0m,
        decimal custosAduaneiros = 0m,
        decimal cambio = 1m,
        Func<decimal, decimal>? frete = null,
        Func<decimal, decimal>? promocao = null)
    {
        _taxaImportacao = taxaImportacao;
        _custosAduaneiros = custosAduaneiros;
        _cambio = cambio;
        _calcularFrete = frete;
        _calcularPromocao = promocao;
    }

    protected override decimal CalcularSubtotal()
    {
        decimal subtotal = base.CalcularSubtotal();
        
        subtotal += _taxaImportacao;
        subtotal += _custosAduaneiros;
        
        if (_calcularFrete != null)
            subtotal += _calcularFrete(subtotal);
            
        if (_calcularPromocao != null)
            subtotal -= _calcularPromocao(subtotal);
            
        subtotal *= _cambio;
        
        return subtotal;
    }

    protected override string EmitirRecibo(decimal total)
    {
        Recibo = $"Commercial Invoice: ${total:F2}";
        return Recibo;
    }
}

