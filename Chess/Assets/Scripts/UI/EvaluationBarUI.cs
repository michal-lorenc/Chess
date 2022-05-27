using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EvaluationBarUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("XD: " + EvaluateFunction(-255 / 100));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStockfishMsg (string eventString)
    {
        if (!eventString.StartsWith("info depth"))
            return;

        string messageEvalType;
        string[] message = eventString.Split(' ');

        int indexOfMate = -1, indexOfCP = -1;
        indexOfMate = Array.IndexOf(message, "mate");
        indexOfCP = Array.IndexOf(message, "cp");

        if (indexOfMate > -1)
        {
            messageEvalType = $"M{message[indexOfMate + 1]}";
        }
        else
        {
            messageEvalType = $"{message[indexOfCP + 1]}";
        }

        string evaluation = ConvertEvaluation(messageEvalType);


        if (evaluation.StartsWith("M"))
        {

        }
        else
        {
            float evaluationFloat = float.Parse(evaluation);
            evaluationFloat = evaluationFloat / 100;

            float finalVal = EvaluateFunction(evaluationFloat);
            Debug.Log("XD Result: " + finalVal);

        }
    }

    private string ConvertEvaluation (string messageEvalType)
    {
        return messageEvalType;
    }

    private float EvaluateFunction (float x)
    {
        if (x >= -0.01 && x <= 0.01f)
        {
            return 0;
        }
        else if (x < 7)
        {
            return -(0.322495f * (float)Math.Pow(x, 2)) + 7.26599f * x + 4.11834f;
        }
        else
        {
            return (8 * x) / 145 + 5881 / 145;
        }
    }

    public void UpdateBar (string eval)
    {

    }
}
