using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoreboard : MonoBehaviour {

	int score = 0;

	public Camera _camera;

	public NumberDisplay _scoreDisplay;
	public NumberDisplay _remainingDisplay;

	public GameObject scoreTitle;
	public GameObject remainingTitle;

	public int STARTING_BALLS = 9;

	void Start()
	{
		_remainingDisplay.SetNumber(STARTING_BALLS);
		_scoreDisplay.SetNumber(0);
	}


	public void AddPoint() 
	{
		score++;
		_scoreDisplay.SetNumber(score);
		_remainingDisplay.SetNumber(STARTING_BALLS-score);
		//_score.Text = score.ToString();
	}

	public void Reset()
	{
		score = 0;
		_remainingDisplay.SetNumber(STARTING_BALLS);
		_scoreDisplay.SetNumber(0);
		//_score.Text = score.ToString();
	}

	void Update()
	{
		// LookAtCameraSlerp(scoreTitle);
		// LookAtCameraSlerp(remainingTitle);
	}

	public void LookAtCameraSlerp(GameObject obj)
	{
		obj.transform.rotation = Quaternion.Slerp(obj.transform.rotation, Quaternion.LookRotation(-(_camera.transform.position - obj.transform.position)), Time.deltaTime);
	}
}
