using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class TroopScript {
	public string name;
	public int diceFaces;
	public int health;
	public int speed;
	[System.Serializable]


	public enum prefTarget{
		Air,
		Ground,
		All
	};
	public prefTarget TargetType;
	public prefTarget TroopType;

}
