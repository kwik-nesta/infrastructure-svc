using System.ComponentModel;

namespace KwikNesta.Contracts.Enums
{
    public enum AppAuditAction
    {
        // Identity
        [Description("Logged In")] LoggedIn = 1,
        [Description("Logged Out")] LoggedOut,
        [Description("Updated Profile")] UpdatedProfile,
        [Description("Deactivated Account")] DeactivatedAccount,
        [Description("Reactivated Account")] ReactivatedAccount,
        [Description("Suspended Account")] SuspendedAccount,
        [Description("Changed Password")] ChangedPassword,

        // System
        [Description("Data Load Requested")] DataLoadRequest,
        [Description("Data Load Completed")] DataLoadCompleted,
        [Description("Configuration Updated")] ConfigurationUpdated,
        [Description("System Job Executed")] SystemJobExecuted,

        // Property
        [Description("Property Created")] PropertyCreated,
        [Description("Property Updated")] PropertyUpdated,
        [Description("Property Deactivated")] PropertyDeactivated,
        [Description("Property Reactivated")] PropertyReactivated,
        [Description("Property Removed")] PropertyRemoved,

        // Booking
        [Description("Booking Created")] BookingCreated,
        [Description("Booking Updated")] BookingUpdated,
        [Description("Booking Cancelled")] BookingCancelled,
        [Description("Application Submitted")] ApplicationSubmitted,
        [Description("Application Approved")] ApplicationApproved,
        [Description("Application Rejected")] ApplicationRejected,

        // Payment
        [Description("Payment Initiated")] PaymentInitiated,
        [Description("Payment Successful")] PaymentSuccessful,
        [Description("Payment Failed")] PaymentFailed,
        [Description("Refund Issued")] RefundIssued,
        [Description("Deposit Collected")] DepositCollected,
        [Description("Deposit Refunded")] DepositRefunded,

        // Lease
        [Description("Lease Generated")] LeaseGenerated,
        [Description("Lease Signed")] LeaseSigned,
        [Description("Lease Renewed")] LeaseRenewed,
        [Description("Lease Terminated")] LeaseTerminated,

        // Communication
        [Description("Notification Sent")] NotificationSent,
        [Description("Message Sent")] MessageSent,
        [Description("Message Received")] MessageReceived,

        // Security
        [Description("Failed Login Attempt")] FailedLoginAttempt,
        [Description("Suspicious Activity Detected")] SuspiciousActivityDetected,
        [Description("Data Exported")] DataExported,
        [Description("Data Accessed")] DataAccessed
    }
}
