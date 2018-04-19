using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeigthGame : MonoBehaviour, IMinigame {
	List<int> h = new List<int>();

	int _require;

	GameObject[] heights;
	int _score;
	const int MaxScore = 100;
	const float GameTime = 10f;

	public float Progress
	{
		get { return _score / (float)MaxScore; }
	}

	public float MaxTime
	{
		get { return GameTime; }
	}

	// Use this for initialization
	void Awake() {

		heights = new GameObject[10];
		heights[0] = GameObject.Find("Height"); 
		for(int i=1; i<10; i++) {
			heights[i] = GameObject.Find("Height (" + i.ToString()+")"); 
		}
	}

	public int Matching(int num){

		if (num == _require) {
			_require++;
			if (_require == 11) {
				_score += 15;
				Reset ();
				return 2;
			}
			return 1;
		} else {
			return 0;
		}
	}


	public void Reset(){

		_require = 1;
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

	public void Setup()
	{
		Reset();
	}

	public void Finished()
	{
		// throw new System.NotImplementedException();
	}

	public bool Tick()
	{
		return false;
	}
}
