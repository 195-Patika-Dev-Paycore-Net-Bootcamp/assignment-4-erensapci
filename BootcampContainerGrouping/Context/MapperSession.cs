using NHibernate;
using WasteCollectionSystem.Models;
using System.Linq;
using System.Threading.Tasks;
using WasteCollectionSystem.Context;

namespace WasteCollectionSystem.Context
{
    public class MapperSession : IMapperSession
    {
        //The Mapper section we implemented from the IMapperSession. the part where the necessary functions are held
        private readonly ISession session;
        private ITransaction transaction;

        public MapperSession(ISession session)
        {
            this.session = session;
        }

        public IQueryable<Vehicle> Vehicles => session.Query<Vehicle>();
        public IQueryable<Container> Containers => session.Query<Container>();


        public void BeginTransaction()
        {
            transaction = session.BeginTransaction();
        }

        public void Commit()
        {
            transaction.Commit();
        }

        public void Rollback()
        {
            transaction.Rollback();
        }

        public void CloseTransaction()
        {
            if (transaction != null)
            {
                transaction.Dispose();
                transaction = null;
            }
        }

        public void Save(Vehicle entity)
        {
            session.Save(entity);
        }
        public void Update(Vehicle entity)
        {
            session.Update(entity);
        }
        public void Delete(Vehicle entity)
        {
            session.Delete(entity);
        }

        public void Save(Container entity)
        {
            session.Save(entity);
        }
        public void Update(Container entity)
        {
            session.Update(entity);
        }
        public void Delete(Container entity)
        {
            session.Delete(entity);
        }
    }
}
