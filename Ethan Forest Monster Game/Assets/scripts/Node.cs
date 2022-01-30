using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    bool solid = false;

    float f; //sum g + h
    float g; //distance to end node
    float h; //distance to start node

    Vector2 pos;
    Node parent;

    public Node(Vector2 pos)
    {
        this.pos = pos;
    }
    public Node(Vector2 pos, bool isSolid)
    {
        solid = isSolid;
        this.pos = pos;
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
    public void setParent(Node node)
    {
        parent = node;

    }
    public Node getParent()
    {
        return parent;
    }
    public Vector2 getPos()
    {
        return pos;
    }
    public int getG()
    {
        return (int)g;
    }
}
