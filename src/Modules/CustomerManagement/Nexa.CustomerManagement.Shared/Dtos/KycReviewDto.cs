using Nexa.BuildingBlocks.Domain.Dtos;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Shared.Dtos
{
    public class KycReviewDto : EntityDto<string>
    {
        public string CustomerId { get; set; }
        public string KycCheckId { get; set; }
        public string? KycLiveVideoId { get; set; }
        public KycReviewStatus Status { get; set; }
        public KycReviewOutcome? Outcome { get; set; }
    }
}
