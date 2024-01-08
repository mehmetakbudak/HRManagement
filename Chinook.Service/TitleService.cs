using System.Linq;
using Chinook.Data.Repository;
using Chinook.Storage.Entities;

namespace Chinook.Service
{
    public interface ITitleService
    {
        IQueryable<Title> Get();
    }

    public class TitleService : ITitleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TitleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable<Title> Get()
        {
            return _unitOfWork.Repository<Title>().GetAll(x => !x.Deleted);
        }
    }
}

