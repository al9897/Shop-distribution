using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding
{
    public Dictionary<Vector3Int, int> Dist { get; set; }
    public Dictionary<Vector3Int, Vector3Int> From { get; set; }
    private bool BFSed = false;


    private readonly int[] xDir = Utility.DirX;
    private readonly int[] yDir = Utility.DirY;
    Dictionary<Vector3, WorldTile> tiles;

    public PathFinding()
    {
        Dist = new Dictionary<Vector3Int, int>();
        From = new Dictionary<Vector3Int, Vector3Int>();
        tiles = TilemapManager.instance.tiles;
    }

    public PathFinding(TilemapManager tilemapManager)
    {
        tiles = tilemapManager.tiles;
    }


    public void BFS(Vector3Int start, bool moveOnRoad)
    {
        if (BFSed)
            return;

        Dist = new Dictionary<Vector3Int, int>();
        From = new Dictionary<Vector3Int, Vector3Int>();
        tiles = TilemapManager.instance.tiles;

        Queue<Vector3Int> q = new Queue<Vector3Int>();
        q.Enqueue(start);
        Dist.Add(start, 0);

        while (q.Count != 0)
        {
            Vector3Int u = q.Dequeue();

            for (int i = 0; i < xDir.Length; i++)
            {
                Vector3Int v = new Vector3Int(u.x + xDir[i],
                                              u.y + yDir[i], 0);

                WorldTile vTile;
                bool hasV = tiles.TryGetValue(v, out vTile);

                if (!Dist.ContainsKey(v) && hasV && vTile.isRoad == true)
                {
                    Dist.Add(v, Dist[u] + 1);
                    From.Add(v, u);
                    q.Enqueue(v);
                }
            }
        }

        BFSed = true;
    }
    public List<Vector3Int> GetPath(Vector3Int src, Vector3Int dest)
    {
        BFS(src, true);

        List<Vector3Int> path = new List<Vector3Int>();

        Vector3Int v = dest;

        while (v != src)
        {
            path.Add(v);
            v = From[v];
        }
        path.Add(src);
        path.Reverse();

        return path;
    }

    public Vector3 GetNearestRoad(TilemapManager tmManager, Building building)
    {
        var tiles = tmManager.tiles;

        int[] tmpX = new int[] { 0, 0, building.Dimension.Item1, building.Dimension.Item1 };
        int[] tmpY = new int[] { 0, building.Dimension.Item2, building.Dimension.Item2, 0 };

        for (int i = 0; i < 4; i++) // four corners
        {
            Tuple<int, int> corner = new Tuple<int, int>((int)building.WorldCoord.x + tmpX[i],
                                                         (int)building.WorldCoord.y + tmpY[i]);

            for (int step = 1; step <= 1000000000; step++) // to infinity and beyond
            {
                for (int dir = 0; dir < 4; dir++)
                {
                    Vector3 newCoord = new Vector3(corner.Item1 + step * Utility.DirX[dir],
                                                   corner.Item2 + step * Utility.DirY[dir]);

                    WorldTile tmpTile;
                    if (tiles.TryGetValue(newCoord, out tmpTile))
                    {
                        if (tmpTile.isRoad)
                            return newCoord;
                    }
                }
            }
        }
        return new Vector3(-1000000000, -1000000000);
    }
}
