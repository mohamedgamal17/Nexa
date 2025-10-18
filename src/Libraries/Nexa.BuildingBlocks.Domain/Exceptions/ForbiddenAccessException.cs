namespace Nexa.BuildingBlocks.Domain.Exceptions
{
    public class ForbiddenAccessException : Exception
    {
        public ForbiddenAccessException()
        {
            
        }
     

        public ForbiddenAccessException(string code) : base(code)
        {
        }
    }
}
