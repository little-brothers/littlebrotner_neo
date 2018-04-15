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


	public int require = 1;

	// Use this for initialization
	void Start () {

		for(int i=1; i<10; i++) {
			heights[i] = GameObject.Find("Height (" + i.ToString()+")"); 
		}


		Reset ();

	}

	public int Matching(int num){

		if (num == require) {
			require++;
			if (require == 11) {
				cursor.GetComponent<CursorController> ().PointUp (15);
				Reset ();
				return 2;
			}
			return 1;
		} else {
			return 0;
		}
	}


	public void Reset(){

		require = 1;
		for (int i = 1; i <= 10; i++) {
			h.Add (i);
		}
			
		int index;
		for (int i = 0; i < 10; i++) {
			index = Random.Range (0, h.Count);
			Debug.Log ("H COUNT"+h.Count);
			heights [i].GetComponent<Height> ().level = h [index];
			Debug.Log (i + " : " + h [index]);
			heights [i].SetActive (true);
			heights [i].GetComponent<Height> ().Resize ();
			h.RemoveAt (index);
	//why.......last thing doesn't show up
			//h_item.Add (h [index]);
			//Debug.Log (h_item[i]);
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
