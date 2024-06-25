using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Template
{
    public int weight;
    public List<(Vector3, string)> objectList;

    public Vector3 bottomLeft = new Vector3(0, 0, 0);
    public Vector3 topRight = new Vector3(1, 1, 0);

    public int value = 0;
    public int margin = 1;

    public int minDepth;
    public int maxDepth;
    public Template(int weight, int minDepth, int maxDepth, List<(Vector3, GrabbableObject)> objectTuples)
    {
        // Adding objects
        this.objectList = new List<(Vector3, string)>();
        if (objectTuples.Count > 0)
        {
            AddObjects(objectTuples);
        }

        // Weight determines how often the player will see this template
        this.weight = weight;

        // MinDepth and MaxDepth determines at which depth this template is able to spawn
        this.minDepth = minDepth;
        this.maxDepth = maxDepth;
    }
    private void AddObjects(List<(Vector3 position, GrabbableObject grabbableObject)> objectTuples)
    {
        this.bottomLeft = objectTuples[0].position;
        this.topRight = objectTuples[0].position;
        foreach ((Vector3 position, GrabbableObject grabbableObject) objectTuple in objectTuples)
        {
            this.objectList.Add((objectTuple.position, objectTuple.grabbableObject.name));

            int size = objectTuple.grabbableObject.size;
            if (objectTuple.grabbableObject is Pickup pickup)
            {
                this.value += pickup.value;
            }
            else
            {
                this.value += 1;
            }

            if (objectTuple.position.x + (0.5f * size) > topRight.x)
            {
                topRight.x = objectTuple.position.x + (0.5f * size);
            }
            if (objectTuple.position.y + (0.5f * size) > topRight.y)
            {
                topRight.y = objectTuple.position.y + (0.5f * size);
            }
            if (objectTuple.position.x - (0.5f * size) < bottomLeft.x)
            {
                bottomLeft.x = objectTuple.position.x - (0.5f * size);
            }
            if (objectTuple.position.y - (0.5f * size) < bottomLeft.y)
            {
                bottomLeft.y = objectTuple.position.y - (0.5f * size);
            }
        }
        RepositionObjects();
    }
    private void RepositionObjects()
    {
        Vector3Int marginVector = new Vector3Int(margin, margin, 0);
        this.topRight += marginVector;
        this.bottomLeft -= marginVector;

        List<(Vector3, string)> newObjectList = new List<(Vector3, string)>();
        foreach ((Vector3, string) o in this.objectList)
        {
            Vector3 position = o.Item1;
            string name = o.Item2;

            Vector3 newPosition = position - this.bottomLeft;
            newObjectList.Add((newPosition, name));
        }

        this.objectList = newObjectList;
        this.topRight = this.topRight - this.bottomLeft;
        this.bottomLeft = new Vector3(0, 0, 0);
    }
    public List<Vector3Int> TakenPositions(Vector3Int zeroPoint)
    {
        List<Vector3Int> positions = new List<Vector3Int>();
        for (int x = zeroPoint.x; x <= zeroPoint.x + topRight.x; x++)
        {
            for (int y = zeroPoint.y; y <= zeroPoint.y + topRight.y; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                positions.Add(pos);
            }
        }
        return positions;
    }
    public List<Vector3Int> TakenPositionsWithoutMargins(Vector3Int zeroPoint)
    {
        List<Vector3Int> positions = new List<Vector3Int>();
        for (int x = zeroPoint.x + this.margin; x <= zeroPoint.x + topRight.x - this.margin; x++)
        {
            for (int y = zeroPoint.y + this.margin; y <= zeroPoint.y + topRight.y - this.margin; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                positions.Add(pos);
            }
        }
        return positions;
    }
}
