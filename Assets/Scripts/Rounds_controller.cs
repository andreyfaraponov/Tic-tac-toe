using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum Result
{
    Player,
    AI,
    Draw
}
public class Rounds_controller : MonoBehaviour {

    /*          BASE VARIABLES          */
    GameObject[,] field;
    GameObject field_c;
    Ray r;
    RaycastHit coll;
    GameObject buf;
    public int i;
    bool player;
    bool ai;
    bool turn;
    AI_control ai_control;

    /*          SHAPE PREFABS           */
    public GameObject cross_pref;
    public GameObject zero_pref;
    public GameObject empty_pref;
    
    /*          IMAGES & TEXT           */
    public Sprite cross_sprite_ref;
    public Sprite zero_sprite_ref;
    public Text Wins_text;
    public Text Draws_text;
    public Text Loses_text;
    public GameObject player_sprite;
    public GameObject ai_sprite;
    public GameObject next_turn_sprite;
    public Text result_text;

    /*          PANELS                  */
    public GameObject Result_panel;
    public GameObject www;
    Result last_res = Result.Player;


    void Start ()
    {
        ai_control = GameObject.Find("AI_player").GetComponent<AI_control>();
        last_res = (Result)PlayerPrefs.GetInt("last");
        Debug.Log(last_res.ToString());
        field_initialization();
        i = 9;
        game_start();
    }
	
	void LateUpdate ()
    {
        r = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(r, out coll, 200f))
            if (Input.GetMouseButton(0) && coll.collider.gameObject.name == "empty(Clone)"
                && i > 0 && turn == player)
            {
                Debug.Log("Player TURNS");
                buf = Instantiate(shape_init(), coll.transform.position, coll.transform.rotation);
                buf.transform.SetParent(GameObject.Find("Field_control").transform);
                buf.transform.localScale = coll.transform.localScale;
                DestroyImmediate(coll.collider.gameObject);
                null_finder(buf);
                i--;
                find_winner();
                turn = !turn;
                sprite_turn_changer();
            }
        else if (i > 0 && turn == ai)
            {
                ai_control.Ai_turn(this, ref field);
                i--;
                find_winner();
                turn = !turn;
                sprite_turn_changer();
            }
    }
    void sprite_turn_changer()//CHANGES SPRITE FOR NEXT TURN
    {
        if (ai == turn)
            next_turn_sprite.GetComponent<Image>().sprite = ai_sprite.GetComponent<Image>().sprite;
        if (player == turn)
            next_turn_sprite.GetComponent<Image>().sprite = player_sprite.GetComponent<Image>().sprite;
    }
    void game_start()//INITIALIZE NEW GAME FIELD
    {
        www.SetActive(true);
        if (last_res == Result.AI)
        {
            ai = false;
            player_sprite.GetComponent<Image>().sprite = zero_sprite_ref;
            ai_sprite.GetComponent<Image>().sprite = cross_sprite_ref;
            next_turn_sprite.GetComponent<Image>().sprite = ai_sprite.GetComponent<Image>().sprite;
        }
        else
        {
            ai = true;
            player_sprite.GetComponent<Image>().sprite = cross_sprite_ref;
            ai_sprite.GetComponent<Image>().sprite = zero_sprite_ref;
            next_turn_sprite.GetComponent<Image>().sprite = player_sprite.GetComponent<Image>().sprite;
        }
        player = !ai;
    }
    void saveResults(Result res)//SAVE RESULT
    {
        if (res == Result.AI)
            PlayerPrefs.SetInt("lose", PlayerPrefs.GetInt("lose") + 1);
        if (res == Result.Player)
            PlayerPrefs.SetInt("wins", PlayerPrefs.GetInt("wins") + 1);
        if (res == Result.Draw)
            PlayerPrefs.SetInt("draw", PlayerPrefs.GetInt("draw") + 1);
        PlayerPrefs.SetInt("last", (int)res);
    }
    void resultsUpdStatus()//UPDATE SCREEN WITH NEW RESULTS
    {
        Wins_text.text = PlayerPrefs.GetInt("wins").ToString();
        Draws_text.text = PlayerPrefs.GetInt("draw").ToString();
        Loses_text.text = PlayerPrefs.GetInt("lose").ToString();
    }
    void field_initialization()//INITIALIZATION OF FIELD
    {
        field = new GameObject[3, 3];
        resultsUpdStatus();
        field_c = GameObject.Find("Field_control");
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
            {
                field[i, j] = Instantiate(empty_pref, new Vector3(i, j, 0), Quaternion.identity);
                field[i, j].transform.SetParent(field_c.transform);
            }
        field_c.transform.localScale = new Vector3(2, 2, 2);
        field_c.transform.position = new Vector3(field_c.transform.position.x - 3, field_c.transform.position.y, 0);
    } 
    void null_finder(GameObject obj)//ARRAY OBJECT CONTROL
    {
        for (int x = 0; x < 3; x++)
            for (int y = 0; y < 3; y++)
                if (!field[x, y])
                    field[x, y] = obj;
    }
    void find_winner()//DESCR SAME AS THE NAME FT
    {
        if (array_checker())
        {
            if (turn == ai)
            {
                last_res = Result.AI;
                saveResults(last_res);
                result_text.text = "AI WON";
            }
            if (turn == player)
            {
                last_res = Result.Player;
                saveResults(last_res);
                result_text.text = "PLAYER WON";
            }
            resultsUpdStatus();
            field_c.SetActive(false);
            Result_panel.SetActive(true);
            www.SetActive(false);
            return ;
        }
        if (i == 0)
        {
            result_text.text = "THE DRAW";
            last_res = Result.Draw;
            saveResults(last_res);
            field_c.SetActive(false);
            Result_panel.SetActive(true);
            resultsUpdStatus();
            www.SetActive(false);
        }
    }
    public GameObject shape_init()//PREF_OBJ CHANGER
    {
        if (!turn)
            return cross_pref;
        return zero_pref;
    }
    bool array_checker()//ARRAY CHECK && UPDATE
    {
        for (int x = 0; x < 3; x++)
            if (field[x, 0].name == field[x, 1].name &&
                field[x, 0].name == field[x, 2].name &&
                (field[x, 0].name == "Cross(Clone)" || field[x, 0].name == "Zero(Clone)"))
            {
                Debug.Log("WE HAVE CHAMPION!");
                return true;
            }
        for (int y = 0; y < 3; y++)
            if (field[0, y].name == field[1, y].name &&
                field[0, y].name == field[2, y].name &&
                (field[0, y].name == "Cross(Clone)" || field[0, y].name == "Zero(Clone)"))
            {
                Debug.Log("WE HAVE CHAMPION!");
                return true;
            }
        if (field[0, 0].name == field[1, 1].name &&
                field[0, 0].name == field[2, 2].name &&
                (field[0, 0].name == "Cross(Clone)" || field[0, 0].name == "Zero(Clone)"))
        {
            Debug.Log("WE HAVE CHAMPION!");
            return true;
        }
        if (field[2, 0].name == field[1, 1].name &&
        field[2, 0].name == field[0, 2].name &&
        (field[2, 0].name == "Cross(Clone)" || field[2, 0].name == "Zero(Clone)"))
        {
            Debug.Log("WE HAVE CHAMPION!");
            return true;
        }
        return false;
    }
}
