using DotNetArchitecture.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetArchitecture.Repository.Base
{
    public interface IUnitOfWork : IDisposable
    {
        GenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseModel;
        Task Save();
        void Detach(object obj);
        DBContext GetContext();
    }
}
