using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int score = 0;
    public Text scoreDisplay;
    private PlayerController pc;

    [SerializeField] private string sceneToLoadOnPlayerDeath = "MainMenu";
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        pc = player.GetComponent<PlayerController>();
        StartCoroutine(ScoreIncrease_Coroutine());
    }

    private void UpdateHighScore() {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (score > highScore) {
            PlayerPrefs.SetInt("HighScore", score);
        }
    }

    public void UpdateScoreDisplay(int score) {
        int displayScore = score - (score % 5);
        scoreDisplay.text = "Score: " + displayScore;
    }

    void OnEnable() {
        PlayerController.OnPlayerDeath += PlayerDeath;
    }

    void OnDisable() {
        PlayerController.OnPlayerDeath -= PlayerDeath;
    }

    public void PlayerDeath() {
        PlayerPrefs.SetInt("Score", score);
        SceneManager.LoadScene(sceneToLoadOnPlayerDeath);
    }

    IEnumerator ScoreIncrease_Coroutine() {
        while (true)
        {
            score = score + (int)(pc.speedVal * 0.1f);
            UpdateScoreDisplay(score);
            UpdateHighScore();
            yield return new WaitForSeconds(1.0f);
        }
    }

}
