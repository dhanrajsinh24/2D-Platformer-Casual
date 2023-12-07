using UnityEngine;
using System.Collections;

public class CameraFitToScreen : MonoBehaviour {

	public bool widthAdapt, heightAdapt;

	private void Start () {
        ResizeSpriteToScreen();
	}

	private void ResizeSpriteToScreen()
    {
		var sR = GetComponent<SpriteRenderer>();
        if (sR == null) return;      
        var width = sR.sprite.bounds.size.x;
        var height = sR.sprite.bounds.size.y;      
        var worldScreenHeight = Camera.main.orthographicSize * 2f;
        var worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;      
		float x1,y1,z1;

		if(heightAdapt)
			x1 = worldScreenWidth / width;
		else
			x1 = transform.localScale.x;
		if(widthAdapt)
			y1 = worldScreenHeight / height;
		else
			y1 = transform.localScale.y;
		z1 = transform.localScale.z;

		transform.localScale = new Vector3(x1, y1, z1);

    }
}
