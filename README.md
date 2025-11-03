# Sistema de Pedidos - Nacional vs Internacional

**Autor:** Bruno Moura Mathias Fernades Simão

Este projeto implementa um sistema de processamento de pedidos de venda que diferencia pedidos nacionais e internacionais através de herança controlada. A classe base `Pedido` define um ritual fixo de processamento (Validar → Calcular Total → Emitir Recibo) com ganchos virtuais que permitem especialização sem quebrar o contrato base. As políticas de frete e promoção utilizam composição com delegates, evitando proliferação de subclasses e permitindo combinação flexível de comportamentos.

## Como executar

```bash
dotnet test
```

