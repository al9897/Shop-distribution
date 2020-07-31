using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Building
{
    public Vector3 WorldCoord { get; set; }
    public Tuple<int, int> Dimension { get; set; }
    public Vector3 NearestRoad { get; set; }

    public Building(Vector3 worldCoord)
    {	//item 1:width/item2:height
        this.WorldCoord = worldCoord;
        this.Dimension = new Tuple<int, int>(0, 0);
    }
	public override string ToString()
	{
		return "" + this.WorldCoord;
	}

    public abstract void HandleTick(float timeDelta);
}
