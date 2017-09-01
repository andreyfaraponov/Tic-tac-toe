using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class menu_paused : MonoBehaviour {

    public void main_menu()
    {
        //PlayerPrefs.SetInt("wins", wins);
        //PlayerPrefs.SetInt("draw", draw);
        //PlayerPrefs.SetInt("lose", lose);
        SceneManager.LoadScene(0);
       // SceneManager.UnloadSceneAsync(1);
    }
    public void one_more_time()
    {
        SceneManager.LoadScene(1);
    }
}
