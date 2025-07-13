namespace Catalog.Domain.Entities;
public class Categoria : BaseEntity
{
    public string Nome { get; private set; } = string.Empty;
    protected Categoria() { }

    public Categoria(string nome)
    {
        Nome = nome;
    }

    public void EditarCategoria(string novoNome)
    {
        Nome = novoNome;
        AtualizarInfo();
    }
}
