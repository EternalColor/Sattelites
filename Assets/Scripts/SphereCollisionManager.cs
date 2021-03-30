using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SphereCollisionManager : MonoBehaviour
{
    //Size of each cell
    [SerializeField]
    private float cellSize;

    //Tells us how many cells in x direction and how many cells in z direction there are
    [SerializeField]
    private Vector2 cellCountInXAndZDimension;

    //Array of all gameobjects for which we should check the collision
    [SerializeField]
    private List<GameObject> objectsToCheck;

    private IList<Quadrant> quadrants;

    private void Awake() 
    {
        this.quadrants = new List<Quadrant>();

        //We start at half the dimension in negative coordinates (we have a offset, so the middle of our quadrants is the middle of the map)
        //Setup quadrants with X, Z coordinates
        for(int x = -(int)(cellCountInXAndZDimension.x / 2); x < (int)(cellCountInXAndZDimension.x / 2); ++x)
        {
            //cellCountInXAndZDimensions.y is Z component
            for(int z = -(int)(cellCountInXAndZDimension.y / 2); z < (int)cellCountInXAndZDimension.y; ++z)
            {
                this.quadrants.Add(new Quadrant(x, z));
            }
        }
    }

    private void Update() 
    {
        //Update quadrant position and assign to correct Quadrant
        foreach(GameObject objectToCheck in objectsToCheck)
        {
            int quadrantX = (int)Mathf.Floor(objectToCheck.transform.position.x / this.cellSize);
            int quadrantZ = (int)Mathf.Floor(objectToCheck.transform.position.z / this.cellSize);

            //Delete itself in old quadrant, if already registered
            if(this.quadrants.Any(quadr => quadr.ObjectsInQuadrant.Contains(objectToCheck.GetComponent<RotateAround>())))
            {
                this.quadrants.First(quadr => quadr.ObjectsInQuadrant.Contains(objectToCheck.GetComponent<RotateAround>())).ObjectsInQuadrant.Remove(objectToCheck.GetComponent<RotateAround>());
            }

            //If object is outside quadrants then the program will crash
            //Register itself in a new quadrant
            this.quadrants.First(quadr => quadr.X == quadrantX && quadr.Z == quadrantZ).ObjectsInQuadrant.Add(objectToCheck.GetComponent<RotateAround>());
        }
    }

    //Use fixed update for physics calculations rather than update
    private void FixedUpdate()
    {
        IList<GameObject> gameObjectsToDestroy = new List<GameObject>();

        //Check for performance reasons, if nothing needs checking we just continue
        if(this.quadrants.Any(quadr => quadr.NeedsChecking))
        {
            //Array is faster than list, thats why we use it here
            Quadrant[] quadrantsToCheck = this.quadrants.Where(quadr => quadr.NeedsChecking).ToArray();

            foreach(Quadrant quadrantToCheck in quadrantsToCheck)
            {
                //Compare every entry with every entry
                for (int i = 0; i < quadrantToCheck.ObjectsInQuadrant.Count - 1; ++i)
                {
                    for (int j = i + 1; j < quadrantToCheck.ObjectsInQuadrant.Count; ++j)
                    {
                        //If distance between spheres is lower than their radius combined
                        //IMPORTANT! We assume the sphere has a uniform scale, so we just take the x component, because its equal to y and z
                        if(Vector3.Distance(quadrantToCheck.ObjectsInQuadrant[j].transform.position, quadrantToCheck.ObjectsInQuadrant[i].transform.position)
                            < quadrantToCheck.ObjectsInQuadrant[i].transform.localScale.x / 2 + quadrantToCheck.ObjectsInQuadrant[j].transform.localScale.x / 2)
                        {
                            //We collided, but we dont destroy it yet because that would break our loop and its possible that we collided with more than 1 object at a time
                            gameObjectsToDestroy.Add(quadrantToCheck.ObjectsInQuadrant[i].gameObject);
                            gameObjectsToDestroy.Add(quadrantToCheck.ObjectsInQuadrant[j].gameObject);

                            //Can not delete multiple times
                            gameObjectsToDestroy = gameObjectsToDestroy.Distinct().ToList();
                        }
                    }
                }
            }
        }

        //Delete all game objects "marked" as deleted (all that are in the indeciesToDestroy list)
        for(int i = 0; i < gameObjectsToDestroy.Count; ++i)
        {
            //Explicitly remove entry from quadrant gameobject list, so we get no error trying to access it at a later point
            if(this.quadrants.Any(quadr => quadr.ObjectsInQuadrant.Contains(gameObjectsToDestroy[i].GetComponent<RotateAround>())))
            {
                this.quadrants.First(quadr => quadr.ObjectsInQuadrant.Contains(gameObjectsToDestroy[i].GetComponent<RotateAround>())).ObjectsInQuadrant.Remove(gameObjectsToDestroy[i].GetComponent<RotateAround>());
            }

            //Also explicitly remove entry from objectsToCheck list
            if(this.objectsToCheck.Contains(gameObjectsToDestroy[i]))
            {
                this.objectsToCheck.Remove(gameObjectsToDestroy[i]);
            }

            Object.DestroyImmediate(gameObjectsToDestroy[i].gameObject);
        }

        gameObjectsToDestroy.Clear();
    }

    private void OnDrawGizmos() 
    {
        if(this.quadrants != null)
        {
            foreach(Quadrant quadrant in this.quadrants)
            {
                if(quadrant.NeedsChecking)
                {
                    Gizmos.color = Color.red;
                }
                else
                {
                    Gizmos.color = Color.white;
                }

                Gizmos.DrawWireCube
                (
                    new Vector3
                    (
                        quadrant.X * this.cellSize - (this.cellSize / 2),
                        this.cellSize / 2,
                        quadrant.Z * this.cellSize - (this.cellSize / 2)
                    ),
                    new Vector3
                    (
                        this.cellSize,
                        this.cellSize,
                        this.cellSize
                    )
                );
            }    
        }
    }

    public void RegisterRotateAround(RotateAround rotateAroundToRegister)
    {
        this.objectsToCheck.Add(rotateAroundToRegister.gameObject);
    }
}

