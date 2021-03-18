using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    void Start(){
        Time.timeScale = 1;
    }
    public void playGame(){
        SceneManager.LoadScene("Skin Level");
    }
	
	public void loadMenu(){
		SceneManager.LoadScene("MainMenu");
	}
}
