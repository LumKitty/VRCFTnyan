using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.MemoryMappedFiles;
using System.Security.Cryptography;
using UnityEngine;
using VNyanInterface;

namespace VRCFTnyanDLL {
    public class VRCFTnyan : MonoBehaviour, IVNyanPluginManifest, IButtonClickedHandler {
        public string PluginName { get; } = "VRCFTnyan";
        public string Version { get; } = "0.1-beta";
        public string Title { get; } = "VRC Face Tracking for VNyan";
        public string Author { get; } = "LumKitty";
        public string Website { get; } = "https://lum.uk";

        private const string SettingsFileName = "VRCFTnyan.cfg";

        private const string MMFname = "uk.lum.VRCFTnyan.0.1";
        private const int MMFSize = sizeof(float) * 52;
        private static long MMFPos = 0;
        private static MemoryMappedFile mmf;
        private static MemoryMappedViewAccessor mmfAccess;
        private static GameObject objVRCFTnyan;

        public static bool EnableEyes = true;
        public static bool EnableMouth = true;
        public static bool Active = false;
        public static bool FirstStartup = false;
        public static Process ExternalExe;

        private static void ErrorHandler(Exception e) {
            VNyanInterface.VNyanInterface.VNyanParameter.setVNyanParameterString("_lum_liv_err", e.ToString());
            UnityEngine.Debug.Log("[VRCFT] ERR: " + e.ToString());
        }

        private static void Log(string message) {
            UnityEngine.Debug.Log("[VRCFT] " + message);
        }

        public void InitializePlugin() {
            try {
                LoadPluginSettings();
                VNyanInterface.VNyanInterface.VNyanUI.registerPluginButton("VRC Face Tracking (alpha)", this);
                Log("Creating file");
                mmf = MemoryMappedFile.CreateOrOpen(MMFname, MMFSize);
                Log("Creating accessor");
                mmfAccess = mmf.CreateViewAccessor(0, MMFSize, MemoryMappedFileAccess.Read);
                Log("VRCFTnyan " + Version + " started");
                Log("Spawning gameobject: objVRCFTnyan");
                objVRCFTnyan = new GameObject("objVRCFTnyan", typeof(VRCFTnyan));
                objVRCFTnyan.SetActive(false);
            } catch (Exception e) {
                ErrorHandler(e);
            }
        }

        private void LoadPluginSettings() {
            try {
                // Get settings in dictionary
                Log("Reading settings from: " + SettingsFileName);
                Dictionary<string, string> settings = VNyanInterface.VNyanInterface.VNyanSettings.loadSettings(SettingsFileName);
                bool SettingMissing = false;
                if (settings != null) {
                    // Read string value
                    string tempSetting;

                    if (settings.TryGetValue("EnableEyes", out tempSetting)) {
                        if (bool.Parse(tempSetting)) {
                            EnableEyes = true;
                            Log("Eyes will be tracked from VRCFT");
                        } else {
                            EnableEyes = false;
                            Log("Eyes will not be tracked from VRCFT");
                        }
                    } else {
                        Log("EnableEyes setting missing. Defaulting to enabled");
                        EnableEyes = true;
                        SettingMissing = true;
                    }
                    if (settings.TryGetValue("EnableMouth", out tempSetting)) {
                        if (bool.Parse(tempSetting)) {
                            EnableMouth = true;
                            Log("Mouth will be tracked from VRCFT");
                        } else {
                            EnableMouth = false;
                            Log("Mouth will not be tracked from VRCFT");
                        }
                    } else {
                        Log("EnableMouth setting missing. Defaulting to enabled");
                        EnableMouth = true;
                        SettingMissing = true;
                    }

                } else {
                    Log("No settings file detected, using defaults");
                    SettingMissing = true;
                }
                if (SettingMissing) {
                    Log("Writing settings file");
                    SavePluginSettings();
                }
            } catch (Exception e) {
                ErrorHandler(e);
            }
        }

        private void SavePluginSettings() {
            Dictionary<string, string> settings = new Dictionary<string, string>();
            settings["EnableEyes"] = EnableEyes.ToString();
            settings["EnableMouth"] = EnableMouth.ToString();

            VNyanInterface.VNyanInterface.VNyanSettings.saveSettings(SettingsFileName, settings);
        }

