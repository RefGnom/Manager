using System.Collections.Generic;
using System.Linq;

namespace Manager.Core.IntegrationTestsCore.Configuration.ConfigurationActions;

public class ConfigurationActionCollection : List<IConfigurationAction>
{
    public void AddActionWithRemovingExcludedActionTypes(IConfigurationAction action)
    {
        if (action.ExcludedTypes.Length > 0)
        {
            RemoveAll(x => action.ExcludedTypes.Contains(x.Type));
        }

        Add(action);
    }
}