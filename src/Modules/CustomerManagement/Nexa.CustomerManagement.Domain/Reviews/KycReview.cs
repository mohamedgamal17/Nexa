using Nexa.BuildingBlocks.Domain;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Domain.Reviews
{
    public class KycReview : BaseEntity
    {
        public string CustomerId { get; set; }
        public string KycCheckId { get; set; }
        public string? KycLiveVideoId { get; set; }
        public KycReviewStatus Status { get; set; }
        public KycReviewOutcome? Outcome { get; set; }
        private KycReview() { }
        public KycReview(string customerId, string kycCheckId, string? kycLiveVideoId)
        {
            CustomerId = customerId;
            KycCheckId = kycCheckId;
            KycLiveVideoId = kycLiveVideoId;
        }
        public void Complete(KycReviewOutcome outcome)
        {
            if (Status == KycReviewStatus.Pending)
            {
                Status = KycReviewStatus.Completed;
                Outcome = outcome;
            }
        }
  
    }
}
