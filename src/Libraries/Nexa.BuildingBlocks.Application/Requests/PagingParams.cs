using FluentValidation;
using Nexa.BuildingBlocks.Domain.Consts;

namespace Nexa.BuildingBlocks.Application.Requests
{
    public class PagingParams
    {
        public int Skip { get; set; } = 0;
        public int Length { get; set; } = 10;
    }

    public class PagingParamasValidator<T> : AbstractValidator<PagingParams> where T : PagingParams
    {
        public PagingParamasValidator()
        {
            RuleFor(x => x.Skip)
                .GreaterThanOrEqualTo(0)
                .WithErrorCode(string.Format(GlobalErrorConsts.GreaterThanOrEqualTo.Code))
                .WithMessage(string.Format(GlobalErrorConsts.GreaterThanOrEqualTo.Message, 0));

            RuleFor(x => x.Length)
                .GreaterThan(0)
                .WithErrorCode(GlobalErrorConsts.GreaterThan.Code)
                .WithMessage(string.Format(GlobalErrorConsts.GreaterThan.Message, 0))
                .LessThanOrEqualTo(100)
                .WithErrorCode(GlobalErrorConsts.GreaterThan.Code)
                .WithMessage(string.Format(GlobalErrorConsts.GreaterThan.Message, 100));
        }
    }

}
