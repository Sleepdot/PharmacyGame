using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Phase1 : Phase
{
    public GameObject hivesEnemy;
    public GameObject strepEnemy;
    public GameObject urticariaEnemy;

    //Initialize populates the data for the enemies and waves
    public override void Initialize()
    {
        base.Initialize();

        //Enemies for different waves
        //Hives
        DataManager.WaveEnemy w1Hives = new DataManager.WaveEnemy(hivesEnemy, 1f, 5);
        DataManager.WaveEnemy w2Hives = new DataManager.WaveEnemy(hivesEnemy, 1f, 10);
        DataManager.WaveEnemy w3Hives = new DataManager.WaveEnemy(hivesEnemy, 1f, 15);
        //Strep
        DataManager.WaveEnemy w2Strep = new DataManager.WaveEnemy(strepEnemy, 2f, 5);
        DataManager.WaveEnemy w3Strep = new DataManager.WaveEnemy(strepEnemy, 2f, 10);
        //Urticaria
        DataManager.WaveEnemy w3Urticaria = new DataManager.WaveEnemy(urticariaEnemy, 3f, 5);

        //Wave 1
        DataManager.Wave w1 = new DataManager.Wave();
        DataManager.WaveEnemy[] w1Enemies = {w1Hives};
        w1.enemies = w1Enemies;
        
        //Wave 2
        DataManager.Wave w2 = new DataManager.Wave();
        DataManager.WaveEnemy[] w2Enemies = {w2Hives, w2Strep};
        w2.enemies = w2Enemies;

        //Wave 3
        DataManager.Wave w3 = new DataManager.Wave();
        DataManager.WaveEnemy[] w3Enemies = {w3Hives, w3Strep, w3Urticaria};
        w3.enemies = w3Enemies;

        DataManager.Wave[] newWaves = {w1, w2, w3};
        waves = newWaves;

        waveDescriptions = new string[]{
            "Hives: x5", //first wave
            "Hives: x10\nStreptococcus: x5", //Second wave
            "Hives: x15\nStreptococcus: x10\nUrticaria: x5" //Third Wave
        };
        
    }

    void Awake(){
        Initialize();
    }

    protected override void Start(){
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
}
