using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AlphabetUI : MonoBehaviour
{
    public RectTransform CapitalAlphabetRect;
    public RectTransform SmallAlphabetRect;
    public RectTransform CapitalContainerRect;
    public RectTransform SmallContainerRect;
    Sequence SEQ;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void AnimationSequence()
    {
        SmallAlphabetRect.localScale = Vector3.zero;
        SmallAlphabetRect.anchoredPosition = new Vector2((CapitalAlphabetRect.rect.width / 2 + 20), 0);

        CapitalContainerRect.gameObject.SetActive(true);
        SmallContainerRect.gameObject.SetActive(true);

        CapitalAlphabetRect.localScale = Vector3.zero;
        CapitalAlphabetRect.anchoredPosition = new Vector2((-CapitalAlphabetRect.rect.width / 2 + 20), 0);

        SEQ.Kill();

        SEQ = DOTween.Sequence();
        SEQ.Append(CapitalAlphabetRect.DOScale(Vector3.one, 0.75f).SetEase(Ease.OutBack))
            .Join(SmallAlphabetRect.DOScale(Vector3.one, 0.75f).SetEase(Ease.OutBack))
            .Join(CapitalContainerRect.DOScale(Vector3.one,0.5f))
            .Join(SmallContainerRect.DOScale(Vector3.one,0.5f))
            .Append(CapitalAlphabetRect.DOScale(Vector3.one, 0.75f).SetDelay(0.5f))
            .Join(SmallAlphabetRect.DOScale(Vector3.one , 0.75f))
            .Join(CapitalAlphabetRect.DOMove(CapitalContainerRect.position,0.75f))
            .Join(SmallAlphabetRect.DOMove(SmallContainerRect.position, 0.75f));

    }


    public void HideUI()
    {
        CapitalContainerRect.DOScale(Vector3.zero, 0.5f);
        SmallContainerRect.DOScale(Vector3.zero, 0.5f);
        CapitalAlphabetRect.localScale = Vector3.zero;
        SmallAlphabetRect.localScale = Vector3.zero;
    }


    void Update()
    {
        
    }
}
