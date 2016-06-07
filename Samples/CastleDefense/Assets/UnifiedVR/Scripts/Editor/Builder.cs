using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

    public class Builder
    {
        static private string APP_NAME = PlayerSettings.productName.Trim();

        static private string buildKey = "FW";
        static private string buildNumber = "1";
        static private string buildPlatEnv = "development";
        static private string buildVerCode = "1";
        static private string buildVerStr = "1.0.0";

    #region Menu Items

    static private bool ShowConfirmDialog()
    {
        return EditorUtility.DisplayDialog(
            string.Format("Building {0}", APP_NAME),
            "Are you sure? This will take a while.",
            "OK",
            "Cancel"
        );
    }

    [MenuItem("Build/Android", false, 0)]
    static public void BuildAndroidFromMenu()
    {
        bool proceed = ShowConfirmDialog();
        if (!proceed) return;

        Build(BuildTarget.Android);
    }

    [MenuItem("Build/iOS", false, 1)]
    static public void BuildiOSFromMenu()
    {
        bool proceed = ShowConfirmDialog();
        if (!proceed) return;

        Build(BuildTarget.iOS);
    }

    [MenuItem("Build/Win32", false, 3)]
    static public void BuildWin32FromMenu()
    {
        bool proceed = ShowConfirmDialog();
        if (!proceed) return;

        Build(BuildTarget.StandaloneWindows);
    }


    [MenuItem("Build/OSX", false, 4)]
    static public void BuildOSXFromMenu()
    {
        bool proceed = ShowConfirmDialog();
        if (!proceed) return;

        Build(BuildTarget.StandaloneOSXIntel);
    }
    #endregion


    #region Build Platforms
    static public void BuildAndroid()
        {
            ReadEnvVariables();
            Build(BuildTarget.Android);
        }

        static public void BuildWin32()
        {
            ReadEnvVariables();
            Build(BuildTarget.StandaloneWindows);
        }

        static public void BuildiOS()
        {
            ReadEnvVariables();
            Build(BuildTarget.iOS);
        }

        static public void BuildOSX()
        {
            ReadEnvVariables();
            Build(BuildTarget.StandaloneOSXIntel);
        }
        #endregion

        #region Build Generic
        static private void SetDefineSymbols(BuildTarget buildTarget)
        {
            BuildTargetGroup group = BuildTargetGroup.Android;
            if (buildTarget == BuildTarget.StandaloneWindows ||
                buildTarget == BuildTarget.StandaloneOSXIntel)
            {
                group = BuildTargetGroup.Standalone;
            }
            else if (buildTarget == BuildTarget.iOS)
            {
                group = BuildTargetGroup.iOS;
            }

            // do nothing for now
            if (group == BuildTargetGroup.Android) { }
        }

        static private void Build(BuildTarget buildTarget)
        {

            // Switch build target 
            EditorUserBuildSettings.SwitchActiveBuildTarget(buildTarget);

            string buildName = APP_NAME;
            switch (buildTarget)
            {
                case BuildTarget.Android:
                    {
                        // SetAndroidProperties();
                        buildName = buildName + ".apk";
                        break;
                    }

                case BuildTarget.StandaloneWindows:
                    {
                        buildName = buildName + ".exe";
                        break;
                    }

                case BuildTarget.StandaloneOSXIntel:
                    {
                        buildName = buildName + ".app";
                        break;
                    }
            }


            // Make sure the output directory exists
            string buildDirectory = Path.Combine("Builds", buildTarget.ToString());
            string buildPath = Path.Combine(buildDirectory, buildName);
            System.IO.Directory.CreateDirectory(buildDirectory);

            // Grab the scenes
            string[] scenes = GetEnabledEditorScenes();

            // Set build options
            BuildOptions options = BuildOptions.None;
            options |= BuildOptions.ShowBuiltPlayer;
        //options |= BuildOptions.ConnectWithProfiler;
        //options |= BuildOptions.Development;

        // Debug.Log("scenes: " + scenes[0].ToString() + ", buildir: " + buildDirectory + ", buildpath: " + buildPath + ", target: " + buildTarget.ToString());


           string result = BuildPipeline.BuildPlayer(scenes, buildPath, buildTarget, options);


            // Clean up
            switch (buildTarget)
            {
                case BuildTarget.Android: CleanupAndroidProperties(); break;
            }

        }
        #endregion

        #region Helper functions
        static private string[] GetEnabledEditorScenes()
        {
            EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
            List<string> sceneNames = new List<string>();
            foreach (EditorBuildSettingsScene scene in scenes)
            {
                if (!scene.enabled) continue;
                sceneNames.Add(scene.path);
            }
            return sceneNames.ToArray();
        }


        static private void ReadEnvVariables()
        {
            string temp = "";
            temp = Environment.GetEnvironmentVariable("bamboo_buildKey");
            if (string.IsNullOrEmpty(temp) == false) buildKey = temp;

            temp = Environment.GetEnvironmentVariable("bamboo_buildNumber");
            if (string.IsNullOrEmpty(temp) == false) buildNumber = temp;

            temp = Environment.GetEnvironmentVariable("bamboo_PlatformEnv");
            if (string.IsNullOrEmpty(temp) == false) buildPlatEnv = temp;

            temp = Environment.GetEnvironmentVariable("bamboo_VersionCode");
            if (string.IsNullOrEmpty(temp) == false) buildVerCode = temp;

            temp = Environment.GetEnvironmentVariable("bamboo_VersionString");
            if (string.IsNullOrEmpty(temp) == false) buildVerStr = temp;


            // version code
            int versionCode = Convert.ToInt32(buildVerCode);
            PlayerSettings.iOS.buildNumber = versionCode.ToString();
            PlayerSettings.Android.bundleVersionCode = versionCode;

            // version string
            PlayerSettings.bundleVersion = buildVerStr;

            AssetDatabase.SaveAssets();
        }
        #endregion

        #region Android specific
        static private void SetAndroidProperties()
        {
            PlayerSettings.Android.keyaliasName = "unifiedvr";
            PlayerSettings.Android.keyaliasPass = "unifiedvr";
            PlayerSettings.Android.keystorePass = "unifiedvr";


            EditorUserBuildSettings.androidBuildSubtarget = MobileTextureSubtarget.ETC2;
        }
        static private void CleanupAndroidProperties()
        {
            PlayerSettings.Android.keyaliasName = "";
            PlayerSettings.Android.keyaliasPass = "";
            PlayerSettings.Android.keystorePass = "";
        }
        #endregion
    }
