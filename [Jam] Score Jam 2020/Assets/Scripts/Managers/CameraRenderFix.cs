using UnityEngine;

public class CameraRenderFix : MonoBehaviour
{
	public Canvas canvas;

	private void Awake ()
	{
		UpdateCameraCanvas();
	}

	public void UpdateCameraCanvas ()
	{
		canvas.worldCamera = Camera.main;
	}
}