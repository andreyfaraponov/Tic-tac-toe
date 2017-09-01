using UnityEngine;
using UnityEngine.SceneManagement;

public class diff_turner : MonoBehaviour {

    public void    normal()
    {
        PlayerPrefs.SetInt("diff", 0);
        SceneManager.LoadScene(1);
    }

    public void    nightmare()
    {
        PlayerPrefs.SetInt("diff", 1);
        SceneManager.LoadScene(1);
    }

    public void     hell()
    {
        PlayerPrefs.SetInt("diff", 2);
        SceneManager.LoadScene(1);
    }
}
