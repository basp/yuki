namespace Yuki.Data
{
    using System.Data.Entity;
    using System.Linq;

    public class Repository<T> where T : class, IEntity
    {
        protected readonly DataContext context;

        public Repository(DataContext context)
        {
            this.context = context;
        }

        public void Delete(T entity)
        {
            this.context.Set<T>().Attach(entity);
            this.context.Entry(entity).State = EntityState.Deleted;
            this.context.SaveChanges();
        }

        public T GetById(int id)
        {
            return this.context.Set<T>()
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == id);
        }

        public void Insert(T entity)
        {
            this.context.Set<T>().Add(entity);
            this.context.SaveChanges();
        }

        public void Update(T entity)
        {
            this.context.Set<T>().Attach(entity);
            this.context.Entry(entity).State = EntityState.Modified;
            this.context.SaveChanges();
        }
    }
}
