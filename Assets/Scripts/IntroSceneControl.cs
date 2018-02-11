using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroSceneControl : MonoBehaviour {

	private Animation anim;
	private AnimationClip[] clips;



	[SerializeField]
	public GameObject linkClick;


	//private Button qBtn;

	// Use this for initialization
	void Start () {

		//qBtn = GetComponent<Button>();
		//Debug.Log ("1");
		//StartCoroutine(Wait());

		//animation.Play("FadeIn");
		//ArrayList ani = GetComponent<Animations> ();

		//gameObject.animation.Play(ani[0]);
	}






	
	// Update is called once per frame
	void Update () {
		
	}

	public void PressSkip()
	{
		SceneManager.LoadScene("TutorialScene");
	}



	public void NextClick(){

		Debug.Log ("CLICK!!");
		//GameObject nextLink2;
		StartCoroutine(Wait());
		Debug.Log ("wait!!");


		//GameObject.Destroy(gameObject);

	}



	public void LastClick(string scene){


	
		gameObject.SetActive (false);
		SceneManager.LoadScene(scene);

	}


	IEnumerator Wait()
	{
		Debug.Log ("www!!");
		yield return new WaitForSeconds (0.1f);
		linkClick.SetActive(true);
		gameObject.SetActive (false);
		//Debug.Log ("3");
		//qBtn.interactable = true;
	}


	IEnumerator Next()
	{
		Debug.Log ("3");
		yield return new WaitForSeconds (2);
		linkClick.SetActive(true);
		gameObject.SetActive (false);

	}
}


