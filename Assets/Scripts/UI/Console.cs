using System.Collections.Generic;
using UnityEngine;

public class Console : MonoBehaviour
{
    private List<string> logMessages = new List<string>();
    private Vector2 scrollPosition = Vector2.zero;

    [Header("Log Type")]
    public bool log = false;
    public bool warning = false;
    public bool error = false;
    public bool exception = false;

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        // Filter for not display warnings
        if (type == LogType.Log)
        {
            if (log == true)
            {
                logMessages.Add(logString);
            }
        } else if (type == LogType.Warning)
        {
            if (warning == true)
            {
                logMessages.Add(logString);
            }
        } else if (type == LogType.Error)
        {
            if (error == true)
            {
                logMessages.Add(logString);
            }
        } else if (type == LogType.Exception)
        {
            if (exception == true)
            {
                logMessages.Add(logString);
            }
        }

        // Scroll to the bottom when a new message is added
        scrollPosition.y = Mathf.Infinity;
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, Screen.width - 20, Screen.height - 20));
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        foreach (var message in logMessages)
        {
            GUILayout.Label(message);
        }

        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }
}