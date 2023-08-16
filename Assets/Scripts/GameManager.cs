using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    private static GameManager instance;

    bool gameHasEnded = false;

    [SerializeField]
    private GameObject coinPrefab;

    [SerializeField]
    private Text coinTxt;

    private int collectedCoins;

    [SerializeField]
    private GameObject grenadePrefab;

    [SerializeField]
    private Text grenadeTxt;

    public int collectedGrenades;

    [SerializeField]
    private GameObject portalgunPrefab;

    [SerializeField]
    private Text portalgunTxt;

    public int collectedPortalGun;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    private void Start()
    {
        collectedCoins = PlayerPrefs.GetInt("CurrentScore");
    }

    public GameObject CoinPrefab
    {
        get
        {
            return coinPrefab;
        }
    }

    // keeps track of the collected coins
    public int CollectedCoins
    {
        get
        {
            return collectedCoins;
        }

        set
        {
            coinTxt.text = value.ToString();
            collectedCoins = value;
        }
    }

    public GameObject GrenadePrefab
    {
        get
        {
            return grenadePrefab;
        }
    }

    // keeps track of the collected grenades
    public int CollectedGrenades
    {
        get
        {
            return collectedGrenades;
        }

        set
        {
            grenadeTxt.text = value.ToString();
            collectedGrenades = value;
        }
    }

    public GameObject PortalGunPrefab
    {
        get
        {
            return portalgunPrefab;
        }
    }

    // keeps track of the collected portal gun
    public int CollectedPortalGun
    {
        get
        {
            return collectedPortalGun;
        }

        set
        {
            portalgunTxt.text = value.ToString();
            collectedPortalGun = value;
        }
    }

    // changes the bool for ending the game
    public void EndGame()
    {
        if (gameHasEnded == false)
        {
            gameHasEnded = true;
            Debug.Log("GameOver!!!!");
        }   
    }

    
}
