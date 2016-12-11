namespace Yuki.Commands
{
    using System;
    using PowerArgs;

    public class InsertScriptRunRequest : RepositoryRequest, IScriptRunRecord<int>
    {
        [ArgRequired]
        public bool IsOneTimeScript { get; set; }

        [ArgRequired]
        public string ScriptName { get; set; }

        [ArgRequired]
        public int VersionId { get; set; }

        [ArgIgnore]
        public string Sql { get; set; }

        [ArgIgnore]
        public string Hash { get; set; }

        [ArgIgnore]
        public string EnteredBy { get; set; }
    }
}
