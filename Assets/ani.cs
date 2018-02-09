using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ani : MonoBehaviour {


	[SerializeField]
	int startDelay;

	public Animator animator;

	void Start()
	{
		StartCoroutine(Example());
		animator.Play("purple");
	}

	IEnumerator Example()
	{
		
		yield return new WaitForSeconds(startDelay);

	}



	
	// Update is called once per frame
	void Update () {
		animator.Play("purple");
	}
}
