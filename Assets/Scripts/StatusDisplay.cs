
using UnityEngine;
using UnityEngine.UI;

public class StatusDisplay : MonoBehaviour {

    public enum WatchType {
        Money,
        Tax,
        Health,
    }

    [SerializeField]
    WatchType watch;

    MyStatus.DataUpdateNotifier<int> _notifier;
	Color CautionColor = new Color(0.8f, 0.2f, 0.2f);
	Color NormalColor = Color.white;

	public GameObject energy;


    void Start() {

        switch (watch) {
        case WatchType.Money:
            _notifier = MyStatus.instance.money;
            break;

        case WatchType.Tax:
            _notifier = MyStatus.instance.tax;
            break;

        case WatchType.Health:
            _notifier = MyStatus.instance.health;
            break;
        }

        if (_notifier == null) {
            Debug.Assert(false, "need valid notifier type!");
            return;
        }

        _notifier.OnUpdate += OnValueUpdated;
        OnValueUpdated(_notifier.value);
    }

    void OnDestroy() {
        if (_notifier != null) {
            _notifier.OnUpdate -= OnValueUpdated;
        }
    }

    void OnValueUpdated(int value)
    {
        var textMesh = GetComponent<TextMesh>();
        if (textMesh != null) {
            textMesh.text = value.ToString();
        }

        var uiText = GetComponent<Text>();
        if (uiText != null) {
            uiText.text = value.ToString();
        }

        if (MyStatus.instance.money.value < MyStatus.instance.tax.value) {
            textMesh.color = CautionColor;
        //	uiText.color = CautionColor;
            Debug.Log (textMesh.color);
        //Debug.Log(energy._energys);

        } else {
            textMesh.color = NormalColor;
        //	uiText.color = NormalColor;
            //Debug.Log (uiText.color);
        }
    }
}