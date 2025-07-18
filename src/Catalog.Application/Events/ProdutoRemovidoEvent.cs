using Catalog.Domain.Events;

namespace Catalog.Application.Events;
public class ProdutoRemovidoEvent : IProdutoRemovidoEvent
{
    public Guid Id { get; init; }
}
