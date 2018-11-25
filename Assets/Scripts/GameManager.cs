using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;
using UnityEngine.UI;
using System.Linq;
using MagicLeap;

public class GameManager : MonoBehaviour {
	[SerializeField, Tooltip("The reference to the class to handle results from.")]
	private BaseRaycast _raycast;

	[SerializeField, Tooltip("Debug Canvas")]
	private GameObject debugCanvas;

	[SerializeField, Tooltip("Head Camera")]
	public Camera _mainCamera;

	public GameObject _cursor;

	[SerializeField, Tooltip("The default distance for the cursor when a hit is not detected.")]
	private float _defaultDistance = 7.0f;

	public Transform _controller;

	private GameObject _objectBeingPlaced;

	public GameObject _vacuumBase;

	public GameObject _planeVisualizer;

	public Scoreboard _scoreboard;

	public float _groundY;

	public GameObject _plane;

	public Collider vacuumBaseCollider;

	public List<GameObject> pointsCollected;

	public ControllerConnectionHandler connectionHandler;

	// Use this for initialization
	void Start () {
		MLInput.OnControllerButtonDown += OnButtonDown;
		MLInput.OnTriggerDown += OnTriggerDown;

		pointsCollected = new List<GameObject>();
  	}

// Callback invoked when an asset has just been imported.
	#region Event Handlers
	private void OnButtonDown(byte controllerId, MLInputControllerButton button)
	{
		if(button == MLInputControllerButton.Bumper)
		{
			debugCanvas.SetActive(!debugCanvas.activeSelf);
			// var controllerParts = GameObject.Find("Parts");
			// controllerParts.SetActive(!controllerParts.activeInHierarchy);
		}
	}
	
	private void OnTriggerDown(byte controllerId, float triggerVal)
	{
		if(_cursor.activeInHierarchy)
		{
			_plane.SetActive(true);
			_plane.transform.position = _cursor.transform.position;
			// _plane.transform.rotation = _cursor.transform.rotation;
			_groundY = _cursor.transform.position.y;
			_cursor.SetActive(false);

			//_plane.GetComponent<MeshRenderer>().enabled = false;
			// _planeVisualizer.GetComponent<PlaneVisualizer>().Clear();
			// _planeVisualizer.SetActive(false);
		}
		else
		{
			pointsCollected.ForEach(x => x.SetActive(true));
			pointsCollected.Clear();
			
			_plane.SetActive(false);
			_scoreboard.Reset();

			// _planeVisualizer.SetActive(true);
			_cursor.SetActive(true);
		}
	}

	public void AddPoint(GameObject obj)
	{
		MLInputController controller = connectionHandler.ConnectedController;
		if (controller != null)
		{
			// Demonstrate haptics using callbacks.
			controller.StartFeedbackPatternVibe(MLInputControllerFeedbackPatternVibe.Bump, MLInputControllerFeedbackIntensity.High);
		}

		_scoreboard.AddPoint();
		obj.SetActive(false);
		pointsCollected.Add(obj);
	}

	/// <summary>
	/// Callback handler called when raycast has a result.
	/// Updates the transform an color on the Hit Position and Normal from the assigned object.
	/// </summary>
	/// <param name="state"> The state of the raycast result.</param>
	/// <param name="result"> The hit results (point, normal, distance).</param>
	/// <param name="confidence"> Confidence value of hit. 0 no hit, 1 sure hit.</param>
	public void OnRaycastHit(MLWorldRays.MLWorldRaycastResultState state, RaycastHit result, float confidence)
	{
		if (state == MLWorldRays.MLWorldRaycastResultState.RequestFailed || state == MLWorldRays.MLWorldRaycastResultState.NoCollision)
		{
			// No hit found, set it to default distance away from controller ray

		}
		else
		{
			// Hit found -- Update the object's position and normal.

			//calculate ground collision
			


			
			// _objectBeingPlaced.transform.position = result.point;
			// _objectBeingPlaced.transform.up = result.normal;
			// var originPoint = _raycast.RayOrigin;
			// originPoint.y = result.point.y; //put camera + hit on the same plane...

			//_objectBeingPlaced.transform.LookAt(originPoint); //...so lookat only affects y axis rotation
		}	
    }
	#endregion

	// Update is called once per frame
	void Update () {
		//calculate ground collision
		if(!_cursor.activeInHierarchy)
		{
			RaycastHit hit;
			var vacForward =  Quaternion.AngleAxis(-27f, _controller.right) * _controller.forward;

			// Does the ray intersect any objects excluding the player layer
			if (Physics.Raycast(_controller.position, vacForward, out hit, 20))
			{					
				// Debug.DrawRay(_controller.position, vacForward, Color.red, 200f);

				// var verticalDistToGround = _groundY - _controller.position.y;
				// var magnitudeToGround = verticalDistToGround/vacForward.y;

				// var collisionDirection = vacForward * magnitudeToGround;

				_vacuumBase.transform.position = new Vector3(
					hit.point.x, 
					hit.point.y, 
					hit.point.z);

				//set rotation
				var groundedFwd = new Vector3(_controller.transform.up.x, 0, _controller.transform.up.z);
				_vacuumBase.transform.forward = groundedFwd;

				_vacuumBase.transform.position += vacuumBaseCollider.transform.localScale.x/2.7f * _vacuumBase.transform.forward;
			}
		}
	}
}
