using UnityEditor;
using UnityEngine;

public class BuildScript
{
    [MenuItem("Build/Build Windows")]
    public static void BuildWindows()
    {
        string[] scenes =new[] {"Assets/Scenes/MainScene.unity"};
        string buildPath = Path.Combine(Application.dataPath, "../Builds/Windows/GameName.exe");
        BuildPipeline.BuildPlayer(scenes, buildPath, BuildTarget.StandaloneWindows64, BuildOptions.None);
        Debug.Log("Windows build completed: " + buildPath);
    }
}