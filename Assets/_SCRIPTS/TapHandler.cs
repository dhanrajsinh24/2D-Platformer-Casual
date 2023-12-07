/*===============================================================================
Copyright (c) 2015-2018 PTC Inc. All Rights Reserved.
 
Copyright (c) 2015 Qualcomm Connected Experiences, Inc. All Rights Reserved.
 
Vuforia is a trademark of PTC Inc., registered in the United States and other 
countries.
===============================================================================*/
using UnityEngine;

public class TapHandler : MonoBehaviour
{
    private const float DOUBLE_TAP_MAX_DELAY = 0.5f; //seconds
    private float mTimeSinceLastTap;

    protected int mTapCount;

    private void Start()
    {
        mTapCount = 0;
        mTimeSinceLastTap = 0;
    }


    private void Update()
    {
		//if(allowDoubleJump)	HandleTap();
    }
 
    private void HandleTap()
    {
        if (mTapCount == 1)
        {
            mTimeSinceLastTap += Time.deltaTime;
            if (mTimeSinceLastTap > DOUBLE_TAP_MAX_DELAY)
            {
                // too late for double tap, 
                // we confirm it was a single tap
            
                // reset touch count and timer
                mTapCount = 0;
                mTimeSinceLastTap = 0;
            }
        }
        else if (mTapCount == 2)
        {
            // we got a double tap
            OnDoubleTap();

            // reset touch count and timer
            mTimeSinceLastTap = 0;
            mTapCount = 0;
        }

        if (Input.GetMouseButtonUp(0))
        {
            mTapCount++;
            if (mTapCount == 1)
            {
                OnSingleTap();
            }
        }
    }

    protected virtual void OnSingleTap() { }

	public static bool useDoubleJump;
    protected virtual void OnDoubleTap()
    {
		Debug.Log("DoubleTap");
		useDoubleJump = true;
    }

}
