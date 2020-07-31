using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Utility
{
    public static int[] DirX = new int[] { 0, 1, 0, -1 };
    public static int[] DirY = new int[] { 1, 0, -1, 0 };

    public static void Swap<T>(ref T a, ref T b)
    {
        T tmp = a;
        a = b;
        b = tmp;
    }

    public static Vector3 GetRotation(Vector3 dir, Vector3 rot)
    {
        if (dir.x == 0)
        {
            if (dir.y == 1)
                rot.z = 0f;
            else
                rot.z = 180f;
        }
        else
        {
            if (dir.x == 1)
                rot.z = -90f;
            else
                rot.z = 90f;
        }

        return rot;
    }
}
