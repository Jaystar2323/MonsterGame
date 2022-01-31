using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class MonsterManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Tilemap collisionMap;
    [SerializeField] GameObject player;
    [SerializeField] GameObject monster;
    [SerializeField] Tilemap grid;
    GameManager gm;
    PlayerManager pm;
    int moves;
    private Transform monsT;
    float time;
    List<Vector2> path;
    void Start()
    {
        monsT = monster.GetComponent<Transform>();
        pm = GetComponent<PlayerManager>();
        gm = GetComponent<GameManager>();

        time = 0;
        pm.changeDieSpriteMonster(0);

        monsT.position = new Vector3(12.5f, 6.5f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (moves > 0 && time >= 1)
        {

            //Vector2[] tiles = getAvailabeTiles();

            if (path == null)
            {
                Debug.Log("Null");
                time = 0;
                return;
            }
            Vector2 bestMove = path[0];
            path.RemoveAt(0);
            bestMove.x += 0.5f;
            bestMove.y += 0.5f;
         
            //float bestDistance = float.MaxValue;
            //for (int i = 0; i < tiles.Length; i++)
            //{
            //    float d1 = Vector2.Distance(tiles[i], player.GetComponent<Transform>().position);
            //    if (d1 < bestDistance)
            //    {
            //        bestDistance = d1;
            //        bestMove = tiles[i];
            //    }
            //}

            moves -= 1;
            if (moves == 0)
            {
                pm.monsterFinishTurn();
            }
            bestMove = gm.getTpTree(bestMove);
            monsT.transform.position = bestMove;

            checkTouchingPlayer();
            time = 0;
        }
        pm.changeDieSpriteMonster(moves);

        time += Time.deltaTime;
    }

    public void takeTurn(int moves)
    {
        grid.GetComponent<FogOfWar>().init();

        path = gm.findPath(monsT.position, player.GetComponent<Transform>().position);
        //Debug.Log(path);

        //Check tp trees

        this.moves = moves;
        pm.changeDieSpriteMonster(moves);
        time = 0;
        Debug.Log("Monster gets " + moves + " moves!");

    }
    public Vector2[] getAvailabeTiles()
    {
        Vector2[] tiles = new Vector2[4];
        //int i = 0;
        tiles[0] = move(1,0);
        tiles[1] = move(-1, 0);
        tiles[2] = move(0, 1);
        tiles[3] = move(0, -1);
        return tiles;
    }
    public int getMoves()
    {
        return moves;
    }

    public Vector2 move(int x, int y)
    {
        Vector3 posInit = new Vector3(monsT.transform.position.x + x, monsT.transform.position.y + y, monsT.transform.position.z);
        Vector3Int position = grid.WorldToCell(posInit);
        TileBase tile2 = grid.GetTile(position);
        TileBase tile = collisionMap.GetTile(position);
        

        if (tile == null && tile2 != null)
        {
            return posInit;
        }
        else
        {
            return new Vector2(20000, 20000);
        }
    }
    public void checkTouchingPlayer()
    {
        float j = GameManager.distance(player.GetComponent<Transform>().position, monsT.transform.position);
        if (j < 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Debug.Log("Player Died!");

        }
    }
}
