# Texto para Geração de Slides - Sistema de Pedidos (9 Slides)

**Versão Simplificada para Apresentação**

---

## Slide 1: Apresentação

**Título:** Sistema de Pedidos - Nacional vs Internacional

**Autor:** Bruno Moura Mathias Fernades Simão

**O que é:** Um sistema que processa pedidos de venda usando duas técnicas importantes:
- **Herança** para diferenciar pedidos nacionais e internacionais
- **Composição** para adicionar frete e promoções de forma flexível

**Resultado:** Código organizado, sem repetição e fácil de expandir

---

## Slide 2: O Problema que Resolvi

**Cenário:**
Uma loja vende produtos no Brasil e no exterior. Os dois tipos de pedido:

✅ **Fazem a mesma coisa:**
- Validar pedido → Calcular total → Emitir recibo

❌ **Mas são diferentes em:**
- Nacional emite NF-e / Internacional emite Commercial Invoice
- Internacional tem taxa de importação, custos extras e câmbio
- Frete e promoção variam independente do tipo

**Desafio:** Como fazer sem repetir código e permitir combinações flexíveis?

---

## Slide 3: Solução Parte 1 - Herança (para o TIPO do pedido)

**Por que Herança?**
Um pedido nacional SEMPRE será nacional. Um internacional SEMPRE será internacional.
Isso é a "essência" do pedido.

**Como funciona:**
- **Classe base `Pedido`** = define o ritual fixo (Validar → Calcular → Emitir)
- **`PedidoNacional`** = personaliza para Brasil (emite NF-e)
- **`PedidoInternacional`** = personaliza para exterior (emite Invoice, adiciona taxas)

**Vantagem:** O código sempre segue a mesma ordem, mas cada tipo faz do seu jeito.

---

## Slide 4: Solução Parte 2 - Composição (para POLÍTICAS flexíveis)

**Por que Composição?**
Frete e promoção NÃO definem o tipo do pedido. São "extras" que combinam livremente.

**Problema se usasse herança:**
- PedidoNacionalComFreteFixoECupom
- PedidoNacionalComFretePercentualSemPromocao
- E mais 20 classes diferentes... 😱

**Solução:**
Uso "peças encaixáveis" (delegates) que podem ser combinadas:
- **Frete:** Fixo ou Percentual
- **Promoção:** Nenhuma ou Cupom

**Resultado:** Qualquer combinação possível sem criar classes novas!

---

## Slide 5: Como Funciona na Prática

**Exemplo Real de Uso:**

```csharp
var pedido = new PedidoNacional(
    frete: FreteStrategies.Fixo(10m),
    promocao: PromocaoStrategies.Cupom(20m)
);
pedido.Processar();
```

**O que acontece:**
1. Validar → verifica se tem itens
2. Calcular → subtotal R$100 + frete R$10 - cupom R$20 = R$90
3. Emitir → gera NF-e com total R$90

**Facilidade:** Posso trocar `Fixo` por `Percentual` sem mudar nada no código principal!

---

## Slide 6: Por que Isso é Bom?

**3 Benefícios Principais:**

**1. Não repete código**
- O ritual de processar está em um lugar só
- Cada tipo só modifica o que é diferente

**2. Fácil de expandir**
- Quer adicionar frete expresso? Basta criar `FreteStrategies.Expresso()`
- Não precisa mexer nas classes existentes

**3. Seguro de usar**
- O código sempre funciona da mesma forma
- Não preciso verificar se é nacional ou internacional

---

## Slide 7: Comprovação - Testes

**11 Testes Automatizados (todos passaram ✓)**

**Testes de Herança (LSP):**
- Função genérica aceita qualquer tipo de pedido
- Funciona igual para Nacional e Internacional
- Não preciso fazer verificações manuais de tipo

**Testes de Composição:**
- Frete fixo: R$100 + R$10 = R$110 ✓
- Frete percentual 10%: R$100 + R$10 = R$110 ✓
- Cupom desconto: R$100 - R$20 = R$80 ✓
- Combinações múltiplas funcionam ✓

---

## Slide 8: Resultados do Projeto

**O que entreguei:**

✅ **Código funcional em C#**
- Classe base Pedido
- PedidoNacional e PedidoInternacional
- Estratégias de Frete e Promoção

✅ **Testes validando tudo (11/11 passando)**

✅ **Documentação completa**
- Como funciona (design)
- Por que fiz assim (justificativa)

✅ **Publicado no GitHub**
- Código organizado
- Pronto para usar e expandir

---

## Slide 9: Conclusão - O que Aprendi

**Duas Técnicas, Dois Usos:**

**Herança = para ESSÊNCIA**
- Quando algo "é" de um tipo
- Exemplo: um pedido nacional É nacional (não muda)

**Composição = para COMPORTAMENTO**
- Quando algo "tem" ou "usa" características
- Exemplo: um pedido TEM frete (que pode mudar)

**Lição Principal:**
Não existe solução única. Usar a técnica certa para cada problema deixa o código mais limpo e fácil de manter.

**Repositório GitHub:** https://github.com/brunomouramathias/sistema-pedidos-heranca-composicao

---

## 🎨 Instruções de Design para a IA de Slides

**TEMA:** 
- **Usar tema ESCURO obrigatoriamente**
- Fundo escuro com texto claro
- Cores de destaque: azul, verde, laranja em tons vibrantes

**IMAGENS:**
- **EVITAR imagens de pessoas**
- **EVITAR imagens genéricas de estoque**
- Preferir: diagramas, ícones, ilustrações técnicas simples
- Se usar imagens: apenas ícones de código, setas, boxes, fluxogramas
- Foco no CONTEÚDO, não em decoração

**ESTILO:**
- Minimalista e profissional
- Fonte clara e legível
- Espaçamento adequado
- Diagramas simples de classes (se necessário)

---

## 💡 Dicas para Apresentar (para você lembrar)

**Slide 1:** "Eu fiz um sistema que processa pedidos usando duas técnicas: herança para tipos e composição para políticas"

**Slide 2:** "O problema é que pedidos nacionais e internacionais fazem a mesma coisa mas de forma diferente"

**Slide 3:** "Usei herança porque o tipo do pedido nunca muda - nacional sempre é nacional"

**Slide 4:** "Usei composição para frete e promoção porque eles combinam de várias formas - seria impossível criar uma classe pra cada combinação"

**Slide 5:** "Veja como é simples usar: você monta o pedido escolhendo as peças que quer"

**Slide 6:** "Os benefícios são: não repete código, fácil expandir, seguro usar"

**Slide 7:** "Fiz 11 testes automatizados que provam que tudo funciona"

**Slide 8:** "No final entreguei código funcional, testes passando, documentação e publiquei no GitHub"

**Slide 9:** "A lição é: herança para essência, composição para comportamento - cada uma no lugar certo"

