using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using Assets.Script.Save;

//create a new cover class that contain a grid and a tilemap to be serializable
[Serializable]
public class MapData
{
    public List<SerializedBuilding> sBuildings;

    public MapData(List<Building> buildings)
    {
        sBuildings = new List<SerializedBuilding>();

        foreach (Building building in buildings)
        {
            SerializedBuilding sb = new SerializedBuilding(building);
            this.sBuildings.Add(sb);
        }
    }    
}
