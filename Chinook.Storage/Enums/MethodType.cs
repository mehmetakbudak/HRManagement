using System.ComponentModel;

namespace Chinook.Storage.Enums
{
    public enum MethodType
    {
        [Description("GET")]
        GET = 1,
        [Description("POST")]
        POST,
        [Description("PUT")]
        PUT,
        [Description("DELETE")]
        DELETE
    }
}
