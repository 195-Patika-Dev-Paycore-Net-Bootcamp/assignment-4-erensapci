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
            int element = 0; 
            int clusterRemain = numberOfGroups;

            double numberOfContainers = NumberOfElementsInCluster(orderedContainers.Count, numberOfGroups); 

            foreach (var container in orderedContainers) 
            {
                element++; 
                count++; 

                if (NewMethod(count, clusterRemain, numberOfContainers)) 
                {
                    groups.Add(orderedContainers.GetRange(element - count, count)); 
                    clusterRemain--; 
                    count = 0; 
                }
                if (clusterRemain == 1 && element == orderedContainers.Count)
                {
                    groups.Add(orderedContainers.GetRange(element - count, count));
                    clusterRemain--;
                    count--;
                }
            }
            return groups;
        }

        private static bool NewMethod(int count, int clusterRemain, double numberOfContainers)
        {
            return count == numberOfContainers && clusterRemain > 1;
        }

        private double NumberOfElementsInCluster(double numberOfElements, double numberOfClusters) 
        {
            double result = numberOfElements / numberOfClusters;
            double numberAndHalf = 0.5;
            for (int i = 1; i < numberOfElements; i++) 
            {

                if (result == numberAndHalf) 
                {
                    result -= 0.5;
                }
                numberAndHalf += i;
            }

            double numberOfElementsInCluster = Math.Round(result);
            return numberOfElementsInCluster;
        } 

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

        //algorithm that calculates how many full turns of the big loop are made
        private static int GetLoopCount(int ListSize, int GroupSize)
        {
            if (ListSize % GroupSize == 0)
            {
                return ListSize / GroupSize;
            }
            else
            {
                return (ListSize / GroupSize) + 1;
            }

        }

    }
}
