using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageAnimator : MonoBehaviour
{
    [SerializeField]
    List<Sprite> sprites;

    [SerializeField]
    float duration = 0.5f;

    void OnEnable()
    {
        StartCoroutine(runAnimation());
    }

    IEnumerator runAnimation()
    {
        int idx = 0;
        if (sprites.Count == 0)
            yield break;

        Image image = GetComponent<Image>();
        while (true) {
            yield return new WaitForSeconds(duration);
            idx = ++idx % sprites.Count;
            image.sprite = sprites[idx];
        }
    }
}