        public void LaunchEXEHelper() {
            ExternalExe = new Process();
            string Result = "";
            ExternalExe.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "\\Items\\Assemblies\\VRCFTnyan.exe";
            Log("Looking for external EXE at: " + ExternalExe.StartInfo.FileName);
            ExternalExe.StartInfo.Arguments = "";
            ExternalExe.StartInfo.UseShellExecute = false;
            ExternalExe.StartInfo.RedirectStandardInput = false;
            ExternalExe.StartInfo.RedirectStandardOutput = false;
            ExternalExe.StartInfo.RedirectStandardError = false;
            ExternalExe.StartInfo.CreateNoWindow = false;
            ExternalExe.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
            ExternalExe.EnableRaisingEvents = true;
            Log("Start process");
            ExternalExe.Start();
        }

        public void pluginButtonClicked() {
            Active = !Active;
            objVRCFTnyan.SetActive(Active);
            if (Active && !FirstStartup) {
                LaunchEXEHelper();
            }
        }

        public void OnApplicationQuit() {
            ExternalExe.Kill();
        }

        private static void UpdateVNyan(string BlendshapeName) {
            float Value;
            mmfAccess.Read<float>(MMFPos, out Value);
            VNyanInterface.VNyanInterface.VNyanAvatar.setBlendshapeOverride(BlendshapeName, Value);
            MMFPos += sizeof(float);
            if (Value != 0) {
                Log(MMFPos.ToString().PadRight(4,' ')+BlendshapeName.PadRight(20, ' ') + ": " + Value.ToString());
            }
        }
        
        public void Update() {
            try {
                MMFPos = 0;
                //Log("Update called");
                if (EnableEyes) {
                    MMFPos = 0;
                    UpdateVNyan("BrowDownLeft");
                    UpdateVNyan("BrowDownRight");
                    UpdateVNyan("BrowInnerUp");
                    UpdateVNyan("BrowOuterUpLeft");
                    UpdateVNyan("BrowOuterUpRight");
                    UpdateVNyan("CheekPuff");
                    UpdateVNyan("CheekSquintLeft");
                    UpdateVNyan("CheekSquintRight");
                    UpdateVNyan("EyeBlinkLeft");
                    UpdateVNyan("EyeBlinkRight");
                    UpdateVNyan("EyeLookDownLeft");
                    UpdateVNyan("EyeLookDownRight");
                    UpdateVNyan("EyeLookInLeft"); // LeftEye LookRight
                    UpdateVNyan("EyeLookInRight"); // RightEye LookLeft
                    UpdateVNyan("EyeLookOutLeft"); // LeftEye LookLeft
                    UpdateVNyan("EyeLookOutRight"); // RightEye LookRight
                    UpdateVNyan("EyeLookUpLeft");
                    UpdateVNyan("EyeLookUpRight");
                    UpdateVNyan("EyeSquintLeft");
                    UpdateVNyan("EyeSquintRight");
                    UpdateVNyan("EyeWideLeft");
                    UpdateVNyan("EyeWideRight");
                } else {
                    MMFPos = 22 * sizeof(float);
                }
                if (EnableMouth) { 
                    UpdateVNyan("JawForward");
                    UpdateVNyan("JawLeft");
                    UpdateVNyan("JawOpen");
                    UpdateVNyan("JawRight");
                    UpdateVNyan("MouthClose");
                    UpdateVNyan("MouthDimpleLeft");
                    UpdateVNyan("MouthDimpleRight");
                    UpdateVNyan("MouthFrownLeft");
                    UpdateVNyan("MouthFrownRight");
                    UpdateVNyan("MouthFunnel");
                    UpdateVNyan("MouthLeft");
                    UpdateVNyan("MouthLowerDownLeft");
                    UpdateVNyan("MouthLowerDownRight");
                    UpdateVNyan("MouthPressLeft");
                    UpdateVNyan("MouthPressRight");
                    UpdateVNyan("MouthPucker");
                    UpdateVNyan("MouthRight");
                    UpdateVNyan("MouthRollUpper");
                    UpdateVNyan("MouthRollLower");
                    UpdateVNyan("MouthShrugUpper");
                    UpdateVNyan("MouthShrugLower");
                    UpdateVNyan("MouthSmileLeft");
                    UpdateVNyan("MouthSmileRight");
                    UpdateVNyan("MouthStretchLeft");
                    UpdateVNyan("MouthStretchRight");
                    UpdateVNyan("MouthUpperUpLeft");
                    UpdateVNyan("MouthUpperUpRight");
                    UpdateVNyan("NoseSneerLeft");
                    UpdateVNyan("NoseSneerRight");
                    UpdateVNyan("TongueOut");
                }
            } catch (Exception e) {
                ErrorHandler(e);
            }
        }
    }
}