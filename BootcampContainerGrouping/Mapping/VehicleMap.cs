using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using WasteCollectionSystem.Models;

namespace BootcampWasteCollectionSystem.Mapping
{
    public class VehicleMap : ClassMapping<Vehicle>
    {

        //Mapping operations performed within the VehicleMap constructor method
        public VehicleMap()
        {
            //The Id field is matched to the specified column to be of type long
            Id(x => x.Id, x =>
            {
                x.Type(NHibernateUtil.Int64);
                x.Column("id");
                x.Generator(Generators.Increment);
            });

            ////VehicleName column is set to string type and maximum 50 characters
            Property(b => b.VehicleName, x =>
            {
                x.Length(50);
                x.Column("vehiclename");
                x.Type(NHibernateUtil.String);
                x.NotNullable(true);
            });

            //The VehiclePlate field is matched as a string of 14 units long.
            Property(b => b.VehiclePlate, x =>
            {
                x.Length(14);
                x.Column("vehicleplate");
                x.Type(NHibernateUtil.String);
                x.NotNullable(true);
            });

            Table("vehicles");
        }

    }
}