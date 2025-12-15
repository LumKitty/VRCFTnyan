using Common.Logging.Configuration;
using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Text;
using VNyanInterface;

using VRCFTnyan.Osc;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

namespace VRCFTnyan {
    internal static class UpdateVNyanBlendshapes {
        static VRCFTParameterWeights _weights = new VRCFTParameterWeights();
        private static MemoryMappedFile mmf = null;
        private static MemoryMappedViewAccessor mmfAccess;
        private const string MMFname = "uk.lum.VRCFTnyan.0.1";
        private const int MMFSize = sizeof(float) * 52;
        private static long MMFPos = 0;

        public static void InitialiseMMF() {
            if (mmf == null) {
                VRCFTnyan.Log("Creating file");
                mmf = MemoryMappedFile.CreateOrOpen(MMFname, MMFSize);
                VRCFTnyan.Log("Creating accessor");
                mmfAccess = mmf.CreateViewAccessor(0, MMFSize);
            }
        }

        static void UpdateVNyan(string BlendshapeName, float Value) {
            if (Value != 0) {
                VRCFTnyan.Log(BlendshapeName.PadRight(20, ' ') + ": " + Value.ToString());
            }
            mmfAccess.Write(MMFPos, Value);
            MMFPos += sizeof(float);
        }
        
