using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scanner.Core.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> GetRepository<T>() where T : BaseEntity;
        Task CommitAsync();
        void Commit();
    }
}
