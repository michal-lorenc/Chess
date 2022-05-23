using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

/// <summary>
/// Class that creates and communicates with Stockfish 15 process
/// </summary>
public class UCI : MonoBehaviour
{
    private Process chessEngineProcess;

    void Start()
    {
      /*  StartChessEngine();

        SendCommand("uci");
        SendCommand("isready");
        SendCommand("ucinewgame");
        SendCommand("position startpos moves e2e4 e7e5");
        SendCommand("go infinite"); */
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SendCommand("stop");
    }

    public void StartChessEngine ()
    {
        ProcessStartInfo si = new ProcessStartInfo()
        {
            FileName = Application.dataPath + "/Stockfish/stockfish_15_x64_popcnt.exe",
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
    }

    public void StopChessEngine()
    {
        chessEngineProcess.Kill();
    }

    public void SendCommand (string command)
    {
        chessEngineProcess.StandardInput.WriteLine(command);
        chessEngineProcess.StandardInput.Flush();
    }

    private void OnOutputReceived (string outputString)
    {
        UnityEngine.Debug.Log(outputString);
    }

    private void OnErrorReceived (string errorString)
    {
        UnityEngine.Debug.Log(errorString);
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
            StopChessEngine();
    }

}
