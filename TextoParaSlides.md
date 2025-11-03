# Texto Completo para Geração de Slides - Sistema de Pedidos (Nacional vs Internacional)

Este documento contém todo o conteúdo necessário para criar uma apresentação sobre o projeto de Sistema de Pedidos utilizando Herança, Composição, Interfaces e LSP em C#.

---

## Slide 1: Título e Contexto

**Título:** Sistema de Pedidos - Herança, Composição e LSP em C#

**Subtítulo:** Implementação de Pedidos Nacionais e Internacionais

**Autor:** Bruno Moura Mathias Fernades Simão

**Contexto:** Este projeto demonstra a aplicação prática de conceitos fundamentais de Programação Orientada a Objetos para criar um sistema de processamento de pedidos de venda que diferencia pedidos nacionais e internacionais.

---

## Slide 2: O Problema

**Cenário Real:** Uma loja processa pedidos de venda nacionais e internacionais

**Desafios Identificados:**
- Ambos os tipos de pedido seguem o mesmo ritual: Validar → Calcular Total → Emitir Recibo
- Mas existem diferenças cruciais nas regras de impostos, taxas e formato do recibo
- Pedidos nacionais emitem NF-e, enquanto internacionais emitem Commercial Invoice
- Pedidos internacionais têm taxas de importação, custos aduaneiros e câmbio
- Políticas extras como frete e promoção variam independentemente do tipo de pedido

**Questão Central:** Como modelar isso sem duplicar código e mantendo flexibilidade?

---

## Slide 3: Ritual Comum de Processamento

**Fluxo Fixo de Processamento:**

1. **Validar o Pedido**
   - Verificar se há itens válidos
   - Garantir dados mínimos necessários

2. **Calcular Total**
   - Processar subtotal base
   - Aplicar regras específicas (taxas, impostos, frete, promoções)

3. **Emitir Recibo**
   - Gerar documento fiscal apropriado
   - Formato varia conforme tipo (NF-e ou Commercial Invoice)

**Princípio:** O ritual é o mesmo, mas os detalhes de cada etapa podem variar

---

## Slide 4: Diferenças entre Nacional e Internacional

**Pedido Nacional:**
- Recibo: NF-e (Nota Fiscal eletrônica)
- Formato fiscal brasileiro
- Impostos nacionais já embutidos
- Moeda: Real (R$)

**Pedido Internacional:**
- Recibo: Commercial Invoice
- Documento de exportação/importação
- Taxa de importação adicional
- Custos aduaneiros
- Aplicação de câmbio
- Moeda: Dólar ($)

**Observação:** Essas diferenças definem "o que é" cada tipo de pedido

---

## Slide 5: Herança por Especialização

**Por que usar Herança?**

A herança é apropriada quando a variação está na essência do objeto. Um pedido nacional sempre será nacional, e um internacional sempre será internacional durante todo seu ciclo de vida.

**Vantagens da Abordagem:**
- Ritual fixo garantido pela classe base através do método `Processar()`
- Especialização segura através de ganchos virtuais protegidos
- Polimorfismo permite tratar ambos os tipos uniformemente
- LSP (Liskov Substitution Principle) garante substituibilidade

**Classes Sealed:** PedidoNacional e PedidoInternacional são marcadas como `sealed` para evitar herança indevida e manter controle da hierarquia

---

## Slide 6: Ganchos Virtuais (Template Method)

**Classe Base Pedido:**

O método público `Processar()` orquestra o ritual fixo, mas chama ganchos virtuais protegidos que permitem especialização:

**Ganchos Disponíveis:**
- `CalcularSubtotal()`: retorna o subtotal após aplicar regras específicas
- `EmitirRecibo(decimal total)`: retorna o recibo no formato apropriado
- `Validar()`: opcional, permite validações customizadas

**Benefício:** O cliente usa apenas `Processar()`, sem precisar conhecer os detalhes internos ou tipo específico do pedido

---

