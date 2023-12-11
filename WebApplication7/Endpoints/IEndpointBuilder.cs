namespace WebApplication7.Endpoints
{
    public interface IEndpointBuilder
    {
        static abstract IEndpointRouteBuilder RegisterEndpoints(IEndpointRouteBuilder builder);
    }
}