using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerCollisionManager : MonoBehaviour {
    public float eligibleDistanceForCollider = 0.45f;
    public Transform platform;
	public Collider2D platfromCollider;
	private Collider2D myCollider;

	private Rigidbody2D playerR;
    public SquareCornerTrigger[] top, bottom, left, right;
	public float outOfSightY = 5.8f;

	// Use this for initialization
	private void Start()
	{
		platform = GameManager.instance.platforms[2];
		playerR = GetComponent<Rigidbody2D>();

		GameManager.instance.currentPlatform = platform.gameObject;
		platfromCollider = platform.GetComponent<Collider2D>();
		myCollider = GetComponent<Collider2D>();

	}

	private void Update()
    {
		if (CheckSquareColliderTrigger())
		{
			//Debug.Log(""+platformSpawnStarted + GameManager.platformActive);
			if (!platformSpawnStarted && GameManager.platformActive)
			{
				var playerStable = playerR.velocity.magnitude <= 0.1f;
                if (playerStable)
                {
					//Debug.Log("player successfully landed");
					platformSpawnStarted = true;

					if (GameManager.tutorial) {
						FindObjectOfType<GameManager>().TutorialFinished();
					} 

					StartCoroutine(PlacedOnPlatform());
                }
			}
		}
		else {
			//Debug.Log(platformSpawnStarted);
			platformSpawnStarted = false;
		} 

        if (platform == null) return;
		//Debug.Log(CheckEligibleForCollider());
        if (CheckEligibleForCollider())
        {
            //platfromCollider.isTrigger = false;
            Physics2D.IgnoreCollision(platfromCollider, myCollider, false);
        }
        else
        {
            //platfromCollider.isTrigger = true;
            Physics2D.IgnoreCollision(platfromCollider, myCollider, true);
        }
    }

	public void GenerateTutorialPosition() {
		//Calculate random pos
        var up = false;
        var rand = Random.Range(0, 1);
        if (rand == 1) up = true;

        var yPos = 0f;
        if (up) yPos = Random.Range(1, 3.5f);
        else yPos = Random.Range(-1, -3.5f);
		lastYPos = yPos;

		var xPos = Random.Range(GameManager.instance.minXPos, GameManager.instance.maxXPos);
		lastXPos = xPos;

		platform.position = new Vector3(xPos, yPos, 0);

		platform.gameObject.SetActive(true);
	}


	private float lastXPos = 1;
	private float lastYPos = 1;

	private float xPos, yPos;
	private int randPlatform;
	private bool valuesGenerated;

	private IEnumerator PlacedOnPlatform()
    {
		StartCoroutine(GenerateXYPos());
        while (!valuesGenerated) yield return null;
        valuesGenerated = false;

        GameManager.instance.Success(xPos, yPos, randPlatform);

        yield return null;
        //Score update locally
        ScoreUpdate(); 
    }

	private IEnumerator GenerateXYPos() {

        var minXPos = GameManager.instance.minXPos;
        var maxXPos = GameManager.instance.maxXPos;
        var minYPos = GameManager.instance.minYPos;
        var maxYPos = GameManager.instance.maxYPos;

        var minXDistance = GameManager.instance.minXDistance;
        var minYDistance = GameManager.instance.minYDistance;

        while (true)
        {
            yield return null;

            xPos = Random.Range(minXPos, maxXPos);
            var dist = Mathf.Abs(xPos - lastXPos);
            Debug.Log(dist);
            if (dist >= minXDistance)
            {
				Debug.Log("XDist - " + dist + ", XPos - "+xPos);
                lastXPos = xPos;
                break;
            }
        }

        while (true)
        {
            yield return null;
            yPos = Random.Range(minYPos, maxYPos);
            var dist = Mathf.Abs(yPos - lastYPos);
            Debug.Log(dist);
            if (dist >= minYDistance)
            {
				Debug.Log("YDist - " + dist + ", YPos - " + yPos);
                lastYPos = yPos;
                break;
            }
        }

        randPlatform = Random.Range(0, GameManager.instance.platforms.Length);

		valuesGenerated = true;
		//Debug.Log("ValuesGenereated");
	}


	private int score = 0;
   
    public void ScoreUpdate()
    {
		//5 points
        score += 5;
		//Debug.Log("score" + score);
        PlayerPrefs.SetInt("New", score);
        //int best = 0;
        if (score > PlayerPrefs.GetInt("Best"))
            PlayerPrefs.SetInt("Best", score);

		StartCoroutine(UpdateStarsAnimation(5, score-5));
    }

    private IEnumerator UpdateStarsAnimation(int amt, int current) {

        var waitAmt = 0.2f/amt;
        var incr = 0;
		var curVal = 0;
        if(amt > 25) incr = 3;
        else incr = 1;
        while(true) {
            if(curVal + incr > amt) incr = amt - curVal;
            curVal += incr;
            //incr is stars
			//Debug.Log("curval" + curVal);
			foreach (var t in GameManager.instance.newScoreTxt)
    			t.text = "" + (curVal + current);

            if(curVal == amt) break;
            yield return new WaitForSeconds(waitAmt);
        }
    }

    private bool platformSpawnStarted;

    private bool CheckSquareColliderTrigger()
    {
        var clear = true;
		foreach (var sCT in top) if (!sCT.amTouched) clear = false;
      	if(clear) return clear;

        clear = true;
		foreach (var sCT in bottom) if (!sCT.amTouched) clear = false;
		if (clear) return clear;
      
        clear = true;
		foreach (var sCT in left) if (!sCT.amTouched) clear = false;
		if (clear) return clear;
      
        clear = true;
		foreach (var sCT in right) if (!sCT.amTouched) clear = false;
		if (clear) return clear;
         
        return clear;
    }

    private bool CheckEligibleForCollider()
    {
		if (platform == null) return false;
        var eligible = false;
        var yDif = transform.position.y - platform.position.y;
		if (yDif >= eligibleDistanceForCollider)
            eligible = true;
        return eligible;
    }

	public bool portalEnabled;

	private void OnTriggerEnter2D(Collider2D collision)
	{
      
		if (collision.CompareTag("Border"))
		{
			//Debug.Log("Trigger-" + collision.tag);
			portalEnabled = true;
		}
	}


	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.CompareTag("Border"))
        {
			//Cancel user input
			GetComponent<PlayerPhysicsManager>().inputAllowed = false;
			playerR.gravityScale = GetComponent<PlayerPhysicsManager>().gravityOfPlayer;
			PlayerPhysicsManager.left = ScreenTouchManager.left;
			PlayerPhysicsManager.right = ScreenTouchManager.right;
			//ScreenTouchManager.left = false;
			//ScreenTouchManager.right = false;
        }      
	}

	private void OnCollisionExit2D(Collision2D collision)
    {
		
        if (collision.collider.CompareTag("Border"))
        {
			
        }
    }

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Border"))
        {
			myCollider.isTrigger = false;
			GetComponent<PlayerPhysicsManager>().playerStarted = true;
        }
		//Debug.Log(collision.tag);

	}

}
