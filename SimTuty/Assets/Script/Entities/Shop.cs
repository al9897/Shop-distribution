using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
public class Shop : Building
{
	public string Status { get; set; }
	public string Address { get; set; }
	public int Capacity { get; set; }
	public int Inventory { get; set; }

	// Average Goods sold each day
	public float ExpectedSellingPerDay { get; set; }
	public int Threshold { get; set; }
	public BuildingManager BM { get; set; }
    public int GoodsSold { get; set; }
	public bool WaitingForDelivery = false;

	// For poisson process
	private Queue<float> SellingTimePoint;

	public void Attach(BuildingManager bm)
    {
		BM = bm;
    }

	public Shop(Vector3 worldCoord)
        : base(worldCoord)
    {
        Dimension = new Tuple<int, int>(5, 5);
		PathFinding pf = new PathFinding();
		NearestRoad = pf.GetNearestRoad(TilemapManager.instance, this);
        GoodsSold = 0;
		SellingTimePoint = new Queue<float>();
	}

	public override string ToString() 
	{
		return "Shop "
			+ Address + " sold " + GoodsSold;
	}

	public float PoissonProcess(float rate)
	{
		System.Random rnd = new System.Random();
		float nextTime = Convert.ToSingle(-Math.Log(1.0 - rnd.NextDouble())) / rate;
		return nextTime;
	}

	public void RequestTruck()
    {
		int amountToDeliver = Capacity - Inventory;
		BM.SummonTruck(amountToDeliver, this);
	}

	public void Sell(float timeDelta)
	{
		float tmpTime = TimeTickSystem.CurrentSimTime - timeDelta;

		while(true)
		{
			
			if (SellingTimePoint.Count == 0)
			{
				// time until the next customer arrives
				float timeToNextCustomer = PoissonProcess(ExpectedSellingPerDay / (24f * 60f * 60f));
				// Convert real time to sim time
				timeToNextCustomer = TimeTickSystem.ToSimTime(timeToNextCustomer);

				tmpTime += timeToNextCustomer;
				SellingTimePoint.Enqueue(tmpTime);
			}

			if (SellingTimePoint.Peek() > TimeTickSystem.CurrentSimTime)
				break;


			if (Inventory > 0)
				Inventory--;
                 GoodsSold++;
            BM.GoodsSold++;
            System.Random rnd = new System.Random();
            BM.Revenue += UnityEngine.Random.Range(10.0f, 100.0f);
            _ = SellingTimePoint.Dequeue();
            

			UnityEngine.Debug.Log(this.Inventory);

			if (Inventory <= Threshold && WaitingForDelivery == false)
			{
				// summon truck
				WaitingForDelivery = true;
				int amountToDeliver = Capacity - Inventory;
				BM.SummonTruck(amountToDeliver, this);
			}
		}
	}

    
	public override void HandleTick(float timeDelta)
	{
		Sell(timeDelta);
	}

    
}
