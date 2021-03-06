﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TV01 : MonoBehaviour {

	Text _energyText;

	[SerializeField]
	private Text _stateText;

	[SerializeField]
	VerticalLayoutGroup list;

	Text _notAvailable;
	GameObject _joblist;

	void Start () {
		Utilities.SetUIParentFit(GameObject.FindGameObjectWithTag("RootCanvas"), gameObject);

		_joblist = transform.Find("ScrollView").gameObject;
		_notAvailable = transform.Find("NotAvailable").GetComponent<Text>();
		_energyText = transform.Find("Energy").GetComponent<Text>();
		MyStatus.instance.health.OnUpdate += updateHealth;
		updateHealth(MyStatus.instance.health);

		MyStatus.instance.lastWork.OnUpdate += updateJobAvailable;
		updateJobAvailable(MyStatus.instance.lastWork);

		_notAvailable.text = checkJobAvailable();
		_notAvailable.gameObject.SetActive(true);
		if (_notAvailable.text == "")
		{
			var jobs = Database<Work>.instance.ToList(); //.Where(job => MyStatus.Check(job.condition));
			var jobAvailable = jobs.Any(job => MyStatus.Check(job.condition));
			if (!jobAvailable) {
				_notAvailable.text = "Little Brothers are not allowed to work yet.";
				_stateText.text = "Not available";

			} else {
				var listElemTmpl = Resources.Load<GameObject>("WorkListElement");
				jobs.Sort((l, r) => {
					var lActivate = MyStatus.Check(l.condition);
					var rActivate = MyStatus.Check(r.condition);
					if (lActivate != rActivate) 
						return lActivate ? -1 : 1;

					return l.id < r.id ? -1 : 1;
				});

				foreach (var job in jobs)
				{
					// var elem = GameObject.Instantiate(listElemTmpl, list.transform).GetComponent<WorkListElement>();
					var elem = GameObject.Instantiate(listElemTmpl, list.transform).GetComponent<WorkListElement>();
					elem.transform.localScale = Vector3.one;
					elem.work = job;
					elem.OnJobSelected += OnJobSelected;

					jobAvailable |= MyStatus.Check(job.condition);
				}
			}
		}

		//_stateText.text = string.Format("HP:{0}/G:{1}", MyStatus.instance.health.value, MyStatus.instance.money.value);
	}

	void OnDestroy()
	{
		MyStatus.instance.health.OnUpdate -= updateHealth;
		MyStatus.instance.lastWork.OnUpdate -= updateJobAvailable;
	}

	void OnJobSelected(Work job)
	{
		if (job.health <= MyStatus.instance.health)
		{
			ConfirmPopup.Setup(string.Format("Are you sure do work '{0}'?", job.name), () => {
				MyStatus.instance.health.value -= job.health;
				MyStatus.instance.lastWorkId = job.id;
				MyStatus.instance.lastWork.value = MyStatus.instance.day;

				// 화면 전환
				GameObject.FindObjectOfType<RoomSwitcher>().setRoomIdx(1, () => {
					MyStatus.instance.ResetAllHooks();
					SceneManager.LoadScene("MiningScene");
				});

				GameObject.Destroy(gameObject);
			});
		}
	}

	string checkJobAvailable()
	{
		if (MyStatus.instance.day.value == MyStatus.instance.lastWork.value)
			return "You have already worked today";


		if (MyStatus.instance.day.value == MyStatus.instance.homeDestroyed.value)
			return "All work was stopped because of the huge sand storm.";

		if (MyStatus.instance.invasion > 0)
			return "All work is prohibited to prepare for external aggression.";

		return "";
	}

	void updateHealth(int value)
	{
		_energyText.text = value.ToString();
	}
	
	void updateJobAvailable(int day)
	{
		var jobErr = checkJobAvailable();
		bool jobAvailable = jobErr.Equals ("");
		_joblist.SetActive(jobAvailable);
		_notAvailable.text = jobErr;
		if(jobAvailable){
			_stateText.text = "select today's job"; 
		}else{
			_stateText.text = "not available"; 
		}
	}
}
