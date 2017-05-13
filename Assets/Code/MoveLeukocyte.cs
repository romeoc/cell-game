using UnityEngine;
using System.Collections;

public class MoveLeukocyte : MonoBehaviour 
{
	private GameObject player;
	
	private bool isEndPointSet = false;
	private float distanceFromPlayer;
	private Vector3 destination;
	private LineRenderer guide;
	private int currentHealth;

	public int health;
	public float speed;
	public float maxDistance;
	public float minimumSpeed = 0.025f;
	public bool isReleased = false;

	void Start()
	{
		// Set current health
		currentHealth = health;

		//Set object Position to Bone Marrow (player) position
		this.player = GameObject.Find("Player");
		this.transform.position = player.transform.position;

		//Retrieve line renderer and set it's color
		guide = GameObject.Find ("Guides").GetComponent<LineRenderer> ();
		guide.material = new Material(Shader.Find("Sprites/Default"));
		guide.SetColors (new Color(0.5f, 0, 0, 0.3f), new Color(0.5f, 0, 0, 0));
	}

	void Update()
	{
		//Save mouse position after left click release and enable object movement
		if (!isEndPointSet && Input.GetMouseButtonUp (0)) {
			isEndPointSet = true;
			isReleased = true;

			// Play rocket sound
			GameObject.Find("Player").GetComponent<AudioSource>().Play();

			//calculate destination (a point that continuous on the line from the object to the player)
			destination = player.transform.position - this.transform.position;
			destination = player.transform.position + ( 12f * destination.normalized);
		}

		//move object
		if (isReleased) {
			//If current position is the same as the destionation (the player center point was clicked),
			//then move the leokocyte (shot) forward
			if (this.transform.position == destination) {
				destination = Vector3.up;
			}

			guide.enabled = false;

			// Check if speed is at least the minimum speed
			float movementSpeed = Time.deltaTime * speed * distanceFromPlayer;
			movementSpeed = (movementSpeed < minimumSpeed) ? minimumSpeed : movementSpeed;

			this.transform.position = Vector3.MoveTowards(this.transform.position,
			                                              destination,
			                                              movementSpeed);
		} else {
			//if mouse button is still down make the object follow the mouse position
			Vector3 mousePosition = Input.mousePosition;
			mousePosition.z = 10;
			mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

			distanceFromPlayer = Vector3.Distance (mousePosition, player.transform.position);
			//check for maximum distance from bone marrow (player)
			if (distanceFromPlayer <= maxDistance) {
				this.transform.position = mousePosition;
			} else {
				//if maximum distance is reached set object on max Distance boundary
				float fractionPercentage = maxDistance / distanceFromPlayer;
				this.transform.position = ((mousePosition - player.transform.position) * fractionPercentage) 
											+ player.transform.position;
			}

			guide.enabled = true;

			Vector3 guideStartPoint = this.transform.position;
			guideStartPoint.z = -1;

			Vector3 guideEndPoint = player.transform.position - this.transform.position;
			guideEndPoint = player.transform.position + ( 12f * guideEndPoint.normalized);
			guideEndPoint.z = -1;

			guide.SetPosition(0, guideStartPoint);
			guide.SetPosition(1, guideEndPoint);
		}
	}

	public void applyDamage(int amount)
	{
		currentHealth -= amount;
		if (currentHealth <= 0) {
			Destroy(this.gameObject);
		}
	}

	public int getScoreMultiplier()
	{
		return health - currentHealth;
	}
}
