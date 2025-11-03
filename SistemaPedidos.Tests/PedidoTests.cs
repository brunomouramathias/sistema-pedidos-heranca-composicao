using SistemaPedidos;
using SistemaPedidos.Delegates;
using Xunit;

namespace SistemaPedidos.Tests;

public class PedidoTests
{
    [Fact]
    public void PedidoBase_DeveProcessarCorretamente()
    {
        var pedido = new Pedido();
        pedido.Itens.Add("Item 1");
        
        pedido.Processar();
        
        Assert.NotNull(pedido.Recibo);
        Assert.Contains("Recibo:", pedido.Recibo);
    }

    [Fact]
    public void PedidoNacional_DeveSubstituirPedidoBase_SemDowncast()
    {
        Pedido pedido = new PedidoNacional();
        pedido.Itens.Add("Item Nacional");
        
        pedido.Processar();
        
        Assert.NotNull(pedido.Recibo);
        Assert.Contains("NF-e:", pedido.Recibo);
    }

    [Fact]
    public void PedidoInternacional_DeveSubstituirPedidoBase_SemDowncast()
    {
        Pedido pedido = new PedidoInternacional();
        pedido.Itens.Add("Item Internacional");
        
        pedido.Processar();
        
        Assert.NotNull(pedido.Recibo);
        Assert.Contains("Commercial Invoice:", pedido.Recibo);
    }

    [Fact]
    public void LSP_ClienteUsaPedidoGenerico_FuncionaComNacional()
    {
        Pedido pedido = new PedidoNacional();
        pedido.Itens.Add("Produto");
        
        ProcessarPedidoGenerico(pedido);
        
        Assert.NotNull(pedido.Recibo);
    }

    [Fact]
    public void LSP_ClienteUsaPedidoGenerico_FuncionaComInternacional()
    {
        Pedido pedido = new PedidoInternacional();
        pedido.Itens.Add("Produto");
        
        ProcessarPedidoGenerico(pedido);
        
        Assert.NotNull(pedido.Recibo);
    }

    private void ProcessarPedidoGenerico(Pedido pedido)
    {
        pedido.Processar();
    }
}

