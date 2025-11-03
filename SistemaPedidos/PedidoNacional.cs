namespace SistemaPedidos;

public sealed class PedidoNacional : Pedido
{
    private Func<decimal, decimal>? _calcularFrete;
    private Func<decimal, decimal>? _calcularPromocao;

    public PedidoNacional(Func<decimal, decimal>? frete = null, Func<decimal, decimal>? promocao = null)
    {
        _calcularFrete = frete;
        _calcularPromocao = promocao;
    }

    protected override decimal CalcularSubtotal()
    {
        decimal subtotal = base.CalcularSubtotal();
        
        if (_calcularFrete != null)
            subtotal += _calcularFrete(subtotal);
            
        if (_calcularPromocao != null)
            subtotal -= _calcularPromocao(subtotal);
            
        return subtotal;
    }

    protected override string EmitirRecibo(decimal total)
    {
        Recibo = $"NF-e: {total:C}";
        return Recibo;
    }
}

