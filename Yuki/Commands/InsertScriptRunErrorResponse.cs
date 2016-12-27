namespace Yuki.Commands
{
    public class InsertScriptRunErrorResponse
    {
        public int ScriptRunErrorId { get; set; }

        public string ScriptName { get; set; }

        public string VersionName { get; set; }

        public string SqlErrorPart { get; set; }

        public string ErrorMessage { get; set; }
    }
}
