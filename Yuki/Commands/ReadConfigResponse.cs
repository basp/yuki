namespace Yuki.Commands
{
    using System.Diagnostics.Contracts;
    using Newtonsoft.Json;

    public class ReadConfigResponse<T>
    {
        public ReadConfigResponse(T config)
        {
            Contract.Requires(config != null);

            this.Config = config;
        }

        [JsonProperty(PropertyName = "config")]
        public T Config { get; private set; }
    }
}
