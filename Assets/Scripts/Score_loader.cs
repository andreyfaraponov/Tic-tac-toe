using UnityEngine;
using UnityEngine.UI;

public class Score_loader : MonoBehaviour {

    public Text wins;
    public Text draw;
    public Text lose;

	void LateUpdate () {
        wins.text = PlayerPrefs.GetInt("wins").ToString();
        draw.text = PlayerPrefs.GetInt("draw").ToString();
        lose.text = PlayerPrefs.GetInt("lose").ToString();
    }

    public void refresh_stats()
    {
        PlayerPrefs.SetInt("lose", 0);
        PlayerPrefs.SetInt("wins", 0);
        PlayerPrefs.SetInt("draw", 0);
        PlayerPrefs.SetInt("last", 0);
    }
}
