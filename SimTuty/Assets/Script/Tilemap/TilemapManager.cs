using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    // this TilemapManager object is unique
    // Can always be obtained with TilemapManager.instance
    public static TilemapManager instance;
    public Tilemap Tilemap;

    public Dictionary<Vector3, WorldTile> tiles;

    private void Awake()
    {
        instance = this;

        GetWorldTiles();
    }

    private void GetWorldTiles()
    {
        tiles = new Dictionary<Vector3, WorldTile>();

        foreach (Vector3Int pos in Tilemap.cellBounds.allPositionsWithin)
        {
            var localPlace = new Vector3Int(pos.x, pos.y, pos.z);

            if (!Tilemap.HasTile(localPlace)) continue;

            var tile = new WorldTile
            {
                LocalPlace = localPlace,
                WorldLocation = Tilemap.CellToWorld(localPlace),
                TileBase = Tilemap.GetTile(localPlace),
                TilemapMember = Tilemap,
                Name = localPlace.x + "," + localPlace.y,
                Cost = 1 // TODO: Change this with the proper cost from ruletile
            };
            //bug.Log(Tilemap.GetSprite(localPlace).name);
            
            if(Tilemap.GetSprite(localPlace).name.ToString() == "roguelikeCity_magenta_747")
            {
                tile.isRoad = false;
            }
            else { tile.isRoad = true; }

            tiles.Add(tile.WorldLocation, tile);
            //bug.Log(tile.isRoad + " check");
            //Debug.Log(tile.WorldLocation + ", " + tile.LocalPlace);
        }
    }
}
