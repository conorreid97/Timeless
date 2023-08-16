using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchLevel : MonoBehaviour {

    [SerializeField]
    private string loadLevel;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (GameManager.Instance.CollectedPortalGun >= 1)
        {
            if (other.CompareTag("Player"))
            {
                FindObjectOfType<AudioManager>().Play("LevelComplete");
                Debug.Log("Switch");
                SceneManager.LoadScene(loadLevel);
            }
        }
        
    }
}
