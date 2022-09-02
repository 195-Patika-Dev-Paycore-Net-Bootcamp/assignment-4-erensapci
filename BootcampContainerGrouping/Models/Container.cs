namespace WasteCollectionSystem.Models
{

    //Model class where the properties of the container class are defined as public virtual
    public class Container
    {
        public virtual long Id { get; set; }
        public virtual string ContainerName { get; set; }
        public virtual double Latitude { get; set; }
        public virtual double Longitude { get; set; }
        public virtual long VehicleId { get; set; }
    }
}