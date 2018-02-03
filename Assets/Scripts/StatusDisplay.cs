
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
    }
}