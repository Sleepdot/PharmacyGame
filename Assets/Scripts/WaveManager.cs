using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveManager : MonoBehaviour
{
    public GameObject[] Phases; //All phases should be instantiated but set to inactive at first
    public GameObject startWaveButton;
    public bool isCoroutineCalled;

    public int phaseIndex;
    public GameObject[] enemies;
    public TextMeshProUGUI descriptionText; //The text for describing the enemies in the wave
    public TextMeshProUGUI waveNumText;
    public GameObject patientInfoPanel;

    void Start(){
        phaseIndex = 0;
        isCoroutineCalled = false;
        patientInfoPanel.SetActive(true);
        Phase currentPhase = Phases[phaseIndex].GetComponent<Phase>();
        currentPhase.SetDescription();
        currentPhase.SetPatientDescrition();
    }
    void Update(){
        //Start the phase if it hasn't already been started
        if(GameManager.isPhaseStarted && !isCoroutineCalled && phaseIndex < Phases.Length){
            StartCoroutine(StartCurrentPhase());
            isCoroutineCalled = true;
        }

        if(GameManager.isWaveStarted){
            startWaveButton.SetActive(false);
        }
        else{
            startWaveButton.SetActive(true);
        }
    }
    
    IEnumerator StartCurrentPhase(){
        //Set the current wave to active
        Phases[phaseIndex].SetActive(true);
        //Wait until the phase is complete
        while(GameManager.isPhaseStarted){
            yield return null;
        }
        phaseIndex++;
        Debug.Log("Phase complete!");
        if(phaseIndex >= Phases.Length){
            Debug.Log("Level complete!");
        }
        else{
            Phases[phaseIndex].SetActive(true);
            Phase currentPhase = Phases[phaseIndex].GetComponent<Phase>();
            currentPhase.SetDescription();
            currentPhase.SetPatientDescrition();
            patientInfoPanel.SetActive(true);
        }
        isCoroutineCalled = false;
    }

}
