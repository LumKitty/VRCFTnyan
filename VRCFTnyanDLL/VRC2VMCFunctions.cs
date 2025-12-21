using System;
using System.Collections.Generic;
using System.Text;

namespace VRCFTnyanDLL {
    internal static class VRC2VMCFunctions {
        /// <summary>
        /// VRCFTのX系パラメータの値をPerfectSyncのMoveRightの値に変換する。
        /// X系パラメータは0.0が移動なし、+1.0が右側への移動最大、-1.0が左側への移動最大を表す。
        /// PerfectSyncのMoveRightは0.0が移動なし、+1.0が右側への移動最大を表す。
        /// </summary>
        /// <param name="vrcftX">0.0が移動なし、+1.0が右側への移動最大、-1.0が左側への移動最大。</param>
        /// <returns>0.0が移動なし、+1.0が右側への移動最大</returns>
        internal static float MoveRight(float vrcftX) {
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
        internal static float MoveLeft(float vrcftX) {
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
        internal static float MoveUp(float vrcftY) {
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
        internal static float MoveDown(float vrcftY) {
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
        internal static float MoveForward(float vrcftZ) {
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
        internal static float EyeWide(float vrcftEyeLid) {
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
        internal static float EyeBlink(float vrcftEyeLid) {
            if (vrcftEyeLid < 0.8f) {
                return 1f - vrcftEyeLid * 10f / 8f;
            } else {
                return 0f;
            }
        }

    }
}
