using Catalog.Domain.Entities;

namespace Catalog.Domain.Repositories;
public interface IProdutoRepository
{
    Task<Produto?> ObterPorIdAsync(long id);
    Task<IEnumerable<Produto>> ObterTodosAsync();
    Task<IEnumerable<Produto>> ObterPorCategoriaAsync(string nomeCategoria);
    Task<IEnumerable<Produto>> BuscarAsync(string termo);
    Task AdicionarAsync(Produto produto);
    Task AtualizarAsync(Produto produto);
    Task RemoverAsync(Produto produto);
}
