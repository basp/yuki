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

        public Client GetClientById(int clientId)
        {
            return this.context.Clients
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == clientId);
        }

        public void DeleteClient(int clientId)
        {
            throw new NotImplementedException();
        }

        public void InsertClient(Client client)
        {
            this.context.Clients.Add(client);
            this.context.SaveChanges();
        }

        public void UpdateClient(Client client)
        {
            this.context.Clients.Attach(client);
            this.context.Entry(client).State = EntityState.Modified;
            this.context.SaveChanges();
        }
    }
}
