namespace Catalog.Domain.Entities;
public class Produto : BaseEntity
{
    public string Nome { get; private set; } = string.Empty;
    public string Descricao { get; private set; } = string.Empty;
    public decimal Preco { get; private set; }
    public bool Disponivel { get; private set; }
    public Categoria Categoria { get; private set; }

    protected Produto() { }

    public Produto(string nome, string descricao, decimal preco, Categoria categoria)
    {
        Nome = nome;
        Descricao = descricao;
        Preco = preco;
        Disponivel = true;
        Categoria = categoria;
    }
    public void Atualizar(string nome, string descricao, decimal preco, Categoria categoria)
    {
        Nome = nome;
        Descricao = descricao;
        Preco = preco;
        Categoria = categoria;
    }
    public void TornarIndisponivel() => Disponivel = false;
    public void TornarDisponivel() => Disponivel = true;
}