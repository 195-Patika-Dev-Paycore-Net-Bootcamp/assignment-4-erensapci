namespace WasteCollectionSystem.Models
{

    //Model class where the properties of the Vehicle class are defined as public virtual
    public class Vehicle
    {
        public virtual long Id { get; set; }
        public virtual string VehicleName { get; set; }
        public virtual string VehiclePlate { get; set; }
    }
}
