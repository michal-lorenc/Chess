using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Threading.Tasks;
using System;
using Debug = UnityEngine.Debug;

/// <summary>
/// Class that creates and communicates with Stockfish 15 process
/// </summary>
public class UniversalChessInterface
{
    private Process chessEngineProcess;
    private bool isProcessRunning = false;

    public EventHandler<string> OnEvaluationChanged;
    public EventHandler<string> OnDepthChanged;
    public EventHandler<string> OnBestMoveFound;

    private Queue<string> outputFromChessEngineProcess = new Queue<string>();

    public UniversalChessInterface ()
    {
        StartChessEngine();
    }

    public void StartChessEngine ()
    {
        isProcessRunning = true;
        _ = PushEventsToMainThread();

        ProcessStartInfo si = new ProcessStartInfo()
        {
            FileName = GetStockfishLocation(),
            UseShellExecute = false,
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Hidden,
            RedirectStandardError = true,
            RedirectStandardInput = true,
            RedirectStandardOutput = true
        };

        chessEngineProcess = new Process();
        chessEngineProcess.StartInfo = si;
        chessEngineProcess.OutputDataReceived += (s, e) => OnOutputReceived(e.Data);
        chessEngineProcess.ErrorDataReceived += (s, e) => OnErrorReceived(e.Data);
        chessEngineProcess.Start();
        chessEngineProcess.BeginErrorReadLine();
        chessEngineProcess.BeginOutputReadLine();

        SendCommand("ucinewgame");
        SendCommand("isready");
    }

    public void StopChessEngine ()
    {
        isProcessRunning = false;
        chessEngineProcess.Kill();
    }

    private string GetStockfishLocation ()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsPlayer:
                return Application.dataPath + "/Stockfish/Windows/stockfish_15_x64_popcnt.exe";
            case RuntimePlatform.LinuxEditor:
            case RuntimePlatform.LinuxPlayer:
                return Application.dataPath + "/Stockfish/Linux/stockfish_15_x64";
            case RuntimePlatform.Android:
                return Application.dataPath + "/Stockfish/Android/stockfish.android.armv7";
            default:
                throw new PlatformNotSupportedException();
        }
    }

    public void SendCommand (string command)
    {
        chessEngineProcess.StandardInput.WriteLine(command);
        chessEngineProcess.StandardInput.Flush();
    }

    public void CalculatePosition (string fen, int depth = 20)
    {
        SendCommand("stop");
        SendCommand($"position fen {fen}");
        SendCommand($"go depth {depth}");
    }

    private void OnOutputReceived (string outputString)
    {
        if (string.IsNullOrWhiteSpace(outputString))
            return;

        outputFromChessEngineProcess.Enqueue(outputString);
    }

    private void OnErrorReceived (string errorString)
    {
        Debug.Log("UCI Error: " + errorString);
    }

    /// <summary>
    /// This method runs on Unity Main Thread.
    /// It makes sure that all outputs are processed on main unity thread.
    /// </summary>
    private async Task PushEventsToMainThread ()
    {
        while (isProcessRunning)
        {
            if (outputFromChessEngineProcess.Count > 0)
            {
                string newOutput = outputFromChessEngineProcess.Dequeue();
                ProcessOutput(newOutput);
            }

            await Task.Yield();
        }
    }

    /// <summary>
    /// Process output from chess engine and fire events on main thread.
    /// </summary>
    /// <param name="outputString"></param>
    private void ProcessOutput (string outputString)
    {
        string[] outputSplit = outputString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        if (outputString.StartsWith("info depth"))
        {
            OnDepthChanged?.Invoke(this, outputSplit[2]);

            int indexOfMate = -1, indexOfCP = -1;
            indexOfMate = Array.IndexOf(outputSplit, "mate");
            indexOfCP = Array.IndexOf(outputSplit, "cp");

            if (indexOfMate > -1)
            {
                OnEvaluationChanged?.Invoke(this, "M" + outputSplit[indexOfMate + 1]);
            }
            else if (indexOfCP > -1)
            {
                OnEvaluationChanged?.Invoke(this, outputSplit[indexOfCP + 1]);
            }
        }
        else if (outputString.Contains("Final evaluation"))
        {
            string evaluation = outputSplit[2];
            OnEvaluationChanged?.Invoke(this, evaluation);
            Debug.Log("Evaluation: " + evaluation);
        }
        else if (outputString.Contains("bestmove"))
        {
            OnBestMoveFound?.Invoke(this, outputString);
            Debug.Log(outputString);
        }
        else
        {
            Debug.Log(outputString);
        }
    }
}
