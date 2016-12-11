namespace Yuki.Commands
{
    using System;
    using PowerArgs;

    public class InsertScriptRunErrorRequest : IScriptRunErrorRecord
    {
        [ArgRequired]
        public string ErrorMessage { get; set; }

        [ArgRequired]
        public string RepositoryPath { get; set; }

        [ArgRequired]
        public string RepositoryVersion { get; set; }

        [ArgRequired]
        public string ScriptName { get; set; }

        [ArgRequired]
        public string Sql { get; set; }

        [ArgRequired]
        public string SqlErrorPart { get; set; }

        [ArgIgnore]
        public string EnteredBy { get; set; }
    }
}
