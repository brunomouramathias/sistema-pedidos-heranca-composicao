namespace SistemaPedidos.Delegates;

public static class PromocaoStrategies
{
    public static Func<decimal, decimal> Nenhuma()
    {
        return subtotal => 0m;
    }

    public static Func<decimal, decimal> Cupom(decimal valorDesconto)
    {
        return subtotal => valorDesconto;
    }
}

