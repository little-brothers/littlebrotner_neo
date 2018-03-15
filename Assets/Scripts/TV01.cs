﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
		if (_notAvailable.text.Equals("Please select a job"))
		{
			var listElemTmpl = Resources.Load<GameObject>("WorkListElement");
			var jobs = Database<Work>.instance.ToList().Where(job => MyStatus.Check(job.condition));
			foreach (var job in jobs)
			{
				// var elem = GameObject.Instantiate(listElemTmpl, list.transform).GetComponent<WorkListElement>();
				var elem = GameObject.Instantiate(listElemTmpl, list.transform).GetComponent<WorkListElement>();
				elem.transform.localScale = Vector3.one;
				elem.work = job;
				elem.OnJobSelected += OnJobSelected;
			}

			if (jobs.Count() == 0)
			{
				_notAvailable.text = "No jobs are required in Utopia";
				_stateText.text = _notAvailable.text;
			}
			else
			{
				_notAvailable.gameObject.SetActive(false);
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
				MyStatus.instance.money.value += job.payment;
				MyStatus.instance.lastWork.value = MyStatus.instance.day;
				GameObject.Destroy(gameObject);
			});
		}
	}

	string checkJobAvailable()
	{
		if (MyStatus.instance.day == MyStatus.instance.lastWork)
			return "You have already worked today";

		if (MyStatus.instance.homeDestroyed)
			return "Home facilities are destoryed so I can't do any job";

		if (MyStatus.instance.invasion > 0)
			return "The invasion of outer space has begun so I can't do any job";

		return "Please select a job";
	}

	void updateHealth(int value)
	{
		_energyText.text = value.ToString();
	}
	
	void updateJobAvailable(int day)
	{
		var jobErr = checkJobAvailable();
		_joblist.SetActive(jobErr.Equals("Please select a job"));
		_notAvailable.text = jobErr;
		_stateText.text = jobErr;
	}
}
