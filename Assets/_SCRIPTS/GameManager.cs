using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

	public float minYPos, maxYPos;
	public float minXPos, maxXPos;
	public float minXDistance, minYDistance;

	public static GameManager instance;

	public GameObject myPlayer;
	public Transform[] platforms;

	public PlayerCollisionManager pCM;
	public GameObject currentPlatform;

	public static bool tutorial;
	
	public GameObject player;
	public Rigidbody2D currentPlayerR;
	
	public GameObject ttJTxt, ljTxt, asTxt, jpTxt;

	public bool longJumpStarted;

	private int totalStars;
	private Coroutine starSpawnCoroutine;
	
	public Image fadeInGO;
	public GameObject squareExplosion;
	
	public Text[] newScoreTxt, bestScoreTxt;
	
	public static bool platformActive = true;

	private Animation platformAnim;
	public AnimationClip fadeIn, fadeOut;
	public AnimationClip fadeYellow;
	
	public GameObject star;
	public Transform startPosition;
	public List<StarManager> allStars = new List<StarManager>();
	public List<Animation> allStarAnims = new List<Animation>();
	private const float starSpeed = 0.5f;

	private void Awake()
	{
		instance = this;

		tutorial = PlayerPrefs.GetInt("Tutorial") != 1;
	}

	private void Start()
	{
		UpdateScore();      

		myPlayer = Instantiate(player);
		currentPlayerR = myPlayer.GetComponent<Rigidbody2D>();

		pCM = myPlayer.GetComponent<PlayerCollisionManager>();
      
		var scaleAmount = FindObjectOfType<AspectRatioManager>().scaleAmount;
		var scale = scaleAmount * currentPlayerR.transform.localScale.x;
		currentPlayerR.transform.localScale = new Vector3(scale, scale, 1);
      
		pCM.eligibleDistanceForCollider *= scaleAmount;
        pCM.enabled = true;
		myPlayer.GetComponent<PlayerPhysicsManager>().enabled = true;

		foreach(var t in platforms) 
		{
			scale = scaleAmount * t.localScale.x;
			t.transform.localScale = new Vector3(scale, scale, 1);
		}

		if(!tutorial) 
		{
			starSpawnCoroutine = StartCoroutine(StarSpawnSystem());

            StartCoroutine(ShowFirstPlatform());
		}
		else 
		{
			Tutorial();
		}
	}


	private void Tutorial() 
	{
		StartCoroutine(TapToJump());
	}
	
	public void LongJumpStarted() 
	{
		ttJTxt.SetActive(false);
		ljTxt.SetActive(true);
	}

	public void StarStarted() 
	{
		ljTxt.SetActive(false);
		asTxt.SetActive(true);
		starSpawnCoroutine = StartCoroutine(StarSpawnSystem(0f));
		StartCoroutine(PlatformStarted());
	}

	private IEnumerator PlatformStarted() 
	{
		yield return new WaitForSeconds(10f);
		asTxt.SetActive(false);
		jpTxt.SetActive(true);

		pCM.GenerateTutorialPosition();
	}

	private IEnumerator TapToJump() 
	{
		yield return new WaitForSeconds(1f);
		ttJTxt.SetActive(true);
	}

	public void TutorialFinished() 
	{
		Debug.Log("Tutorial finished");
		jpTxt.SetActive(false);
        tutorial = false;
        PlayerPrefs.SetInt("Tutorial", 1);
	}

	private IEnumerator ShowFirstPlatform()
    {
        yield return new WaitForSeconds(1f);
		pCM.platform.gameObject.SetActive(true);
    }

	private IEnumerator StarSpawnSystem(float waitTime = 0.5f) 
	{
		yield return new WaitForSeconds(waitTime);

		StarSpawn();
		totalStars++;

		if (totalStars == 12) {
			StopCoroutine(starSpawnCoroutine);
			yield break;
		} 

		var seconds = 0f;
		switch (totalStars)
		{
			case 1:
			case 2:
				seconds = 5f - waitTime;
				break;
			case >= 3 and <= 5:
				seconds = 10f - waitTime;
				break;
			case <= 8:
				seconds = 20f - waitTime;
				break;
			case <= 11:
				seconds = 40f - waitTime;
				break;
		}
		yield return new WaitForSeconds(seconds);

		starSpawnCoroutine = StartCoroutine(StarSpawnSystem());
	}
	
	public void GameOver() 
	{
		Handheld.Vibrate();
      
		if (starSpawnCoroutine != null) StopCoroutine(starSpawnCoroutine);
		FindObjectOfType<ScreenTouchManager>().StopInput();
        FindObjectOfType<ScreenTouchManager>().enabled = false;
		currentPlayerR.GetComponent<Collider2D>().enabled = false;
		currentPlayerR.isKinematic = true;
		currentPlayerR.velocity = Vector2.zero;
		currentPlayerR.angularVelocity = 0;

		currentPlayerR.GetComponent<Animation>().Play();
        Camera.main.GetComponent<CameraShake>().enabled = true;
		currentPlayerR.gameObject.SetActive(false);
		var gO = Instantiate(squareExplosion);
		gO.transform.position = currentPlayerR.transform.position;
		gO.SetActive(true);
	}

	private IEnumerator SquareDisable() 
	{
		yield return new WaitForSeconds(0.15f);

		currentPlayerR.gameObject.SetActive(false);
	}

	public void GameOverMainScreen() 
	{
		UpdateScore();
		StartCoroutine(FadePreAd());
	}

	private IEnumerator FadePreAd() {
      
		yield return new WaitForSeconds(1f);

		var fixedColor = fadeInGO.color;
        fixedColor.a = 1;
		fadeInGO.color = fixedColor;
		fadeInGO.CrossFadeAlpha(0f, 0f, true);
		fadeInGO.CrossFadeAlpha(1, 1, true);
		yield return new WaitForSeconds(1f);

		if(tutorial) 
		{
			SceneManager.LoadScene(1);
		}
	}

	private void UpdateScore()
    {
		foreach (var t in newScoreTxt) t.text = "0";
		foreach (var t in bestScoreTxt) t.text = "0";

        var newScore = PlayerPrefs.GetInt("New");
        var bestScore = PlayerPrefs.GetInt("Best");

		foreach(var t in newScoreTxt) t.text = ""+newScore;
		foreach (var t in bestScoreTxt) t.text = ""+bestScore;
    }

	public void Success(float xPos, float yPos, int randPlatform) 
	{
		platformActive = false;
		StartCoroutine(FadeColor(xPos, yPos, randPlatform));     
	}

	private IEnumerator FadeColor(float xPos, float yPos, int randPlatform)
    {
		var animationClip = fadeYellow;

		var animation =
			pCM.platform.GetComponent<Animation>();
		animation.clip = animationClip;
		animation.Play();

        yield return new WaitForSeconds(0.5f);

		StartPositionPlatform(xPos, yPos, randPlatform);
    }

	private void StartPositionPlatform(float xPos, float yPos, int randPlatform) 
	{
		
		pCM.platform = platforms[randPlatform];
		pCM.platfromCollider = platforms[randPlatform].GetComponent<Collider2D>();
        foreach (var tr in platforms)
        {
			tr.position = new Vector3(5, tr.position.y, 0);
			var anim = tr.GetComponent<Animation>();
			if (anim.isPlaying) anim.Stop();

        }
      
		var targetSprite = platforms[randPlatform].GetComponent<SpriteRenderer>();
        targetSprite.color = new Color(targetSprite.color.r, targetSprite.color.g, targetSprite.color.b, 1f);
		platforms[randPlatform].position = new Vector3(xPos, yPos, 0);

		platforms[randPlatform].gameObject.SetActive(true);
        //FADE IN
		platforms[randPlatform].GetComponent<Animation>().clip = fadeIn;
		platforms[randPlatform].GetComponent<Animation>().Play();

        platformActive = true; //Now platform can be occupied again
	}

	public void StarSpawn() 
	{
		var gO = Instantiate(star) as GameObject;
		var starManager = gO.GetComponent<StarManager>();
		allStarAnims.Add(starManager.GetComponent<Animation>());
		allStars.Add(starManager);
		gO.transform.position = startPosition.position; //POSITION
		var scale = FindObjectOfType<AspectRatioManager>().scaleAmount * gO.transform.localScale.x;
		gO.transform.localScale = new Vector3(scale, scale, 1);
	
		var x = Random.Range(-1f, 1f); if (x == 0) x = 1f; 
		var y = Random.Range(-1f, 0f); if (y == 0) y = -1f;

		starManager.AssignStar(new Vector2(x, y), starSpeed); //SPEED

		StartCoroutine(AnimateStar(gO.GetComponent<StarManager>()));

	}

	private IEnumerator AnimateStar(StarManager starManager) 
	{
		
		starManager.gameObject.GetComponent<Animation>().Play();
		yield return new WaitForSeconds(0.5f);
		starManager.gameObject.GetComponent<Animation>().Stop();

		starManager.GetComponent<Rigidbody2D>().isKinematic = false;
		starManager.enabled = true;
	}
}
