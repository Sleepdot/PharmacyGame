using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelButton : MonoBehaviour
{

    public GameObject currentTower;
    public GameObject placeButton;
    public void Cancel(){
        Destroy(currentTower);
        GameManager.towerBeingPlaced = false;
        placeButton.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
