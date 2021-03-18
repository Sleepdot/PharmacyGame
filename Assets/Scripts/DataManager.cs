using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    //Enumerations
    public enum Effects
    {
        none,
        decreasedPatientHealth,
        increasedEnemyStrength,
        decreasedTowerStrength,
        increasedEnemySpeed,
        decreasedTowerAttackSpeed
    }
    //A running list of all the diseases in the game
    public enum DiseaseDescriptor //disease names
    {
        Urticaria,
        PoisonIvy,
        FireAnts
    }
    //A running list off all the enemies in the game
    public enum EnemyNames{
        Hives,
        Strep,
        Urticaria,
        Itch,
        Rash,
        PoisonIvy,
        Pain,
        Anaphylaxis
    }
    //A running list of all the towers
    public enum TowerNames{
        Diphenhydramine, //Antihistamine, good for strep, hives, and Urticaria
        Hydrocortisone, //Good against hives
        Acetaminophen, //General purpose painkiller
        Loratadine, //Weak antihistamine
        SoapAndWater, //Good for cleaning wounds and stopping spread of things such as poison ivy
        Prednisone, //Steroid for moderate to severe use cases
        Centirizine, //Weak anti-histamine
        IcePack, //Good for swelling and itching
        EpiPen //To treat anaphylasix reaction
    }
    //A running list of all the levels
    public enum Levels{
        skinLevel
    }

    //Struct to hold info about an enemy in a specific wave
    public struct WaveEnemy{
        public WaveEnemy(GameObject enemy, float delay, int num){
            this.enemy = enemy;
            this.delay = delay;
            numToSpawn = num;
        }
        public GameObject enemy;
        public float delay;
        public int numToSpawn;
    }
    
    //A struct for information about a specific wave
    public struct Wave{
        public WaveEnemy[] enemies;
    }

    //Structures
    public struct Disease //a disease has a name, symptoms, cause, and a list of risks
    {
        DiseaseDescriptor name;
        List<Symptom> symptoms;
        Cause cause;
        List<PatientInfo.RiskFactor> atRisk; //list of risks that puts the patient at risk of the disease
    }
    public struct Symptom
    {
        string description; //Description that is displayed to the player
    }
    public struct Cause //each cause has a list of treatment
    {
        string description; //Description that is displayed to the player

        List<Treatment> treatments;
    }
    struct Treatment
    {
        string description;
    }
    
    //Methods
    //*******GeneratePatient*******
    //Creates a patient psuedo randomly based on the available disease
    //information
    public static PatientInfo GeneratePatient(){
        PatientInfo patient = new PatientInfo();

        return patient;
    }


    //Data -> these are technically global variables
    Dictionary<DiseaseDescriptor, List<Cause> > diseaseCauses;
    Dictionary<DiseaseDescriptor, List<Symptom> > diseaseSymptoms;
    Dictionary<DiseaseDescriptor, List<PatientInfo.RiskFactor> > diseaseRiskFactors;
    
    //Contains information about which towers are better against which enemies
    public static Dictionary<TowerNames, EnemyNames[]> TowerEffectiveness;

    void Awake(){
        //Initialize the tower effectiveness dictionary
        TowerEffectiveness = new Dictionary<TowerNames, EnemyNames[]>{
            {TowerNames.Diphenhydramine, new EnemyNames[]{EnemyNames.Hives, EnemyNames.Rash, EnemyNames.Itch, EnemyNames.Strep, EnemyNames.Urticaria}},
            {TowerNames.Hydrocortisone, new EnemyNames[]{EnemyNames.Hives, EnemyNames.Itch}},
            {TowerNames.Acetaminophen, new EnemyNames[]{EnemyNames.Pain}},
            {TowerNames.Loratadine, new EnemyNames[]{EnemyNames.PoisonIvy, EnemyNames.Rash}},
            {TowerNames.SoapAndWater, new EnemyNames[]{EnemyNames.Itch, EnemyNames.Rash, EnemyNames.PoisonIvy}},
            {TowerNames.Prednisone, new EnemyNames[]{EnemyNames.PoisonIvy}},
            {TowerNames.Centirizine, new EnemyNames[]{EnemyNames.Itch}},
            {TowerNames.IcePack, new EnemyNames[]{EnemyNames.Pain, EnemyNames.Itch}},
            {TowerNames.EpiPen, new EnemyNames[]{EnemyNames.Anaphylaxis}}
        };
    }

}
