namespace Chinook.Model.Entities
{
    public class AccessRightEndpoint
    {
        public int AccessRightId { get; set; }

        public AccessRight AccessRight { get; set; }

        public string Endpoint { get; set; }

        public string Method { get; set; }

        public bool IsActive { get; set; }

        public bool Deleted { get; set; }
    }
}

