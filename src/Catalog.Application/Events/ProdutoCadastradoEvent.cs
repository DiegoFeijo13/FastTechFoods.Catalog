using Catalog.Domain.Events;

namespace Catalog.Application.Events;
internal class ProdutoCadastradoEvent : IProdutoCadastradoEvent
{
    public Guid Id { get; init; }
    public string Nome { get; init; } = string.Empty;
    public string Categoria { get; init; } = string.Empty;
    public decimal Preco { get; init; }
}
