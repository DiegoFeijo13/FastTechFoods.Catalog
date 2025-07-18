using Catalog.Domain.Events;

namespace Catalog.Application.Events;
public class ProdutoAtualizadoEvent : IProdutoAtualizadoEvent
{
    public Guid Id { get; init; }
    public string Nome { get; init; } = string.Empty;
    public string Categoria { get; init; } = string.Empty;
    public decimal Preco { get; init; }
    public bool Disponibilidade { get; init; }
}
