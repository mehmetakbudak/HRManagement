using System.Linq;
using Chinook.Data.Repository;
using Chinook.Storage.Entities;

namespace Chinook.Service
{
    public interface ITitleService
    {
        IQueryable<TitleDmo> Get();
    }

    public class TitleService : ITitleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TitleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable<TitleDmo> Get()
        {
            return _unitOfWork.Repository<TitleDmo>().GetAll(x => !x.Deleted);
        }
    }
}

