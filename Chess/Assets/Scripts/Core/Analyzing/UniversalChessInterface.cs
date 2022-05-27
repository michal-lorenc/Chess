using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;
using Debug = UnityEngine.Debug;

/// <summary>
/// Class that creates and communicates with Stockfish 15 process
/// </summary>
public class UniversalChessInterface
{
    private Process chessEngineProcess;

    public EventHandler<string> OnEvaluationChanged;
    public EventHandler<string> OnBestMoveFound;

    public UniversalChessInterface ()
    {
        StartChessEngine();
    }

    public void StartChessEngine ()
    {
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
        SendCommand($"position fen {fen}");
        SendCommand($"go depth {depth}");
    }

    private void OnOutputReceived (string outputString)
    {
        if (string.IsNullOrWhiteSpace(outputString))
            return;

        if (outputString.Contains("Final evaluation"))
        {
            string evaluation = outputString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[2];
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

    private void OnErrorReceived (string errorString)
    {
        Debug.Log("UCI Error: " + errorString);
    }
}
