# Tracking hand joint positions

An Unity Hololens 2 app to track hand/finger joint positions and export to txt file.

![image](https://github.com/bluewolf2735/Hololens2_GetHandJointPositions/blob/master/GetHandJointPositions.gif)

## Requirements

- Visual Studio 2019
- Unity 2019.2.10f1
- Windows 10.0.18362.0 SDK
- MRTK (Microsoft Mixed Reality Toolkit) 2.1.0


## Usage

Air tap the white cude in application will trigger to Start/Stop tracking positions and display on panel.

#### Running on Unity

You can find txt file on path like:
``
C:\Users\zach\AppData\LocalLow\DefaultCompany\TryGetJointPosition\yyyy_MM_dd_T_HH_mm_ss.txt
``

#### Running on Hololens 2

You can get txt file via Windows Device Portal on path like:
``
UserFolders\LocalAppData\TryGetJointPosition\RoamingState\yyyy_MM_dd_T_HH_mm_ss.txt
``

## Data Format

```json
{
  "Time" : "10:28:41.451226",
  "MainCamera" : [0.03561179,-0.06738269,1.261862],
  "Frame" : 0,
  "Left" : {
    "Wrist" : [-0.1001644,-0.1397457,1.698841],
    "ThumbMetacarpal" : [-0.07663216,-0.1329312,1.709142],
    "ThumbProximal" : [-0.0417842,-0.1165145,1.726112],
    "ThumbDistal" : [-0.02161106,-0.0995589,1.742543],
    "ThumbTip" : [-0.007432401,-0.0919666,1.750698],
    "IndexMetacarpal" : [-0.08542445,-0.1275559,1.708451],
    "IndexKnuckle" : [-0.05257943,-0.08020162,1.721133],
    "IndexMiddle" : [-0.03865055,-0.04696392,1.741495],
    "IndexDistal" : [-0.03156399,-0.03061442,1.753956],
    "IndexTip" : [-0.02822369,-0.02014075,1.76209],
    "MiddleMetacarpal" : [-0.09273973,-0.1253929,1.708474],
    "MiddleKnuckle" : [-0.07244878,-0.07605846,1.721609],
    "MiddleMiddle" : [-0.06465857,-0.07142945,1.761683],
    "MiddleDistal" : [-0.06706598,-0.0951803,1.771359],
    "MiddleTip" : [-0.06954965,-0.1105744,1.766425],
    "RingMetacarpal" : [-0.1023688,-0.1221396,1.709112],
    "RingKnuckle" : [-0.09046443,-0.07829773,1.727192],
    "RingMiddle" : [-0.08403707,-0.08222707,1.762365],
    "RingDistal" : [-0.08355215,-0.1032565,1.758216],
    "RingTip" : [-0.08382712,-0.1161228,1.745788],
    "PinkyMetacarpal" : [-0.1106531,-0.121735,1.711501],
    "PinkyKnuckle" : [-0.1069923,-0.08048041,1.735678],
    "PinkyMiddle" : [-0.1011764,-0.08479947,1.758378],
    "PinkyDistal" : [-0.09784788,-0.1022681,1.756635],
    "PinkyTip" : [-0.09418933,-0.1120949,1.747394]}
}
```

## License

[MIT © Richard McRichface.](../LICENSE)
