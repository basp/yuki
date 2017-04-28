namespace Yuki.Model
{
    public class WorkspaceUser
    {
        public int Id
        {
            get;
            private set;
        }

        public int WorkspaceId
        {
            get;
            set;
        }

        public int UserId
        {
            get;
            set;
        }

        public bool IsActive
        {
            get;
            set;
        }
    }
}
