using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class BuildScript
{
    public enum BuildType
    {
        Client,
        Server
    }

    public enum Platform
    {
        Windows,
        Mac,
        Linux
    }

    static string[] scenePaths = { "Assets/Scenes/Current/00_ServerScene.unity", "Assets/Scenes/Current/01_MainMenuScene.unity", "Assets/Scenes/Current/02_OnlineMenuScene.unity", "Assets/Scenes/Current/03_GameScene.unity", "Assets/Scenes/Current/04_TutorialScene.unity", "Assets/Scenes/Current/05_LoreScene.unity", "Assets/Scenes/Current/06_CreditsScene.unity" };

    public static string[] ConfigureScenes(BuildType buildType)
    {
        var scenes = new List<string>();
        if (buildType == BuildType.Server)
        {
            scenes.Add(scenePaths[0]);
        } else
        {
            for (int i = 1; i < scenePaths.Length; i++)
            {
                scenes.Add(scenePaths[i]);
            }
        }

        return scenes.ToArray();
    }

    [MenuItem("Build/Server Build (Windows)")]
    public static void PerformServerBuildWindows()
    {
        PerformBuild(BuildType.Server, Platform.Windows);
    }

    [MenuItem("Build/Client Build (Windows)")]
    public static void PerformClientBuildWindows()
    {
        PerformBuild(BuildType.Client, Platform.Windows);
    }

    [MenuItem("Build/All Builds (Windows)")]
    public static void PerformAllBuildsWindows()
    {
        PerformServerBuildWindows();
        PerformClientBuildWindows();
    }

    [MenuItem("Build/Server Build (Mac iOS)")]
    public static void PerformServerBuildMac()
    {
        PerformBuild(BuildType.Server, Platform.Mac);
    }

    [MenuItem("Build/Client Build (Mac iOS)")]
    public static void PerformClientBuildMac()
    {
        PerformBuild(BuildType.Client, Platform.Mac);
    }

    [MenuItem("Build/All Builds (Mac iOS)")]
    public static void PerformAllBuildsMac()
    {
        PerformServerBuildMac();
        PerformClientBuildMac();
    }

    [MenuItem("Build/Server Build (Linux)")]
    public static void PerformServerBuildLinux()
    {
        PerformBuild(BuildType.Server, Platform.Linux);
    }

    [MenuItem("Build/Client Build (Linux)")]
    public static void PerformClientBuildLinux()
    {
        PerformBuild(BuildType.Client, Platform.Linux);
    }

    [MenuItem("Build/All Builds (Linux)")]
    public static void PerformAllBuildsLinux()
    {
        PerformServerBuildLinux();
        PerformClientBuildLinux();
    }

    private static void PerformBuild(BuildType buildType, Platform platform)
    {
        Debug.Log("Performing build: " + buildType);
        string buildPath = "Builds/" + platform.ToString() + "/" + buildType.ToString() +"/Skyrats.exe";

        BuildTarget buildTarget = BuildTarget.StandaloneWindows64;
        if (platform == Platform.Mac)
            buildTarget = BuildTarget.StandaloneOSX;
        else if (platform == Platform.Linux)
            buildTarget = BuildTarget.StandaloneLinux64;

        BuildReport report = BuildPipeline.BuildPlayer(ConfigureScenes(buildType), buildPath, buildTarget, BuildOptions.None);

        // Check if the build succeeded
        if (report.summary.result == BuildResult.Succeeded)
        {
            Debug.Log(buildType + " Build succeeded for platform " + platform + "!");
        }
        else
        {
            Debug.LogError(buildType + " Build for platform " + platform + "failed.");
        }
    }
}