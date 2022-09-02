using WasteCollectionSystem.Models;
using System.Linq;
using System.Threading.Tasks;

namespace WasteCollectionSystem.Context
{
    public interface IMapperSession
    {
        //The interface with mapping functions that we need to define
        void BeginTransaction();
        void Commit();
        void Rollback();
        void CloseTransaction();
        void Save(Vehicle entity);
        void Update(Vehicle entity);
        void Delete(Vehicle entity);

        void Save(Container entity);
        void Update(Container entity);
        void Delete(Container entity);

        IQueryable<Vehicle> Vehicles { get; }
        IQueryable<Container> Containers { get; }
    }
}
