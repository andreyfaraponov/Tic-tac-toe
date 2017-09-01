using UnityEngine.SceneManagement;
using UnityEngine;

public class pause_menu : MonoBehaviour {

    public void     main_menu()
    {
        //PlayerPrefs.SetInt("wins", wins);
        //PlayerPrefs.SetInt("draw", draw);
        //PlayerPrefs.SetInt("lose", lose);
        SceneManager.LoadScene(0);
        SceneManager.UnloadSceneAsync(1);
    }
}
