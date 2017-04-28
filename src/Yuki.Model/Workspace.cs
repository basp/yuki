namespace Yuki.Model
{
    using System.Collections.Generic;

    public class Workspace
    {
        public Workspace()
        {
            this.Projects = new List<Project>();
            this.Timers = new List<Timer>();
        }

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

        public virtual ICollection<Project> Projects
        {
            get;
            private set;
        }

        public virtual ICollection<Timer> Timers
        {
            get;
            private set;
        }
    }
}