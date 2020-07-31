using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;


/*
 * This class manages building placement.
 * It gives hovering effect (showing building at the cursor, continuously)
 * and Place building using left click
 * 
 * **/
public delegate void DonePlacingHandler();

public class BuildingPlacementManager : MonoBehaviour
{
    private WorldTile _tile;

    public GameObject BuildingTilemap;

    public Grid grid;

    public Type BuildingType; // Shop or warehouse ? 

    public event DonePlacingHandler DonePlacingEventHandler;

    // instantiate a transparent house for hovering effect
    private bool templateCreated = false;

    public string templateName { get; set; }

    private void Hover(Vector3Int worldPoint)
    {
        if (!templateCreated)
        {
            var tmp = Instantiate(BuildingTilemap, grid.transform);
            tmp.transform.position = worldPoint;
            tmp.name = BuildingTilemap.name + "Template";

            templateName = tmp.name;

            templateCreated = true;
        }
        else
        {
            var houseTilemap = GameObject.Find(templateName);
            houseTilemap.transform.position = worldPoint;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // convert click coord (relative to current camera position)
        // to World coord (true coord)
        Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var worldPoint = new Vector3Int(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y), 0); // truncate

        Hover(worldPoint);

        // Dictionary of tile
        var dictTiles = GameObject.Find("MainGrid").GetComponent<TilemapManager>().tiles;

        if (Input.GetMouseButtonDown(0))
        {
            PlaceBuilding(worldPoint, dictTiles);
        }
        else if (Input.GetKey(KeyCode.Escape)) // Cancel placement
        {
            CancelPlacement();
        }
	
    }

    private void PlaceBuilding(Vector3Int worldPoint, Dictionary<Vector3, WorldTile> dictTiles)
    {
        Vector3 worldCoord = new Vector3(worldPoint.x, 
                                         worldPoint.y, 
                                         worldPoint.z);

        // get tile at coord worldPoint, then store in _tile
        if (dictTiles.TryGetValue(worldPoint, out _tile))
        {
            // Check if the building placement is allowed
            Building newBuilding = (Building)Activator.CreateInstance(BuildingType, worldCoord); // Create a building instance
            var buildingManager = BuildingManager.instance;

            if (CheckBuildOnPavement(newBuilding) && buildingManager.AddBuilding(newBuilding))
            {
                var tmp = Instantiate(BuildingTilemap, grid.transform);
                tmp.transform.position = worldPoint;
                CancelPlacement();
            }
		
        }
    }

    private bool CheckBuildOnPavement(Building building)
    {
        for(int i = 0; i <= building.Dimension.Item1; i++)
        {
            for(int j = 0; j <= building.Dimension.Item2; j++)
            {
                Vector3 tmpCoord = new Vector3((int)building.WorldCoord.x + i,
                                               (int)building.WorldCoord.y + j);
                WorldTile tmpTile;
                bool tileExist = TilemapManager.instance.tiles.TryGetValue(tmpCoord, out tmpTile);

                if(!tileExist || tmpTile.isRoad)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void CancelPlacement()
    {
        var template = GameObject.Find(templateName);
        Destroy(template);

        var thisPlacementManager = GameObject.Find(this.name);
        Destroy(thisPlacementManager);

        if (DonePlacingEventHandler != null)
            DonePlacingEventHandler.Invoke();
    }
}