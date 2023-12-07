using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour {
	private float startTimer;
	private int totalTime;
	public bool cDTimerRunning;
	public int currentTime;

	public static CountDownTimer instance;

	private void Awake()
    {
        // Limit the number of instances to one
        if (instance == null)
        {
            instance = this;           
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // duplicate
            Destroy(gameObject);
        }
    }   
	private void OnEnable()
	{
		if(PlayerPrefs.GetInt("AllowAd", 1) == 1) {
			//Debug.Log("AllowAd");
		}
		else {
			//Debug.Log("not AllowAd");
			StartTheTimer();	
		}      
	}

	public void StartTheTimer() {
		var passedTime = PlayerPrefs.GetInt("Timer");
        //Debug.Log("Passed time - " + passedTime);
		totalTime = 120;
        totalTime -= passedTime;

		startTimer = Time.realtimeSinceStartup;
		cDTimerRunning = true;
	}


	public void StopTheTimer() {
		cDTimerRunning = false;
	}

	// Update is called once per frame
	private void Update () {
		if(cDTimerRunning) {
			var time = Mathf.FloorToInt(Time.realtimeSinceStartup - startTimer);
			if (currentTime != time) {
            
				currentTime = time;

				var seconds = currentTime % 60;
                var minutes = currentTime / 60;
                var secondS = (seconds < 10) ? "0" + seconds : seconds.ToString();

                var finalTime = "" + minutes + ":" + secondS;
                
				//Debug.Log(currentTime);
            
				if (currentTime >= totalTime) 
				{
                    
					//Time reached
					cDTimerRunning = false;
				} 
			} 
		}
	}

	private void OnApplicationQuit()
	{
		//Debug.Log("OnApplicationQuit");
		PlayerPrefs.SetInt("Timer", currentTime);
	}

	private void OnApplicationPause(bool pause)
	{
		//Debug.Log("OnApplicationPause");
		if(pause) {
			PlayerPrefs.SetInt("Timer", currentTime);
		}
	}

	private void OnDestroy()
	{
		//Debug.Log("OnDestroy");
		PlayerPrefs.SetInt("Timer", currentTime);
	}
}
