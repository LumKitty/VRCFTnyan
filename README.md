# VRC Face Tracking Support for VNyan
Early beta, use at your own risk. Requires VNyan 1.6.6 or later

## Instructions
1. Install like any other VNyan plugin, by copying to VNyan\Items\Assemblies
2. Ensure that loading external plugins is enabled in VNyan
3. Launch [VRC Face Tracking](https://github.com/benaclejames/VRCFaceTracking)
4. Either:  
   * Change OSC send port in VRCFT to match VNyan's (default: 28569) OR
   * Change VNyan's OSC recieve port to match VRCFTs (Settings -> Misc) to match VCRFT's (default: 9000) 
6. Either:
   * Click the "VRC Face Tracking" button in the VNyan plugins menu OR
   * Call the trigger ```_lum_vrcft_start``` from VNyan's "Call Trigger" node
9. Stop by either clicking the button again or call ```_lum_vrcft_stop```

## Configuration
After first start, VRCFTnyan.cfg will be created in your VNyan profile. It has two options:  
```EnableEyes``` - Eyes will be tracked from your headset. Set to ```false``` if you do not have an eye tracker  
```EnableMouth``` - Mouth will be tracked from your headset. Set to ```false``` if you do not have a mouth tracker  
```LogLevel``` - 0: Minimal logging. 1: basic logging. 2: Very spammy logs. 69: Extremely spammy logs  
Restart VNyan for changes to take effect.

## Disclaimer
This is beta code, use it at your own risk. It works for me.

## Credits
A lot of this code was stolen from [VRCFTtoVMCP](https://github.com/tkns3/VRCFTtoVMCP) by [tkns3](https://github.com/tkns3)

## Shameless self promo
### https://twitch.tv/LumKitty
