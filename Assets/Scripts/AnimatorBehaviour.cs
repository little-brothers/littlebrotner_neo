using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class AnimationClip // 애니메이션 상태 객체
{
    [SerializeField]
    private string _name = string.Empty;
    /// <summary>
    /// 상태의 이름
    /// </summary>
    public string name { get { return _name; } }
    [SerializeField]
    private float _fps = 0.1f;
    /// <summary>
    /// 상태의 애니메이션 fps
    /// </summary>
    public float fps { get { return _fps; } }
    [SerializeField]
    private Sprite[] _frames = null;
    /// <summary>
    /// 상태의 애니메이션 프레임
    /// </summary>
    public Sprite[] frames { get { return _frames; } }
    [SerializeField]
    private bool _isLoop;
    /// <summary>
    /// 상태의 무한루프 여부
    /// </summary>
    public bool isLoop { set { _isLoop = value; } get { return _isLoop; } }
}
public abstract class AnimatorBehaviour : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _target = null; // 타겟 애니메이션 객체
    public SpriteRenderer target { get { return _target; } }
    [SerializeField]
    private List<AnimationClip> _states = new List<AnimationClip>(); // 상태들
    AnimationClip _nowAnimation = null; // 현재 실행중인 애니메이션
    /// <summary>
    /// 현재 실행중인 애니메이션을 바꿈
    /// </summary>
    /// <param name="stateName">상태 이름</param>
    /// <returns>True = 성공, False = 실패</returns>
    public bool setState(string stateName)
    {
        for (int i = 0; i < _states.Count; i++)
        {
            if (_states[i].name.CompareTo(stateName).Equals(0))
            {
                _nowAnimation = _states[i];
                return true;
            }
        }
        Debug.Log("Can't found State!, StateName: " + stateName);
        return false;
    }

    public bool setState(int index)
    {
        if (index < 0 || index >= _states.Count)
        {
            return false;
        }
        _nowAnimation = _states[index];
        return true;
    }

    public IEnumerator runAnimation()
    {
        if (_nowAnimation == null)
        {
            Debug.Log("현재 상태가 null입니다!");
            yield break;
        }
        if (_nowAnimation.frames.Length.Equals(0))
        {
            Debug.Log("프레임이 없습니다!");
            yield break;
        }
        int spritesIndex = 0;
        while(true)
        {
            _target.sprite = _nowAnimation.frames[spritesIndex];
            if (spritesIndex + 1 < _nowAnimation.frames.Length)
                ++spritesIndex;
            else
            {
                if (_nowAnimation.isLoop)
                    spritesIndex = 0;
                else
                    break;
            }
            yield return new WaitForSeconds(_nowAnimation.fps); 
        }
    }
    public IEnumerator runAnimation(GameObject callbackTarget, string callbackName)
    {
        if (_nowAnimation == null)
        {
            Debug.Log("현재 상태가 null입니다!");
            yield break;
        }
        if (_nowAnimation.frames.Length.Equals(0))
        {
            Debug.Log("프레임이 없습니다!");
            yield break;
        }
        int spritesIndex = 0;
        while (true)
        {
            _target.sprite = _nowAnimation.frames[spritesIndex];
            if (spritesIndex + 1 < _nowAnimation.frames.Length)
                ++spritesIndex;
            else
            {
                if (_nowAnimation.isLoop)
                    spritesIndex = 0;
                else
                    break;
            }
            yield return new WaitForSeconds(_nowAnimation.fps);
        }
        callbackTarget.SendMessage(callbackName);
    }
}