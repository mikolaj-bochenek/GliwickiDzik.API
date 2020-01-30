using System.Threading.Tasks;
using GliwickiDzik.Data;

namespace GliwickiDzik.API.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _dataContext;

        public UnitOfWork(DataContext dataContext)
        {
            _dataContext = dataContext;
            Messages = new MessageRepository(_dataContext);
        }
        public IMessageRepository Messages { get; private set; }

        public async Task<bool> SaveAllAsync()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }
        
        public void Dispose()
        {
            _dataContext.Dispose();
        }
    }
}