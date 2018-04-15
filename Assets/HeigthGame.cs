using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeigthGame : MonoBehaviour {

	public GameObject cursor;

	[SerializeField]
	public GameObject[] heights;

	List<int> h = new List<int>();
	//List<int> h_item = new List<int>();


	public int require = 10;

	// Use this for initialization
	void Start () {

		for(int i=1; i<10; i++) {
			heights[i] = GameObject.Find("Height (" + i.ToString()+")"); 
		}


		Reset ();

	}

	public bool Matching(int num){

		if (num == require) {
			require--;
			Debug.Log (require);
			if (require == 0) {
				cursor.GetComponent<CursorController> ().PointUp (15);
				Reset ();
			}
			return true;
		} else {
			return false;
		}
	}


	void Reset(){

		require = 10;
		for (int i = 1; i <= 10; i++) {
			h.Add (i);
		}
			
		int index;
		for (int i = 0; i < 10; i++) {
			index = Random.Range (0, h.Count);
			heights [i].GetComponent<Height> ().level = h [index];
			h.RemoveAt (index);
			heights [i].GetComponent<Height> ().Resize ();
			heights [i].SetActive (true);
			//why.......last thing doesn't show up
			//h_item.Add (h [index]);
			//Debug.Log (h_item[i]);
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
