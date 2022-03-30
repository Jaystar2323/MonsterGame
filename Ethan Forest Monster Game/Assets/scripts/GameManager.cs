using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{

    //circle 0,1 - square 2,3 - triangle 5,6
    public Vector2[] tpTrees = new Vector2[6];
    //public Vector2 key;
    //public Vector2 end1, end2;

    public Vector2 up = new Vector2(0, 1);
    public Vector2 down = new Vector2(0, -1);
    public Vector2 left = new Vector2(-1, 0);
    public Vector2 right = new Vector2(1, 0);
    public static Node[,] nodes = new Node[26,16];

    [SerializeField] Tilemap tm;
    [SerializeField] Transform player;

    //Random useful static functions
    public static float distance(Vector2 entity1, Vector2 entity2)
    {
        return Mathf.Abs(entity1.x - entity2.x) + Mathf.Abs(entity1.y - entity2.y);
    }

    //Random useful non-static functions
    public Vector2 getTpTree(Vector2 loc)
    {
        //Debug.Log(loc + " " + tpTrees[2] + " " + tpTrees[3]);
        if (GameManager.distance(loc, tpTrees[0]) < 1)
        {
            return tpTrees[1];
        }
        if (GameManager.distance(loc, tpTrees[1]) < 1)
        {
            return tpTrees[0];
        }
        if (GameManager.distance(loc, tpTrees[2]) < 1)
        {
            return tpTrees[3];
        }
        if (GameManager.distance(loc, tpTrees[3]) < 1)
        {
            return tpTrees[2];
        }
        if (GameManager.distance(loc, tpTrees[4]) < 1)
        {
            return tpTrees[5];
        }
        if (GameManager.distance(loc, tpTrees[5]) < 1)
        {
            return tpTrees[4];
        }
        return loc;
    }

    public List<Vector2> findPath(Vector2 start, Vector2 end)
    {
        Vector3Int startC = tm.WorldToCell(new Vector3(start.x, start.y, 0));
        Vector3Int endC = tm.WorldToCell(new Vector3(end.x, end.y, 0));
        start.x = startC.x;
        start.y = startC.y;
        end.x = endC.x;
        end.y = endC.y;
        
        Vector2 curPos = start;
        if (start.Equals(end))
        {
            return null;
        }

        initNodes();
        getNode(start).reset();

        List<Vector2> path = new List<Vector2>();

        List<Vector2> open = new List<Vector2>();
        List<Vector2> closed = new List<Vector2>();
        open.Add(start);



        while (!curPos.Equals(end))
        {
            calcValues(open, closed, curPos, up, start, end);
            calcValues(open, closed, curPos, down, start, end);

            calcValues(open, closed, curPos, left, start, end);
            calcValues(open, closed, curPos, right, start, end);
            if (open.Count == 0)
            {

                //Tile tile2 = (Tile)tm.GetTile(new Vector3Int((int)start.x, (int)start.y, 0));
                //tile2.color = new Color(0, 0, 1);
                //tm.RefreshTile(new Vector3Int((int)start.x, (int)start.y, 0));

                //tile2 = (Tile)tm.GetTile(new Vector3Int((int)end.x, (int)end.y, 0));
                //tile2.color = new Color(0, 0, 1);
                //tm.RefreshTile(new Vector3Int((int)end.x, (int)end.y, 0));

                Debug.Log("No path found");
                return null;
            }
            else
            {
                curPos = open[0];
                open.RemoveAt(0);
                closed.Add(curPos);
                //Tile tile = (Tile)tm.GetTile(new Vector3Int((int)curPos.x, (int)curPos.y, 0));
                //tile.color = new Color(0, 0.2f, 0);
                //tm.RefreshTile(new Vector3Int((int)curPos.x, (int)curPos.y, 0));

            }


        }
        while (getNode(curPos).getF() != -1)
        {
            path.Add(curPos);
            curPos = getNode(curPos).getParent().getPos();
        }
        path.Reverse();

        //foreach (Vector2 t in path)
        //{
        //    //Tile tile = (Tile)tm.GetTile(new Vector3Int((int)t.x, (int)t.y, 0));
        //    //tile.color = new Color(0.5f, 0, 0.5f, 0.5f);
        //    //tm.RefreshTile(new Vector3Int((int)t.x, (int)t.y, 0));
        //}
        //Debug.Log("Path");
        return path; //Path shouldn't include start tile
    }

    void calcValues(List<Vector2> open, List<Vector2> closed, Vector2 curPos, Vector2 dir, Vector2 start, Vector2 end)
    {
        Vector2 newPose = curPos + dir;
        Node t = getNode(newPose);

        if (t != null && (!t.isSolid() || newPose.Equals(end)))
        {
            if (open.Contains(newPose) || closed.Contains(newPose))
            {
                return;
            }
            t.setParent(getNode(curPos));

            int f = t.calculateValues(newPose, start, end);
            int compf1 = getParentSwapValue(newPose, up, start, end);
            int compf2 = getParentSwapValue(newPose, down, start, end);
            int compf3 = getParentSwapValue(newPose, left, start, end);
            int compf4 = getParentSwapValue(newPose, right, start, end);

            if (compf1 < f)
            {
                f = compf1;
                getParentSwapValue(newPose, up, start, end);
                //Debug.Log("NO f:" + f + " Pos:" + newPose + " Dir:" + dir + " Old Parent:" + curPos + " New Parent:" + getNode(newPose + dir).getF());

            }
            else if (compf2 < f)
            {
                f = compf2;
                getParentSwapValue(newPose, down, start, end);
                //Debug.Log("NO f:" + f + " Pos:" + newPose + " Dir:" + dir + " Old Parent:" + curPos + " New Parent:" + getNode(newPose + dir).getF());

            }
            else if (compf3 < f)
            {
                f = compf3;
                getParentSwapValue(newPose, left, start, end);
                //Debug.Log("NO f:" + f + " Pos:" + newPose + " Dir:" + dir + " Old Parent:" + curPos + " New Parent:" + getNode(newPose + dir).getF());

            }
            else if (compf4 < f)
            {
                f = compf4;
                getParentSwapValue(newPose, right, start, end);
                //Debug.Log("NO f:" + f + " Pos:" + newPose + " Dir:" + dir + " Old Parent:" + curPos + " New Parent:" + getNode(newPose + dir).getF());

            }
            else
            {
               // Debug.Log("Yes");
                t.setParent(getNode(curPos));
                f = t.calculateValues(newPose, start, end);
            }




            int i = 0;
            while (i < open.Count && getNode(open[i]).getF() < f)
            {
                i++;
            }
            while (i < open.Count && getNode(open[i]).getF() == f && getNode(open[i]).getG() < t.getG())
            {
                i++;
            }
            open.Insert(i, newPose);
            //Tile tile = (Tile)tm.GetTile(new Vector3Int((int)newPose.x, (int)newPose.y, 0));
            //tile.color = new Color(1, 0, 0);
            //tm.RefreshTile(new Vector3Int((int)newPose.x, (int)newPose.y, 0));



        }
    }

    public static void printList(List<Vector2> list)
    {
        string str = "";
        foreach (Vector2 i in list)
        {
            str += i + " ";
        }
        str += "Count: "+list.Count;
       // Debug.Log(str);
    }
    public static Node getNode(Vector2 tile)
    {
        if ((int)tile.x + 13 >= nodes.GetLength(0) || (int)tile.y + 8 >= nodes.GetLength(1) || (int)tile.x +13 < 0 || (int)tile.y + 8 < 0)
        {
            return null;
        }
        return nodes[(int)tile.x+13, (int)tile.y+8];

    }
    void initNodes()
    {
        for (int i = 0; i < nodes.GetLength(0); i++)
        {
            for (int j = 0; j < nodes.GetLength(1); j++)
            {
                nodes[i,j].init();
            }
        }
    }

    int getParentSwapValue(Vector2 node, Vector2 dir, Vector2 start, Vector2 end)
    {
        Node n = getNode(node);
        //Debug.Log(dir);
        Node parent = getNode(node + dir);
        if (parent == null || parent.getF() <= 0)
        {
           // Debug.Log("not open yet or out of bounds");
            return int.MaxValue;
        }
        n.setParent(getNode(node + dir));
        int f = n.calculateValues(node, start, end);
        return f;
    }
}
