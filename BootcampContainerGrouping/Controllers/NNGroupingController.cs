using BootcampContainerGrouping.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nest;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using WasteCollectionSystem.Context;
using WasteCollectionSystem.Models;

namespace BootcampContainerGrouping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NNGroupingController : ControllerBase
    {
        private readonly IMapperSession session;
        public NNGroupingController(IMapperSession session)
        {
            this.session = session;
        }
        //The part where the grouping is made with the Id information and the number of clusters entered from the user.

        [HttpGet]
        public List<List<ContainerWithPoint>> groups(long vehicleId, int numberOfGroups)
        {

            //The part where the clustering process takes place
            var groups = new List<List<ContainerWithPoint>>();
            List<Container> containerList = session.Containers.Where(x => x.VehicleId == vehicleId).ToList();
            var neighbors = new Point[containerList.Count + 1];
            neighbors[0] = new Point(0, 0);
            var unOrderedContainers = new List<ContainerWithPoint>();

            for (int i = 0; i < containerList.Count; i++)
            {
                neighbors[i+1] = new Point((int)containerList[i].Latitude, (int)containerList[i].Longitude);
                var newContainerWPoint = new ContainerWithPoint();
                newContainerWPoint.Latitude = (int)containerList[i].Latitude;
                newContainerWPoint.Longitude = (int)containerList[i].Longitude;
                newContainerWPoint.Point = new Point((int)containerList[i].Latitude, (int)containerList[i].Longitude);
                newContainerWPoint.VehicleId = containerList[i].VehicleId;
                newContainerWPoint.Id = containerList[i].Id;
                newContainerWPoint.ContainerName = containerList[i].ContainerName;
                unOrderedContainers.Add(newContainerWPoint);

            }

            var h = 1000;
            var sourcePoint = new Point(0, 0);
            var orderedContainers = new List<ContainerWithPoint>();

            var nearestNeighbors = GetNeighbors(sourcePoint, h, neighbors);
            for(int i = 0; i < nearestNeighbors.Length; i++)
            {
                for(int j = 0; j< unOrderedContainers.Count; j++)
                {
                    if (nearestNeighbors[i].Equals(unOrderedContainers[j].Point))
                    {
                        orderedContainers.Add(unOrderedContainers[j]);
                    }
                }
            }

            //Checking if the container list is greater than numberofGroups
            if (numberOfGroups > containerList.Count)
            {
                return groups;
            }
            int count = 0;
            int component = 0; 
            int GroupingData = numberOfGroups;

            double numberOfContainers = NumberOfElementsInCluster(orderedContainers.Count, numberOfGroups); 

            foreach (var container in orderedContainers) 
            {
                component++; 
                count++; 

                if (ifStatement(count, GroupingData, numberOfContainers)) 
                {
                    groups.Add(orderedContainers.GetRange(component - count, count));
                    GroupingData--; 
                    count = 0; 
                }
                if (GroupingData == 1 && component == orderedContainers.Count)
                {
                    groups.Add(orderedContainers.GetRange(component - count, count));
                    GroupingData--;
                    count--;
                }
            }
            return groups;
        }

        private static bool ifStatement(int count, int GroupingData, double numberOfContainers)
        {
            return count == numberOfContainers && GroupingData > 1;
        }

        private double NumberOfElementsInCluster(double numberOfElements, double numberOfClusters) 
        {
            double result = numberOfElements / numberOfClusters;
            double HalfOfNumber = 0.5;
            for (int i = 1; i < numberOfElements; i++) 
            {

                if (result == HalfOfNumber) 
                {
                    result -= 0.5;
                }
                HalfOfNumber += i;
            }

            double numberOfElementsInCluster = Math.Round(result);
            return numberOfElementsInCluster;
        }
        //the section where the data is filled into the point
        public static Point[] GetNeighbors(Point point, int h, Point[] neighbors)
        {
            return neighbors
                .Select(p => new { Point = p, Distance = CalculateDistanceBetweenPoints(point, p) })
                .Where(pointAndDistance => pointAndDistance.Distance <= Math.Pow(h, 2))
                .OrderBy(pointAndDistance => pointAndDistance.Distance)
                .Select(pointAndDistance => pointAndDistance.Point)
                .ToArray();
        }

        public static double CalculateDistanceBetweenPoints(Point originPoint, Point destinationPoint)
        {
            return Math.Pow(originPoint.X - destinationPoint.X, 2)
                + Math.Pow(originPoint.Y - destinationPoint.Y, 2);
        }

    }
}
