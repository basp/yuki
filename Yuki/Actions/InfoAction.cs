namespace Yuki.Actions
{
    using Maybe;
   
    public class InfoAction : IAction<InfoArgs, InfoResponse>
    {
        private readonly Context ctx;

        public InfoAction(Context ctx)
        {
            this.ctx = ctx;
        }

        public IMaybeError<InfoResponse> Execute(InfoArgs args)
        {
            var info = new InfoResponse()
            { 
                Config = this.ctx.Config  
            };

            return MaybeError.Create(info);
        }
    }
}
