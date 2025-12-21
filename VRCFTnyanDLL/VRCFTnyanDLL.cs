using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.MemoryMappedFiles;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;
using VNyanInterface;
using static VRCFTnyanDLL.VRC2VMCFunctions;

namespace VRCFTnyanDLL {
    public class VRCFTnyan : MonoBehaviour, IVNyanPluginManifest, IButtonClickedHandler, ITriggerHandler {
        public string PluginName { get; } = "VRCFTnyan";
        public string Version { get; } = "0.3-beta";
        public string Title { get; } = "VRC Face Tracking for VNyan";
        public string Author { get; } = "LumKitty";
        public string Website { get; } = "https://lum.uk";

        private const string SettingsFileName = "VRCFTnyan.cfg";

        private static GameObject objVRCFTnyan;

        internal static bool EnableEyes = true;
        internal static bool EnableMouth = true;
        internal static bool Active = false;
        private static Process ExternalExe;
        private static ProcessStartInfo StartInfo = new ProcessStartInfo();

        internal static void ErrorHandler(Exception e) {
            VNyanInterface.VNyanInterface.VNyanParameter.setVNyanParameterString("_lum_liv_err", e.ToString());
            UnityEngine.Debug.Log("[VRCFT] ERR: " + e.ToString());
        }

        internal static void Log(string message) {
            UnityEngine.Debug.Log("[VRCFT] " + message);
        }

        internal static void Log(byte[] message) {
            string StrMessage = "";
            foreach (byte b in message) {
                StrMessage += (char)b;
            }
            Log(StrMessage);
        }

