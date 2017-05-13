using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour 
{
	public int nestSize;
	public GUIText missionText;
	public GUIText scoreText;

	private int enemiesLeft;
	private int score;

	void Start () 
	{
		enemiesLeft = nestSize;
		score = 0;

		updateMission ();
		updateScore ();
	}
	
	void updateMission()
	{
		missionText.text = enemiesLeft.ToString();
	}

	void updateScore()
	{
		scoreText.text = score.ToString();
	}

	public void missionScoreDecrement()
	{
		if (enemiesLeft > 0) {
			enemiesLeft--;
			updateMission ();
			if (enemiesLeft == 0) {
				showLevelCompleteScreen ();
			}
		}
	}

	public void addScore(int score)
	{
		this.score += score;
		updateScore ();
	}

	void showLevelCompleteScreen()
	{
		//
	}
}
