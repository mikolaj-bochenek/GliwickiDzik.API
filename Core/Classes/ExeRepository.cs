using GliwickiDzik.API.Core.Interfaces;
using GliwickiDzik.API.Data;
using GliwickiDzik.API.Models;
using GliwickiDzik.Data;

namespace GliwickiDzik.API.Core.Classes
{
    public class ExeRepository : GenericRepository<ExerciseModel>, IExeRepository
    {
        public ExeRepository(DataContext dataContext) : base(dataContext) {}
    }
}