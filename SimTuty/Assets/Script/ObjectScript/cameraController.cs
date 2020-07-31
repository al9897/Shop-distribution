using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
	// Start is called before the first frame update
	private float speed = 20f;
	private float borderThickness = 10f;
	private float scrollSpeed = 30.0f;
	//private float minZ = -85f;
	//private float maxZ = 15f;
	private float minY = -65f;
	private float maxY = 16f;
	private float minX = -50f;
	private float maxX = 50f;
	private Camera mainCamera;
	// Update is called once per frame
	void Start()
	{
		mainCamera = Camera.main;
	}
	void Update()
	{
		Vector3 pos = transform.position;
		if (Input.mousePosition.y >= Screen.height - borderThickness)
		{
			pos.y += speed * Time.deltaTime;
		}
		if (Input.mousePosition.y <= borderThickness)
		{
			pos.y -= speed * Time.deltaTime;
		}
		if (Input.mousePosition.x >= Screen.width - borderThickness)
		{
			pos.x += speed * Time.deltaTime;
		}
		if (Input.mousePosition.x <= borderThickness)
		{
			pos.x -= speed * Time.deltaTime;
		}
		//zoom camera
		float scroll = Input.GetAxis("Mouse ScrollWheel");

		mainCamera.orthographicSize -= scroll * scrollSpeed * 100f * Time.deltaTime;
		//make sure that your value is never smaller than the min and never larger than the max. prevent mouse from moving out of the map
		pos.x = Mathf.Clamp(pos.x, minX, maxX);
		pos.y = Mathf.Clamp(pos.y, minY, maxY);
		if ((pos.x >= minX && pos.x <= maxX) || (pos.y >= minY && pos.y <= maxY))
		{
			transform.position = pos;
		}

	}
}
