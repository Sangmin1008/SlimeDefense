using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIClickScaleTweenHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private RectTransform rectTransform;
    
    [SerializeField] private float duration;
    [SerializeField] private Vector2 targetScale;
    [SerializeField] private AnimationCurve easeOutCurve;
    
    
    private ProgressTweener clickTweener;

    private void Awake()
    {
        clickTweener = new ProgressTweener();

        rectTransform = GetComponent<RectTransform>();
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        clickTweener
            .SetCurve(easeOutCurve)
            .Play(
                onUpdate: ratio => rectTransform.localScale = Vector3.LerpUnclamped(Vector3.one, targetScale, ratio), 
                duration: duration,
                ignoreTimeScale: true
            );
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        clickTweener
            .SetCurve(easeOutCurve)
            .Play(
                onUpdate: ratio => rectTransform.localScale = Vector3.LerpUnclamped(targetScale, Vector3.one, ratio), 
                duration: 0.05f,
                ignoreTimeScale: true
            );
    }
    
    private void OnDestroy()
    {
        clickTweener?.Stop();
    }
}