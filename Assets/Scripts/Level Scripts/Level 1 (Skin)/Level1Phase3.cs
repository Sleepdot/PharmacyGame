using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Phase3 : Phase
{
    public GameObject itchingEnemy;
    public GameObject painEnemy;
    public GameObject anaphylaxisEnemy;

    //Initialize populates the data for the enemies and waves
    public override void Initialize()
    {
        base.Initialize();

        //Enemies for different waves
        //itching
        DataManager.WaveEnemy w1Itching = new DataManager.WaveEnemy(itchingEnemy, 1f, 5);
        DataManager.WaveEnemy w2Itching = new DataManager.WaveEnemy(itchingEnemy, 1f, 10);
        DataManager.WaveEnemy w3Itching = new DataManager.WaveEnemy(itchingEnemy, 1f, 15);
        //pain
        DataManager.WaveEnemy w1Pain = new DataManager.WaveEnemy(painEnemy, 2f, 5);
        DataManager.WaveEnemy w2Pain = new DataManager.WaveEnemy(painEnemy, 2f, 10);
        //anaphylaxis
        DataManager.WaveEnemy w1Anaphylaxis = new DataManager.WaveEnemy(anaphylaxisEnemy, 3f, 5);
        
        //wave1
        DataManager.Wave w1 = new DataManager.Wave();
        DataManager.WaveEnemy[] w1Enemies = {w1Itching};
        w1.enemies = w1Enemies;

        //wave2
        DataManager.Wave w2 = new DataManager.Wave();
        DataManager.WaveEnemy[] w2Enemies = {w1Itching, w1Pain};
        w2.enemies = w2Enemies;

        //wave3
        DataManager.Wave w3 = new DataManager.Wave();
        DataManager.WaveEnemy[] w3Enemies = {w1Itching, w1Pain, w1Anaphylaxis};
        w3.enemies = w3Enemies;

        DataManager.Wave[] newWaves = {w1, w2, w3};
        waves = newWaves;

        waveDescriptions = new string[]{
            "Itching: x5", //first wave
            "Itching: x10\nPain: x5", //Second wave
            "Itching: x15\nPain: x10\nAnaphylaxis: x5" //Third Wave
        };
    }

    void Awake(){
        Initialize();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
