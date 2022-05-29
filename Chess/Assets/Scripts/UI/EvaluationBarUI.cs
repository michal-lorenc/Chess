using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class EvaluationBarUI : MonoBehaviour
{
    [SerializeField] private Image bar;
    [SerializeField] private TextMeshProUGUI evaluationText;
    [SerializeField] private TextMeshProUGUI depthText;

    private void Start ()
    {
        Chess.Singleton.UCI.OnEvaluationChanged += (s, e) => OnEvaluationChanged(e);
        Chess.Singleton.UCI.OnDepthChanged += (s, e) => OnDepthChanged(e);
    }

    private void OnDepthChanged (string depth)
    {
        depthText.text = "Depth: " + depth;
    }

    public void SwitchDisplay (PieceColor color)
    {
        bar.fillOrigin = color == PieceColor.WHITE ? 0 : 1;
    }

    private void OnEvaluationChanged (string dynamicEvaluation)
    {
        if (dynamicEvaluation[0] == 'M')
        {
            evaluationText.text = dynamicEvaluation;
        }
        else
        {
            float evalValue = (float)Convert.ToDouble(dynamicEvaluation);
            evalValue /= 100;

            float evalValueForBar = EvaluateFunction(evalValue) + 0.5f;
            UpdateBar(evalValueForBar);

            if (evalValue >= 0)
                evaluationText.text = "+" + evalValue.ToString("0.00");
            else
                evaluationText.text = evalValue.ToString("0.00");
        }
    }

    private void UpdateBar (float fillAmount, float animationDuration = 0.5f)
    {
        StopAllCoroutines();
        StartCoroutine(AnimateBar(fillAmount, animationDuration));
    }

    private IEnumerator AnimateBar (float fillAmount, float animationDuration = 0.5f)
    {
        float startingFillAmount = bar.fillAmount;
        float progress = 0;

        while (progress < 1)
        {
            float currentFillAmount = Mathf.Lerp(startingFillAmount, fillAmount, progress);
            bar.fillAmount = currentFillAmount;
            progress += 1 / animationDuration * Time.deltaTime;
            yield return null;
        }

        bar.fillAmount = fillAmount;
    }

    private float EvaluateFunction (float x)
    {
        float result = x / 10f;
        result = Mathf.Clamp(result, 0f, 0.45f);

        return result;
    }
}
