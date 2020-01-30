using System;
using System.Threading.Tasks;

namespace GliwickiDzik.API.Data
{
    public interface IUnitOfWork : IDisposable
    {
         IMessageRepository Messages { get; }
         Task<bool> SaveAllAsync();
    }
}