## Slide 7: Princípio da Substituição de Liskov (LSP)

**O que é LSP?**

Se uma classe base pode ser substituída por uma classe derivada em qualquer parte do código sem alterar o comportamento esperado, então o LSP está sendo respeitado.

**Como garantimos LSP:**

1. **Sem Downcast:** O cliente nunca precisa usar `is` ou casting para determinar o tipo específico
2. **Contrato Mantido:** Processar() sempre funciona e retorna um recibo válido
3. **Invariantes Preservadas:** Validações mínimas são respeitadas por todas as subclasses

**Teste Prático:** Uma função que aceita `Pedido` funciona perfeitamente com `PedidoNacional` ou `PedidoInternacional` sem saber qual tipo recebeu

---

## Slide 8: O Problema das Políticas Variáveis

**Políticas Independentes:**

Frete e promoção são características que variam independentemente do tipo de pedido:

**Frete:**
- Fixo (valor constante)
- Percentual (% sobre o total)

**Promoção:**
- Nenhuma
- Cupom de desconto

**Explosão Combinatória:** Se usássemos herança para cada combinação, teríamos:
- PedidoNacionalFreteFixoPromocaoCupom
- PedidoNacionalFretePercentualSemPromocao
- PedidoInternacionalFreteFixoPromocaoCupom
- E muitas outras...

**Isso é insustentável!**

---

## Slide 9: Composição como Solução

**Por que Composição?**

Frete e promoção não definem "o que é" o pedido, mas sim "como ele se comporta". Essas são características plugáveis.

**Abordagem com Delegates:**

Usamos `Func<decimal, decimal>` como estratégias injetadas no construtor:

**FreteStrategies:**
- `Fixo(decimal valor)`: retorna delegate que adiciona valor fixo
- `Percentual(decimal percentual)`: retorna delegate que calcula percentual

**PromocaoStrategies:**
- `Nenhuma()`: retorna delegate que não aplica desconto
- `Cupom(decimal valor)`: retorna delegate que subtrai valor fixo

**Vantagem:** Qualquer combinação possível sem criar novas subclasses

---

## Slide 10: Flexibilidade sem Interfaces

**Composição Sem Interfaces:**

O exercício pede composição sem criar interfaces formais. A solução usa delegates como peças plugáveis.

**Exemplo de Uso:**

```csharp
var pedido = new PedidoNacional(
    frete: FreteStrategies.Fixo(10m),
    promocao: PromocaoStrategies.Cupom(20m)
);
```

**Benefícios:**
- Combinações livres (frete fixo + cupom, frete percentual + nenhuma promoção, etc.)
- Sem criar hierarquia de classes para cada política
- Fácil adicionar novas estratégias sem modificar classes existentes
- Testável: cada delegate pode ser testado isoladamente

---

## Slide 11: Arquitetura do Sistema

**Estrutura de Classes:**

**Classe Base:**
- `Pedido`: define o contrato e ritual fixo com `Processar()`

**Classes Sealed (Herança):**
- `PedidoNacional`: especializa cálculo e emite NF-e
- `PedidoInternacional`: adiciona taxas/câmbio e emite Commercial Invoice

**Delegates (Composição):**
- `FreteStrategies`: estratégias de cálculo de frete
- `PromocaoStrategies`: estratégias de aplicação de promoções

**Organização:** As estratégias ficam separadas em namespace próprio (SistemaPedidos.Delegates)

---

## Slide 12: Testes e Validação

**Testes de LSP:**

Demonstram que `PedidoNacional` e `PedidoInternacional` podem substituir `Pedido` sem problemas:
- Cliente usa tipo base genérico
- Chamada a `Processar()` funciona sem downcast
- Recibo é gerado corretamente independente do tipo concreto

**Testes de Composição:**

Validam a troca de peças (delegates) sem criar novas subclasses:
- Frete fixo vs percentual
- Promoção com cupom vs sem promoção
- Combinações múltiplas (frete + promoção)

