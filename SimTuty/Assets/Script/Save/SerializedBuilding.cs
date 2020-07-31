using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Save
{
    [Serializable]
    public class SerializedBuilding
    {
        //building prop
        public float[] worldCoord;
        public int[] dimension;
        public float[] nearestRoad;
        public string type; 

        //shop prop
        public string status;  
        public string address;
        public int capacity;
        public int inventory;
        public float expectedSellingPerDay;
        public int threshold;

        //warehouse prop
        public int nrOfStock;
        public int nrOfBigTruck;
        public int nrOfSmallTruck;
        public string name;

        public SerializedBuilding(Building building)
        {
            worldCoord = new float[3];
            dimension = new int[2];
            nearestRoad = new float[3];

            this.worldCoord[0] = building.WorldCoord.x;
            this.worldCoord[1] = building.WorldCoord.y;
            this.worldCoord[2] = building.WorldCoord.z;

            this.dimension[0] = building.Dimension.Item1;
            this.dimension[1] = building.Dimension.Item2;

            this.nearestRoad[0] = building.NearestRoad.x;
            this.nearestRoad[1] = building.NearestRoad.y;
            this.nearestRoad[2] = building.NearestRoad.z;

            if (building is Warehouse)
            {
                type = "warehouse";
                nrOfStock = ((Warehouse)building).NrOfStock;
                nrOfBigTruck = ((Warehouse)building).NrOfBigTruck;
                nrOfSmallTruck = ((Warehouse)building).NrOfSmallTruck;
                name = ((Warehouse)building).Name;

            }
            else
            {
                type = "shop";
                status = ((Shop)building).Status;
                address = ((Shop)building).Address;
                inventory = ((Shop)building).Inventory;
                capacity = ((Shop)building).Capacity;
                expectedSellingPerDay = ((Shop)building).ExpectedSellingPerDay;
                threshold = ((Shop)building).Threshold;
            }
        }
    }
}
