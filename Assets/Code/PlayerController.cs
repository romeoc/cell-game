using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	public GameObject cell;
	public float fireRate;

	private float nextShot;

	void Update() 
	{
		//Check for mousedown event
		if (Input.GetMouseButtonDown(0)) {
			//get all objects on the mouse coordinates
			//we need all of them because the "Barrier" is on the same z coordinate is sometimes returned when using raycast
			RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			foreach (RaycastHit2D hit in hits) {
				//Check if the Player element was clicked and enough time has elapsed since last shot
				if (hit.collider.name == "Player" && Time.time > nextShot) {
					//Set next shot time
					nextShot = Time.time + fireRate;
					//If so instantiate a new shot (leokocyte)
					Instantiate (cell, Input.mousePosition, Quaternion.identity);
				}
			}
		}
	}
}
