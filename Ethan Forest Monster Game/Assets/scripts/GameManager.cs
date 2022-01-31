using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector2 square1;
    public Vector2 square2;
    public Vector2 circle1;
    public Vector2 circle2;
    public Vector2 triangle1;
    public Vector2 triangle2;

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
        if (GameManager.distance(loc, square1) < 1)
        {
            return square2;
        }
        if (GameManager.distance(loc, square2) < 1)
        {
            return square1;
        }
        if (GameManager.distance(loc, circle1) < 1)
        {
            return circle2;
        }
        if (GameManager.distance(loc, circle2) < 1)
        {
            return circle1;
        }
        if (GameManager.distance(loc, triangle1) < 1)
        {
            return triangle2;
        }
        if (GameManager.distance(loc, triangle2) < 1)
        {
            return triangle1;
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
        if (getNode(start).isSolid() || start.Equals(end))
        {
            return null;
        }
        //getTile(start).reset();
        getNode(start).reset();

        List<Vector2> path = new List<Vector2>();
        //open.Add(start);

        List<Vector2> open = new List<Vector2>();
        List<Vector2> closed = new List<Vector2>();
        open.Add(start);

        Vector2 up = new Vector2(0, 1);
        Vector2 down = new Vector2(0, -1);
        Vector2 left = new Vector2(-1, 0);
        Vector2 right = new Vector2(1, 0);

        while (!curPos.Equals(end))
        {
            calcValues(open, closed, curPos, up, start, end);
            calcValues(open, closed, curPos, down, start, end);
            calcValues(open, closed, curPos, left, start, end);
            calcValues(open, closed, curPos, right, start, end);
            if (open.Count == 0)
            {

                Tile tile2 = (Tile)tm.GetTile(new Vector3Int((int)start.x, (int)start.y, 0));
                tile2.color = new Color(0, 0, 1);
                tm.RefreshTile(new Vector3Int((int)start.x, (int)start.y, 0));

                tile2 = (Tile)tm.GetTile(new Vector3Int((int)end.x, (int)end.y, 0));
                tile2.color = new Color(0, 0, 1);
                tm.RefreshTile(new Vector3Int((int)end.x, (int)end.y, 0));

                Debug.Log("No path found");
                return null;
            }
            else
            {
                curPos = open[0];
                open.RemoveAt(0);
                closed.Add(curPos);
                Tile tile = (Tile)tm.GetTile(new Vector3Int((int)curPos.x, (int)curPos.y, 0));
                tile.color = new Color(0, 0.2f, 0);
                tm.RefreshTile(new Vector3Int((int)curPos.x, (int)curPos.y, 0));

            }


        }
        //Debug.Log(curPos + " " + getNode(curPos).getParent().getPos());
        while (getNode(curPos).getF() != -1)
        {

            path.Add(curPos);
            curPos = getNode(curPos).getParent().getPos();
            //Debug.Log(curPos + " " + getNode(curPos).getParent().getPos());
        }
        path.Reverse();

        foreach (Vector2 t in path)
        {
            Tile tile = (Tile)tm.GetTile(new Vector3Int((int)t.x, (int)t.y, 0));
            tile.color = new Color(0.5f, 0, 0.5f, 0.5f);
            tm.RefreshTile(new Vector3Int((int)t.x, (int)t.y, 0));
        }

        return path; //Path shouldn't include start tile
    }

    void calcValues(List<Vector2> open, List<Vector2> closed, Vector2 curPos, Vector2 dir, Vector2 start, Vector2 end)
    {
        Vector2 newPose = curPos + dir;
        Node t = getNode(newPose);
        if (t != null && !t.isSolid())
        {
            

            if (open.Contains(newPose) || closed.Contains(newPose))
            {
                return;
            }
            t.setParent(getNode(curPos));

            int f = t.calculateValues(newPose, start, end);
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
            Tile tile = (Tile)tm.GetTile(new Vector3Int((int)newPose.x, (int)newPose.y, 0));
            tile.color = new Color(1, 0, 0);
            tm.RefreshTile(new Vector3Int((int)newPose.x, (int)newPose.y, 0));
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
        Debug.Log(str);
    }
    public static Node getNode(Vector2 tile)
    {
        //Debug.Log((tile.x) + " " + (tile.y));
        if ((int)tile.x + 13 >= nodes.GetLength(0) || (int)tile.y + 8 >= nodes.GetLength(1) || (int)tile.x +13 < 0 || (int)tile.y + 8 < 0)
        {
            return null;
        }
        return nodes[(int)tile.x+13, (int)tile.y+8];

    }

}
