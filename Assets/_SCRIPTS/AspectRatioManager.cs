using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectRatioManager : MonoBehaviour {

	// Use this for initialization

	private readonly float iPhone8AS = 0.56f; //iPhone 6 aspect ratio width/height as a reference
	public RectTransform[] topPanel, mainPanel;

	public float scaleAmount;

	private void Awake()
	{
		//Debug.Log("Width:" + Screen.width + ", Height:" + Screen.height);
		var aspectRatio = (float)Screen.width / (float)Screen.height;
		aspectRatio = Mathf.Round(aspectRatio * 100f) / 100f;
		//Debug.Log("AR:" + aspectRatio);

		scaleAmount = aspectRatio / iPhone8AS;

#if UNITY_IOS
		if (UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhoneX)
		{

			//UnityEngine.iOS.Device.hideHomeButton = true;

			//UI
			foreach(RectTransform r in topPanel) {
				r.offsetMax = new Vector2(-20, 0);
				r.offsetMin = new Vector2(20, 0);
			}
		foreach (RectTransform r in mainPanel)
            {
				r.offsetMax = new Vector2(0, -60);
            }


		}
#endif
	}
}
