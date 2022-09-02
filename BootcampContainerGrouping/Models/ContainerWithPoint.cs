using System.Drawing;

namespace BootcampContainerGrouping.Models
{
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
