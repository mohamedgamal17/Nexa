using Nexa.BuildingBlocks.Domain;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Domain.Review
{
    public class KycReview : BaseEntity
    {
        public string CustomerId { get; set; }
        public string KycCheckId { get; set; }
        public string? KycLiveVideoId { get; set; }
        public KycReviewType Type { get; set; }
        public KycReviewStatus Status { get; set; }
        public KycReviewOutcome? Outcome { get; set; }
        private KycReview() { }
        public KycReview(string customerId, string kycCheckId, string? kycLiveVideoId, KycReviewType type)
        {
            CustomerId = customerId;
            KycCheckId = kycCheckId;
            KycLiveVideoId = kycLiveVideoId;
            Type = type;
        }
        public void Complete(KycReviewOutcome outcome)
        {
            if(Status == KycReviewStatus.Pending)
            {
                Status = KycReviewStatus.Completed;
                Outcome = outcome;
            }
        }
        public static KycReview Info(string customerId ,string kycCheckId)
        {
            return new KycReview(customerId, kycCheckId, null, KycReviewType.Info);
        }

        public static KycReview Document(string customerId , string kycCheckId , string kycLiveVideoId)
        {
            return new KycReview(customerId, kycCheckId, kycLiveVideoId, KycReviewType.Document);
        }

    }
}
