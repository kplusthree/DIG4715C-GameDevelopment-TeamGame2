using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [HideInInspector]
    public int score;
    [HideInInspector]
    public int detection;
    [HideInInspector]
    public bool gameOver;
    [HideInInspector]
    public bool restart;
    [HideInInspector]
    public bool win;
    [HideInInspector]
    public bool detected;
    [HideInInspector]
    public bool pause;
    [HideInInspector]
    public bool endScreen;

    public Text scoreText;
    public Text detectionText;
    public Text restartText;
    public Text gameOverText;
    public Text pauseText;

    public AudioClip gameMusic;
    public AudioClip winMusic;
    public AudioClip loseMusic;

    public AudioSource musicSource;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        detection = 0;
        scoreText.text = "Score: " + score;
        detectionText.text = "Detection: " + detection + "/100%";
        restartText.text = "";
        gameOverText.text = "";
        pauseText.text = "";

        gameOver = false;
        restart = false;
        detected = false;
        pause = false;
        endScreen = false;

        musicSource.clip = gameMusic;
        musicSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        // checks if the player is trying to quit the game
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
        if (restart == true && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (pause == false && gameOver == false && Input.GetKeyDown(KeyCode.P))
        {
            pauseText.text = "Paused";
            Time.timeScale = 0;
            pause = true;
            musicSource.Pause();
        }
        else if (Input.GetKeyDown(KeyCode.P) && pause == true)
        {
            pauseText.text = "";
            Time.timeScale = 1;
            pause = false;
            musicSource.Play();
        }

        if (detection == 100 && endScreen == false)
        {
            endScreen = true;
            gameOver = true;
            GameOver();
        }

        if (win == true && endScreen == false)
        {
            endScreen = true;
            Win();
        }

        if (gameOver)
        {
            restartText.text = "Press 'R' for Restart";
            restart = true;
        }
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    public void AddPoints(int points)
    {
        score += points;
        UpdateScore();
    }

    void UpdateDetection()
    {
        detectionText.text = "Detection: " + detection + "/100%";
    }

    public void AddDetection()
    {
        if (detected == false && win == false)
        {
            detected = true;
            detection += 1;
            UpdateDetection();
            StartCoroutine(Waiting());
        }
    }

    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(0.1f);
        detected = false;
    }

    public void Win()
    {
        gameOverText.text = "You Win! 'Esc' To Quit";
        gameOver = true;
        musicSource.Stop();
        musicSource.clip = winMusic;
        musicSource.Play();
    }

    public void GameOver()
    {
        gameOverText.text = "Game Over! 'Esc' To Quit";
        musicSource.Stop();
        musicSource.clip = loseMusic;
        musicSource.Play();
    }
}
