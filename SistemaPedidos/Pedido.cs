namespace SistemaPedidos;

public class Pedido
{
    public List<string> Itens { get; protected set; }
    public decimal Subtotal { get; protected set; }
    public decimal Total { get; protected set; }
    public string? Recibo { get; protected set; }

    public Pedido()
    {
        Itens = new List<string>();
    }

    public void Processar()
    {
        Validar();
        CalcularTotal();
        EmitirRecibo(Total);
    }

    protected virtual void Validar()
    {
        if (Itens == null || Itens.Count == 0)
            throw new InvalidOperationException("Pedido deve conter pelo menos um item");
    }

    protected virtual decimal CalcularSubtotal()
    {
        return 100m;
    }

    protected virtual string EmitirRecibo(decimal total)
    {
        Recibo = $"Recibo: {total:C}";
        return Recibo;
    }

    private void CalcularTotal()
    {
        Subtotal = CalcularSubtotal();
        Total = Subtotal;
    }
}

