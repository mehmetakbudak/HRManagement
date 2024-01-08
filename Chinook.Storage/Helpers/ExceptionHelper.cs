using Chinook.Storage.Models;

namespace Chinook.Storage.Helpers
{
    public static class ExceptionHelper
    {
        public static ServiceReturnModel CreateReturnModel(this ServiceResult serviceResult)
        {
            return new ServiceReturnModel
            {
                Data = serviceResult.Data,
                Exceptions = serviceResult.Exceptions,
                Message = serviceResult.Message
            };
        }
    }
}
