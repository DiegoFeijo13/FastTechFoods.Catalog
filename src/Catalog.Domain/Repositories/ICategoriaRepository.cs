using Catalog.Domain.Entities;

namespace Catalog.Domain.Repositories;
public interface ICategoriaRepository
{
    Task<Categoria?> ObterPorIdAsync(Guid id);
    Task<Categoria?> ObterPorNomeAsync(string nome);
    Task<IEnumerable<Categoria>> ObterTodosAsync();
    Task AdicionarAsync(Categoria categoria);
    Task AtualizarAsync(Categoria categoria);
    Task RemoverAsync(Categoria categoria);
}
