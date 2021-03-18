using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Phase : MonoBehaviour
{
    Transform spawnPoint;
    GameObject[] waypoints;
    
    public DataManager.Wave[] waves;
    public string[] waveDescriptions;
    [Multiline]
    public string patientDescrition;
    public DataManager.Levels level;
    public PatientInfo patient;
    public int currentWave;
    public bool isWaveComplete;
    public bool allEnemiesGenerated;
    public TextMeshProUGUI waveNumText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI patientDescriptionText;

    //Initialize populates the data for the enemies and waves
    public virtual void Initialize(){
        //This is meant to be overwritten
        //All the waves should be initialized here
        //All wave descriptions should also be specified
    }

    //Start is called once before the first frame
    protected virtual void Start(){
        currentWave = 0;
        waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        spawnPoint = waypoints[0].transform;
        isWaveComplete = true; //Get ready to start the first wave
    }

    protected virtual void Update(){
        //Get all of the enemies currently in the game
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        //Set the description of the current wave
        SetDescription();
        //If all enemies have been generated and eleminated, then the wave is complete
        if(enemies.Length == 0 && allEnemiesGenerated && !isWaveComplete){
            isWaveComplete = true;
            GameManager.isWaveStarted = false;
            Debug.Log("End of wave");
            currentWave++;
        }
        //If the previous wave is finished and the next wave button has been pressed, start the next wave
        if(GameManager.isWaveStarted && isWaveComplete && currentWave < waves.Length){
            StartWave();
            isWaveComplete = false;
            Debug.Log("Start new wave");
        }
        //Finish the phase
        if(isWaveComplete && currentWave >= waves.Length){
            Debug.Log("Phase is complete");
            GameManager.isPhaseStarted = false;
            gameObject.SetActive(false);
        }
    }

    void StartWave(){
        //Loop through all the enemies in the current wave and start generating them
        foreach(DataManager.WaveEnemy e in waves[currentWave].enemies){
            StartCoroutine(GenerateEnemy(e.enemy, e.delay, e.numToSpawn));
        }
    }

    //GenerateEnemy will generate a certain number of enemies with a specified delay between each
    IEnumerator GenerateEnemy(GameObject enemy, float delay, int numToGenerate){
        
        for(int i = 0; i < numToGenerate; i++){
            GameObject newEnemy = Instantiate(enemy, spawnPoint.position, Quaternion.identity);
            //Get the enemy controller off of the new enemy
            EnemyController eControl = newEnemy.GetComponent<EnemyController>();
            //Populate the waypoints on the new enemy
            eControl.waypoints = new Transform[waypoints.Length];
            for(int j = 0; j < waypoints.Length; j++){
                eControl.waypoints[j] = waypoints[j].transform;
            }
            yield return new WaitForSeconds(delay);
        }
        allEnemiesGenerated = true;
    }

    //SetDescription will set the description for the current wave
    public void SetDescription(){
        waveNumText.SetText("Wave " + (currentWave + 1));
        descriptionText.SetText(waveDescriptions[currentWave]);
    }
    public void SetPatientDescrition(){
        patientDescriptionText.SetText(patientDescrition);
    }
}
