using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BattleStates : MonoBehaviour {
	
	public enum battleStates{
		Wait,
		TakeAction,
		Results
	};

	public battleStates curBattleState;

	public List<HandleTurns> PerformList = new List<HandleTurns>();		

	public Queue<GameObject> playerOne = new Queue<GameObject>();

	public Queue<GameObject> playerTwo = new Queue<GameObject>();

	public GameObject activeTroop1, activeTroop2;

	public List<GameObject> p1Troops = new List<GameObject>();

	public List<GameObject> p2Troops = new List<GameObject>();



	//display items
	[HideInInspector]
	public string p1Type;
	[HideInInspector]
	public string p2Type;
	[HideInInspector]
	public string p1Health;
	[HideInInspector]
	public string p2Health;
	[HideInInspector]
	public bool p1Dead;
	[HideInInspector]
	public bool p2Dead;


	[HideInInspector]
	public bool isAttack1 = false;
	[HideInInspector]
	public bool isAttack2 = false;

	//battle results
	private int p1H, p2H;
	private string result;
	private int round = 0;


	// Use this for initialization
	void Start () {

		curBattleState = battleStates.Wait;

		//instantiates 20 troops from the troop list;
		GameObject spawn;

		GameObject player1 = GameObject.Find("PlayerOne");

		GameObject player2 = GameObject.Find ("PlayerTwo");

		for (int i = 0; i < 20; i++) {
			
			int troopIndex = Random.Range (0, 3);
			spawn = Instantiate (p1Troops [troopIndex] , player1.transform,false);
			spawn.transform.localPosition = new Vector3 (0, 0, 0);

		}
		for (int i = 0; i < 20; i++) {
			
			int troopIndex = Random.Range (0, 3);
			spawn = Instantiate (p2Troops [troopIndex], player2.transform, false);
			spawn.transform.localPosition = new Vector3 (0, 0, 0);

		}

		//adds all the troops in a queue for rotation
		foreach(GameObject go in GameObject.FindGameObjectsWithTag ("Player1")){
			go.SetActive (false);
			playerOne.Enqueue(go);	
		}
		foreach(GameObject go in GameObject.FindGameObjectsWithTag ("Player2")){
			go.SetActive (false);
			playerTwo.Enqueue(go);
		}
			

	}
	
	// Update is called once per frame
	void Update () {
		
		switch (curBattleState) {
		case battleStates.Wait:
			curBattleState = battleStates.TakeAction;
			break;
		case battleStates.TakeAction:
			ActivateTroop ();
			break;
		case battleStates.Results:
			DisplayResults ();
			break;
		}

	}

	//activates a troop from each player team
	public void ActivateTroop(){
		
		if (playerOne.Count != 0 && playerTwo.Count != 0) {
			
			activeTroop1 = (GameObject)playerOne.Peek ();
			activeTroop1.SetActive (true);
		
			activeTroop2 = (GameObject)playerTwo.Peek ();
			activeTroop2.SetActive (true);

		}
	}

	//gets attacker and target details
	public void GetCurrentAction(HandleTurns input){
		
		PerformList.Add (input);

	}

	//sends back battled troops to the end of the queue
	public void DeactivateTroop(){
		
		if (playerOne.Count != 0 && playerTwo.Count != 0) {
			
			activeTroop1.SetActive (false);
			activeTroop2.SetActive (false);
			playerOne.Enqueue ((GameObject)playerOne.Dequeue ());
			playerTwo.Enqueue ((GameObject)playerTwo.Dequeue ());

		}

	}

	//enables attack for current troops and attack is performed in player states script
	public void AttackState(){
		
		if (activeTroop1.GetComponent<PlayerOneStates> ().cur_cooldown >= 0.5f) {
			isAttack1 = true;
			isAttack2 = true;
			round++;
		}
	}

	//gets battle results from both player states
	public void GetPlayerTwoResults(string p1H, string p2T, bool isDead){
		
		p1Health = p1H;
		p2Type = p2T;
		p1Dead = isDead;
		isAttack2 = false;

		if (!isAttack1 && !isAttack2)
			
			curBattleState = battleStates.Results;

	}
	public void GetPlayerOneResults(string p2H, string p1T, bool isDead){
		
		p2Health = p2H;
		p1Type = p1T;
		p2Dead = isDead;
		isAttack1 = false;

		if (!isAttack1 && !isAttack2)
			
			curBattleState = battleStates.Results;
	}

	//print results 
	public void DisplayResults(){	
		
		if (p1Dead && playerOne.Count != 0) {
			
			playerOne.Dequeue ();

			print ("Player one " + p1Type + " was killed");

		} else if (p2Dead && playerTwo.Count != 0) {
			
			playerTwo.Dequeue ();

			print ("Player two " + p2Type + " was killed");

		} else if (playerOne.Count == 0) {
			
			print ("PLAYER TWO IS THE WINNER!!!");
	
			foreach (GameObject go in playerTwo) {	
				print (go.gameObject.GetComponent<PlayerTwoStates> ().troop.name + " " + go.gameObject.GetComponent<PlayerTwoStates> ().troop.health);
			}

		} else if (playerTwo.Count == 0) {
			
			print ("PLAYER ONE IS THE WINNER!!!");

			foreach (GameObject go in playerOne) {
				print (go.gameObject.GetComponent<PlayerOneStates> ().troop.name + " " + go.gameObject.GetComponent<PlayerOneStates> ().troop.health);
			}
		}
		else
			print ("Round: " + round + " Player 1 " + p1Type + " (Health = " + p1Health + ") " + " vs Player 2 " + p2Type + " (Health = " + p2Health + ")");
		
		DeactivateTroop ();

		PerformList.Clear ();

		curBattleState = battleStates.Wait;
	}


}
