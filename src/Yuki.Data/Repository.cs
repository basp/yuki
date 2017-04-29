namespace Yuki.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class Repository
    {
        private readonly DataContext context;

        public Repository(DataContext context)
        {
            this.context = context;
        }
    }
}
