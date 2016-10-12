using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RayCasting : MonoBehaviour {
	
	public enum SteeringState{
		NotTranslating, 
		LockDestination, 
		Translating
	};

	private SteeringState state;
	public Button button; //select a VR controller button
	float distance, speed;
	public Camera CameraFacing; //player camera
	RaycastHit hit;
	SpriteRenderer reticleSprite;
	private Vector3 originalScale, targetPos, dirOfTravel;
	bool hitWayPoint;
  //physical space to transform 
	public Space space; 
	public GameObject reticle;

	// Use this for initialization
	void Start () {
		state = SteeringState.NotTranslating;
		speed = 5 * Time.deltaTime;
		originalScale = transform.localScale;
		reticleSprite = reticle.gameObject.GetComponent<SpriteRenderer> ();
		hitWayPoint = false;
	}
	
	// Update is called once per frame
	void Update () {
		print (state);
		//
		//Raycasting starts here
		//
		if (Physics.Raycast(new Ray(CameraFacing.transform.position,
			CameraFacing.transform.rotation * Vector3.forward),
			out hit))
		{
			distance = hit.distance;
			checkForCollision();
		}
		else
		{
			distance = 100.0f; //if it hits nothing set to default distance
			reticleSprite.color = new Vector4(1, 1, 1, 1);
		}
		reticle.transform.position = CameraFacing.transform.position + CameraFacing.transform.rotation * Vector3.forward * distance;
		reticle.transform.LookAt(CameraFacing.transform.position);
		reticle.transform.Rotate(0.0f, 180.0f, 0.0f);
		if (distance < 10.0f)
		{
			distance *= 1 + 5 * Mathf.Exp(-distance);
		}
		reticle.transform.localScale = originalScale * distance * 0.05f;
		//3d travel states
		//if state is not steering
		if(state == SteeringState.NotTranslating){
			
			if (hitWayPoint) { //reticle turns blue
				
				state = SteeringState.LockDestination;

			} else if (state == SteeringState.LockDestination && button.GetPress ()) {
				
				state = SteeringState.Translating;

			} else {
				//do nothing


			}
		}

		//when target is locked
		if(state == SteeringState.LockDestination){
			
			if (button.GetPress ()) {
				state = SteeringState.Translating;
			}
		}
		//start translation
		if(state == SteeringState.Translating){
			reticleSprite.color = new Vector4 (0, 1, 0, 1); 
			if (Vector3.Distance (space.transform.position, targetPos) > 0.2f) {
				dirOfTravel = targetPos - space.transform.position; //distance
				dirOfTravel.Normalize ();
				space.transform.position = space.transform.position + dirOfTravel * speed ;
			} else {
				//disable current waypoint
				state = SteeringState.NotTranslating;
				reticleSprite.color = new Vector4 (0, 0, 1, 1); 

			}
		}
	}

	public void checkForCollision(){
		if (hit.collider.tag == "Waypoint") {
			targetPos = hit.collider.transform.position;
			hitWayPoint = true;
			reticleSprite.color = new Vector4 (0, 0, 1, 1); 
		} else {
			hitWayPoint = false;
		}
	}
}
