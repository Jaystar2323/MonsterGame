using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MonsterManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Tilemap collisionMap;
    [SerializeField] GameObject player;
    [SerializeField] GameObject monster;
    [SerializeField] Tilemap grid;
    [SerializeField] Text msg;
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
            bestMove = gm.getTpTree(bestMove);
            monsT.transform.position = bestMove;
            moves -= 1;
            
            if (!checkTouchingPlayer() && moves == 0)
            {
                string tilename = grid.GetTile(grid.WorldToCell(monsT.position)).name;
               // Debug.Log(tilename);
                path = gm.findPath(monsT.position, player.GetComponent<Transform>().position);

                if (tilename == "bush" || tilename == "tree_head" || tilename == "giant_tree_trunk" || tilename == "giant_tree_head")
                {
                    msg.text = "Monster is hidden";

                }
                else if (path.Count <= 10)
                {
                    msg.text = path.Count + " moves away!";
                }
                else
                {
                    if (monsT.position.x < 0 && monsT.position.y < 0)
                    {
                        msg.text = "Bottom left";
                    }
                    if (monsT.position.x > 0 && monsT.position.y < 0)
                    {
                        msg.text = "Bottom right";
                    }
                    if (monsT.position.x < 0 && monsT.position.y > 0)
                    {
                        msg.text = "Top left";
                    }
                    if (monsT.position.x > 0 && monsT.position.y > 0)
                    {
                        msg.text = "Top right";
                    }
                }
                pm.monsterFinishTurn();
                
            }


            
            time = 0;
        }
        pm.changeDieSpriteMonster(moves);

        time += Time.deltaTime;
    }

    public void takeTurn(int moves)
    {
        //grid.GetComponent<FogOfWar>().init();
        //moves = 4;
        path = gm.findPath(monsT.position, player.GetComponent<Transform>().position);
        if (path == null)
        {
            return;
        }
        //Debug.Log(moves + " " + path.Count);
        if (path.Count > moves)
        {
            List<Vector2> treePath = null;
            List<Vector2> tp;
            foreach (Vector2 tree in gm.tpTrees)
            {
                tp = gm.findPath(monsT.position, tree);
                
                if (tp != null)
                {
                    //Debug.Log(gm.getTpTree(tree) + " " + (Vector2)player.GetComponent<Transform>().position);
                    List<Vector2> afterTp = gm.findPath(gm.getTpTree(tree), (Vector2)player.GetComponent<Transform>().position);
                    //Debug.Log(afterTp);
                    foreach (Vector2 i in afterTp)
                    {
                        tp.Add(i);
                    }
                    
                   //  Debug.Log(tp.Count);
                    if (treePath == null || tp.Count < treePath.Count)
                    {
                        treePath = tp;
                    }
                }
                

            }
            //Debug.Log(treePath.Count + " " + path.Count);
            if (treePath != null && treePath.Count < path.Count)
            {
                //Debug.Log(treePath.Count + " " + path.Count);
                path = treePath;
            }

        }


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
    public bool checkTouchingPlayer()
    {
        float j = GameManager.distance(player.GetComponent<Transform>().position, monsT.transform.position);
        if (j < 1)
        {
            SceneManager.LoadScene("GameOver"); //Should be death screen
            Debug.Log("Player Died!");
            return true;
        }
        return false;
    }
}
