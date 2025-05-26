using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp_Tak4.Data;

namespace Repositories
{
    public abstract class BaseRepository<T> where T : class
    {
        protected readonly Task4DbContext _context;

        public BaseRepository(Task4DbContext dbContext)
        {
            _context = dbContext;
        }

        public abstract Task<IEnumerable<T>> GetAll();

        public abstract Task Create(T entity);

        public abstract Task Delete(T entity);

        public abstract Task Update(T entity);
    }
}
