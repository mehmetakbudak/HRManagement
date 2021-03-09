using Chinook.Model.Models;

namespace Chinook.Model.Helpers
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
