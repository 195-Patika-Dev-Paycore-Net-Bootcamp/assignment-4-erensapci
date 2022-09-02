using WasteCollectionSystem.Models;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;


namespace BootcampWasteCollectionSystem.Mapping
{
    public class ContainerMap : ClassMapping<Container>
    {
        //Necessary mapping protocols have been implemented within the ContainerMap constructor method
        public ContainerMap()
        {
            //The Id field is matched to the specified column to be of type long
            Id(x => x.Id, x =>
            {
                x.Type(NHibernateUtil.Int64);
                x.Column("id");
                x.Generator(Generators.Increment);
            });

            //ContainerName column is set to string type and maximum 50 characters
            Property(b => b.ContainerName, x =>
            {
                x.Length(50);
                x.Column("containername");
                x.Type(NHibernateUtil.String);
                x.NotNullable(true);
            });

            //latitude column length set to 10 and matched
            Property(b => b.Latitude, x =>
            {
                x.Length(10);
                x.Column("latitude");
                x.Type(NHibernateUtil.Double);
                x.NotNullable(true);
            });

            ////longitude column length set to 10 and matched
            Property(b => b.Longitude, x =>
            {
                x.Length(10);
                x.Column("longitude");
                x.Type(NHibernateUtil.Double);
                x.NotNullable(true);
            });
            //VehicleId column is matched to long data type
            Property(b => b.VehicleId, x =>
            {
                x.Column("vehicleid");
                x.Type(NHibernateUtil.Int64);
                x.NotNullable(true);
            });

            Table("containers");
        }
    }
}