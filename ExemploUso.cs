using SistemaPedidos;
using SistemaPedidos.Delegates;

namespace Exemplos;

/// <summary>
/// Exemplos práticos de uso do Sistema de Pedidos
/// Demonstra herança controlada e composição com delegates
/// </summary>
public class ExemploUso
{
    public static void Main()
    {
        Console.WriteLine("=== Sistema de Pedidos - Exemplos de Uso ===\n");

        // Exemplo 1: Pedido Nacional Simples
        Console.WriteLine("1. Pedido Nacional Simples:");
        var pedidoNacional = new PedidoNacional();
        pedidoNacional.Itens.Add("Notebook");
        pedidoNacional.Processar();
        Console.WriteLine($"   {pedidoNacional.Recibo}");
        Console.WriteLine();

        // Exemplo 2: Pedido Nacional com Frete Fixo
        Console.WriteLine("2. Pedido Nacional com Frete Fixo (R$ 15):");
        var pedidoComFrete = new PedidoNacional(
            frete: FreteStrategies.Fixo(15m)
        );
        pedidoComFrete.Itens.Add("Mouse Gamer");
        pedidoComFrete.Processar();
        Console.WriteLine($"   Total: {pedidoComFrete.Total:C}");
        Console.WriteLine($"   {pedidoComFrete.Recibo}");
        Console.WriteLine();

        // Exemplo 3: Pedido Nacional com Frete Percentual e Cupom
        Console.WriteLine("3. Pedido Nacional com Frete 10% + Cupom R$ 20:");
        var pedidoPromocional = new PedidoNacional(
            frete: FreteStrategies.Percentual(10m),
            promocao: PromocaoStrategies.Cupom(20m)
        );
        pedidoPromocional.Itens.Add("Teclado Mecânico");
        pedidoPromocional.Processar();
        Console.WriteLine($"   Subtotal base: R$ 100");
        Console.WriteLine($"   Frete 10%: +R$ 10");
        Console.WriteLine($"   Cupom: -R$ 20");
        Console.WriteLine($"   Total: {pedidoPromocional.Total:C}");
        Console.WriteLine($"   {pedidoPromocional.Recibo}");
        Console.WriteLine();

        // Exemplo 4: Pedido Internacional
        Console.WriteLine("4. Pedido Internacional (com taxas e câmbio):");
        var pedidoInternacional = new PedidoInternacional(
            taxaImportacao: 20m,
            custosAduaneiros: 10m,
            cambio: 5m
        );
        pedidoInternacional.Itens.Add("Graphics Card");
        pedidoInternacional.Processar();
        Console.WriteLine($"   Subtotal base: R$ 100");
        Console.WriteLine($"   Taxa importação: +R$ 20");
        Console.WriteLine($"   Custos aduaneiros: +R$ 10");
        Console.WriteLine($"   Subtotal: R$ 130");
        Console.WriteLine($"   Câmbio (x5): ${pedidoInternacional.Total:F2}");
        Console.WriteLine($"   {pedidoInternacional.Recibo}");
        Console.WriteLine();

        // Exemplo 5: LSP - Polimorfismo em Ação
        Console.WriteLine("5. LSP - Processar pedidos de forma genérica:");
        ProcessarPedidos(new Pedido[] {
            new PedidoNacional(),
            new PedidoInternacional(),
            new PedidoNacional(frete: FreteStrategies.Fixo(10m))
        });
        Console.WriteLine();

        // Exemplo 6: Composição - Trocar estratégias dinamicamente
        Console.WriteLine("6. Flexibilidade da Composição:");
        var estrategias = new[] {
            ("Frete Fixo", FreteStrategies.Fixo(10m)),
            ("Frete 5%", FreteStrategies.Percentual(5m)),
            ("Frete 15%", FreteStrategies.Percentual(15m))
        };

        foreach (var (nome, estrategia) in estrategias)
        {
            var p = new PedidoNacional(frete: estrategia);
            p.Itens.Add("Produto");
            p.Processar();
            Console.WriteLine($"   {nome}: Total = {p.Total:C}");
        }

        Console.WriteLine("\n=== Demonstração concluída ===");
    }

    /// <summary>
    /// Função genérica que aceita Pedido base
    /// Demonstra LSP: funciona com qualquer derivada sem downcast
    /// </summary>
    private static void ProcessarPedidos(Pedido[] pedidos)
    {
        foreach (var pedido in pedidos)
        {
            pedido.Itens.Add("Item Genérico");
            pedido.Processar();
            Console.WriteLine($"   Processado: {pedido.GetType().Name} - {pedido.Recibo}");
        }
    }
}

