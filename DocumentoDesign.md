# Documento de Design - Sistema de Pedidos (Nacional vs Internacional)

**Autor:** Bruno Moura Mathias Fernades Simão

---

## Fase 1: Conceituação (Sem Código)

### Ritual Comum de Processamento

O processamento de pedidos segue um ritual fixo com três etapas principais:

1. **Validar**: verifica se o pedido possui itens válidos
2. **Calcular Total**: processa o subtotal aplicando regras específicas de cada tipo de pedido
3. **Emitir Recibo**: gera o documento fiscal apropriado ao tipo de pedido

### Diferenças entre Pedidos Nacionais e Internacionais

**Pedidos Nacionais:**
- Formato do recibo: NF-e (Nota Fiscal eletrônica)
- Impostos e taxas: internos ao Brasil
- Frete: calculado sobre o valor do pedido

**Pedidos Internacionais:**
- Formato do recibo: Commercial Invoice
- Taxas adicionais: taxa de importação e custos aduaneiros
- Câmbio: aplicação da taxa de conversão de moeda
- Documento de saída: formulário internacional

### Justificativa: Herança vs Composição

**Herança (para tipo de pedido):**
A escolha de usar herança para especializar pedidos nacionais e internacionais se justifica porque o tipo do pedido determina um comportamento essencial que não muda durante o ciclo de vida do objeto. Um pedido nacional sempre será nacional, e um internacional sempre será internacional. A herança garante que o ritual fixo (Validar → Calcular → Emitir) seja consistente, enquanto permite especialização nos detalhes de cálculo e formato de recibo.

**Composição (para políticas de frete e promoção):**
As políticas de frete e promoção são características independentes e combinam-se de forma flexível. Um pedido pode ter frete fixo ou percentual, promoção com cupom ou nenhuma promoção. Essas variações não definem "o que é" o pedido, mas sim "como ele se comporta". Usar composição com delegates evita a explosão de subclasses (CarroComFreteFixoEPromocaoCupom, CarroComFretePercentualSemPromocao, etc.) e permite trocar comportamentos dinamicamente.

---

## Fase 2: Design Orientado a Objetos (Sem Código)

### Contrato da Classe Pedido

A classe base `Pedido` define:

**Método público fixo:**
- `Processar()`: orquestra o ritual fixo chamando Validar() → CalcularTotal() → EmitirRecibo()

**Ganchos protected virtual:**
- `CalcularSubtotal()`: permite especialização do cálculo
- `EmitirRecibo(decimal total)`: permite especialização do formato do recibo
- `Validar()`: validação opcional customizável

### Três Regras de LSP (Liskov Substitution Principle)

**1. Substituibilidade:** 
Qualquer código que usa `Pedido` deve funcionar igualmente com `PedidoNacional` ou `PedidoInternacional` sem usar `is/downcast`. O cliente chama `Processar()` e recebe um recibo válido, independente do tipo concreto.

**2. Invariantes preservados:**
As subclasses não enfraquecem as validações da base. Se a base exige itens válidos, as derivadas também exigem. O contrato de saída (`EmitirRecibo` retorna string válida) é respeitado.

**3. Contratos de saída equivalentes:**
O método `Processar()` sempre retorna o pedido com recibo calculado e total definido, mantendo a mesma interface de saída para qualquer tipo de pedido.

### Eixos Plugáveis por Composição (Delegates)

**Frete:**
- `Func<decimal, decimal>` → Fixo ou Percentual
- Exemplo: `FreteStrategies.Fixo(10m)` ou `FreteStrategies.Percentual(5m)`

**Promoção:**
- `Func<decimal, decimal>` → Nenhuma ou Cupom
- Exemplo: `PromocaoStrategies.Cupom(20m)` ou `PromocaoStrategies.Nenhuma()`

Esses delegates são injetados no construtor das classes sealed, permitindo qualquer combinação sem criar novas subclasses.

