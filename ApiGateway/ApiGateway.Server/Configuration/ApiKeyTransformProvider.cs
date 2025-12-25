using Microsoft.Extensions.Options;
using Yarp.ReverseProxy.Transforms;
using Yarp.ReverseProxy.Transforms.Builder;

namespace Manager.ApiGateway.Server.Configuration;

public class ApiKeyTransformProvider(
    IOptions<ApiKeysOptions> options
) : ITransformProvider
{
    private readonly ApiKeysOptions apiKeysOptions = options.Value;

    public void ValidateRoute(TransformRouteValidationContext context) { }
    public void ValidateCluster(TransformClusterValidationContext context) { }

    public void Apply(TransformBuilderContext context)
    {
        if (context.Cluster == null)
        {
            return;
        }

        var clusterId = context.Cluster.ClusterId;
        var apiKey = apiKeysOptions.GetApiKey(clusterId);
        context.AddRequestHeader("X-Api-Key", apiKey);
    }
}