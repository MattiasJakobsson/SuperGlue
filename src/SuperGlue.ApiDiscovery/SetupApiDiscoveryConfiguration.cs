using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SuperGlue.Configuration;

namespace SuperGlue.ApiDiscovery
{
    public class SetupApiDiscoveryConfiguration : ISetupConfigurations
    {
        public IEnumerable<ConfigurationSetupResult> Setup(string applicationEnvironment)
        {
            yield return new ConfigurationSetupResult("superglue.ApiDiscoverySetup", environment =>
            {
                environment.RegisterAll(typeof(IApiSource));
                environment.RegisterAll(typeof(IHandleApi));
                environment.RegisterTransient(typeof(IApi), typeof(DefaultApi));
                environment.RegisterTransient(typeof(IApiRegistry), typeof(DefaultApiRegistry));

                environment.SubscribeTo(ConfigurationEvents.AfterApplicationStart, async x =>
                {
                    var sources = x.ResolveAll<IApiSource>();

                    var definitions = new List<ApiDefinition>();

                    foreach (var source in sources)
                        definitions.AddRange(await source.Find(x));

                    if (definitions.Any())
                        await environment.Resolve<IApiRegistry>().Register(x, definitions.ToArray());
                });
            }, "superglue.ContainerSetup");
        }

        public Task Shutdown(IDictionary<string, object> applicationData)
        {
            return Task.CompletedTask;
        }

        public Task Configure(SettingsConfiguration configuration)
        {
            return Task.CompletedTask;
        }
    }
}