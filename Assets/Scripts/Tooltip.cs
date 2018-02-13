using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour {

    Text _text;

    int _showCount = 0;

    void Start() {
        _text = transform.Find("Text").GetComponent<Text>();
        gameObject.SetActive(false);
    }

    void Update() {
        if (_showCount != 0) {
            UpdatePosition();
        }
    }

    void UpdatePosition()
    {
        var pos = transform.position;
        pos.x = Input.mousePosition.x;
        pos.y = Input.mousePosition.y + 20;
        transform.position = pos;
    }

    public void Hide()
    {
        _showCount--;
        if (_showCount <= 0) {
            _showCount = 0;
            gameObject.SetActive(false);
        }
    }

    public void Show(string text)
    {
        _showCount++;
        gameObject.SetActive(true);
        _text.text = text;
        UpdatePosition();
    }
}
