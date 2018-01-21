using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SpriteAnimator : AnimatorBehaviour
{
    [SerializeField]
    string _defaultState = string.Empty;

    void OnEnable()
    {
        if (!_defaultState.CompareTo(string.Empty).Equals(0))
        {
            setState(_defaultState);
            StartCoroutine(runAnimation());
        }
    }
}
