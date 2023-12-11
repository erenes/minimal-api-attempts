namespace Api.Endpoints;

public interface IEndpointBuilder
{
    static abstract IEndpointRouteBuilder RegisterEndpoints(IEndpointRouteBuilder builder);
}