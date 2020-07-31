using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.Tilemaps;
using Assets.Script.Save;
using System;

public class SaveManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {       
    }
    // Update is called once per frame
    void Update()
    {      
    }
    public void SaveData()
    {
        List<Building> buildings = BuildingManager.instance.ListBuildings;
        BinaryFormatter bf = new BinaryFormatter();
        string path = Application.persistentDataPath + "map.temp";
        FileStream fs = new FileStream(path, FileMode.Create);
        MapData md = new MapData(buildings);
        try
        {
            bf.Serialize(fs, md);
            Debug.Log(path);
        }
        catch (IOException) { }
        finally
        {
            fs.Close();
        }
    }
    public void LoadData()
    {
        MapData mapData = null;
        string path = Application.persistentDataPath + "map.temp";
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Open);
            try
            {
                mapData = (MapData)bf.Deserialize(fs);
            }
            catch (IOException) { }
            finally
            {
                fs.Close();
            }
        }
        else
        {
            Debug.LogError("No file");
        }
        if (!(mapData is null))
        {
            List<Building> temp = new List<Building>();
            foreach (SerializedBuilding sb in mapData.sBuildings)
            {
                Building building;
                Vector3 worldCoord = new Vector3(sb.worldCoord[0], sb.worldCoord[1], sb.worldCoord[2]);
                Tuple<int, int> dimension = new Tuple<int, int>(sb.dimension[0], sb.dimension[1]);
                Vector3 nearestRoad = new Vector3(sb.nearestRoad[0], sb.nearestRoad[1], sb.nearestRoad[2]);
                if (sb.type == "warehouse")
                {
                    building = new Warehouse(worldCoord);
                    ((Warehouse)building).NrOfStock = sb.nrOfStock;
                    ((Warehouse)building).NrOfBigTruck = sb.nrOfBigTruck;
                    ((Warehouse)building).NrOfSmallTruck = sb.nrOfSmallTruck;
                    ((Warehouse)building).Name = sb.name;
                }
                else
                {
                    building = new Shop(worldCoord);
                    ((Shop)building).Address = sb.address;
                    ((Shop)building).Status = sb.status;
                    ((Shop)building).Capacity = sb.capacity;
                    ((Shop)building).Inventory = sb.inventory;
                    ((Shop)building).ExpectedSellingPerDay = sb.expectedSellingPerDay;
                    ((Shop)building).Threshold = sb.threshold ;
                }
                building.WorldCoord = worldCoord;
                building.Dimension = dimension;
                building.NearestRoad = nearestRoad;

                TimeTickSystem.onTick += new TimeTickHandler(building.HandleTick);

                temp.Add(building);
            }
            BuildingManager.instance.ListBuildings = temp;
        }
        BuildingManager.instance.ShowAllBuilding();
    }
}
