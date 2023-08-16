using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeartControl : MonoBehaviour {

    public GameObject heart1, heart2, heart3, gameOver;
    public static int health;

    // this will show 3 hearts on the UI and deactivates the game over screen 
    void Start()
    {
        health = 3;
        heart1.gameObject.SetActive(true);
        heart2.gameObject.SetActive(true);
        heart3.gameObject.SetActive(true);
        gameOver.gameObject.SetActive(false);

        
    }

    // keeps the hearts up to date so that when you lose a life you also lose a heart
    // reactivates the game over screen and sets the timescale to 0 so the player cant move
    void Update()
    {
        // limits the amount of hearts you can get to 3
        if (health > 3)
            health = 3;
        switch (health)
        {
            case 3:
                heart1.gameObject.SetActive(true);
                heart2.gameObject.SetActive(true);
                heart3.gameObject.SetActive(true);
                break;
            case 2:
                heart1.gameObject.SetActive(true);
                heart2.gameObject.SetActive(true);
                heart3.gameObject.SetActive(false);
                break;
            case 1:
                heart1.gameObject.SetActive(true);
                heart2.gameObject.SetActive(false);
                heart3.gameObject.SetActive(false);
                break;
            case 0:
                heart1.gameObject.SetActive(false);
                heart2.gameObject.SetActive(false);
                heart3.gameObject.SetActive(false);
                gameOver.gameObject.SetActive(true);
                Time.timeScale = 0;
                break;
        }
    }

    // takes the player back to level 1 
    public void RestartGame()
    {
        SceneManager.LoadScene("Level1");
        
    }

    // loads menu
    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    // quits the game
    public void QuitGame()
    {
        Application.Quit();
    }
}
