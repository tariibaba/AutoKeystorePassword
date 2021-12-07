using System.IO;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class AutoKeystorePasswordWindow : EditorWindow
{
    private static string prevFilePath;
    private static string filePath;

    [MenuItem("Window/Auto Keystore Password")]
    public static void ShowWindow()
    {
        filePath = AutoKeystorePassword.GetFilePath();
        GetWindow(typeof(AutoKeystorePasswordWindow));
    }

    private void OnGUI()
    {
        GUILayout.Label("Keystore Password File");
        filePath = GUILayout.TextField(filePath);
        if (GUILayout.Button("Change"))
        {
            var path = EditorUtility.OpenFilePanel("Choose Keystore Password File", "", "");
            if (path.Length > 0) filePath = path;
            AutoKeystorePassword.UpdateFilePath(filePath);
        };
    }

    private void OnInspectorUpdate()
    {
        Repaint();
    }
}

[InitializeOnLoad]
public class AutoKeystorePassword
{
    private const string PlayerPrefKey = "AutoKeystorePassword.FilePath";

    static AutoKeystorePassword()
    {
        var filePath = GetFilePath();
        UpdatePassword(filePath);
    }

    public static void UpdateFilePath(string filePath)
    {
        PlayerPrefs.SetString(PlayerPrefKey, filePath);
        UpdatePassword(filePath);
    }

    public static string GetFilePath()
    {
        return PlayerPrefs.GetString(PlayerPrefKey, "");
    }

    public static void UpdatePassword(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath)) return;
        try
        {
            var content = File.ReadAllText(filePath);
            PlayerSettings.keystorePass = content;
            PlayerSettings.keyaliasPass = content;
            Debug.Log("AutoKeystorePassword: keystore password updated successfully");
        }
        catch (FileNotFoundException)
        {
            Debug.LogError("AutoKeystorePassword: The keystore password file does not exist.");
        }
    }
}
#endif