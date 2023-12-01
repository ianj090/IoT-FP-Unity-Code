using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private int highScore = 0;
    public Text highScoreText;
    private int latestScore = 0;
    public Text latestScoreText;

    void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "High Score: " + highScore;

        latestScore = PlayerPrefs.GetInt("Score", 0);
        latestScoreText.text = "Latest Score: " + latestScore;

        if (latestScore == highScore && highScore != 0) {
            highScoreText.color = Color.red;
        } else {
            highScoreText.color = Color.black;
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
        Time.timeScale = 1.0f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
