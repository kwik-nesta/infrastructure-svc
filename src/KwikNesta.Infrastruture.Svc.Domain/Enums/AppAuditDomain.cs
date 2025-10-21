using System.ComponentModel;

namespace KwikNesta.Contracts.Enums
{
    public enum AppAuditDomain
    {
        [Description("User identity, profile, and account lifecycle")]
        Identity = 1,

        [Description("System operations, configuration, and background jobs")]
        System,

        [Description("Property listings and catalog changes")]
        Property,

        [Description("Booking lifecycle and application events")]
        Booking,

        [Description("Payments, refunds, and deposits")]
        Payment,

        [Description("Lease or tenancy agreements and lifecycle")]
        Lease,

        [Description("Notifications and in-app messaging")]
        Communication,

        [Description("Security events, access, and compliance")]
        Security
    }
}