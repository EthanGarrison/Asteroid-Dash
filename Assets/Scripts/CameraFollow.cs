using UnityEngine;
using System.Collections;


[AddComponentMenu("Camera-Control/Follow Character")]
public class CameraFollow : MonoBehaviour {
	
	public Transform character;
	//Have to use temporary Vector because transform.position.x or y or z is read-only
	private Vector3 temp;

	void LateUpdate() {

		if(!character)
			return;

		//Vector2 wantedPosition = (character.position);
		//Vector2 currentPositon = (transform.position);

		temp = transform.position;

		temp = character.position;

		temp.z = transform.position.z;

		transform.position = temp;

	}
}
