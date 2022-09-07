using System.Drawing;

namespace BootcampContainerGrouping.Models
{

    //The class I created to transfer the data in the container class to the Point
    public class ContainerWithPoint
    {
        public  long Id { get; set; }
        public  string ContainerName { get; set; }
        public double Latitude { get; set; }
        public  double Longitude { get; set; }
        public  long VehicleId { get; set; }
        public Point Point { get; set; }

    }
}
