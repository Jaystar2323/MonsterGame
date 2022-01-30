using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    bool solid = true;

    float f; //sum g + h
    float g; //distance to end node
    float h; //distance to start node

    Vector2 parent;

    void Start()
    {

    }
    public bool isSolid()
    {
        return solid;
    }

    public void initTile()
    {
        f = 0;
        g = 0;
        h = 0;
        //if (name.Equals("tile"))
        //{
        //    solid = false;

        //}
    }
    public void reset()
    {
        f = -1;
        g = -1;
        h = -1;

    }

    public int calculateValues(Vector2 pos, Vector2 start, Vector2 end)
    {
        g = GameManager.distance(pos, end);
        h = GameManager.distance(pos, start);
        f = g + h;
        return (int)f;
    }

    public float getF()
    {
        return f;
    }
    public void setParent(Vector2 tile)
    {
        parent = tile;

    }
    public Vector2 getParent()
    {
        return parent;
    }
}
