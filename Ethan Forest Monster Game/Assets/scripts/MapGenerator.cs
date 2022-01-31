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
        placeBush(new Vector2(4,3));
        placeBush(new Vector2(-4,-3));
        placeBush(new Vector2(4,-7));
        placeBush(new Vector2(4,3));
        placeHorizontalFallenTreen(new Vector2(-6, 3), 4);
        placeSmallRock(new Vector2(-5, -1), 2);
        placeLargeRock(new Vector2(8, -4), 2);
        placeLargeRock(new Vector2(-10, 1), 2);
        placeTree(new Vector2(-6, -6));
        placeTpTree(new Vector2(11, -7), new Vector2(-12, 3), triangle);
        placeTpTree(new Vector2(0, -5), new Vector2(8, 6), square);
        placeTpTree(new Vector2(-2, 5), new Vector2(4, 0), circle);
        player.RefreshAllTiles();
        fow.init();

        //Debug.Log(gm.findPath(new Vector2(-5, -2), new Vector2(12, 6)).Count);
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
        if (trunk.name.Equals("circle_tree"))
        {
            gm.circle1 = loc1;
            gm.circle2 = loc2;
        }
        else if (trunk.name.Equals("square_tree"))
        {
            gm.square1 = loc1;
            gm.square2 = loc2;
        }
        else if (trunk.name.Equals("triangle_tree"))
        {
            gm.triangle1 = loc1;
            gm.triangle2 = loc2;
        }

        createColliderTile(new Vector3Int((int)loc1.x, (int)loc1.y, 0), trunk);
        createTile(new Vector3Int((int)loc1.x, (int)loc1.y + 1, 0), treeHead);   
        
        createColliderTile(new Vector3Int((int)loc2.x, (int)loc2.y, 0), trunk);
        createTile(new Vector3Int((int)loc2.x, (int)loc2.y + 1, 0), treeHead);


    }
    void placeStart()
    {

    }
    void placeEnd()
    {

    }
    void placeVerticalLog()
    {

    }
    void placeVerticalFallenTree()
    {

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
