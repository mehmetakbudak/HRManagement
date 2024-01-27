using System.ComponentModel;

namespace Chinook.Storage.Enums
{
    public enum CommentStatus
    {
        [Description("Onay Bekliyor")]
        WaitingforApproval = 1,
        [Description("Onaylandı")]
        Approved,
        [Description("Reddedildi")]
        Rejected
    }
}
