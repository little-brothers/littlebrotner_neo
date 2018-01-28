using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TV01 : MonoBehaviour {

	Text _energyText;

	[SerializeField]
	WorkListElement.Work[] jobs;

	[SerializeField]
	VerticalLayoutGroup list;

	GameObject _notAvailable;
	GameObject _joblist;

	bool jobAvailable
	{
		get {
			return VoteManager.currentVote.day != MyStatus.instance.lastWork;
		}
	}

	void Start () {
		Utilities.SetUIParentFit(GameObject.FindGameObjectWithTag("RootCanvas"), gameObject);

		_joblist = transform.Find("ScrollView").gameObject;
		_notAvailable = transform.Find("NotAvailable").gameObject;
		_energyText = transform.Find("Energy").GetComponent<Text>();
		MyStatus.instance.health.OnUpdate += updateHealth;
		updateHealth(MyStatus.instance.health);

		MyStatus.instance.lastWork.OnUpdate += updateJobAvailable;
		updateJobAvailable(MyStatus.instance.lastWork);

		if (jobAvailable)
		{
			var listElemTmpl = Resources.Load<GameObject>("WorkListElement");
			foreach (var job in jobs.Where(j => j.enabled))
			{
				// var elem = GameObject.Instantiate(listElemTmpl, list.transform).GetComponent<WorkListElement>();
				var elem = GameObject.Instantiate(listElemTmpl, list.transform).GetComponent<WorkListElement>();
				elem.transform.localScale = Vector3.one;
				elem.work = job;
				elem.OnJobSelected += OnJobSelected;
			}
		}
	}

	void OnDestroy()
	{
		MyStatus.instance.health.OnUpdate -= updateHealth;
	}

	void OnJobSelected(WorkListElement.Work job)
	{
		if (job.health <= MyStatus.instance.health)
		{
			ConfirmPopup.Setup(string.Format("Are you sure do work '{0}'?", job.name), () => {
				MyStatus.instance.health.value -= job.health;
				MyStatus.instance.money.value += job.payment;
				MyStatus.instance.lastWork.value = VoteManager.currentVote.day;
				GameObject.Destroy(gameObject);
			});
		}
	}

	void updateHealth(int value)
	{
		_energyText.text = value.ToString();
	}
	
	void updateJobAvailable(int day)
	{
		_joblist.SetActive(jobAvailable);
		_notAvailable.SetActive(!jobAvailable);
	}
}
