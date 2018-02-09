using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class panel : MonoBehaviour {


	//public GameObject[] array_objects = new GameObject[16];

	public GameObject[] array_objects_a = new GameObject[8];
	public GameObject[] array_objects_b = new GameObject[8];
	public GameObject[] array_objects_red = new GameObject[8];

	void Start () {

		MyStatus.instance.day.OnUpdate += UpdateDate;
		UpdateDate(MyStatus.instance.day);
	}

	void OnDestroy() {
		MyStatus.instance.day.OnUpdate -= UpdateDate;
	}


	void CheckBinary(int[] array1, GameObject[] array2, bool right){

		for (int i = 0; i < 8; i++) {
			//Debug.Log (i + "----:" + array1 [i] + ":" + array2 [i]);
			if (array1 [i] == 1) {
				array2 [i].SetActive (right);
			} else {
				array2 [i].SetActive (!right);
			}
		}
	}

	void RandomLight(GameObject[] array3){

		for (int i = 0; i < 8; i++) {
			float temp = Random.Range (-1.0f, 1.0f);
//			Debug.Log (temp);
			if (temp >= 0) {
				array3 [i].SetActive (true);
			} else {
				array3 [i].SetActive (false);
			}
		}


	}


	void UpdateDate(int day)
	{
		day.ToString("0");


		int temp = day;
		int[] array_2 = new int[8];

		for(int i=7; i>=0; i--){

			if(temp==0){

				array_2 [i] = 0;
			}
			else{
				array_2 [i] = temp%2;
				temp = (int)Mathf.Floor (temp/2);

			}
		}


		CheckBinary (array_2, array_objects_a, true);
		CheckBinary (array_2, array_objects_b, false);
		RandomLight (array_objects_red);

	}

}
