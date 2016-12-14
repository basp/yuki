namespace Yuki.Commands
{
    using System;
    using System.IO;
    using System.Text;
    using NLog;
    using Optional;

    using static Optional.Option;

    using Req = WriteConfigRequest;
    using Res = WriteConfigResponse;

    public class WriteConfigCommand : ICommand<Req, Res, Exception>
    {
        private static readonly string ConfigurationTemplate =
            $"{nameof(Yuki)}.Resources.{nameof(ConfigurationTemplate)}.sql";

        private readonly ILogger log = LogManager.GetCurrentClassLogger();

        public Option<Res, Exception> Execute(Req request)
        {
            try
            {
                var template = typeof(Program)
                    .Assembly
                    .ReadEmbeddedString(ConfigurationTemplate);

                var encoding = Encoding.UTF8;
                var bytes = encoding.GetBytes(template);
                File.WriteAllText(request.File, template, encoding);

                this.log.Debug($"Wrote {request.File} [{bytes.Length} bytes]");

                var res = new Res()
                {
                    ConfigFile = request.File,
                    NumberOfBytes = bytes.Length,
                };

                return Some<Res, Exception>(res);
            }
            catch (Exception ex)
            {
                var msg = $"Could not write config file {request.File}";
                var error = new Exception(msg, ex);
                return None<Res, Exception>(error);
            }
        }
    }
}
