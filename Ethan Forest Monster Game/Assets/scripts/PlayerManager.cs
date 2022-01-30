using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject die;
    [SerializeField] GameObject dieMonster;
    public Player player;

    public Sprite six;
    public Sprite five;
    public Sprite four;
    public Sprite three;
    public Sprite two;
    public Sprite one;
    public Sprite zero;

    private Sprite[] rollSprites;
    [SerializeField] MonsterManager monster;

    private bool turn = true;




    void Start()
    {
        rollSprites = new Sprite[]{zero, one, two, three, four, five, six};
        changeDieSprite(0);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space) && player.getMoves() <= 0 && turn == true)
        {
            int roll = rollDie();
            changeDieSprite(roll);
            player.takeTurn(roll);
            turn = false;
        }
        if (player.getMoves() == 0 && turn == false && monster.getMoves() == 0)
        {
            monster.takeTurn(rollDie());

        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    int rollDie()
    {
        int roll = Random.Range(1,7);
        return roll;
        
    }
    public void changeDieSprite(int value)
    {
        if (value >= 0 && value <= 6)
        {
            die.GetComponent<SpriteRenderer>().sprite = rollSprites[value];

        }
    }
    public void changeDieSpriteMonster(int value)
    {
        if (value >= 0 && value <= 6)
        {
            dieMonster.GetComponent<SpriteRenderer>().sprite = rollSprites[value];

        }
    }
    public void monsterFinishTurn()
    {
        turn = true;
    }

}
