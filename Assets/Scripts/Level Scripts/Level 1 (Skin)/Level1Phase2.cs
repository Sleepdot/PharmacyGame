using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Phase2 : Phase
{
    public GameObject itchEnemy;
    public GameObject rashEnemy;
    public GameObject poisonIvyEnemy;

    //Initialize populates the data for the enemies and waves
    public override void Initialize()
    {
        base.Initialize();

        //Enemies for different waves
        //itching
        DataManager.WaveEnemy w1Itching = new DataManager.WaveEnemy(itchEnemy, 1f, 5);
        DataManager.WaveEnemy w2Itching = new DataManager.WaveEnemy(itchEnemy, 1f, 10);
        DataManager.WaveEnemy w3Itching = new DataManager.WaveEnemy(itchEnemy, 1f, 15);
        //rash
        DataManager.WaveEnemy w1Rash = new DataManager.WaveEnemy(rashEnemy, 1f, 5);
        DataManager.WaveEnemy w2Rash = new DataManager.WaveEnemy(rashEnemy, 1f, 10);
        //poison ivy
        DataManager.WaveEnemy w1PoisonIvy = new DataManager.WaveEnemy(poisonIvyEnemy, 1f, 5);

        //wave1
        DataManager.Wave w1 = new DataManager.Wave();
        DataManager.WaveEnemy[] w1Enemies = {w1Itching};
        w1.enemies = w1Enemies;

        //wave2
        DataManager.Wave w2 = new DataManager.Wave();
        DataManager.WaveEnemy[] w2Enemies = {w1Itching, w1Rash};
        w2.enemies = w2Enemies;

        //wave3
        DataManager.Wave w3 = new DataManager.Wave();
        DataManager.WaveEnemy[] w3Enemies = {w1Itching, w1Rash, w1PoisonIvy};
        w3.enemies = w3Enemies;

        DataManager.Wave[] newWaves = {w1, w2, w3};
        waves = newWaves;

        waveDescriptions = new string[]{
            "Itching: x5", //first wave
            "Itching: x10\nRash: x5", //Second wave
            "Itching: x15\nRash: x10\nPoison Ivy: x5" //Third Wave
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
