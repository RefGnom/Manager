using System;
using System.Collections.Generic;
using Manager.Core.Common.DependencyInjection.Attributes;

namespace Manager.ApiGateway.Server.Configuration;

[OptionPath("ApiKeys")]
public class ApiKeysOptions
{
    // ReSharper disable once CollectionNeverUpdated.Global
    public required Dictionary<string, string> ClusterToApiKeyDictionary { get; init; } = new();

    public string GetApiKey(string cluster) => ClusterToApiKeyDictionary.TryGetValue(cluster, out var apiKey)
        ? apiKey
        : throw new ArgumentException($"Не нашли апи ключ в настройках для кластера {cluster}");
}