        public static void UpdateBlendshapes() {
            VRCFTParametersStore.CopyTo(_weights);
            MMFPos = 0;
            UpdateVNyan("BrowDownLeft", _weights[VRCFTParametersV2.BrowDownLeft]);
            UpdateVNyan("BrowDownRight", _weights[VRCFTParametersV2.BrowDownRight]);
            UpdateVNyan("BrowInnerUp", _weights[VRCFTParametersV2.BrowInnerUp]);
            UpdateVNyan("BrowOuterUpLeft", _weights[VRCFTParametersV2.BrowOuterUpLeft]);
            UpdateVNyan("BrowOuterUpRight", _weights[VRCFTParametersV2.BrowOuterUpRight]);
            UpdateVNyan("CheekPuff", _weights[VRCFTParametersV2.CheekPuffSuck] > 0 ? _weights[VRCFTParametersV2.CheekPuffSuck] : 0f);
            UpdateVNyan("CheekSquintLeft", _weights[VRCFTParametersV2.CheekSquintLeft]);
            UpdateVNyan("CheekSquintRight", _weights[VRCFTParametersV2.CheekSquintRight]);
            UpdateVNyan("EyeBlinkLeft", EyeBlink(_weights[VRCFTParametersV2.EyeLidLeft]));
            UpdateVNyan("EyeBlinkRight", EyeBlink(_weights[VRCFTParametersV2.EyeLidRight]));
            UpdateVNyan("EyeLookDownLeft", MoveDown(_weights[VRCFTParametersV2.EyeLeftY]));
            UpdateVNyan("EyeLookDownRight", MoveDown(_weights[VRCFTParametersV2.EyeRightY]));
            UpdateVNyan("EyeLookInLeft", MoveRight(_weights[VRCFTParametersV2.EyeLeftX])); // LeftEye LookRight
            UpdateVNyan("EyeLookInRight", MoveLeft(_weights[VRCFTParametersV2.EyeRightX])); // RightEye LookLeft
            UpdateVNyan("EyeLookOutLeft", MoveLeft(_weights[VRCFTParametersV2.EyeLeftX])); // LeftEye LookLeft
            UpdateVNyan("EyeLookOutRight", MoveRight(_weights[VRCFTParametersV2.EyeRightX])); // RightEye LookRight
            UpdateVNyan("EyeLookUpLeft", MoveUp(_weights[VRCFTParametersV2.EyeLeftY]));
            UpdateVNyan("EyeLookUpRight", MoveUp(_weights[VRCFTParametersV2.EyeRightY]));
            UpdateVNyan("EyeSquintLeft", _weights[VRCFTParametersV2.EyeSquintLeft]);
            UpdateVNyan("EyeSquintRight", _weights[VRCFTParametersV2.EyeSquintRight]);
            UpdateVNyan("EyeWideLeft", EyeWide(_weights[VRCFTParametersV2.EyeLidLeft]));
            UpdateVNyan("EyeWideRight", EyeWide(_weights[VRCFTParametersV2.EyeLidRight]));

            UpdateVNyan("JawForward", MoveForward(_weights[VRCFTParametersV2.JawZ]));
            UpdateVNyan("JawLeft", MoveLeft(_weights[VRCFTParametersV2.JawX]));
            UpdateVNyan("JawOpen", _weights[VRCFTParametersV2.JawOpen]);
            UpdateVNyan("JawRight", MoveRight(_weights[VRCFTParametersV2.JawX]));
            UpdateVNyan("MouthClose", _weights[VRCFTParametersV2.MouthClosed]);
            UpdateVNyan("MouthDimpleLeft", _weights[VRCFTParametersV2.MouthDimpleLeft]);
            UpdateVNyan("MouthDimpleRight", _weights[VRCFTParametersV2.MouthDimpleRight]);
            UpdateVNyan("MouthFrownLeft", _weights[VRCFTParametersV2.MouthFrownLeft]);
            UpdateVNyan("MouthFrownRight", _weights[VRCFTParametersV2.MouthFrownRight]);
            UpdateVNyan("MouthFunnel", _weights[VRCFTParametersV2.LipFunnel]);
            UpdateVNyan("MouthLeft", MoveLeft(_weights[VRCFTParametersV2.MouthX]));
            UpdateVNyan("MouthLowerDownLeft", _weights[VRCFTParametersV2.MouthLowerDownLeft]);
            UpdateVNyan("MouthLowerDownRight", _weights[VRCFTParametersV2.MouthLowerDownRight]);
            UpdateVNyan("MouthPressLeft", _weights[VRCFTParametersV2.MouthPressLeft]);
            UpdateVNyan("MouthPressRight", _weights[VRCFTParametersV2.MouthPressRight]);
            UpdateVNyan("MouthPucker", _weights[VRCFTParametersV2.LipPucker]);
            UpdateVNyan("MouthRight", MoveRight(_weights[VRCFTParametersV2.MouthX]));
            UpdateVNyan("MouthRollUpper", _weights[VRCFTParametersV2.LipSuckUpper]);
            UpdateVNyan("MouthRollLower", _weights[VRCFTParametersV2.LipSuckLower]);
            UpdateVNyan("MouthShrugUpper", _weights[VRCFTParametersV2.MouthRaiserUpper]);
            UpdateVNyan("MouthShrugLower", _weights[VRCFTParametersV2.MouthRaiserLower]);
            UpdateVNyan("MouthSmileLeft", _weights[VRCFTParametersV2.MouthCornerPullLeft]);
            UpdateVNyan("MouthSmileRight", _weights[VRCFTParametersV2.MouthCornerPullRight]);
            UpdateVNyan("MouthStretchLeft", _weights[VRCFTParametersV2.MouthStretchLeft]);
            UpdateVNyan("MouthStretchRight", _weights[VRCFTParametersV2.MouthStretchRight]);
            UpdateVNyan("MouthUpperUpLeft", _weights[VRCFTParametersV2.MouthUpperUpLeft]);
            UpdateVNyan("MouthUpperUpRight", _weights[VRCFTParametersV2.MouthUpperUpRight]);
            UpdateVNyan("NoseSneerLeft", _weights[VRCFTParametersV2.NoseSneerLeft]);
            UpdateVNyan("NoseSneerRight", _weights[VRCFTParametersV2.NoseSneerRight]);
            UpdateVNyan("TongueOut", _weights[VRCFTParametersV2.TongueOut]);
        }
        /// <summary>
        /// VRCFTのX系パラメータの値をPerfectSyncのMoveRightの値に変換する。
        /// X系パラメータは0.0が移動なし、+1.0が右側への移動最大、-1.0が左側への移動最大を表す。
        /// PerfectSyncのMoveRightは0.0が移動なし、+1.0が右側への移動最大を表す。
        /// </summary>
        /// <param name="vrcftX">0.0が移動なし、+1.0が右側への移動最大、-1.0が左側への移動最大。</param>
        /// <returns>0.0が移動なし、+1.0が右側への移動最大</returns>
        static float MoveRight(float vrcftX) {
            if (vrcftX > 0) {
                return vrcftX;
            } else {
                return 0f;
            }
        }

        /// <summary>
        /// VRCFTのX系パラメータの値をPerfectSyncのMoveLeftの値に変換する。
        /// X系パラメータは0.0が移動なし、+1.0が右側への移動最大、-1.0が左側への移動最大を表す。
        /// PerfectSyncのMoveLeftは0.0が移動なし、+1.0が左側への移動最大を表す。
        /// </summary>
        /// <param name="vrcftX">0.0が移動なし、+1.0が右側への移動最大、-1.0が左側への移動最大。</param>
        /// <returns>0.0が移動なし、+1.0が左側への移動最大</returns>
        static float MoveLeft(float vrcftX) {
            if (vrcftX > 0) {
                return 0;
            } else {
                return Math.Abs(vrcftX);
            }
        }

