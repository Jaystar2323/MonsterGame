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
        //player.BoxFill(new Vector3Int(0, 0, 0), tile, width, height, -width, -height);
        for (int i = minX; i <= maxX; i++)
        {
            for (int j = minY; j <= maxY; j++)
            {
                player.SetTile(new Vector3Int(i, j, 1), tile);
                setNode(new Vector2(i, j), false);

            }
        }
        placeHorizontalLog(new Vector2(3, 4), 5);
        placeBush(new Vector2(4,3));
        placeBush(new Vector2(-4,-3));
        placeBush(new Vector2(4,-7));
        placeBush(new Vector2(4,3));
        placeHorizontalFallenTreen(new Vector2(-2, 3), 4);
        player.RefreshAllTiles();
        //fow.init(player);
        //List<Vector2> path = gm.findPath(new Vector2(8,4), new Vector2(-1,-4));
        //Debug.Log("Final Path:");
        //GameManager.printList(path);
    }
    
    void placeHorizontalLog(Vector2 start, int length)
    {
        creatColliderTile(new Vector3Int((int)start.x, (int)start.y, 1), log1);

        for (int i = 1; i < length-1; i++)
        {
            creatColliderTile(new Vector3Int((int)start.x + i, (int)start.y, 1), log2);
        }
        creatColliderTile(new Vector3Int((int)start.x + length - 1, (int)start.y, 1), log3);
    }
    void placeBush(Vector2 start)
    {
        player.SetTile(new Vector3Int((int)start.x, (int)start.y, 1), bush);
    }
    void placeHorizontalFallenTreen(Vector2 start, int length)
    {
        creatColliderTile(new Vector3Int((int)start.x, (int)start.y, 1), log1);

        for (int i = 1; i < length - 1; i++)
        {
            creatColliderTile(new Vector3Int((int)start.x + i, (int)start.y, 1), log2);
        }
        creatColliderTile(new Vector3Int((int)start.x + length - 1, (int)start.y, 1), giantTreeLog, r270);
        creatColliderTile(new Vector3Int((int)start.x + length - 1, (int)start.y - 1, 0), giantTreeHead);
        creatColliderTile(new Vector3Int((int)start.x + length, (int)start.y, 0), giantTreeHead, r180);
        creatColliderTile(new Vector3Int((int)start.x + length, (int)start.y - 1, 0), giantTreeHead, r90);
        
    }
    void placeSmallRock(Vector2 start)
    {

    }
    void placeLargeRock(Vector2 start)
    {

    }
    void placeTree(Vector2 start)
    {

    }
    void placeTpTree(Vector2 loc1, Vector2 loc2, AStarTile trunk)
    {

    }
    void setNode(Vector2 pos, bool isSolid)
    {
        GameManager.nodes[(int)pos.x + (-minX), (int)pos.y + (-minY)] = new Node(pos, isSolid);
    }
    void creatColliderTile(Vector3Int pos, Tile t)
    {
        player.SetTile(pos, t);
        collision.SetTile(pos, t);
        setNode(new Vector2(pos.x, pos.y), true);
    }
    void creatColliderTile(Vector3Int pos, Tile t, Matrix4x4 rot)
    {
        Vector3Int s = new Vector3Int((int)pos.x, (int)pos.y, 0);
        creatColliderTile(s, t);
        player.SetTransformMatrix(s, rot);
    }
}
