using System.Collections;
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
        ChessUI.Singleton.Chess.UCI.OnEvaluationChanged += (s, e) => OnEvaluationChanged(e);
        ChessUI.Singleton.Chess.UCI.OnDepthChanged += (s, e) => OnDepthChanged(e);
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
        if (dynamicEvaluation[1] == 'M')
        {
            evaluationText.text = dynamicEvaluation;

            if (dynamicEvaluation[0] == '+')
                UpdateBar(1f);
            else
                UpdateBar(0f);
        }
        else
        {
            float evalValue = (float)Convert.ToDouble(dynamicEvaluation);
            evalValue /= 100;

            float evalValueForBar = 0.5f + EvaluateFunction(evalValue);
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
        result = Mathf.Clamp(result, -0.45f, 0.45f);

        return result;
    }
}
