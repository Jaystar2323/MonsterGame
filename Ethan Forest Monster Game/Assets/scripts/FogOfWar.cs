using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FogOfWar : MonoBehaviour
{
    // Start is called before the first frame update
    Tilemap tm;
    [SerializeField] Transform player;
    public int width;
    public int height;
    void Start()
    {
       // GetComponent<TilemapRenderer>().enabled = true;
        tm = GetComponent<Tilemap>();
        //for (int i = -width; i < width; i++)
        //{
        //    for (int j = -height; j < height; j++)
        //    {
        //        Tile tile = (Tile)tm.GetTile(new Vector3Int(i, j, 0));
        //        tile.color = new Color(1,1,1);
                
        //    }
        //}
        //tm.RefreshAllTiles();
        //updateTiles();
        
    }

    public void updateTiles()
    {
        Vector3Int playerPos = tm.WorldToCell(player.position);
        int[] maxs = {1, 2,3,3,3,2,1 };
        int k = 0;
        for (int j = -3; j <= 3; j ++)
        {
            
            for (int i = -maxs[k]; i <= maxs[k]; i++)
            {
                Tile tile = (Tile)tm.GetTile(new Vector3Int(playerPos.x + i, playerPos.y + j, playerPos.z));
                if (tile != null)
                {
                    if (i == maxs[k] || i == (-maxs[k]) ||  j == 3 || j == -3)
                    {
                        tile.color = Color.gray;
                    }
                    else
                    {
                        tile.color = Color.white;
                    }
                    
                    tm.RefreshTile(new Vector3Int(playerPos.x + i, playerPos.y + j, playerPos.z));
                }
                
            }
            k++;
        }
    }
}
