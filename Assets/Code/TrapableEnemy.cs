using UnityEngine;
using System.Collections;

public class TrapableEnemy : MonoBehaviour 
{
	public float rotationsPerMinute;
	public int speed;
	public int scoreValue;
	public int damage;
	
	private GameObject jailKeeper;
	private bool isTrapped = false;

	public Vector3 destination;
	public bool destinationChanged = false;
	public Vector3 originalDestination;

	void OnTriggerEnter2D(Collider2D other) 
	{
		if (other.tag == "Destroyer" && jailKeeper == null) {
		//Save shot object (leukocyte)
			
			MoveLeukocyte leukocyteScript = other.gameObject.GetComponent<MoveLeukocyte>();
			if (leukocyteScript.isReleased) {
				isTrapped = true;
				jailKeeper = other.gameObject;
			}

			// Apply damage to leukocite
			leukocyteScript.applyDamage(damage);

			// Update texts
			LevelController levelController = GameObject.FindWithTag ("GameController").GetComponent <LevelController>();
			levelController.missionScoreDecrement();
			levelController.addScore(scoreValue * leukocyteScript.getScoreMultiplier());

		} else if (other.tag == "Player") {
		// If the other object is the player

			// set new destination to a point on the far edge of the line between the player and the object
			destination = this.transform.position - other.transform.position;
			destination = this.transform.position + ( 12f * destination.normalized);

		} else if (other.tag == "Enemy" && !isTrapped) {
		// If the other object is also an enemy
			
			// Get other Object Script
			TrapableEnemy otherScript = other.gameObject.GetComponent<TrapableEnemy>();

			// Set the destination to the other objects destination (to make it look like they bounced off)
			if (otherScript.destinationChanged == true) {
				// If the other object's destination was already set
				destination = otherScript.originalDestination;
				otherScript.destinationChanged = false;
			} else {
				// If this is the first object that is changed
				// Save the original destination for the other object
				originalDestination = destination;
				destination = otherScript.destination;
				destinationChanged = true;
			}
		}
	}

	void Start()
	{
		// Set object to a random angle
		Quaternion randomRotation = transform.rotation;
		randomRotation.z = Random.rotation.z;
		transform.rotation = randomRotation;

		//get boundary values
		GameObject boundary = GameObject.FindWithTag("Boundary");
		Vector2 spawnPoints = boundary.GetComponent<BoxCollider2D>().size;

		//Get random vector destination towards the barrier
		Vector3 pointTowardsDestination = new Vector3(
			Random.Range (-spawnPoints.x/2, spawnPoints.x/2),
			Random.Range (-spawnPoints.y/2, spawnPoints.y/2),
			0
		);

		//move destination point on the same line to make movement continuous
		destination = pointTowardsDestination - this.transform.position;
		destination = pointTowardsDestination + ( 12f * destination.normalized);
	}

	void Update()
	{
		//make object rotate continually
		transform.Rotate(0, 0, 6 * rotationsPerMinute * Time.deltaTime);

		//If object is trapped in leukocyte (shot) (this is true after the shot collided with the object)
		if (isTrapped) {
			//destroy object if leukocyte (shot) is destroyed meanwhile
			if (jailKeeper == null) {
				Destroy (this.gameObject);
				return;
			}
			//Make object stay inside the leukocite (shot)
			this.transform.position = Vector3.MoveTowards (
				this.transform.position, 
				jailKeeper.transform.position, 
				Time.deltaTime * 20
			);
		} else {
			//Move the new enemy cell in a direction towards the barrier collider
			this.transform.position = Vector3.MoveTowards(
				this.transform.position,
				destination,
				Time.deltaTime * speed
			);
		}
	}
}
