using System;
using System.Threading.Tasks;

namespace GliwickiDzik.API.Data
{
    public interface IUnitOfWork : IDisposable
    {
        ICommentRepository Comments { get; }
        IMessageRepository Messages { get; }
        Task<bool> SaveAllAsync();
    }
}