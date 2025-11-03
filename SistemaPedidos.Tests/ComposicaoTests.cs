using SistemaPedidos;
using SistemaPedidos.Delegates;
using Xunit;

namespace SistemaPedidos.Tests;

public class ComposicaoTests
{
    [Fact]
    public void FreteFixo_DeveAdicionarValorFixo()
    {
        var pedido = new PedidoNacional(
            frete: FreteStrategies.Fixo(10m)
        );
        pedido.Itens.Add("Item");
        
        pedido.Processar();
        
        Assert.Equal(110m, pedido.Total);
    }

    [Fact]
    public void FretePercentual_DeveCalcularPorcentagem()
    {
        var pedido = new PedidoNacional(
            frete: FreteStrategies.Percentual(10m)
        );
        pedido.Itens.Add("Item");
        
        pedido.Processar();
        
        Assert.Equal(110m, pedido.Total);
    }

    [Fact]
    public void PromocaoNenhuma_NaoDeveAlterarTotal()
    {
        var pedido = new PedidoNacional(
            promocao: PromocaoStrategies.Nenhuma()
        );
        pedido.Itens.Add("Item");
        
        pedido.Processar();
        
        Assert.Equal(100m, pedido.Total);
    }

    [Fact]
    public void PromocaoCupom_DeveAplicarDesconto()
    {
        var pedido = new PedidoNacional(
            promocao: PromocaoStrategies.Cupom(20m)
        );
        pedido.Itens.Add("Item");
        
        pedido.Processar();
        
        Assert.Equal(80m, pedido.Total);
    }

    [Fact]
    public void CombinacaoFreteEPromocao_SemNovasSubclasses()
    {
        var pedido = new PedidoNacional(
            frete: FreteStrategies.Fixo(15m),
            promocao: PromocaoStrategies.Cupom(25m)
        );
        pedido.Itens.Add("Item");
        
        pedido.Processar();
        
        Assert.Equal(90m, pedido.Total);
    }

    [Fact]
    public void PedidoInternacional_ComTaxasEDelegates()
    {
        var pedido = new PedidoInternacional(
            taxaImportacao: 20m,
            custosAduaneiros: 10m,
            cambio: 5m,
            frete: FreteStrategies.Percentual(10m),
            promocao: PromocaoStrategies.Nenhuma()
        );
        pedido.Itens.Add("Item");
        
        pedido.Processar();
        
        Assert.Equal(715m, pedido.Total);
    }
}

