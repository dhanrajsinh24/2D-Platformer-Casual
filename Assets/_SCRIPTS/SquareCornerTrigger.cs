using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareCornerTrigger : MonoBehaviour {

    public enum MyType {
        Top,
        Bottom,
        Left,
        Right
    }
    public MyType myType;
    public bool amTouched;

    private void Start () {
		
	}
	
	// Update is called once per frame
	private void Update () {
		
	}

	private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            //Debug.Log("type - " + myType);
            amTouched = true;
        }
    }

	private void OnTriggerExit2D(Collider2D collision)
    {
		if (collision.gameObject.CompareTag("Platform"))
        {
			//Debug.Log("type - " + myType);
            amTouched = false;
        }

    }
}
