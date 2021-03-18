using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientInfo : MonoBehaviour
{
    //Enumerations for patient variables
    public enum Gender
    {
        male,
        female
    }

    public struct RiskFactor
    {
        string description;
        DataManager.Effects effect; //the effect a given risk has
    }

    public struct Symptom
    {
        string description;

    }


    //Data containers for a specific patient
    public int age;
    public Gender gender;
    public RiskFactor[] risks; //The risk factors an instance of a patient
    public DataManager.Disease disease;

}
