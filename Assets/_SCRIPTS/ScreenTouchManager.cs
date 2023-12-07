using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ScreenTouchManager : MonoBehaviour, IPointerUpHandler, IPointerDownHandler {

	public static bool left, right;
	private Vector2 touchPos;
	private float screenWidth;

	public static ScreenTouchManager instance;

	private GameManager gameManager;

	private void Awake() {
		instance = this;
		screenWidth = Screen.width;
		gameManager = FindObjectOfType<GameManager>();
	}

	private void Update() {
	      
		if (GameManager.tutorial && longJumpStarted)
        {
			if (longJumpFinished || !screenTouched) return;

			var time = Time.time - startTime;

			Debug.Log(time);
			if(time > 1f) {
				Debug.Log("Long jump finished");
				longJumpFinished = true;
				gameManager.StarStarted();
			}
        }

	}
    
	public virtual void OnPointerUp(PointerEventData pData) {

		StopInput();
     
	}

	public void StopInput() {
		screenTouched = false;
        left = false;
        right = false;
	}

    //TUTORIAL
    private int mTapCount;
    private float startTime;
    private bool screenTouched;
    private bool longJumpFinished;
    private bool longJumpStarted;
   
	public virtual void OnPointerDown(PointerEventData pData) {

		if(GameManager.tutorial) {
			if(!longJumpStarted) {
				mTapCount++;
				Debug.Log("mTapCount"+mTapCount);
                if (mTapCount == 6)
                {
                    Debug.Log("Tap to jump finished");
                    longJumpStarted = true;
					gameManager.LongJumpStarted();
                }
			}
         
			if(longJumpStarted && !longJumpFinished) {
				startTime = Time.time;
                screenTouched = true;
			}         
		}      

		touchPos = pData.position;

		if(touchPos.x < screenWidth/2) {
			//Debug.Log("Left");
			left = true;
		} else {
			//Debug.Log("Right");
			right = true;
		}
	}
}