        public void InitializePlugin() {
            try {
                StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "\\Items\\Assemblies\\VRCFTnyan.exe";
                Log("Looking for external EXE at: " + StartInfo.FileName);
                if (System.IO.File.Exists(StartInfo.FileName)) {
                    LoadPluginSettings();
                    VNyanInterface.VNyanInterface.VNyanUI.registerPluginButton("VRC Face Tracking (beta)", this);
                    VNyanInterface.VNyanInterface.VNyanTrigger.registerTriggerListener(this);
                    Log("VRCFTnyan " + Version + " started");
                    Log("Spawning gameobject: objVRCFTnyan");
                    objVRCFTnyan = new GameObject("objVRCFTnyan", typeof(VRCFTnyan));
                    objVRCFTnyan.SetActive(false);

                    StartInfo.UseShellExecute = false;
                    StartInfo.RedirectStandardInput = true;
                    StartInfo.RedirectStandardOutput = false;
                    StartInfo.RedirectStandardError = false;
                    StartInfo.CreateNoWindow = false;
                    StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                } else {
                    Log("EXE not found, disabling plugin");
                }

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

        internal void LaunchEXEHelper(string Param) {
            ExternalExe = new Process();
            ExternalExe.StartInfo = StartInfo;
            ExternalExe.StartInfo.Arguments = Param;
            ExternalExe.EnableRaisingEvents = true;
            Log("Start process");
            ExternalExe.Start();
            
        }
        

        public void triggerCalled(string name, int int1, int int2, int int3, string text1, string text2, string text3) {
            try {
                Log(name);
                if (name.Length > 11) {
                    Log(name.Substring(0, 10).ToLower());
                    if (name.Substring(0, 10).ToLower() == "_lum_vrcft") {
                        Log(name.Substring(10).ToLower());
                        switch (name.Substring(10).ToLower()) {
                            case "_start": StartTracking(); break;
                            case "_stop": StopTracking(); break;
                        }
                    }
                }
            } catch (Exception e) {
                ErrorHandler(e);
            }
        }

        private void StartTracking() {
            Active = true;
            LaunchEXEHelper("START");
            if (EnableEyes || EnableMouth) { objVRCFTnyan.SetActive(true); }
        }

        private void StopTracking () {
            Active = false;
            LaunchEXEHelper("STOP");
            objVRCFTnyan.SetActive(false);
        }
        
        public void pluginButtonClicked() {
            try { 
                if (Active) {
                    StopTracking();
                } else {
                    StartTracking();
                }
            } catch (Exception e) {
                ErrorHandler(e);
            }
        }

        public void OnApplicationQuit() {
            if (Active) { StopTracking(); }
        }

        private static void UpdateVNyan(string BlendshapeName, float Value) {
            VNyanInterface.VNyanInterface.VNyanAvatar.setBlendshapeOverride(BlendshapeName, Value);
            if (Value != 0) {
                Log(BlendshapeName.PadRight(20, ' ') + ": " + Value.ToString());
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private float GetVNyanParam(string Name) {
            return Math.Clamp(VNyanInterface.VNyanInterface.VNyanParameter.getVNyanParameterFloat(Name), -1.0f, 1.0f);
        }


        public void Update() {
            if (EnableEyes) {
                float CheekPuffSuck = GetVNyanParam("CheekPuffSuck");
                UpdateVNyan("BrowDownLeft", GetVNyanParam("v2/browdownleft"));
                UpdateVNyan("BrowDownRight", GetVNyanParam("v2/browdownright")); //
                UpdateVNyan("BrowInnerUp", GetVNyanParam("v2/browinnerup")); //_weights[VRCFTParametersV2.BrowInnerUp]);
                UpdateVNyan("BrowOuterUpLeft", GetVNyanParam("v2/browouterupleft")); //_weights[VRCFTParametersV2.BrowOuterUpLeft]);
                UpdateVNyan("BrowOuterUpRight", GetVNyanParam("v2/browouterupright")); //_weights[VRCFTParametersV2.BrowOuterUpRight]);
                UpdateVNyan("CheekPuff", CheekPuffSuck > 0 ? CheekPuffSuck : 0f); //_weights[VRCFTParametersV2.CheekPuffSuck] > 0 ? _weights[VRCFTParametersV2.CheekPuffSuck] : 0f);
                UpdateVNyan("CheekSquintLeft", GetVNyanParam("v2/cheeksquintleft")); //_weights[VRCFTParametersV2.CheekSquintLeft]);
                UpdateVNyan("CheekSquintRight", GetVNyanParam("v2/cheeksquintright")); //_weights[VRCFTParametersV2.CheekSquintRight]);
                UpdateVNyan("EyeBlinkLeft", EyeBlink(GetVNyanParam("v2/eyelidleft"))); //EyeBlink(_weights[VRCFTParametersV2.EyeLidLeft]));
                UpdateVNyan("EyeBlinkRight", EyeBlink(GetVNyanParam("v2/eyelidright"))); //EyeBlink(_weights[VRCFTParametersV2.EyeLidRight]));
                UpdateVNyan("EyeLookDownLeft", MoveDown(GetVNyanParam("v2/eyelefty"))); //MoveDown(_weights[VRCFTParametersV2.EyeLeftY]));
                UpdateVNyan("EyeLookDownRight", MoveDown(GetVNyanParam("v2/eyerighty"))); //MoveDown(_weights[VRCFTParametersV2.EyeRightY]));
                UpdateVNyan("EyeLookInLeft", MoveRight(GetVNyanParam("v2/eyeleftx"))); //MoveRight(_weights[VRCFTParametersV2.EyeLeftX])); // LeftEye LookRight
                UpdateVNyan("EyeLookInRight", MoveLeft(GetVNyanParam("v2/eyerightx"))); //MoveLeft(_weights[VRCFTParametersV2.EyeRightX])); // RightEye LookLeft
                UpdateVNyan("EyeLookOutLeft", MoveLeft(GetVNyanParam("v2/eyeleftx"))); //MoveLeft(_weights[VRCFTParametersV2.EyeLeftX])); // LeftEye LookLeft
                UpdateVNyan("EyeLookOutRight", MoveRight(GetVNyanParam("v2/eyerightx"))); //MoveRight(_weights[VRCFTParametersV2.EyeRightX])); // RightEye LookRight
                UpdateVNyan("EyeLookUpLeft", MoveUp(GetVNyanParam("v2/eyelefty"))); //MoveUp(_weights[VRCFTParametersV2.EyeLeftY]));
                UpdateVNyan("EyeLookUpRight", MoveUp(GetVNyanParam("v2/eyerighty"))); //MoveUp(_weights[VRCFTParametersV2.EyeRightY]));
                UpdateVNyan("EyeSquintLeft", GetVNyanParam("v2/eyesquintleft")); //_weights[VRCFTParametersV2.EyeSquintLeft]);
                UpdateVNyan("EyeSquintRight", GetVNyanParam("v2/eyesquintright")); //_weights[VRCFTParametersV2.EyeSquintRight]);
                UpdateVNyan("EyeWideLeft", EyeWide(GetVNyanParam("v2/eyelidleft"))); //EyeWide(_weights[VRCFTParametersV2.EyeLidLeft]));
                UpdateVNyan("EyeWideRight", EyeWide(GetVNyanParam("v2/eyelidright"))); //EyeWide(_weights[VRCFTParametersV2.EyeLidRight]));
            }
            if (EnableMouth) {
                UpdateVNyan("JawForward", MoveForward(GetVNyanParam("v2/jawz"))); //MoveForward(_weights[VRCFTParametersV2.JawZ]));
                UpdateVNyan("JawLeft", MoveLeft(GetVNyanParam("v2/jawx"))); //MoveLeft(_weights[VRCFTParametersV2.JawX]));
                UpdateVNyan("JawOpen", GetVNyanParam("v2/jawopen")); //_weights[VRCFTParametersV2.JawOpen]);
                UpdateVNyan("JawRight", MoveRight(GetVNyanParam("v2/jawx"))); //MoveRight(_weights[VRCFTParametersV2.JawX]));
                UpdateVNyan("MouthClose", GetVNyanParam("v2/mouthclosed")); //_weights[VRCFTParametersV2.MouthClosed]);
                UpdateVNyan("MouthDimpleLeft", GetVNyanParam("v2/mouthdimpleleft")); //_weights[VRCFTParametersV2.MouthDimpleLeft]);
                UpdateVNyan("MouthDimpleRight", GetVNyanParam("v2/mouthdimpleright")); //_weights[VRCFTParametersV2.MouthDimpleRight]);
                UpdateVNyan("MouthFrownLeft", GetVNyanParam("v2/mouthfrownleft")); //_weights[VRCFTParametersV2.MouthFrownLeft]);
                UpdateVNyan("MouthFrownRight", GetVNyanParam("v2/mouthfrownright")); //_weights[VRCFTParametersV2.MouthFrownRight]);
                UpdateVNyan("MouthFunnel", GetVNyanParam("v2/lipfunnel")); //_weights[VRCFTParametersV2.LipFunnel]);
                UpdateVNyan("MouthLeft", MoveLeft(GetVNyanParam("v2/mouthx"))); //MoveLeft(_weights[VRCFTParametersV2.MouthX]));
                UpdateVNyan("MouthLowerDownLeft", GetVNyanParam("v2/mouthlowerdownleft")); //_weights[VRCFTParametersV2.MouthLowerDownLeft]);
                UpdateVNyan("MouthLowerDownRight", GetVNyanParam("v2/mouthlowerdownright")); //_weights[VRCFTParametersV2.MouthLowerDownRight]);
                UpdateVNyan("MouthPressLeft", GetVNyanParam("v2/mouthpressleft")); //_weights[VRCFTParametersV2.MouthPressLeft]);
                UpdateVNyan("MouthPressRight", GetVNyanParam("v2/mouthpressright")); //_weights[VRCFTParametersV2.MouthPressRight]);
                UpdateVNyan("MouthPucker", GetVNyanParam("v2/lippucker")); //_weights[VRCFTParametersV2.LipPucker]);
                UpdateVNyan("MouthRight", MoveRight(GetVNyanParam("v2/mouthx"))); //MoveRight(_weights[VRCFTParametersV2.MouthX]));
                UpdateVNyan("MouthRollUpper", GetVNyanParam("v2/lipsuckupper")); //_weights[VRCFTParametersV2.LipSuckUpper]);
                UpdateVNyan("MouthRollLower", GetVNyanParam("v2/lipsucklower")); //_weights[VRCFTParametersV2.LipSuckLower]);
                UpdateVNyan("MouthShrugUpper", GetVNyanParam("v2/mouthraiserupper")); //_weights[VRCFTParametersV2.MouthRaiserUpper]);
                UpdateVNyan("MouthShrugLower", GetVNyanParam("v2/mouthraiserlower")); //_weights[VRCFTParametersV2.MouthRaiserLower]);
                UpdateVNyan("MouthSmileLeft", GetVNyanParam("v2/mouthcornerpullleft")); //_weights[VRCFTParametersV2.MouthCornerPullLeft]);
                UpdateVNyan("MouthSmileRight", GetVNyanParam("v2/mouthcornerpullright")); //_weights[VRCFTParametersV2.MouthCornerPullRight]);
                UpdateVNyan("MouthStretchLeft", GetVNyanParam("v2/mouthstretchleft")); //_weights[VRCFTParametersV2.MouthStretchLeft]);
                UpdateVNyan("MouthStretchRight", GetVNyanParam("v2/mouthstretchright")); //_weights[VRCFTParametersV2.MouthStretchRight]);
                UpdateVNyan("MouthUpperUpLeft", GetVNyanParam("v2/mouthupperupleft")); //_weights[VRCFTParametersV2.MouthUpperUpLeft]);
                UpdateVNyan("MouthUpperUpRight", GetVNyanParam("v2/mouthupperupright")); //_weights[VRCFTParametersV2.MouthUpperUpRight]);
                UpdateVNyan("NoseSneerLeft", GetVNyanParam("v2/nosesneerleft")); //_weights[VRCFTParametersV2.NoseSneerLeft]);
                UpdateVNyan("NoseSneerRight", GetVNyanParam("v2/nosesneerright")); //_weights[VRCFTParametersV2.NoseSneerRight]);
                UpdateVNyan("TongueOut", GetVNyanParam("v2/tongueout")); //_weights[VRCFTParametersV2.TongueOut]);
            }
        }
    }
}

/*
/tracking/eye/centerpitchyaw
/tracking/eye/centerpitchyaw_2
/tracking/eye/eyesclosedamount
v2/eyesquintright
v2/eyesquintleft
v2/browouterupright
v2/browouterupleft
v2/cheeksquintright
v2/cheeksquintleft
v2/jawopen
v2/mouthclosed
v2/mouthupperupright
v2/mouthupperupleft
v2/nosesneerright
v2/nosesneerleft
v2/mouthlowerdownright
v2/mouthlowerdownleft
v2/mouthcornerpullright
v2/mouthcornerpullleft
v2/mouthfrownright
v2/mouthfrownleft
v2/mouthstretchright
v2/mouthstretchleft
v2/mouthdimpleright
v2/mouthdimpleleft
v2/mouthraiserupper
v2/mouthraiserlower
v2/mouthpressright
v2/mouthpressleft
v2/tongueout
v2/browdownright
v2/browdownleft
v2/eyeleftx
v2/eyelefty
v2/eyerightx
v2/eyerighty
v2/eyelidleft
v2/eyelidright
v2/browinnerup
v2/jawx
v2/jawz
v2/cheekpuffsuck
v2/mouthx
v2/lipsuckupper
v2/lipsucklower
v2/lipfunnel
v2/lippucker
*/