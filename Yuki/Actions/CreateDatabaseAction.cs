namespace Yuki.Actions
{
    public class CreateDatabaseAction : IAction<CreateDatabaseArgs>
    { 
        public IMaybeError Execute(CreateDatabaseArgs args)
        {
            return new MaybeError();
        }
    }
}
