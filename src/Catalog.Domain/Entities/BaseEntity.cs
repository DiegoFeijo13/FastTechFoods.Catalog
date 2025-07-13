namespace Catalog.Domain.Entities;
public abstract class BaseEntity
{
    public Guid Id { get; init; } = new Guid();
    public DateTime DataCriacao { get; init; } = DateTime.UtcNow;
    public DateTime? DataAtualizacao { get; private set; }

    public void AtualizarInfo()
    {
        DataAtualizacao = DateTime.UtcNow;
    }
}
