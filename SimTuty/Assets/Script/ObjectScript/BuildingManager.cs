using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

//public delegate BuildingAddedHandler(Vector3)

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager instance;

    public List<Building> ListBuildings;

	public Building[] temp;
    public float Revenue { get; set; }
    public int GoodsSold { get; set; }

	/* For truck testing */
	bool BFSed = false;
	List<Vector3Int> path;
	bool truckCreated;
	//GameObject truck;
	int counter = 0;
    Shop shopWatch = null;

	// Start is called before the first frame update

	void Start()
    {
        ListBuildings = new List<Building>();
        Revenue = 0;
        instance = this;
		temp = new Building[1];

        if (SceneManager.GetActiveScene().name == "SimulationScene")
        {
            SaveManager sm = new SaveManager();
            sm.LoadData();
        }

    }

    void Update()
    {
        //for debugging XD
        //List<Building> s = getShops();
        //shopWatch =(Shop)s[0];
        //Shop shopw2 = (Shop)s[1];
        //if(shopWatch!=null)
        //{
        //    UnityEngine.Debug.Log(shopWatch.ToString() + " 1");
        //    UnityEngine.Debug.Log(shopw2.ToString() + " 2");

        //}
        var tmpShop = getShops();

        string content = "";

        foreach(Shop s in tmpShop)
        {
            content += s.Inventory.ToString() + ", ";
        }
        Debug.Log(content);
		if (Input.GetKeyDown(KeyCode.Return))
		{
			if(ListBuildings != null)
			{
				foreach (Building b in ListBuildings)
				{
					if (b is Shop)
					{
						CSVManager.AppendToReport(
						new string[5]
						{
						((Shop)b).Status+" ",
						TimeTickSystem.ToRealTime().Day+":"+TimeTickSystem.ToRealTime().Hour,
						((Shop)b).GoodsSold+" ",
						((Shop)b).Capacity+" ",
						((Shop)b).Inventory+" "
						}
						);
					}
				}

			}
		}
	}
    



    public void SummonTruck(int amountToDeliver, Shop shop)
    {
        List<int> PathLengths = new List<int>();
        List<Building> warehouses = getWarehouses();
        GameObject truck;
        foreach (Building b in warehouses)
        {
                PathFinding pf1 = new PathFinding();
                Vector3Int src1 = new Vector3Int((int)b.NearestRoad.x,
                                                (int)b.NearestRoad.y, 0);
                //Debug.Log("from: " + src.ToString());
                pf1.BFS(src1, true);


                Vector3Int dest1 = new Vector3Int((int)shop.NearestRoad.x,
                                                 (int)shop.NearestRoad.y, 0);
                //Debug.Log("to: " + dest.ToString());
                //Debug.Log("cost: " + pf.Dist[dest]);

                path = pf1.GetPath(src1, dest1);
                for (int i = path.Count - 1; i >= 0; i--)
                    path.Add(path[i]);

                PathLengths.Add(path.Count);
        }

        Warehouse nearestWarehouse = (Warehouse)warehouses[PathLengths.IndexOf(PathLengths.Min())];
        shopWatch = shop;
        PathFinding pf = new PathFinding();
        Vector3Int src = new Vector3Int((int)nearestWarehouse.NearestRoad.x,
                                        (int)nearestWarehouse.NearestRoad.y, 0);
        //Debug.Log("from: " + src.ToString());
        pf.BFS(src, true);


        Vector3Int dest = new Vector3Int((int)shop.NearestRoad.x,
                                         (int)shop.NearestRoad.y, 0);
        //Debug.Log("to: " + dest.ToString());
        //Debug.Log("cost: " + pf.Dist[dest]);

        path = pf.GetPath(src, dest);
        for (int i = path.Count - 1; i >= 0; i--)
            path.Add(path[i]);

        //BFSed = true;
        
        //check if trucks are available
        if(nearestWarehouse.isAvailable())
        {
            nearestWarehouse.NrOfStock -= amountToDeliver;

            truck = Resources.Load<GameObject>("Prefab/Vehicle/truck-small");
            truck = Instantiate(truck);
            truck.transform.position = path[0];

            var tmpClass = truck.GetComponent<TruckScript>();
            tmpClass.path = path;
            tmpClass.truck = truck;
            tmpClass.shop = shop;
            tmpClass.amount = amountToDeliver;
            nearestWarehouse.NrOfTruckRunning++;
            tmpClass.warehouse = nearestWarehouse;

        }
        else
        {
            nearestWarehouse.waiting.Add(shop);
        }
    }
    //Notify shop when truck is available
    public void NotifyShop(Shop shop)
    {
        shop.RequestTruck();
    }

    public void ShowAllBuilding()
	{
		foreach (Building building in ListBuildings)
		{
			string link = "Prefab/Building/";
			link += (building is Shop) ? "Shop" : "Warehouse";
			GameObject buildingObject = Resources.Load<GameObject>(link);

			var tmp = Instantiate(buildingObject, GameObject.Find("MainGrid").transform);
			tmp.transform.position = building.WorldCoord;

            if(building is Shop)
            {
                tmp.GetComponent<ShopScript>().ThisShop = (Shop)building;
                ((Shop)building).Attach(this);
            }
            else
            {
                ((Warehouse)building).Attach(this);
            }
		}
	}

    public List<Building> getWarehouses()
    {
        List<Building> w = new List<Building>();
        foreach(Building b in ListBuildings)
        {
            if(b is Warehouse)
            {
                w.Add(b);
            }
        }
        return w;
    }

    public List<Building> getShops()
    {
        List<Building> w = new List<Building>();
        foreach (Building b in ListBuildings)
        {
            if (b is Shop)
            {
                w.Add(b);
            }
        }
        return w;
    }

    public List<Building> getList()
	{
		return this.ListBuildings;
	}
    public bool AddBuilding(Building building)
    {
        //Debug.Log(building.WorldCoord);

        // Upper Left Corner
        Tuple<int, int> l = new Tuple<int, int>((int)building.WorldCoord.x, 
                                                (int)building.WorldCoord.y + building.Dimension.Item2);
        // Lower Right Corner
        Tuple<int, int> r = new Tuple<int, int>((int)building.WorldCoord.x + building.Dimension.Item1,
                                                (int)building.WorldCoord.y);

        foreach (var build in ListBuildings)
        {
            //Debug.Log(build.WorldCoord);
            //Debug.Log(l + " | " + r);

            // Upper Left Corner
            Tuple<int, int> bl = new Tuple<int, int>((int)build.WorldCoord.x,
                                                     (int)build.WorldCoord.y + building.Dimension.Item2);
            // Lower Right Corner
            Tuple<int, int> br = new Tuple<int, int>((int)build.WorldCoord.x + building.Dimension.Item1,
                                                     (int)build.WorldCoord.y);

            //Debug.Log(bl + " | " + br);

            if (IsOverlapped(l, r, bl, br))
                return false;
        }

        // finish loop, no collision
        ListBuildings.Add(building);
        return true;
    }
	public void getList(List<Building> gt)
	{
		if(ListBuildings!=null)
		{
			foreach (var bd in ListBuildings)
			{
				gt.Add(bd);
			}
		}	
	}
	public Building getObjectBuilding()
	{
		
			Building bdTemp = temp[0];
			Tuple<int, int> l = new Tuple<int, int>((int)bdTemp.WorldCoord.x,
												 (int)bdTemp.WorldCoord.y + 5);
			// Lower Right Corner
			Tuple<int, int> r = new Tuple<int, int>((int)bdTemp.WorldCoord.x + 5,
													(int)bdTemp.WorldCoord.y);

			foreach (var build in ListBuildings)
			{
				//Debug.Log(build.WorldCoord);
				//Debug.Log(l + " | " + r);

				// Upper Left Corner
				Tuple<int, int> bl = new Tuple<int, int>((int)build.WorldCoord.x,
														 (int)build.WorldCoord.y + 5);
				// Lower Right Corner
				Tuple<int, int> br = new Tuple<int, int>((int)build.WorldCoord.x + 5,
														 (int)build.WorldCoord.y);

				//Debug.Log(bl + " | " + br);

				if (IsOverlapped(l, r, bl, br))
					return build;
			}	
		
		return null;
	}
	public bool IsOverlapped(Tuple<int,int> l, Tuple<int, int> r, Tuple<int, int> bl, Tuple<int, int> br)
    {
        // completely to the left/right
        if (br.Item1 <= l.Item1 || r.Item1 <= bl.Item1)
            return false;

        // completely to the top/bottom
        if (br.Item2 >= l.Item2 || r.Item2 >= bl.Item2)
            return false;

        return true;
    }

    private void Swap(ref object a, ref object b)
    {
        object tmp = a;
        a = b;
        b = tmp;
    }
}
