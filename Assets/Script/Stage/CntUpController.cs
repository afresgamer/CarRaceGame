using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class CntUpController : MonoBehaviour
{
    private TextMeshProUGUI cntUpText;
    private CanvasGroup canvasGroup; 

    void Start()
    {
        cntUpText = GetComponent<TextMeshProUGUI>();
        canvasGroup = GetComponent<CanvasGroup>();

        var sequence = DOTween.Sequence();

        sequence
            .OnStart(() => UpdateText("3"))
            .Append(FadeOutText())
            .AppendCallback(() => UpdateText("2"))
            .Append(FadeOutText())
            .AppendCallback(() => UpdateText("1"))
            .Append(FadeOutText())
            .AppendCallback(() => UpdateText("START"))
            .Append(DelayScale())
            .OnComplete(() => GameStart());
        
    }

    //テキストの更新
    private void UpdateText(string text)
    {
        canvasGroup.alpha = 1.0f;
        cntUpText.text = text;
    }

    //フェードアウトさせる
    private Tween FadeOutText()
    {
        return canvasGroup.DOFade(0, 1.0f);
    }

    private Tween DelayScale()
    {
        return cntUpText.gameObject.GetComponent<RectTransform>().DOScale(Vector3.one * 2, 1.0f);
    }

    private void GameStart()
    {
        CarGameManager.Instance.IsGameStart = true;
        canvasGroup.alpha = 1.0f;
        cntUpText.gameObject.SetActive(false);
    }
}
