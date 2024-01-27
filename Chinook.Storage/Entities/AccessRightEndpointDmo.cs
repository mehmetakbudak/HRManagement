using Chinook.Storage.Enums;

namespace Chinook.Storage.Entities
{
    public class AccessRightEndpointDmo: BaseModel
    {
        public int AccessRightId { get; set; }

        public AccessRightDmo AccessRight { get; set; }

        public string Endpoint { get; set; }

        public MethodType Method { get; set; }

        public int RouteLevel { get; set; }
    }
}

