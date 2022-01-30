using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    private int moves;

    public Grid grid;
    public GameObject gm;
    private PlayerManager pm;
    private MonsterManager mm;
    [SerializeField] private Tilemap map;
    private RaycastHit2D[] hit;
    void Start()
    {
        pm = gm.GetComponent<PlayerManager>();
        mm = gm.GetComponent<MonsterManager>();

        setPosition(-5, -2);
        map.GetComponent<FogOfWar>().updateTiles();
    }

    // Update is called once per frame
    void Update()
    {
        if (moves > 0 && Input.GetMouseButton(0))
        {
            Vector3 newPos = getMousePos();
            if (((newPos.x == transform.position.x + 1 || newPos.x == transform.position.x - 1)  && newPos.y == transform.position.y) || ((newPos.y == transform.position.y + 1 || newPos.y == transform.position.y - 1) && newPos.x == transform.position.x))
            {
                
                if (map.GetTile(map.WorldToCell(newPos)) != null)
                {
                    if ((moves == 1 && map.GetTile(map.WorldToCell(newPos)).name == "tile") || moves > 1)
                    {
                        GetComponent<Transform>().transform.position = newPos;
                        moves -= 1;
                        pm.changeDieSprite(moves);
                        mm.checkTouchingPlayer();
                        map.GetComponent<FogOfWar>().updateTiles();
                    }

                    
                }
                
            }
        }
    }



    public void takeTurn(int moves)
    {
        this.moves = moves;
    }
    public int getMoves()
    {
        return moves;
    }
    private Vector3 getMousePos()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 newPos = grid.WorldToCell(mousePosition);
        newPos.x += 0.5f;
        newPos.y += 0.5f;
        return newPos;
    }
    public void setPosition(int x, int y)
    {
        Vector2 pos = new Vector2(x, y);
        pos.x += 0.5f;
        pos.y += 0.5f;
        transform.position = pos;
    }

}
