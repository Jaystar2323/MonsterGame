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

    [SerializeField] AStarTile tile;
    [SerializeField] AStarTile log1;
    [SerializeField] AStarTile log2;
    [SerializeField] AStarTile log3;
    [SerializeField] AStarTile bush;
    // Start is called before the first frame update
    int minX;
    int maxX;
    int minY;
    int maxY;
    void Start()
    {
        minX = -width - 1;
        maxX = width;
        minY = -height;
        maxY = height;
        //player.BoxFill(new Vector3Int(0, 0, 0), tile, width, height, -width, -height);
        for (int i = minX; i <= maxX; i++)
        {
            for (int j = minY; j <= maxY; j++)
            {
                ScriptableObject.CreateInstance<AStarTile>();
                player.SetTile(new Vector3Int(i, j, 1), tile);
            }
        }
        placeHorizontalLog(new Vector2(3, 4), 5);
        placeBush(new Vector2(4,3));
        placeBush(new Vector2(-4,-3));
        placeBush(new Vector2(4,-7));
        placeBush(new Vector2(4,3));
        player.RefreshAllTiles();


        for (int i = minX; i <= maxX; i++)
        {
            for (int j = minY; j <= maxY; j++)
            {
                gm.getTile(new Vector2(i, j)).initTile();
            }
        }
        Debug.Log(gm.findPath(new Vector2(4,4), new Vector2(-1,-4)));
    }
    
    void placeHorizontalLog(Vector2 start, int length)
    {
        player.SetTile(new Vector3Int((int)start.x, (int)start.y, 1), log1);
        collision.SetTile(new Vector3Int((int)start.x, (int)start.y, 1), log1);

        for (int i = 1; i < length-1; i++)
        {
            player.SetTile(new Vector3Int((int)start.x+i, (int)start.y, 1), log2);
            collision.SetTile(new Vector3Int((int)start.x+i, (int)start.y, 1), log2);
        }
        player.SetTile(new Vector3Int((int)start.x+length-1, (int)start.y, 1), log3);
        collision.SetTile(new Vector3Int((int)start.x+length-1, (int)start.y, 1), log3);
    }
    void placeBush(Vector2 start)
    {
        player.SetTile(new Vector3Int((int)start.x, (int)start.y, 1), bush);
    }
    void placeHorizontalFallenTreen(Vector2 start, int length)
    {

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
}
