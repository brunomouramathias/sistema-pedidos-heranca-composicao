namespace SistemaPedidos.Delegates;

public static class FreteStrategies
{
    public static Func<decimal, decimal> Fixo(decimal valor)
    {
        return subtotal => valor;
    }

    public static Func<decimal, decimal> Percentual(decimal percentual)
    {
        return subtotal => subtotal * (percentual / 100m);
    }
}

