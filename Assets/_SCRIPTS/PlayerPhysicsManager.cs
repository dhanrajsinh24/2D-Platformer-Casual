using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.Networking;

public class PlayerPhysicsManager : MonoBehaviour
{

    public Rigidbody2D playerR;

	public Vector2 leftForce, rightForce, zeroForce, straightForce;
    public float speed = 500f;
    public float gravityOfPlayer;

    public void Start()
    {      
		playerR = GetComponent<Rigidbody2D>();
        gravityOfPlayer = playerR.gravityScale;

        playerR.isKinematic = false;
    }

	public bool inputAllowed;

	public static bool left, right;

	private void Update()
	{
		if(!inputAllowed) {
			if(PlayerPhysicsManager.left != ScreenTouchManager.left || PlayerPhysicsManager.right != ScreenTouchManager.right) {
				//Debug.Log("Input allowed again");
				inputAllowed = true;
			}
		}
	}

	public bool playerStarted;
	//bool startPreviousLogic;
	private void FixedUpdate()
    {
		if (!playerStarted) return;
		if (!inputAllowed) return;
        if (playerR == null) return;
      
		if(ScreenTouchManager.left || ScreenTouchManager.right) {
			if (ScreenTouchManager.left)
            {
                playerR.velocity = Vector2.zero;
                playerR.gravityScale = 0;
                playerR.AddForce(speed * leftForce, ForceMode2D.Force);
                playerR.angularVelocity = 0f;
                playerR.AddTorque(speed * 0.0004f, ForceMode2D.Impulse);

            }
			else if (ScreenTouchManager.right)
            {
                playerR.velocity = Vector2.zero;
                playerR.gravityScale = 0;
                //Debug.Log("Right");
                playerR.AddForce(speed * rightForce, ForceMode2D.Force);
                playerR.angularVelocity = 0f;
                playerR.AddTorque(speed * (-0.0004f), ForceMode2D.Impulse);
            }
		}
		else {
			if(playerR.gravityScale != gravityOfPlayer)
			playerR.gravityScale = gravityOfPlayer;
		}

    }

	private bool playerRepositioned;
    public float outOfSightY;
}
