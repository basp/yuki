namespace Yuki.Data
{
    public class Tag : IEntity
    {
        public int Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public int WorkspaceId
        {
            get;
            set;
        }
    }
}
