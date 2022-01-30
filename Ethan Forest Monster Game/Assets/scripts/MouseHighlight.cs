using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MouseHighlight : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Tile tile;
    Vector3Int currTile;
    Tilemap tm;
    void Start()
    {
        tm = GetComponent<Tilemap>();
    }

    // Update is called once per frame
    
    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int mouseT = tm.WorldToCell(mousePosition);

        if (!currTile.Equals(mouseT))
        {
            tm.SetTile(mouseT, tile);
            tm.SetTile(currTile, null);
            currTile = mouseT;
        }

    }
}
