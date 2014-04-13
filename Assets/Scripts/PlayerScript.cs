using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	Rigidbody2D player;
	int playerPoints;
	int playerMoves;
	int playerUpgradeFuel;
	int playerVictory;
	int playerTurns;
	bool isDragging = false;
	bool playerMoving;
	Vector2 dragNode;
	float speed = 10f;
	public GUIStyle textStyle;
	public GUIStyle upgradeStyle;
	public GUIStyle fixStyle;
	public GUIStyle doneStyle;

	// Use this for initialization
	void Start () {
		player = this.gameObject.rigidbody2D;
		playerPoints = 0;
		playerMoves = 5;
		playerUpgradeFuel = 0;
		playerVictory = 0;
		playerTurns = 0;
		playerMoving = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if(Input.touchCount != 0)
			MoveToTouch(Input.touches[0]);

		if(Input.GetKey(KeyCode.End))
			Application.Quit();
	
	}

	void MoveToTouch(Touch touch) {
		Vector3 touchPositionOriginal = Camera.main.ScreenToWorldPoint(touch.position);
		Vector2 touchPosition = new Vector2(touchPositionOriginal.x, touchPositionOriginal.y);
		Vector2 playerPosition = new Vector2(player.transform.position.x, player.transform.position.y);
		// player.transform.position = new Vector3(Mathf.Lerp(player.transform.position.x, touchPosition.x, Time.deltaTime), Mathf.Lerp(player.transform.position.y, touchPosition.y, Time.deltaTime), 0);
		switch(touch.phase){
			case TouchPhase.Began:
				if(Mathf.Abs(touchPosition.x - playerPosition.x) < .4 && Mathf.Abs(touchPosition.y - playerPosition.y) < .4){
					dragNode = new Vector2(touchPosition.x, touchPosition.y);
					isDragging = true;
					player.transform.localScale = new Vector3(2, 2, 0);
				}
				break;
			case TouchPhase.Moved:
				if(isDragging){
					dragNode = new Vector2(-touchPosition.x*speed, -touchPosition.y*speed);
					// player.transform.Rotate(new Vector3(0, 0, Mathf.Atan(dragNode.y/dragNode.x)+90));
				}
				break;
			case TouchPhase.Ended:
				if(isDragging){
					float normalizedX = (Mathf.Cos(dragNode.y/dragNode.x));
					float normalizedY = (Mathf.Sin(dragNode.y/dragNode.x));
					if(dragNode.x < 0){
						normalizedX *= -1;
						normalizedY *= -1;
					}
					player.AddForce(new Vector2(normalizedX*200, normalizedY*200));
					playerMoving = true;
					Debug.Log(dragNode);
					Debug.Log("X: " + normalizedX);
					Debug.Log("Y: " + normalizedY);
					Debug.Log(new Vector2(normalizedX*200, normalizedY*200));
					isDragging = false;
					player.transform.localScale = new Vector3(1, 1, 0);
				}
				break;
			default:
				break;
		}


	}

	void OnGUI(){
		GUI.contentColor = Color.green;
		GUI.Label(new Rect(15,15,100,20), "Points: " + playerPoints, textStyle);

		GUI.Label(new Rect(15,35,100,20), "Moves: " + playerMoves, textStyle);

		GUI.Label(new Rect(15,55,100,20), "Turns: " + playerTurns, textStyle);

		if(playerMoves < 1){
			if(GUI.Button(new Rect((Screen.width/2)-200, (Screen.height/2)+100, 100, 100), "", upgradeStyle)){
				playerPoints -= 500;
				playerUpgradeFuel++;
			}
			if(GUI.Button(new Rect((Screen.width/2)-50, (Screen.height/2)+100, 100, 100), "", fixStyle)){
				playerPoints -= 1000;
				playerVictory++;
			}

			if(GUI.Button(new Rect((Screen.width/2)+100, (Screen.height/2)+100, 100, 100), "", doneStyle)){
				EndOfTurn();
			}
		}
		if(playerVictory > 4 && playerMoves > 0){
			GUI.Label(new Rect((Screen.width/2)-150, (Screen.height/2)-100, 300, 200), "You can now return to Earth!\nIt took you " + playerTurns + " flights.", textStyle);
			EndOfGame();
		}
		GUI.contentColor = Color.white;
	}

	void OnCollisionEnter2D(Collision2D coll){
		if((coll.gameObject.tag == "AsteroidS" || coll.gameObject.tag == "AsteroidM" || coll.gameObject.tag == "AsteroidL") && playerMoving){
			player.velocity = Vector3.zero;
			playerMoving = false;
			playerMoves--;
			if(coll.gameObject.tag == "AsteroidS"){
				playerPoints += 50;
			}
			else if(coll.gameObject.tag == "AsteroidM"){
				playerPoints += 150;
			}
			else if(coll.gameObject.tag == "AsteroidL"){
				playerPoints += 300;
			}
			Object.Destroy(coll.gameObject);
		}
	}

	void EndOfTurn(){
		playerTurns++;
		playerMoves += 5 + playerUpgradeFuel;
	}

	void EndOfGame(){

	}
}
