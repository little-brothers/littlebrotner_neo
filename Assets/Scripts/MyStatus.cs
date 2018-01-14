using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyStatus : MonoBehaviour {

	// singleton
	static MyStatus _instance = null;

	public static MyStatus instance
	{
		get {
			if (_instance == null)
				_instance = GameObject.FindObjectOfType<MyStatus>();

			Debug.Assert(_instance != null);
			return _instance;
		}
	}

	public class DataUpdateNotifier<T>
	{
		delegate void DataUpdatedEvent(T value);
		DataUpdatedEvent OnUpdate;

		// 형태 변환 없이 대입을 가능하게 해줌
		public static implicit operator DataUpdateNotifier<T>(T value)
		{
			var ret = new DataUpdateNotifier<T>();
			ret._value = value;
			return ret;
		}

		T _value = default(T);
		T value {
			get { return _value; }
			set { _value = value; OnUpdate(value); }
		}
	}


	/* [-100, 100] 범위를 갖는 성향 */
	public DataUpdateNotifier<int> political = new DataUpdateNotifier<int>(); // [사회주의 - 민주주의]
	public DataUpdateNotifier<int> economy = new DataUpdateNotifier<int>();   // [공산주의 - 자본주의]
	public DataUpdateNotifier<int> mechanic = new DataUpdateNotifier<int>();  // [신토불이 - 산업발전]
	public DataUpdateNotifier<int> money = new DataUpdateNotifier<int>();
}
