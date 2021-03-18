using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceButton : MonoBehaviour
{
    public TowerController currentTower;
    public GameObject cancelButton;
    public void PlaceTower(){
        //Make sure you can only place a tower in a valid spot
        if(!currentTower.isOverlapping){
            GameManager.currency = GameManager.currency - currentTower.cost;
            GameManager.towerBeingPlaced = false;
            currentTower.isPlaced = true;
            cancelButton.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
