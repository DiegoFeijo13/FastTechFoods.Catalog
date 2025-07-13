using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;
using Catalog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Repositories;
internal class CategoriaRepository : ICategoriaRepository
{
    private readonly AppDbContext _dbContext;

    public CategoriaRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AdicionarAsync(Categoria categoria)
    {
       await _dbContext.Categorias.AddAsync(categoria);
    }

    public Task AtualizarAsync(Categoria categoria)
    {
        _dbContext.Categorias.Update(categoria);
        return Task.CompletedTask;
    } public Task RemoverAsync(Categoria categoria)
    {
        _dbContext.Categorias.Remove(categoria);
        return Task.CompletedTask;
    }
   

    public async Task<Categoria?> ObterPorIdAsync(long id)
    {
        return await _dbContext.Categorias.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Categoria>> ObterTodosAsync()
    {
        return await _dbContext.Categorias.AsNoTracking().ToListAsync();
    }

}
