using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Truck
{
    public int Capacity { get; set; }
    public int Inventory { get; set; }
    private bool isBig;

    // WorldSpeed: a multiple of number of tile
    // e.g: WorldSpeed = 1.5
    // then the truck move 1.5 * tile.pixelPerUnit / second
    public float WorldSpeed { get; set; }

    // Name of the sprite file to render
    public string SpriteName { get; set; }

    public Truck()
    {
        //default value for small truck is set here
        Capacity = 0;
        Inventory = 0;
    }

    public void IsBigTruck()
    {
        isBig = true;
        //should set value for big truck
        Capacity = 0;
        Inventory = 0;
    }
}
