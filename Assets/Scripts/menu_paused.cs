using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class menu_paused : MonoBehaviour {

    public void main_menu()
    {
        SceneManager.LoadScene(0);
    }
    public void one_more_time()
    {
        SceneManager.LoadScene(1);
    }

    public void appExit()
    {
        Application.Quit();
    }
}
