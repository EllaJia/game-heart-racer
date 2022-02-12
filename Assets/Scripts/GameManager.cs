using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool gameStarted;
    public GameObject platformSpawner;
    public GameObject gamePlayUI;
    public GameObject menuUI;
    public Text highScoreText;
    public Text scoreText;
    AudioSource audioSource;
    public AudioClip[] gameMusic;

    int score = 0;
    int highScore;
    private void Awake() // called before Start function.
    {
        if (instance == null)
        {
            instance = this;  // After this, the instance variable would be available everywhere
        }

        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore");
        highScoreText.text = "Best Score : " + highScore;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameStarted)
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameStart();
            }
        }
    }

    public void GameStart()
    {
        gameStarted = true;
        platformSpawner.SetActive(true);
        
        menuUI.SetActive(false);
        gamePlayUI.SetActive(true);

        // play audio
        audioSource.clip = gameMusic[1];
        audioSource.Play(); // play the audio in loop

        // StartCoroutine("UpdateScore"); // use this line, Heart is a bonus point; not use this line, Heart is the only way to get score
    }

    public void GameOver()
    {
        platformSpawner.SetActive(false);
        StopCoroutine("UpdateScore");
        SaveHighScore();
        Invoke("ReloadLevel", 1f);

    }

    void ReloadLevel()
    {
        SceneManager.LoadScene("Game");
    }

    IEnumerator UpdateScore()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f); // wait for 1 seconds while the game is played
            score++;
            scoreText.text = score.ToString(); // convert score integer to a string so that we can use it in UI text element
            // print(score);
        }
    }

    public void IncrementScore()
    {
        score += 1;
        scoreText.text = score.ToString(); // use this line, Heart is the only way to get score; not use this line, Heart is a bonus point
        audioSource.PlayOneShot(gameMusic[2], 0.2f); // only to play it one time, the second variable is for controlling the volume (20% of the full volume)
    }

    void SaveHighScore()
    {
        if (PlayerPrefs.HasKey("HighScore"))
        {
            // already have a highscore
            if (score > PlayerPrefs.GetInt("HighScore"))
            {
                PlayerPrefs.SetInt("HighScore", score);
            }
        }
        else
        {
            // playing for the first time
            PlayerPrefs.SetInt("HighScore", score);
        }
    }
}

