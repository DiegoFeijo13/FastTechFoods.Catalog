namespace Catalog.Domain.Entities;
public abstract class BaseEntity
{
    public long Id { get; init; }
    public DateTime DataCriacao { get; init; } = DateTime.UtcNow;
    public DateTime? DataAtualizacao { get; private set; }

    public void AtualizarInfo()
    {
        DataAtualizacao = DateTime.UtcNow;
    }
}
