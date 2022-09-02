using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WasteCollectionSystem.Context;
using WasteCollectionSystem.Models;

namespace BootcampContainerGrouping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupingController : ControllerBase
    {
        private readonly IMapperSession session;
        public GroupingController(IMapperSession session)
        {
            this.session = session;
        }
        //The part where the grouping is made with the Id information and the number of clusters entered from the user.
        
        [HttpGet]
        public List<List<Container>> groups (long vehicleId, int numberOfGroups)
        {

            //The part where the clustering process takes place

            
            var groups = new List<List<Container>>();
            List<Container> containerList = session.Containers.Where(x => x.VehicleId == vehicleId).ToList();

            //Checking if the container list is greater than numberofGroups
            if (numberOfGroups > containerList.Count)
            {
                return groups;
            }

            // Initializing empty lists as many as the number of groups
            for (int i=0; i<numberOfGroups;i++)
            {
                groups.Add(new List<Container>());
            }
            
            var count = 0;
            int remainingContainer = containerList.Count % numberOfGroups;

            //The number of loops is found according to the section of the list by the numberOfGroups.
            for (int j=0; j<GetLoopCount(containerList.Count, numberOfGroups);j++) 
            {
                
                for (int i = 0; i < numberOfGroups; i++)
                {
                    //If the remainingContainer is not zero and the last loop is entered,
                    //the values ​​outside of all loops are frozen by the number of containers remaining and the loop is exited.
                    if (remainingContainer!=0 && j== GetLoopCount(containerList.Count, numberOfGroups)-1 && i == remainingContainer )
                    {
                        break;
                    }

                    groups[i].Add(containerList[count]);
                    count++;


                }
            }           
            return groups;
        }

        //algorithm that calculates how many full turns of the big loop are made
        private static int GetLoopCount(int ListSize, int GroupSize)
        {
            if(ListSize % GroupSize == 0)
            {
                return ListSize / GroupSize;
            }
            else
            {
                return (ListSize / GroupSize)+1;
            }
          
        }
    }
}
