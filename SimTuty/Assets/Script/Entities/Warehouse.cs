using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Warehouse : Building
{
	public int NrOfStock { get; set; }
	public int NrOfBigTruck { get; set; }
	public int NrOfSmallTruck { get; set; }
	public string Name { get; set; }
	public BuildingManager BM { get; set; }
	public List<Shop> waiting = new List<Shop>();
	public List<Truck> runningbigTruck; //for later
	public List<Truck> runningSmallTruck; //for later

	public int NrOfTruckRunning { get; set; }
	public Warehouse(Vector3 worldCoord)
        : base(worldCoord)
    {
        Dimension = new Tuple<int, int>(5, 7);
		NrOfStock = 1000;
		PathFinding pf = new PathFinding();
		NearestRoad = pf.GetNearestRoad(TilemapManager.instance, this);
	}

	public override void HandleTick(float timeDelta)
	{

	}

	public void NotifyAvailable()
    {
		if(waiting.Count>0)
        {
			BM.NotifyShop(waiting[0]);
			waiting.RemoveAt(0);
		}
		else
		{ }
    }

	public void Attach(BuildingManager bm)
    {
		BM = bm;
    }

	public bool isAvailable()
    {
		if(NrOfTruckRunning < NrOfSmallTruck+NrOfBigTruck)
        {
			return true;
        }
		return false;
    }

	public override string ToString()
	{
        return "Warehouse " + Name + " has " + NrOfStock + " stocks";
	}
}
