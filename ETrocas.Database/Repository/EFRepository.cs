using ETrocas.Domain.Entities;
using ETrocas.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections;
//logica de como o banco vai acessar e manipular os dados.
namespace ETrocas.Database.Repository
{
    public class EFRepository<T> : IRepository<T> where T : class, IEntity
    {
        //injeção de deprencia 
        private readonly ETrocasDbContext _eTrocasDb;
        private readonly DbSet<T> _dbSet;

        public EFRepository(ETrocasDbContext eTrocasDb)
        {
            _eTrocasDb = eTrocasDb;
            _dbSet = _eTrocasDb.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking()
                                .ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task DeleteAsync(Guid id)
        {
            //acha o id se nao achar/for nulo retorna.
            var entity = await _dbSet.FindAsync(id);
            //remove a entity e salva no banco. O ! esta garantindo para o visual studio que entidade nao e nula
            _dbSet.Remove(entity!);
            await _eTrocasDb.SaveChangesAsync();
        }

        public async Task<bool> ExistAsync(Guid id)
        {
          return await _dbSet.AnyAsync(e => e.Id == id);
        }
    }
}
