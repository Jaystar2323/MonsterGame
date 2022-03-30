using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] Tilemap player;
    [SerializeField] Tilemap collision;
    [SerializeField] GameManager gm;
    [SerializeField] FogOfWar fow;
    Vector2 playerPosition;

    [SerializeField] Tile tile;
    [SerializeField] Tile log1;
    [SerializeField] Tile log2;
    [SerializeField] Tile log3;
    [SerializeField] Tile bush;
    [SerializeField] Tile giantTreeHead;
    [SerializeField] Tile giantTreeLog;
    [SerializeField] Tile rock;
    [SerializeField] Tile largeRock;
    [SerializeField] Tile rockCrumble;
    [SerializeField] Tile treeTrunk;
    [SerializeField] Tile treeHead;
    [SerializeField] Tile triangle;
    [SerializeField] Tile square;
    [SerializeField] Tile circle;
    [SerializeField] Tile key;
    [SerializeField] Tile end1;
    [SerializeField] Tile end2;
    [SerializeField] Tile roamRocks;



    // Start is called before the first frame update
    int minX;
    int maxX;
    int minY;
    int maxY;

    Matrix4x4 r90 = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, 90f), Vector3.one);
    Matrix4x4 r180 = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, 180f), Vector3.one);
    Matrix4x4 r270 = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, 270f), Vector3.one);

    void Start()
    {
        playerPosition = new Vector2(-5,-2); //Player Spawn Location Hard Coded, change later
        minX = -width - 1;
        maxX = width;
        minY = -height -1;
        maxY = height;
        for (int i = minX; i <= maxX; i++)
        {
            for (int j = minY; j <= maxY; j++)
            {
                player.SetTile(new Vector3Int(i, j, 0), tile);
                setNode(new Vector2(i, j), false);

            }
        }
        placeHorizontalLog(new Vector2(3, 4), 5);
        placeHorizontalLog(new Vector2(-11, -3), 3);
        placeHorizontalFallenTreen(new Vector2(-6, 3), 4);
        placeHorizontalFallenTreen(new Vector2(4, 1), 5);
        placeSmallRock(new Vector2(-5, -1), 2);
        placeSmallRock(new Vector2(6, 3), 2);
        placeSmallRock(new Vector2(-11, -7), 1);
        placeLargeRock(new Vector2(8, -4), 2);
        placeLargeRock(new Vector2(-10, 1), 2);
        placeTree(new Vector2(-6, -6));
        placeTpTree(new Vector2(11, 0), new Vector2(-12, 3), triangle);
        placeTpTree(new Vector2(0, -5), new Vector2(8, 6), square);
        placeTpTree(new Vector2(-2, 5), new Vector2(2, 0), circle);
        placeVerticalLog(new Vector2(0, 0), 4);
        placeVerticalLog(new Vector2(8, -6), 2);
        placeVerticalFallenTree(new Vector2(5, -3), 4);

        int keyquad = Random.Range(1, 5);
        int endquad = Random.Range(1, 5);
        while (endquad == keyquad)
        {
            endquad = Random.Range(1, 5);
        }

        while (!placeKey(keyquad)) { }
        while (!placeEnd(endquad)) { }
  
        for (int i = minX; i <= maxX; i++)
        {
            for (int j = minY; j <= maxY; j++)
            {
                if (i == playerPosition.x && j == playerPosition.y) 
                {
                    continue;
                }
                float rand = Random.Range(0f,1f);
                if (rand <= 0.07 && player.GetTile(new Vector3Int(i, j, 0)).name == "tile")
                {
                    placeBush(new Vector2(i,j));
                }
                if (rand >= 0.99 && player.GetTile(new Vector3Int(i, j, 0)).name == "tile")
                {
                    createTile(new Vector3Int(i, j, 0), roamRocks);
                }
            }
        }

        player.RefreshAllTiles();
        fow.init();

        //Debug.Log(gm.findPath(new Vector2(-7.5f, 0.5f), new Vector2(-8.5f, -4.5f)));
        //Debug.Log(gm.findPath(new Vector2(12,6), new Vector2(-5,-2)).Count);

    }
    
    void placeHorizontalLog(Vector2 start, int length)
    {
        createColliderTile(new Vector3Int((int)start.x, (int)start.y, 0), log1);

        for (int i = 1; i < length-1; i++)
        {
            createColliderTile(new Vector3Int((int)start.x + i, (int)start.y, 0), log2);
        }
        createColliderTile(new Vector3Int((int)start.x + length - 1, (int)start.y, 0), log3);
    }
    void placeBush(Vector2 start)
    {
        player.SetTile(new Vector3Int((int)start.x, (int)start.y, 0), bush);
    }
    void placeHorizontalFallenTreen(Vector2 start, int length)
    {
        createColliderTile(new Vector3Int((int)start.x, (int)start.y, 0), log1);

        for (int i = 1; i < length - 1; i++)
        {
            createColliderTile(new Vector3Int((int)start.x + i, (int)start.y, 0), log2);
        }
        createTile(new Vector3Int((int)start.x + length - 1, (int)start.y, 0), giantTreeLog, r270);
        createTile(new Vector3Int((int)start.x + length - 1, (int)start.y - 1, 0), giantTreeHead);
        createTile(new Vector3Int((int)start.x + length, (int)start.y, 0), giantTreeHead, r180);
        createTile(new Vector3Int((int)start.x + length, (int)start.y - 1, 0), giantTreeHead, r90);
        
    }

    //Crumble pos
    //0 = no crumbles
    //1 = left
    //2 = right
    void placeSmallRock(Vector2 start, int crumblePos)
    {
        createColliderTile(new Vector3Int((int)start.x, (int)start.y, 0), rock);
        if (crumblePos == 1)
        {
            createColliderTile(new Vector3Int((int)start.x - 1, (int)start.y, 0), rockCrumble);
        }
        else if (crumblePos == 2)
        {
            createColliderTile(new Vector3Int((int)start.x + 1, (int)start.y, 0), rockCrumble, r270);
        }
    }
    //Crumble pos
    //0 = no crumbles
    //1 = left
    //2 = right
    void placeLargeRock(Vector2 start, int crumblePos)
    {
        createColliderTile(new Vector3Int((int)start.x, (int)start.y, 0), largeRock);
        createColliderTile(new Vector3Int((int)start.x + 1, (int)start.y, 0), largeRock, r90);
        createColliderTile(new Vector3Int((int)start.x, (int)start.y + 1, 0), largeRock , r270);
        createColliderTile(new Vector3Int((int)start.x + 1, (int)start.y + 1, 0), largeRock, r180);

        if (crumblePos == 1)
        {
            createColliderTile(new Vector3Int((int)start.x - 1, (int)start.y, 0), rockCrumble);
        }
        else if (crumblePos == 2)
        {
            createColliderTile(new Vector3Int((int)start.x + 2, (int)start.y, 0), rockCrumble, r270);
        }

    }
    void placeTree(Vector2 start)
    {
        createColliderTile(new Vector3Int((int)start.x, (int)start.y, 0), treeTrunk);
        createTile(new Vector3Int((int)start.x, (int)start.y+1, 0), treeHead);
    }
    void placeTpTree(Vector2 loc1, Vector2 loc2, Tile trunk)
    {
        Vector2 mid = new Vector2(0.5f, 0.5f);
        if (trunk.name.Equals("circle_tree"))
        {
            gm.tpTrees[0] = loc1 + mid;
            gm.tpTrees[1] = loc2 + mid;
        }
        else if (trunk.name.Equals("square_tree"))
        {
            gm.tpTrees[2] = loc1 + mid;
            gm.tpTrees[3] = loc2 + mid;
        }
        else if (trunk.name.Equals("triangle_tree"))
        {
            gm.tpTrees[4] = loc1 + mid;
            gm.tpTrees[5] = loc2 + mid;
        }

        createColliderTile(new Vector3Int((int)loc1.x, (int)loc1.y, 0), trunk);
        createTile(new Vector3Int((int)loc1.x, (int)loc1.y + 1, 0), treeHead);   
        
        createColliderTile(new Vector3Int((int)loc2.x, (int)loc2.y, 0), trunk);
        createTile(new Vector3Int((int)loc2.x, (int)loc2.y + 1, 0), treeHead);


    }
    void placeStart()
    {

    }
    bool placeEnd(int quad)
    {
        int multiplyX = 1;
        int multiplyY = 1;
        if (quad == 2)
        {
            multiplyX = -1;
        }
        else if (quad == 3)
        {
            multiplyX = -1;
            multiplyY = -1;
        }
        else if (quad == 4)
        {
            multiplyY = -1;
        }

        Vector3Int randSpot = new Vector3Int(multiplyX * Random.Range(0, maxX-1), multiplyY * Random.Range(0, maxY), 0);
        if (player.GetTile(randSpot).name == "tile" && player.GetTile(randSpot+Vector3Int.right).name == "tile" && !(randSpot.x == playerPosition.x && randSpot.y == playerPosition.y) && !(randSpot.x+1 == playerPosition.x && randSpot.y == playerPosition.y))
        {
            createTile(randSpot, end1);
            createTile(randSpot+Vector3Int.right, end2);
            return true;
        }
        return false;
    }

    //Returns whether successfully placed the key
    bool placeKey(int quad)
    {
        int multiplyX = 1;
        int multiplyY = 1;
        if (quad == 2)
        {
            multiplyX = -1;
        }
        else if (quad == 3)
        {
            multiplyX = -1;
            multiplyY = -1;
        }
        else if (quad == 4)
        {
            multiplyY = -1;
        }

        Vector3Int randSpot = new Vector3Int(multiplyX*Random.Range(0, maxX), multiplyY * Random.Range(0, maxY), 0);
        Debug.Log(randSpot);
        if (player.GetTile(randSpot).name == "tile" && !(randSpot.x == playerPosition.x && randSpot.y == playerPosition.y))
        {
            createTile(randSpot, key);
            return true;
        }
        return false;
    }
    void placeVerticalLog(Vector2 start, int length)
    {
        createColliderTile(new Vector3Int((int)start.x, (int)start.y, 0), log1, r270);

        for (int i = 1; i < length - 1; i++)
        {
            createColliderTile(new Vector3Int((int)start.x, (int)start.y - i, 0), log2, r270);
        }
        createColliderTile(new Vector3Int((int)start.x, (int)start.y - length + 1, 0), log3, r270);
    }
    void placeVerticalFallenTree(Vector2 start, int length)
    {
        createColliderTile(new Vector3Int((int)start.x, (int)start.y, 0), log1, r270);

        
        for (int i = 1; i < length - 1; i++)
        {
            createColliderTile(new Vector3Int((int)start.x, (int)start.y - i, 0), log2, r270);
        }
        createTile(new Vector3Int((int)start.x, (int)start.y - length + 1, 0), giantTreeLog, r180);
        createTile(new Vector3Int((int)start.x -1, (int)start.y - length + 1, 0), giantTreeHead, r270);
        createTile(new Vector3Int((int)start.x, (int)start.y - length, 0), giantTreeHead, r90);
        createTile(new Vector3Int((int)start.x -1, (int)start.y - length, 0), giantTreeHead);

    }



    //Helper functions
    void setNode(Vector2 pos, bool isSolid)
    {
        GameManager.nodes[(int)pos.x + (-minX), (int)pos.y + (-minY)] = new Node(pos, isSolid);
    }
    void createColliderTile(Vector3Int pos, Tile t)
    {
        player.SetTile(pos, t);
        collision.SetTile(pos, t);
        setNode(new Vector2(pos.x, pos.y), true);
    }
    void createColliderTile(Vector3Int pos, Tile t, Matrix4x4 rot)
    {
        createColliderTile(pos, t);
        player.SetTransformMatrix(pos, rot);
    }
    void createTile(Vector3Int pos, Tile t, Matrix4x4 rot)
    {
        player.SetTile(pos, t);
        player.SetTransformMatrix(pos, rot);
    }
    void createTile(Vector3Int pos, Tile t)
    {
        player.SetTile(pos, t);
    }
    

}
