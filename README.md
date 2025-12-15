# VRC Face Tracking Support for VNyan
Early alpha, use at your own risk

## Instructions
1. Install like any other VNyan plugin, by copying to VNyan\Items\Assemblies
2. Ensure that loading external plugins is enabled in VNyan
3. Launch [VRC Face Tracking](https://github.com/benaclejames/VRCFaceTracking)
4. Either click the "VRC Face Tracking" button in the VNyan plugins menu or call the trigger ```_lum_vrcft_start``` from VNyan's "Call Trigger" node
5. If it's working you should a DOS console window pop up and start spamming. This will be hidden in the final release, just minimise it :)
6. Stop by either clicking the button again or call ```_lum_vrcft_stop```

## Configuration
After first start, VRCFTnyan.cfg will be created in your VNyan profile. It has two options:  
```EnableEyes``` - Eyes will be tracked from your headset. Set to ```false``` if you do not have an eye tracker  
```EnableMouth``` - Mouth will be tracked from your headset. Set to ```false``` if you do not have a mouth tracker  
Restart VNyan for changes to take effect.

## Disclaimer
This is early alpha code, use it at your own risk. It works for me, but I do not have an eye tracker

## Credits
80% of this code was stolen from [VRCFTtoVMCP](https://github.com/tkns3/VRCFTtoVMCP) by [tkns3](https://github.com/tkns3)

## Shameless self promo
### https://twitch.tv/LumKitty
