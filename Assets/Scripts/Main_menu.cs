using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_menu : MonoBehaviour {

    int wins;
    int lose;
    int draw;

    void Start()
    {
        wins = PlayerPrefs.GetInt("wins");
        lose = PlayerPrefs.GetInt("lose");
        draw = PlayerPrefs.GetInt("draw");
    }

    public void    exit()
    {
        Application.Quit();
    }
}
