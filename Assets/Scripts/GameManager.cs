using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{


    public static int currency;
    public static int playerHealth;
    public GameObject[] towers; //Prefabs of all possible towers
    public GameObject towerSelectButton;
    public GameObject towerDescriptionPanel;
    public GameObject gameOverPanel;
    public GameObject waveInfoButton;
    public TextMeshProUGUI buyingDescriptionText; //The text to populate when looking to buy a tower
    public TextMeshProUGUI buyingNameText; //The name of a tower the player is looking to buy
    public TextMeshProUGUI selectedDescription; //The description of a selected tower
    public TextMeshProUGUI selectedName; //The name of a selected tower
    public static bool towerBeingPlaced;
    public static bool towerIsSelected;
    public static bool isWaveStarted; //Should be true after player presses button and during whole wave.
    public static bool isPhaseStarted;
    public bool isGameOver;
    Dictionary<string, int> towerCosts;
    public Transform spawnPoint; //The point where the enemies spawn
    public static TowerController selectedTower;
    public int prefabVal;
    public TextMeshProUGUI currencyText;
    public TextMeshProUGUI healthText;
    public AudioClip loseGameSound; // sound effect for ewhen patient dies
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1; //Make sure game doesn't start paused
        towerCosts = new Dictionary<string, int>();
        audioSource = GetComponent<AudioSource>();
        currency = 50;
        playerHealth = 100;
        towerBeingPlaced = false;
        towerIsSelected = false;
        isWaveStarted = false;
        isPhaseStarted = false;
        isGameOver = false;
        TowerCostDictionary();
        Debug.Log(towerCosts["Blue"]);
        Debug.Log(currency);
    }

    // Update is called once per frame
    void Update()
    {
        if(playerHealth <= 0 && !isGameOver){ //only call gameOver once
            gameOver();
            isGameOver = true;
            return;
        }
        else if(isGameOver){ //Don't go through other logic if the game is over
            return;
        }
        //Set the currency text to whatever the current value is
        currencyText.SetText("Currency: " + currency.ToString());
        //Set the current health
        healthText.SetText("Health: " + playerHealth);

        //Zoom out when a tower is being placed
        if(towerBeingPlaced){
            Camera.main.orthographicSize = 10.0f;
        }
        else{
            Camera.main.orthographicSize = 6.5f;
        }

        if(towerIsSelected){
            towerDescriptionPanel.SetActive(true);
            selectedTower.SetDescription(selectedName, selectedDescription);
        }
    }

    //Sets the prefab value to the corresponding tower that is trying to be bought
    public void setPreFabVal(int fabVal){
        prefabVal = fabVal;
        //Set the description text for this tower
        TowerController tower = towers[prefabVal].GetComponent<TowerController>();
        buyingNameText.SetText(Enum.GetName(typeof(DataManager.TowerNames), tower.towerDescriptor));
        buyingDescriptionText.SetText(tower.towerDescription);
    }

    // TowerCostDictionary makes a dictionary that can be referenced when buying a tower
    void TowerCostDictionary()
    {
        for(int i = 0; i < towers.Length; i++)
        {
            TowerController currTower = towers[i].GetComponent<TowerController>();
            towerCosts[currTower.towerName] = currTower.cost;
        }
    }

    void gameOver(){
        Debug.Log("gameOver Called");
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
        towerSelectButton.SetActive(false);
        waveInfoButton.SetActive(false);
        audioSource.PlayOneShot(loseGameSound, 1f);
    }

    public void returnToMainMenu(){
        SceneManager.LoadScene("MainMenu");
    }


    // BuyTower is called when a user tries to buy a tower
    bool BuyTower(string name)
    {
        int cost = towerCosts[name];
        if(cost <= currency)
        {
            return true;
        }else{
            return false;
        }
    }

    //Creates a tower after it has been bought
    public void createTower(){
        TowerController currTower = towers[prefabVal].GetComponent<TowerController>();
        if(prefabVal < 0){
            Debug.LogError("Invalid prefab value index");
        }
        else{
            if(BuyTower(currTower.towerName)){
                 GameObject newTower = Instantiate(towers[prefabVal], spawnPoint.position, spawnPoint.rotation);
                 Debug.Log(currency);
            }
            else{
                Debug.Log("Not enough currency");
                towerSelectButton.SetActive(true);
            }
        }
    }

    //sellTower is called when a user tries to sell a selected tower.
    public void sellTower()
    {   
        //Half of the cost of the selected tower is returned. 
        currency += (int)(selectedTower.cost * 0.5f);
        towerSelectButton.SetActive(true);
        Destroy(selectedTower.gameObject);
        deselectTower();
    }

    public void upgradeTower(){
        selectedTower.Upgrade();
    }

    public void deselectTower(){
        towerIsSelected = false;
        towerDescriptionPanel.SetActive(false);
    }

    public void StartWave(){
        Time.timeScale = 1;
        isWaveStarted = true;
        isPhaseStarted = true;
    }

    public void StartPhase(){
        isPhaseStarted = true;
    }

    void OnDrawGizmos()
    {
        //Make the camera bounds always visible
        float verticalHeightSeen = Camera.main.orthographicSize * 2.0f;
        float verticalWidthSeen = verticalHeightSeen * Camera.main.aspect;
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, new Vector3(verticalWidthSeen, verticalHeightSeen, 0));
    }

}
