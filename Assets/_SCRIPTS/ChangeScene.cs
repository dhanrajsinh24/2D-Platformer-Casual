using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

	// Use this for initialization
	private void Start () {
		
	}
	
	// Update is called once per frame
	private void Update () {
		
	}

	public void ToggleScene() {
		if (SceneManager.GetActiveScene().buildIndex == 0)
			SceneManager.LoadScene(SceneManager.GetSceneByBuildIndex(1).buildIndex);
		else
			SceneManager.LoadScene(SceneManager.GetSceneByBuildIndex(0).buildIndex);
	}
}
