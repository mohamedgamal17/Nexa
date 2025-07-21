
namespace Nexa.CustomerManagement.Domain.Customers
{
    public enum VerificationState
    {
        Pending = 0,
        InReview = 5,
        Verified = 10,
        Rejected = 15,
        Expired = 20
    }
}
