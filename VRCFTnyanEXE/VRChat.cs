using Common.Logging.Configuration;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace VRCFTnyan
{
    internal static class VRChat
    {
        public static readonly string VRCDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).Replace("Roaming", "LocalLow"), "VRChat", "VRChat");
        public static readonly string VRCOSCFolder = Path.Combine(VRCDataFolder, "OSC");

        public static void CreateVRCAvatarFile(string filename)
        {
            string jsonText = ResourceString(filename);
            // VRCFTnyan.Log("JSON: " + jsonText);
            string vrcAvatarsFolder = VRCAvatarsFolder();
            if (jsonText.Length > 0 && vrcAvatarsFolder.Length > 0)
            {
                using var writer = new StreamWriter(Path.Combine(vrcAvatarsFolder, filename), false, new UTF8Encoding(true));
                writer.Write(jsonText);
            }
        }

        public static string VRCAvatarsFolder()
        {
            foreach (var userFolder in Directory.GetDirectories(VRCOSCFolder))
            {
                string avatarsFolder = Path.Combine(userFolder, "Avatars");
                if (Directory.Exists(avatarsFolder))
                {
                    return avatarsFolder;
                }
            }
            return "";
        }

        /*public static string ResourceString(string filename)
        {
            string resource = $"VRCFTnyan.Resources.{filename}";
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream? stream = assembly.GetManifestResourceStream(resource))
            {
                if (stream != null)
                {
                    using StreamReader reader = new StreamReader(stream);
                    return reader.ReadToEnd();
                }
            }
            return "";
        }*/

        // TODO: This is really fucking ugly but it works. Maybe I'll fix it at some point

        public static string ResourceString(string filename) {
            switch (filename) {
                case "avtr_00000000-0000-0000-0000-000000000000.json" :
                    return @"
{
  ""id"": ""avtr_00000000-0000-0000-0000-000000000000"",
  ""name"": ""[VRCFTtoVNyan] Tracking is OFF"",
  ""parameters"": []
}
";
                    break;
                case "avtr_00000000-0000-0000-0000-000000000001.json":
                    return @"
{
  ""id"": ""avtr_00000000-0000-0000-0000-000000000001"",
  ""name"": ""[VRCFTtoVNyan] Tracking is ON"",
  ""parameters"": [
    {
      ""name"": ""v2/EyeLeftX"",
      ""input"": {
        ""address"": ""/avatar/parameters/v2/EyeLeftX"",
        ""type"": ""Float""
      }
    },
    {
      ""name"": ""v2/EyeLeftY"",
      ""input"": {
        ""address"": ""/avatar/parameters/v2/EyeLeftY"",
        ""type"": ""Float""
      }
    },
    {
      ""name"": ""v2/EyeRightX"",
      ""input"": {
        ""address"": ""/avatar/parameters/v2/EyeRightX"",
        ""type"": ""Float""
      }
    },
    {
      ""name"": ""v2/EyeRightY"",
      ""input"": {
        ""address"": ""/avatar/parameters/v2/EyeRightY"",
        ""type"": ""Float""
      }
    },
    {
      ""name"": ""v2/BrowInnerUp"",
      ""input"": {
        ""address"": ""/avatar/parameters/v2/BrowInnerUp"",
        ""type"": ""Float""
      }
    },
    {
      ""name"": ""v2/BrowDownLeft"",
      ""input"": {
        ""address"": ""/avatar/parameters/v2/BrowDownLeft"",
        ""type"": ""Float""
      }
    },
    {
      ""name"": ""v2/BrowDownRight"",
      ""input"": {
        ""address"": ""/avatar/parameters/v2/BrowDownRight"",
        ""type"": ""Float""
      }
    },
    {
      ""name"": ""v2/BrowOuterUpLeft"",
      ""input"": {
        ""address"": ""/avatar/parameters/v2/BrowOuterUpLeft"",
        ""type"": ""Float""
      }
    },
    {
      ""name"": ""v2/BrowOuterUpRight"",
      ""input"": {
        ""address"": ""/avatar/parameters/v2/BrowOuterUpRight"",
        ""type"": ""Float""
      }
    },
    {
      ""name"": ""v2/EyeLidLeft"",
      ""input"": {
        ""address"": ""/avatar/parameters/v2/EyeLidLeft"",
        ""type"": ""Float""
      }
    },
    {
      ""name"": ""v2/EyeLidRight"",
      ""input"": {
        ""address"": ""/avatar/parameters/v2/EyeLidRight"",
        ""type"": ""Float""
      }
    },
    {
      ""name"": ""v2/EyeSquintLeft"",
      ""input"": {
        ""address"": ""/avatar/parameters/v2/EyeSquintLeft"",
        ""type"": ""Float""
      }
    },
    {
      ""name"": ""v2/EyeSquintRight"",
      ""input"": {
        ""address"": ""/avatar/parameters/v2/EyeSquintRight"",
        ""type"": ""Float""
      }
    },
    {
      ""name"": ""v2/CheekPuffSuck"",
      ""input"": {
        ""address"": ""/avatar/parameters/v2/CheekPuffSuck"",
        ""type"": ""Float""
      }
    },
    {
      ""name"": ""v2/CheekSquintLeft"",
      ""input"": {
        ""address"": ""/avatar/parameters/v2/CheekSquintLeft"",
        ""type"": ""Float""
      }
    },
    {
      ""name"": ""v2/CheekSquintRight"",
      ""input"": {
        ""address"": ""/avatar/parameters/v2/CheekSquintRight"",
        ""type"": ""Float""
      }
    },
    {
      ""name"": ""v2/NoseSneerLeft"",
      ""input"": {
        ""address"": ""/avatar/parameters/v2/NoseSneerLeft"",
        ""type"": ""Float""
      }
    },
    {
      ""name"": ""v2/NoseSneerRight"",
      ""input"": {
        ""address"": ""/avatar/parameters/v2/NoseSneerRight"",
        ""type"": ""Float""
      }
    },
    {
      ""name"": ""v2/JawOpen"",
      ""input"": {
        ""address"": ""/avatar/parameters/v2/JawOpen"",
        ""type"": ""Float""
      }
    },
    {
      ""name"": ""v2/JawZ"",
      ""input"": {
        ""address"": ""/avatar/parameters/v2/JawZ"",
        ""type"": ""Float""
      }
    },
    {
      ""name"": ""v2/JawX"",
      ""input"": {
        ""address"": ""/avatar/parameters/v2/JawX"",
        ""type"": ""Float""
      }
    },
    {
      ""name"": ""v2/LipFunnel"",
      ""input"": {
        ""address"": ""/avatar/parameters/v2/LipFunnel"",
        ""type"": ""Float""
      }
    },
    {
      ""name"": ""v2/LipPucker"",
      ""input"": {
        ""address"": ""/avatar/parameters/v2/LipPucker"",
        ""type"": ""Float""
      }
    },
    {
      ""name"": ""v2/MouthX"",
      ""input"": {
        ""address"": ""/avatar/parameters/v2/MouthX"",
        ""type"": ""Float""
      }
    },
    {
      ""name"": ""v2/LipSuckUpper"",
      ""input"": {
        ""address"": ""/avatar/parameters/v2/LipSuckUpper"",
        ""type"": ""Float""
      }
    },
    {
      ""name"": ""v2/LipSuckLower"",
      ""input"": {
        ""address"": ""/avatar/parameters/v2/LipSuckLower"",
        ""type"": ""Float""
      }
    },
    {
      ""name"": ""v2/MouthRaiserUpper"",
      ""input"": {
        ""address"": ""/avatar/parameters/v2/MouthRaiserUpper"",
        ""type"": ""Float""
      }
    },
    {
      ""name"": ""v2/MouthRaiserLower"",
      ""input"": {
        ""address"": ""/avatar/parameters/v2/MouthRaiserLower"",
        ""type"": ""Float""
      }
    },
    {
      ""name"": ""v2/MouthClosed"",
      ""input"": {
        ""address"": ""/avatar/parameters/v2/MouthClosed"",
        ""type"": ""Float""
      }
    },
    {
      ""name"": ""v2/MouthCornerPullLeft"",
      ""input"": {
        ""address"": ""/avatar/parameters/v2/MouthCornerPullLeft"",
        ""type"": ""Float""
      }
    },
    {
      ""name"": ""v2/MouthCornerPullRight"",
      ""input"": {
        ""address"": ""/avatar/parameters/v2/MouthCornerPullRight"",
        ""type"": ""Float""
      }
    },
    {
      ""name"": ""v2/MouthFrownLeft"",
      ""input"": {
        ""address"": ""/avatar/parameters/v2/MouthFrownLeft"",
        ""type"": ""Float""
      }
    },
    {
      ""name"": ""v2/MouthFrownRight"",
      ""input"": {
        ""address"": ""/avatar/parameters/v2/MouthFrownRight"",
        ""type"": ""Float""
      }
    },
    {
      ""name"": ""v2/MouthDimpleLeft"",
      ""input"": {
        ""address"": ""/avatar/parameters/v2/MouthDimpleLeft"",
        ""type"": ""Float""
      }
    },
    {
      ""name"": ""v2/MouthDimpleRight"",
      ""input"": {
        ""address"": ""/avatar/parameters/v2/MouthDimpleRight"",
        ""type"": ""Float""
      }
    },
    {
      ""name"": ""v2/MouthUpperUpLeft"",
      ""input"": {
        ""address"": ""/avatar/parameters/v2/MouthUpperUpLeft"",
        ""type"": ""Float""
      }
    },
    {
      ""name"": ""v2/MouthUpperUpRight"",
      ""input"": {
        ""address"": ""/avatar/parameters/v2/MouthUpperUpRight"",
        ""type"": ""Float""
      }
    },
    {
      ""name"": ""v2/MouthLowerDownLeft"",
      ""input"": {
        ""address"": ""/avatar/parameters/v2/MouthLowerDownLeft"",
        ""type"": ""Float""
      }
    },
    {
      ""name"": ""v2/MouthLowerDownRight"",
      ""input"": {
        ""address"": ""/avatar/parameters/v2/MouthLowerDownRight"",
        ""type"": ""Float""
      }
    },
    {
      ""name"": ""v2/MouthPressLeft"",
      ""input"": {
        ""address"": ""/avatar/parameters/v2/MouthPressLeft"",
        ""type"": ""Float""
      }
    },
    {
      ""name"": ""v2/MouthPressRight"",
      ""input"": {
        ""address"": ""/avatar/parameters/v2/MouthPressRight"",
        ""type"": ""Float""
      }
    },
    {
      ""name"": ""v2/MouthStretchLeft"",
      ""input"": {
        ""address"": ""/avatar/parameters/v2/MouthStretchLeft"",
        ""type"": ""Float""
      }
    },
    {
      ""name"": ""v2/MouthStretchRight"",
      ""input"": {
        ""address"": ""/avatar/parameters/v2/MouthStretchRight"",
        ""type"": ""Float""
      }
    },
    {
      ""name"": ""v2/TongueOut"",
      ""input"": {
        ""address"": ""/avatar/parameters/v2/TongueOut"",
        ""type"": ""Float""
      }
    }
  ]
}
";
                    break;
                default:
                    return "";

            }

        }

    }
}
