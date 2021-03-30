//We divide the whole map into quadrants (on the x and z plane) -> X, Z are the coordinates of the quadrant
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Quadrant
{
    public int X;

    public int Z;

    //We physics check all objects that can rotate around an attractor
    public IList<RotateAround> ObjectsInQuadrant;

    public bool NeedsChecking => this.ObjectsInQuadrant.Count >= 2;

    public Quadrant(int x, int z)
    {
        this.X = x;
        this.Z = z;
        this.ObjectsInQuadrant = new List<RotateAround>();
    }
}