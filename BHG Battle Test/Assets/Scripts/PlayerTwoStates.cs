using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTwoStates : MonoBehaviour {
	public TroopScript troop;

	[HideInInspector]
	public enum actionStates{
		Processing,
		AddToList,
		Waiting,
		Selecting,
		Action,
		Dead
	}

	public actionStates curState;

	public bool isDead = false;

	private GameObject progressBar;
	public float cur_cooldown = 0f;
	private float max_cooldown = .5f;
	private BattleStates BS;
	private GameObject targetObj;

	HandleTurns myAttack = new HandleTurns (); 


	// Use this for initialization
	void Start () {
		
		curState = actionStates.Processing;

		BS = GameObject.Find("BattleManager").GetComponent<BattleStates>();

		progressBar = GameObject.Find ("ProgressBar2").gameObject;
	}


	// Update is called once per frame
	void Update () {
		
		switch (curState) {

		case actionStates.Processing:
			UpdateProgressBar ();
			break;

		case actionStates.AddToList:
			break;

		case actionStates.Waiting:
			//do nothing - wait for attack button
			break;

		case actionStates.Selecting: 
			GetTargetDetails ();
			break;

		case actionStates.Action:
			StartCoroutine ("AttackPlayer");
			break;

		case actionStates.Dead:			
			break;
		}

		if (BS.isAttack2 == true)
			curState = actionStates.Action;
		
	}

	//progress to get troops ready for next round
	void UpdateProgressBar(){
		
		cur_cooldown += Time.deltaTime;

		float cool_rate = cur_cooldown / max_cooldown;

		progressBar.transform.localScale = new Vector3 (Mathf.Clamp (cool_rate, 0, 1), progressBar.transform.localScale.y, progressBar.transform.localScale.z);

		if (cur_cooldown >= max_cooldown) {
			curState = actionStates.Selecting;
		}

	}

	public void GetTargetDetails(){
		
		myAttack.attacker = troop.name;

		myAttack.attackObject = this.gameObject;

		myAttack.targetObject = BS.activeTroop1;
			
		BS.GetCurrentAction (myAttack);

		curState = actionStates.Waiting; 

		if (myAttack.targetObject.GetComponent<PlayerOneStates> ().troop.TargetType != troop.TargetType) {
			
		}

	} 


	public IEnumerator AttackPlayer(){
		
		if (BS.isAttack2 && (cur_cooldown >= max_cooldown)) {
		
			int p1Health = BS.activeTroop1.GetComponent<PlayerOneStates> ().troop.health;

			if (troop.name == "Air" || troop.name == "Balloon") {

				p1Health -= (Random.Range (1, troop.diceFaces) + troop.speed);

			} else if (myAttack.targetObject.GetComponent<PlayerOneStates> ().troop.TroopType != troop.TargetType) {
				//reduce the damage by 2
				p1Health -= (Random.Range (1, troop.diceFaces) + troop.speed) / 2;

			} else {

				p1Health -= (Random.Range (1, troop.diceFaces) + troop.speed);
			}

			BS.activeTroop1.GetComponent<PlayerOneStates> ().troop.health = p1Health;
			//player one is killed
			if (p1Health <= 0) {
				isDead = true;
			}

			BS.GetPlayerTwoResults (p1Health.ToString (), troop.name.ToString (), isDead); 

			curState = actionStates.Waiting;

			}

		yield return null;
	}

	void OnEnable(){
		
		cur_cooldown = 0;
		curState = actionStates.Processing;
	}

}
