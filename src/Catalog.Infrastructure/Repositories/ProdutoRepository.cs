using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;
using Catalog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Repositories;
internal class ProdutoRepository : IProdutoRepository
{
    private readonly AppDbContext _dbContext;

    public ProdutoRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AdicionarAsync(Produto produto)
    {
        await _dbContext.Produtos.AddAsync(produto);
    }

    public Task AtualizarAsync(Produto produto)
    {
        _dbContext.Produtos.Update(produto);
        return Task.CompletedTask;
    }
    public Task RemoverAsync(Produto produto)
    {
        _dbContext.Produtos.Remove(produto);
        return Task.CompletedTask;
    }

    public async Task<Produto?> ObterPorIdAsync(Guid id)
    {
        return await _dbContext.Produtos.AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Produto>> ObterTodosAsync()
    {
        return await _dbContext.Produtos.AsNoTracking()
            .Take(100)
            .ToListAsync();
    }
    public async Task<IEnumerable<Produto>> BuscarAsync(string termo)
    {
        return await _dbContext.Produtos.AsNoTracking()
             .Where(p => p.Nome.Contains(termo) || p.Descricao.Contains(termo))
             .ToListAsync();
    }

    public async Task<IEnumerable<Produto>> ObterPorCategoriaAsync(Guid categoriaId)
    {
        return await _dbContext.Produtos.AsNoTracking()
            .Where(p => p.CategoriaId == categoriaId)
            .Take(100)
            .ToListAsync();
    }
}
