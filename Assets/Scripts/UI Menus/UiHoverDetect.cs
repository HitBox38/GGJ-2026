using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(DOTweenAnimation))]
public class HoverDetect : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    private DOTweenAnimation _tweenAnimation;

    private bool _isHovering;
    
    private void Awake()
    {
        _tweenAnimation = GetComponent<DOTweenAnimation>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isHovering) return;
        _tweenAnimation.DORestart();
        _tweenAnimation.DOPlay();
        _isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_isHovering) return;
        _tweenAnimation.DOPlayBackwards();
        _tweenAnimation.DORestart();
        _isHovering = false;
    }
}