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
        Vector2 curPos = start;
        getTile(start).reset();
        

        List<Vector2> path = new List<Vector2>();
        path.Add(start);

        List<Vector2> open = new List<Vector2>();
        List<Vector2> closed = new List<Vector2>();
        closed.Add(start);

        Vector2 up = new Vector2(0, 1);
        Vector2 down = new Vector2(0, -1);
        Vector2 left = new Vector2(-1, 0);
        Vector2 right = new Vector2(0, 1);

        int c = 0;
        while (!curPos.Equals(end) && c < 100)
        {
            //Debug.Log("hi");
            calcValues(open, closed, curPos, up, start, end);
            calcValues(open, closed, curPos, down, start, end);
            calcValues(open, closed, curPos, left, start, end);
            calcValues(open, closed, curPos, right, start, end);
            //Debug.Log(open.Count);
            if (open[0] == null)
            {
                return null;
            }
            else
            {
                curPos = open[0];
                open.RemoveAt(0);
                closed.Add(curPos);
            }
            c++;
            Debug.Log(c + ": " + curPos + " Open: ");
            printList(open);

        }
        if (c == 100)
        {
            return null;
        }
        path.Add(curPos);

        while (getTile(curPos).getF() != -1)
        {
            curPos = getTile(curPos).getParent();
            path.Add(curPos);
        }

        return path; //Path shouldn't include start tile
    }

    void calcValues(List<Vector2> open, List<Vector2> closed, Vector2 curPos, Vector2 dir, Vector2 start, Vector2 end)
    {
        //Debug.Log("hi");
        Vector2 newPose = curPos + dir;
        AStarTile t = getTile(newPose);
        if (t != null && !t.isSolid())
        {
            if (open.Contains(newPose) || closed.Contains(newPose))
            {
                return;
            }

            int f = t.calculateValues(newPose, start, end);
            int i = 0;

            while (i < open.Count && getTile(open[i]).getF() <= f)
            {
                i++;
            }
            t.setParent(curPos);
            open.Insert(i, newPose);
        }
    }

    public void printList(List<Vector2> list)
    {
        string str = "";
        foreach (Vector2 i in list)
        {
            str += getTile(i).getF() + " ";
        }
        Debug.Log(str);
    }
    public AStarTile getTile(Vector2 tile)
    {
        return (AStarTile)tm.GetTile(new Vector3Int((int)tile.x, (int)tile.y, 1));
    }

}
