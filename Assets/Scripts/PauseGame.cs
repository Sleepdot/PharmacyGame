using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public static bool paused;
	public GameObject pausePanel;
	
	void Start(){
		paused = false;
	}
	public void Resume(){
		pausePanel.SetActive(false);
		Time.timeScale = 1;
		paused = false;
	}
	
	public void Pause(){
		pausePanel.SetActive(true);
		Time.timeScale = 0;
		paused = true;
		
	}
}
