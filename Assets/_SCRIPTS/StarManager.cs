using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarManager : MonoBehaviour {

	public Vector2 startSpeed;
	public Vector2 currentSpeed;
	public float speed;
	public Rigidbody2D starR;

	private void Start () {
		starR = GetComponent<Rigidbody2D>();
		var velocity = startSpeed.normalized * speed;
		starR.velocity = velocity;
		toSpeed = startSpeed;
		//Debug.Log(starR.velocity);
	}

	public void AssignStar(Vector2 vel, float mag) {
		startSpeed = vel;
		speed = mag;
	}
	
	// Update is called once per frame
	private void Update () {
		if(!collided) {
			currentSpeed = starR.velocity;
		}

		//if(starR.velocity.magnitude)
	}

	private void FixedUpdate()
	{
		starR.velocity = toSpeed.normalized * speed;
	}

	private bool collided;
	private Vector2 toSpeed;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (!enabled) {
			Debug.Log("Not enabled");
			return;
		} 
		collided = true;
        //Debug.Log(currentSpeed);
        toSpeed = Vector2.Reflect(currentSpeed, collision.contacts[0].normal);
        

		if (collision.collider.CompareTag("Player"))
        {
			Debug.Log("GameOver");
			//starR.isKinematic = true;
            GameManager.instance.GameOver();
            GameManager.instance.GameOverMainScreen();
        }
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		collided = false;  
		//if (collision.collider.CompareTag("Border") || collision.collider.CompareTag("Star"))
        //{
			       
        //}
	}
}
