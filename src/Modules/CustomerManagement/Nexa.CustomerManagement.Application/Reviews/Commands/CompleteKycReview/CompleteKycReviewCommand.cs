using Nexa.BuildingBlocks.Application.Requests;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Application.Reviews.Commands.CompleteKycReview
{
    public class CompleteKycReviewCommand : ICommand
    {
        public string KycCheckId { get; set; }

        public KycReviewOutcome Outcome { get; set; }
    }
}
