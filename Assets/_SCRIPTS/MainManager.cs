using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
	public RectTransform topPanel, mainPanel;
	private void Awake()
	{
		Application.targetFrameRate = 60;
	
		#if UNITY_IOS
        if (UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhoneX)
        {
			
            //UnityEngine.iOS.Device.hideHomeButton = true;

            //UI
			topPanel.offsetMax = new Vector2(-20, 0);
            topPanel.offsetMin = new Vector2(20, 0);

            mainPanel.offsetMax = new Vector2(0, -60);
        }
#endif
	}


	private void Start()
	{

		LoadScene();
      
		UpdateScore();

		//PlayerPrefs.DeleteAll();
	}

	public Animation ttpAnim, iconAnim;

	public void PlayNow()
	{
		ttpAnim.Stop();
		ttpAnim["FadeOut"].speed = 10;
		ttpAnim.Play("FadeOut");

		iconAnim.Play();

		StartCoroutine(WaitAndGoToGame());
	}

	private AsyncOperation asyncOperation;

	private void LoadScene()
    {      
        //Begin to load the Scene you specify
        asyncOperation = SceneManager.LoadSceneAsync(1);
        asyncOperation.allowSceneActivation = false;
    }

	private IEnumerator WaitAndGoToGame() {
		yield return new WaitForSeconds(.25f);
		asyncOperation.allowSceneActivation = true;
	}

	public Text newScoreTxt, bestScoreTxt;

	private void UpdateScore() {
		var newScore = PlayerPrefs.GetInt("New");
		var bestScore = PlayerPrefs.GetInt("Best");

		//Debug.Log(newScore + "" + bestScore);

		newScoreTxt.text = "" + newScore;
		bestScoreTxt.text = "" + bestScore;
	}   
}