        /// <summary>
        /// VRCFTのY系パラメータの値をPerfectSyncのMoveUpの値に変換する。
        /// Y系パラメータは0.0が移動なし、+1.0が上側への移動最大、-1.0が下側への移動最大を表す。
        /// PerfectSyncのMoveUpは0.0が移動なし、+1.0が上側への移動最大を表す。
        /// </summary>
        /// <param name="vrcftY">0.0が移動なし、+1.0が上側への移動最大、-1.0が下側への移動最大。</param>
        /// <returns>0.0が移動なし、+1.0が上側への移動最大</returns>
        static float MoveUp(float vrcftY) {
            if (vrcftY > 0) {
                return vrcftY;
            } else {
                return 0f;
            }
        }

        /// <summary>
        /// VRCFTのY系パラメータの値をPerfectSyncのMoveDownの値に変換する。
        /// Y系パラメータは0.0が移動なし、+1.0が上側への移動最大、-1.0が下側への移動最大を表す。
        /// PerfectSyncのMoveDownは0.0が移動なし、+1.0が下側への移動最大を表す。
        /// </summary>
        /// <param name="vrcftY">0.0が移動なし、+1.0が上側への移動最大、-1.0が下側への移動最大。</param>
        /// <returns>0.0が移動なし、+1.0が下側への移動最大</returns>
        static float MoveDown(float vrcftY) {
            if (vrcftY > 0) {
                return 0;
            } else {
                return Math.Abs(vrcftY);
            }
        }

        /// <summary>
        /// VRCFTのY系パラメータの値をPerfectSyncのMoveForwardの値に変換する。
        /// Z系パラメータは0.0が移動なし、+1.0が前側への移動最大、-1.0が後側への移動最大を表す。
        /// PerfectSyncのMoveForwardは0.0が移動なし、+1.0が前側への移動最大を表す。
        /// </summary>
        /// <param name="vrcftZ">0.0が移動なし、+1.0が前側への移動最大、-1.0が後側への移動最大。</param>
        /// <returns>0.0が移動なし、+1.0が前側への移動最大</returns>
        static float MoveForward(float vrcftZ) {
            if (vrcftZ > 0) {
                return vrcftZ;
            } else {
                return 0f;
            }
        }

        /// <summary>
        /// VRCFTのEyeLid系パラメータの値をPerfectSyncのEyeWideの値に変換する。
        /// EyeLid系パラメータは0.0が目を閉じる、+0.8が目を開く(通常)、+1.0が目を大きく開くを表す。
        /// PerfectSyncのEyeWideは0.0が移動なし、+1.0が目を大きく開くの移動最大を表す。
        /// VRCFTの「EyeLid=0.0」がPerfectSyncの「EyeWide=0.0 + EyeBlink=1.0」。
        /// VRCFTの「EyeLid=0.8」がPerfectSyncの「EyeWide=0.0 + EyeBlink=0.0」。
        /// VRCFTの「EyeLid=1.0」がPerfectSyncの「EyeWide=1.0 + EyeBlink=0.0」。
        /// </summary>
        /// <param name="vrcftEyeLid">0.0が目を閉じる、+0.8が目を開く(通常)、+1.0が目を大きく開く</param>
        /// <returns>0.0が移動なし、+1.0が目を大きく開くの移動最大</returns>
        static float EyeWide(float vrcftEyeLid) {
            if (vrcftEyeLid < 0.8f) {
                return 0f;
            } else {
                return (vrcftEyeLid - 0.8f) * 5f;
            }
        }

        /// <summary>
        /// VRCFTのEyeLid系パラメータの値をPerfectSyncのEyeBlinkの値に変換する。
        /// EyeLid系パラメータは0.0が目を閉じる、+0.8が目を開く(通常)、+1.0が目を大きく開くを表す。
        /// PerfectSyncのEyeBlinkは0.0が移動なし、+1.0が目を閉じるの移動最大を表す。
        /// VRCFTの「EyeLid=0.0」がPerfectSyncの「EyeWide=0.0 + EyeBlink=1.0」。
        /// VRCFTの「EyeLid=0.8」がPerfectSyncの「EyeWide=0.0 + EyeBlink=0.0」。
        /// VRCFTの「EyeLid=1.0」がPerfectSyncの「EyeWide=1.0 + EyeBlink=0.0」。
        /// </summary>
        /// <param name="vrcftEyeLid">0.0が目を閉じる、+0.8が目を開く(通常)、+1.0が目を大きく開く</param>
        /// <returns>0.0が移動なし、+1.0が目を閉じるの移動最大</returns>
        static float EyeBlink(float vrcftEyeLid) {
            if (vrcftEyeLid < 0.8f) {
                return 1f - vrcftEyeLid * 10f / 8f;
            } else {
                return 0f;
            }
        }
    }
}
