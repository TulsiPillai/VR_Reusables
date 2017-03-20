using UnityEngine;
using UnityEditor;
using System.Collections;

public class ChildCollider : MonoBehaviour {
	[MenuItem("My Tools/Create collider")]
	static void FitToChildren() {
		foreach (GameObject parentObject in Selection.gameObjects) {
			BoxCollider BC = parentObject.AddComponent<BoxCollider> ();
			Bounds mainBounds = new Bounds (Vector3.zero,Vector3.zero);
			mainBounds = BC.bounds; // to avoid incorrect encapsulation

			for (int i = 0; i < parentObject.transform.childCount; i++) {
				Renderer R = parentObject.transform.GetChild(i).GetComponent<Renderer>();
				print (R.bounds);
				mainBounds.Encapsulate (R.bounds);
				print (mainBounds.size);//get mesh extents from children 
			}

			BC.size = mainBounds.size;
			BC.center = mainBounds.center - parentObject.transform.position; //move colider to child center
		}


	}

}
