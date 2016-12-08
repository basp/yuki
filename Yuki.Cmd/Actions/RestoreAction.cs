namespace Yuki.Actions
{
    public class RestoreAction : IAction<RestoreArgs>
    {
        private readonly Context ctx;

        public RestoreAction(Context ctx)
        {
            this.ctx = ctx;
        }

        public void Execute(RestoreArgs args)
        {
            var connectionString = $"Server=";
        }
    }
}
