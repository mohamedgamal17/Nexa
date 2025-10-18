using FluentAssertions;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;
namespace Nexa.Application.Tests.Extensions
{
    public static class ResultAssertionExtensions
    {
        public static void ShouldBeSuccess<T>(this Result<T> result)
        {
            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
            result.Value.Should().NotBeNull();
        }

        public static void ShoulBeFailure<T>(this Result<T> result, Type exceptionType , NexaError? error = null)
        {
            result.IsFailure.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
            result.Exception.Should().BeOfType(exceptionType);

            if(error != null)
            {
                var exception = (NexaException)result.Exception;

                exception.Should().NotBeNull();

                exception!.Code.Should().Be(error.Code);
            }
        }
    }
}
