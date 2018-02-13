using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class text_print_a : MonoBehaviour {


	private Text tex;
	string temp;

	[SerializeField]
	float delayTime;
	[SerializeField]
	float termTime = 0.15f;

	// Use this for initialization
	void Start () {

		tex = GetComponent<Text>();

		temp = tex.text; //get string
	
		tex.text = ""; //clear
			StartCoroutine("Printing");
	}


	IEnumerator Printing()


	{
		yield return new WaitForSeconds(delayTime);
		for (int j = 0; j < temp.Length; j++) 
		{
			tex.text += temp[j]; // put character 
			yield return new WaitForSeconds(termTime);// time
		}
		StopCoroutine ("Printing");
	}

	// Update is called once per frame
	void Update () {
		
	}
}
