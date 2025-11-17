using FluentValidation;
using Nexa.Accounting.Shared.Consts;
using Nexa.Accounting.Shared.Services;
using Nexa.BuildingBlocks.Domain.Consts;
using Nexa.Transactions.Domain.Transfers;

namespace Nexa.Transactions.Application.Extensions
{
    public static class FluentValidationExtensions
    {
        public static IRuleBuilderOptions<T,string?> ValidateWallet<T>(this IRuleBuilder<T,string?> ruleBuilder ,
               IWalletService walletService
            )
        {
            return ruleBuilder
               .NotNull()
                .WithErrorCode(GlobalErrorConsts.Required.Code)
                .WithMessage(GlobalErrorConsts.Required.Message)
                .MustAsync(async (x, ct) =>
                {
                    var wallet = await walletService.GetWalletById(x, ct);
                    return wallet != null;
                })
                .WithErrorCode(WalletErrorConsts.WalletNotExist.Code)
                .WithMessage(WalletErrorConsts.WalletNotExist.Message)
                .MaximumLength(TransferTableConsts.WalletIdLength)
                .WithErrorCode(GlobalErrorConsts.MaxLength.Code)
                .WithMessage(string.Format(GlobalErrorConsts.MaxLength.Message, TransferTableConsts.WalletIdLength));
        }


        public static IRuleBuilderOptions<T, string?> ValidateFundingResoruce<T>(this IRuleBuilder<T, string?> ruleBuilder,
                IFundingResourceService fundingResourceService
            )
        {
            return ruleBuilder
                .NotNull()
                .WithErrorCode(GlobalErrorConsts.Required.Code)
                .WithMessage(GlobalErrorConsts.Required.Message)
                .MustAsync(async (x, ct) =>
                {
                    var fundingResource = await fundingResourceService.GetFundingResourceById(x, ct);
                    return fundingResource != null;
                })
                .WithErrorCode(BankAccountErrorConsts.BankAccountNotExist.Code)
                .WithMessage(BankAccountErrorConsts.BankAccountNotExist.Message)
                .MaximumLength(TransferTableConsts.WalletIdLength)
                .WithErrorCode(GlobalErrorConsts.MaxLength.Code)
                .WithMessage(string.Format(GlobalErrorConsts.MaxLength.Message, TransferTableConsts.WalletIdLength));
        }

        public static IRuleBuilderOptions<T, decimal> ValidateMoney<T>(this IRuleBuilder<T, decimal> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThan(0)
                .WithErrorCode(GlobalErrorConsts.GreaterThan.Code)
                .WithMessage(string.Format(GlobalErrorConsts.GreaterThan.Message, 0));
        }

    }
}