**Resultado:** Todos os 11 testes passaram com sucesso

---

## Slide 13: Decisões de Design

**Quando usar Herança:**
- Variação está na essência/identidade do objeto
- Comportamento fixo com especialização de ritual
- Relação "é-um" verdadeira (Carro é um Veículo)

**Quando usar Composição:**
- Variação está em características independentes
- Combinações flexíveis de comportamentos
- Relação "tem-um" ou "usa-um"

**No Projeto:**
- Herança para tipo de pedido (essência)
- Composição para políticas (comportamento plugável)

**Resultado:** Código flexível, robusto e fácil de manter

---

## Slide 14: Vantagens da Solução

**Manutenibilidade:**
- Código organizado e com responsabilidades claras
- Fácil adicionar novos tipos de pedido (basta estender Pedido)
- Fácil adicionar novas políticas (basta criar novo delegate)

**Extensibilidade:**
- Novas estratégias de frete/promoção sem modificar classes existentes
- Sem explosão de subclasses

**Testabilidade:**
- LSP garante que testes com tipo base funcionam para derivadas
- Delegates testáveis isoladamente
- Testes de composição validam combinações

**Princípios SOLID:**
- Single Responsibility: cada classe tem uma responsabilidade
- Open/Closed: aberto para extensão, fechado para modificação
- Liskov Substitution: subclasses substituem base sem quebras

---

## Slide 15: Implementação em C#

**Tecnologias Utilizadas:**
- .NET 8.0
- xUnit para testes unitários
- Delegates para composição
- Sealed classes para controle de hierarquia

**Estrutura do Repositório:**
- SistemaPedidos (projeto principal)
- SistemaPedidos.Tests (testes unitários)
- README conciso
- Documento de design (Fases 1 e 2)

**Qualidade:**
- Projeto compila sem erros
- Todos os testes passam (11/11)
- Código organizado e limpo

---

## Slide 16: Lições Aprendidas

**Herança não é sempre a solução:**
Usar herança para tudo leva a hierarquias complexas e inflexíveis. Composição oferece alternativa poderosa.

**LSP é fundamental:**
Respeitar o princípio de substituição garante polimorfismo verdadeiro e código mais robusto.

**Delegates são poderosos:**
Em C#, delegates permitem composição elegante sem necessidade de interfaces formais para casos simples.

**Template Method funciona:**
Definir ritual fixo na base com ganchos virtuais é padrão eficaz para especialização controlada.

**Testes validam design:**
Testes de LSP e composição comprovam que as decisões de arquitetura estão corretas.

---

## Slide 17: Conclusão

**Objetivo Alcançado:**

Implementamos um sistema de pedidos que:
- Diferencia pedidos nacionais e internacionais através de herança controlada
- Permite políticas flexíveis de frete e promoção através de composição
- Respeita o Princípio da Substituição de Liskov
- Evita explosão de subclasses
- É testável e extensível

**Conceitos Aplicados:**
- Herança por especialização de ritual
- Composição com delegates
- Template Method Pattern
- LSP (Liskov Substitution Principle)
- SOLID principles

**Resultado:** Código limpo, robusto e fácil de manter, demonstrando boas práticas de orientação a objetos.

---

## Informações Adicionais para IA de Slides

**Tom da Apresentação:** Técnico mas acessível, com foco em decisões de design e justificativas

**Elementos Visuais Sugeridos:**
- Diagramas de classe mostrando Pedido → PedidoNacional/PedidoInternacional
- Ilustrações do fluxo Validar → Calcular → Emitir
- Comparação visual entre hierarquia de herança vs composição
- Exemplos de código curtos e objetivos
- Ícones representando Nacional (bandeira Brasil) e Internacional (globo)

**Cores Sugeridas:**
- Azul para herança e estrutura base
- Verde para composição e flexibilidade
- Amarelo/laranja para destaques e pontos importantes

**Duração Estimada:** 5-6 slides principais + slides de apoio = apresentação de 15-20 minutos

