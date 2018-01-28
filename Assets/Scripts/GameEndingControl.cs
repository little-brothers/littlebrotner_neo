using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEndingControl : MonoBehaviour {

	[SerializeField]
	private Text _endingText;

	private void Start () 
	{
		// 돈, 체력, 수치 3개
		int money = MyStatus.instance.money.value;
		int health = MyStatus.instance.health.value;
		int economy = MyStatus.instance.economy.value;
		int political = MyStatus.instance.political.value;
		int mechanic = MyStatus.instance.mechanic.value;
		_endingText.text = "M: " + money.ToString() + "|H: " + health.ToString() + "|E: " + economy.ToString() +
		"|P: " + political.ToString() + "|ME: " + mechanic.ToString();
	}
	
}
