using Grpc.Core;
using Grpc.Core.Interceptors;
using JMS.Application.Helpers;
using JMS.Domain.ErrorTreatment;

namespace GrpcApi.ErrorHandler;

public class ExceptionInterceptorGRPC : Interceptor
{

    private readonly ICustomLogger _logger;

    public ExceptionInterceptorGRPC( ICustomLogger logger)
    {
        _logger = logger;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
       TRequest request,
       ServerCallContext context,
       UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (RpcException )
        {
            throw;
        }
        catch (ValidationException ex)
        {
            _logger.Log("Validation Error", ex);

            // Customize gRPC error here
            throw new RpcException(new Status(StatusCode.InvalidArgument, ex.ErrorCode));
        }
        catch (Exception ex)
        {
            _logger.Log("Unhandled exception in gRPC call",ex);

            // Customize gRPC error here
            throw new RpcException(new Status(StatusCode.Internal, "An error occurred"));
        }
    }

}
