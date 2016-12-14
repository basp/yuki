namespace Yuki.Commands
{
    using System;
    using System.IO;
    using Newtonsoft.Json;
    using Optional;

    using static Optional.Option;

    public class ReadConfigCommand : ICommand<ReadConfigRequest, ReadConfigResponse<IProjectConfig>, Exception>
    {
        public Option<ReadConfigResponse<IProjectConfig>, Exception> Execute(ReadConfigRequest request)
        {
            try
            {
                var json = File.ReadAllText(request.ConfigFile);
                var cfg = JsonConvert.DeserializeObject<DefaultConfig>(json);
                return Some<ReadConfigResponse<IProjectConfig>, Exception>(new ReadConfigResponse<IProjectConfig>(cfg));
            }
            catch (Exception ex)
            {
                return None<ReadConfigResponse<IProjectConfig>, Exception>(ex);
            }
        }
    }
}
