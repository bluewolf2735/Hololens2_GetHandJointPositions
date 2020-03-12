using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TryGetPosition : MonoBehaviour, IMixedRealitySourceStateHandler, IMixedRealityHandJointHandler
{
    public FileIO fileio;
    private bool handlerTrigger;
    private string JointPositionFileName;

    public TextMesh rightHandText;
    public TextMesh leftHandText;
    public TextMesh filenameText;

    //finger joint position dictionary
    private Dictionary<string, string> HandJointDic;    //outermost
    private Dictionary<string, string> HandJointRight;  //right hand
    private Dictionary<string, string> HandJointLeft;   //left hand

    private int FrameSN;                                //position data serial number

    public void OnHandJointsUpdated(InputEventData<IDictionary<TrackedHandJoint, MixedRealityPose>> eventData)
    {
        //throw new System.NotImplementedException();
        HandJointDic.Clear();

        string thisData;
        string panelData;
        
        var dataTime = DateTime.Now.ToString("HH:mm:ss.ffffff");
        //Debug.Log(dataTime);
        panelData = String.Format("{0}", "Time:" + dataTime + ",\n");

        //Add timestamp to dictionsry
        HandJointDic.Add("Time", String.Format("\"{0}\"", dataTime));

        var camera = Camera.main;
        HandJointDic.Add("MainCamera", String.Format("[{0},{1},{2}]", camera.transform.position.x, camera.transform.position.y, camera.transform.position.z));


        if (eventData.Handedness == Handedness.Right)
        {
            HandJointDic.Add("Frame", FrameSN.ToString());
            panelData = String.Format("{0}{1}", panelData, "\t" + "Right:\n");
            panelData = String.Format("{0}{1}", panelData, GetJointPosition(eventData));
            //fileio.WriteStringToFile(thisData, JointPositionFileName, "txt");

            //add right hand data dictionary
            HandJointDic.Add("Right", DictionaryToString(GetJointPositionDic(eventData, HandJointRight)));
            thisData = DictionaryToString(HandJointDic);
            Debug.Log("This Data:" + thisData);
            rightHandText.text = panelData;
            if (FrameSN == 0) {
                fileio.WriteStringToFile(thisData, JointPositionFileName, "txt");
            }
            else {
                fileio.WriteStringToFile("," + thisData, JointPositionFileName, "txt");
            }
            //fileio.WriteStringToFile(thisData + ",", JointPositionFileName, "txt");
            FrameSN++;
        }
        else if (eventData.Handedness == Handedness.Left)
        {
            HandJointDic.Add("Frame", FrameSN.ToString());
            panelData = String.Format("{0}{1}", panelData, "\t" + "Left:\n");
            panelData = String.Format("{0}{1}", panelData, GetJointPosition(eventData));
            //fileio.WriteStringToFile(thisData, JointPositionFileName, "txt");

            //add left hand data dictionary
            HandJointDic.Add("Left", DictionaryToString(GetJointPositionDic(eventData, HandJointLeft)));
            thisData = DictionaryToString(HandJointDic);
            Debug.Log("This Data:" + thisData);
            leftHandText.text = panelData;
            if (FrameSN == 0)
            {
                fileio.WriteStringToFile(thisData, JointPositionFileName, "txt");
            }
            else
            {
                fileio.WriteStringToFile("," + thisData, JointPositionFileName, "txt");
            }
            //fileio.WriteStringToFile(thisData + ",", JointPositionFileName, "txt");
            FrameSN++;
        }

        //Debug.Log("This Data:" + thisData);
    }

    public void OnSourceDetected(SourceStateEventData eventData)
    {
        //throw new System.NotImplementedException();
        var hand = eventData.Controller as IMixedRealityHand;
        if (hand != null)
        {
            Debug.Log("Source detected:" + hand.ControllerHandedness);
            
        }
    }

    public void OnSourceLost(SourceStateEventData eventData)
    {
        //throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        HandJointDic = new Dictionary<string, string>();
        HandJointRight = new Dictionary<string, string>();
        HandJointLeft = new Dictionary<string, string>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void TriggerHandler()
    {
        if (handlerTrigger)
        {
            //Stop to track position data
            CoreServices.InputSystem.UnregisterHandler<IMixedRealityHandJointHandler>(this);
            Debug.Log("DISABLE");
            rightHandText.text = "Right Hand:";
            leftHandText.text = "Left Hand:";
            filenameText.text = "FileName:";
        }
        else
        {
            //Start to track position data
            FrameSN = 0;
            JointPositionFileName = DateTime.Now.ToString("yyyy_MM_dd_T_HH_mm_ss");//抓取當前時間做檔名
            Debug.Log("NEW FILE NAME:" + JointPositionFileName);
            fileio.CreateDataFile(JointPositionFileName, "txt");
            filenameText.text = JointPositionFileName;
            CoreServices.InputSystem.RegisterHandler<IMixedRealityHandJointHandler>(this);
            Debug.Log("ENABLE");
        }
        handlerTrigger = !handlerTrigger;
    }

    //Get data print on panel in scene
    private string GetJointPosition(InputEventData<IDictionary<TrackedHandJoint, MixedRealityPose>> eventData) {
        string thisData = "";
        //Wrist
        if (eventData.InputData.TryGetValue(TrackedHandJoint.Wrist, out MixedRealityPose wristPose))
        {
            //Debug.Log("Right Wrist POSE:" + wristPose.Position);
            thisData = String.Format("{0}{1}", thisData, "\t\t" + "Wrist: " + wristPose.Position.x + ", " + wristPose.Position.y + ", " + wristPose.Position.z + ",\n");
        }

        //Thumb
        if (eventData.InputData.TryGetValue(TrackedHandJoint.ThumbMetacarpalJoint, out MixedRealityPose thumbMetacarpalPose))
        {
            thisData = String.Format("{0}{1}", thisData, "\t\t" + "ThumbMetacarpal: " + thumbMetacarpalPose.Position.x + ", " + thumbMetacarpalPose.Position.y + ", " + thumbMetacarpalPose.Position.z + ",\n");
        }
        if (eventData.InputData.TryGetValue(TrackedHandJoint.ThumbProximalJoint, out MixedRealityPose thumbProximalPose))
        {
            thisData = String.Format("{0}{1}", thisData, "\t\t" + "ThumbProximal: " + thumbProximalPose.Position.x + ", " + thumbProximalPose.Position.y + ", " + thumbProximalPose.Position.z + ",\n");
        }
        if (eventData.InputData.TryGetValue(TrackedHandJoint.ThumbDistalJoint, out MixedRealityPose thumbDistalPose))
        {
            thisData = String.Format("{0}{1}", thisData, "\t\t" + "ThumbDistal: " + thumbDistalPose.Position.x + ", " + thumbDistalPose.Position.y + ", " + thumbDistalPose.Position.z + ",\n");
        }
        if (eventData.InputData.TryGetValue(TrackedHandJoint.ThumbTip, out MixedRealityPose thumbTipPose))
        {
            thisData = String.Format("{0}{1}", thisData, "\t\t" + "ThumbTip: " + thumbTipPose.Position.x + ", " + thumbTipPose.Position.y + ", " + thumbTipPose.Position.z + ",\n");
        }

        //Index
        if (eventData.InputData.TryGetValue(TrackedHandJoint.IndexMetacarpal, out MixedRealityPose indexMetacarpalPose))
        {
            thisData = String.Format("{0}{1}", thisData, "\t\t" + "IndexMetacarpal: " + indexMetacarpalPose.Position.x + ", " + indexMetacarpalPose.Position.y + ", " + indexMetacarpalPose.Position.z + ",\n");
        }
        if (eventData.InputData.TryGetValue(TrackedHandJoint.IndexKnuckle, out MixedRealityPose indexKnucklePose))
        {
            thisData = String.Format("{0}{1}", thisData, "\t\t" + "IndexKnuckle: " + indexKnucklePose.Position.x + ", " + indexKnucklePose.Position.y + ", " + indexKnucklePose.Position.z + ",\n");
        }
        if (eventData.InputData.TryGetValue(TrackedHandJoint.IndexMiddleJoint, out MixedRealityPose indexMiddlePose))
        {
            thisData = String.Format("{0}{1}", thisData, "\t\t" + "IndexMiddle: " + indexMiddlePose.Position.x + ", " + indexMiddlePose.Position.y + ", " + indexMiddlePose.Position.z + ",\n");
        }
        if (eventData.InputData.TryGetValue(TrackedHandJoint.IndexDistalJoint, out MixedRealityPose indexDistalPose))
        {
            thisData = String.Format("{0}{1}", thisData, "\t\t" + "IndexDistal: " + indexDistalPose.Position.x + ", " + indexDistalPose.Position.y + ", " + indexDistalPose.Position.z + ",\n");
        }
        if (eventData.InputData.TryGetValue(TrackedHandJoint.IndexTip, out MixedRealityPose indexTipPose))
        {
            thisData = String.Format("{0}{1}", thisData, "\t\t" + "IndexTip: " + indexTipPose.Position.x + ", " + indexTipPose.Position.y + ", " + indexTipPose.Position.z + ",\n");
        }

        //Middle
        if (eventData.InputData.TryGetValue(TrackedHandJoint.MiddleMetacarpal, out MixedRealityPose middleMetacarpalPose))
        {
            thisData = String.Format("{0}{1}", thisData, "\t\t" + "MiddleMetacarpal: " + middleMetacarpalPose.Position.x + ", " + middleMetacarpalPose.Position.y + ", " + middleMetacarpalPose.Position.z + ",\n");
        }
        if (eventData.InputData.TryGetValue(TrackedHandJoint.MiddleKnuckle, out MixedRealityPose middleKnucklePose))
        {
            thisData = String.Format("{0}{1}", thisData, "\t\t" + "MiddleKnuckle: " + middleKnucklePose.Position.x + ", " + middleKnucklePose.Position.y + ", " + middleKnucklePose.Position.z + ",\n");
        }
        if (eventData.InputData.TryGetValue(TrackedHandJoint.MiddleMiddleJoint, out MixedRealityPose middleMiddlePose))
        {
            thisData = String.Format("{0}{1}", thisData, "\t\t" + "MiddleMiddle: " + middleMiddlePose.Position.x + ", " + middleMiddlePose.Position.y + ", " + middleMiddlePose.Position.z + ",\n");
        }
        if (eventData.InputData.TryGetValue(TrackedHandJoint.MiddleDistalJoint, out MixedRealityPose middleDistalPose))
        {
            thisData = String.Format("{0}{1}", thisData, "\t\t" + "MiddleDistal: " + middleDistalPose.Position.x + ", " + middleDistalPose.Position.y + ", " + middleDistalPose.Position.z + ",\n");
        }
        if (eventData.InputData.TryGetValue(TrackedHandJoint.MiddleTip, out MixedRealityPose middleTipPose))
        {
            thisData = String.Format("{0}{1}", thisData, "\t\t" + "MiddleTip: " + middleTipPose.Position.x + ", " + middleTipPose.Position.y + ", " + middleTipPose.Position.z + ",\n");
        }

        //Ring
        if (eventData.InputData.TryGetValue(TrackedHandJoint.RingMetacarpal, out MixedRealityPose ringMetacarpalPose))
        {
            thisData = String.Format("{0}{1}", thisData, "\t\t" + "RingMetacarpal: " + ringMetacarpalPose.Position.x + ", " + ringMetacarpalPose.Position.y + ", " + ringMetacarpalPose.Position.z + ",\n");
        }
        if (eventData.InputData.TryGetValue(TrackedHandJoint.RingKnuckle, out MixedRealityPose ringKnucklePose))
        {
            thisData = String.Format("{0}{1}", thisData, "\t\t" + "RingKnuckle: " + ringKnucklePose.Position.x + ", " + ringKnucklePose.Position.y + ", " + ringKnucklePose.Position.z + ",\n");
        }
        if (eventData.InputData.TryGetValue(TrackedHandJoint.RingMiddleJoint, out MixedRealityPose ringMiddlePose))
        {
            thisData = String.Format("{0}{1}", thisData, "\t\t" + "RingMiddle: " + ringMiddlePose.Position.x + ", " + ringMiddlePose.Position.y + ", " + ringMiddlePose.Position.z + ",\n");
        }
        if (eventData.InputData.TryGetValue(TrackedHandJoint.RingDistalJoint, out MixedRealityPose ringDistalPose))
        {
            thisData = String.Format("{0}{1}", thisData, "\t\t" + "RingDistal: " + ringDistalPose.Position.x + ", " + ringDistalPose.Position.y + ", " + ringDistalPose.Position.z + ",\n");
        }
        if (eventData.InputData.TryGetValue(TrackedHandJoint.RingTip, out MixedRealityPose ringTipPose))
        {
            thisData = String.Format("{0}{1}", thisData, "\t\t" + "RingTip: " + ringTipPose.Position.x + ", " + ringTipPose.Position.y + ", " + ringTipPose.Position.z + ",\n");
        }

        //Pinky
        if (eventData.InputData.TryGetValue(TrackedHandJoint.PinkyMetacarpal, out MixedRealityPose pinkyMetacarpalPose))
        {
            thisData = String.Format("{0}{1}", thisData, "\t\t" + "PinkyMetacarpal: " + pinkyMetacarpalPose.Position.x + ", " + pinkyMetacarpalPose.Position.y + ", " + pinkyMetacarpalPose.Position.z + ",\n");
        }
        if (eventData.InputData.TryGetValue(TrackedHandJoint.PinkyKnuckle, out MixedRealityPose pinkyKnucklePose))
        {
            thisData = String.Format("{0}{1}", thisData, "\t\t" + "PinkyKnuckle: " + pinkyKnucklePose.Position.x + ", " + pinkyKnucklePose.Position.y + ", " + pinkyKnucklePose.Position.z + ",\n");
        }
        if (eventData.InputData.TryGetValue(TrackedHandJoint.PinkyMiddleJoint, out MixedRealityPose pinkyMiddlePose))
        {
            thisData = String.Format("{0}{1}", thisData, "\t\t" + "PinkyMiddle: " + pinkyMiddlePose.Position.x + ", " + pinkyMiddlePose.Position.y + ", " + pinkyMiddlePose.Position.z + ",\n");
        }
        if (eventData.InputData.TryGetValue(TrackedHandJoint.PinkyDistalJoint, out MixedRealityPose pinkyDistalPose))
        {
            thisData = String.Format("{0}{1}", thisData, "\t\t" + "PinkyDistal: " + pinkyDistalPose.Position.x + ", " + pinkyDistalPose.Position.y + ", " + pinkyDistalPose.Position.z + ",\n");
        }
        if (eventData.InputData.TryGetValue(TrackedHandJoint.PinkyTip, out MixedRealityPose pinkyTipPose))
        {
            thisData = String.Format("{0}{1}", thisData, "\t\t" + "PinkyTip: " + pinkyTipPose.Position.x + ", " + pinkyTipPose.Position.y + ", " + pinkyTipPose.Position.z + ",\n");
        }

        return thisData;
    }

    // Get position data write to txt file
    private Dictionary<string, string> GetJointPositionDic(InputEventData<IDictionary<TrackedHandJoint, MixedRealityPose>> eventData, Dictionary<string, string> handDicBody)
    {
        handDicBody.Clear();

        //Wrist
        if (eventData.InputData.TryGetValue(TrackedHandJoint.Wrist, out MixedRealityPose wristPose))
        {
            //Debug.Log("Right Wrist POSE:" + wristPose.Position);
            handDicBody.Add("Wrist", PositionArrayToString(wristPose));
        }

        //Thumb
        if (eventData.InputData.TryGetValue(TrackedHandJoint.ThumbMetacarpalJoint, out MixedRealityPose thumbMetacarpalPose))
        {
            handDicBody.Add("ThumbMetacarpal", PositionArrayToString(thumbMetacarpalPose));
        }
        if (eventData.InputData.TryGetValue(TrackedHandJoint.ThumbProximalJoint, out MixedRealityPose thumbProximalPose))
        {
            handDicBody.Add("ThumbProximal", PositionArrayToString(thumbProximalPose));
        }
        if (eventData.InputData.TryGetValue(TrackedHandJoint.ThumbDistalJoint, out MixedRealityPose thumbDistalPose))
        {
            handDicBody.Add("ThumbDistal", PositionArrayToString(thumbDistalPose));
        }
        if (eventData.InputData.TryGetValue(TrackedHandJoint.ThumbTip, out MixedRealityPose thumbTipPose))
        {
            handDicBody.Add("ThumbTip", PositionArrayToString(thumbTipPose));
        }

        //Index
        if (eventData.InputData.TryGetValue(TrackedHandJoint.IndexMetacarpal, out MixedRealityPose indexMetacarpalPose))
        {
            handDicBody.Add("IndexMetacarpal", PositionArrayToString(indexMetacarpalPose));
        }
        if (eventData.InputData.TryGetValue(TrackedHandJoint.IndexKnuckle, out MixedRealityPose indexKnucklePose))
        {
            handDicBody.Add("IndexKnuckle", PositionArrayToString(indexKnucklePose));
        }
        if (eventData.InputData.TryGetValue(TrackedHandJoint.IndexMiddleJoint, out MixedRealityPose indexMiddlePose))
        {
            handDicBody.Add("IndexMiddle", PositionArrayToString(indexMiddlePose));
        }
        if (eventData.InputData.TryGetValue(TrackedHandJoint.IndexDistalJoint, out MixedRealityPose indexDistalPose))
        {
            handDicBody.Add("IndexDistal", PositionArrayToString(indexDistalPose));
        }
        if (eventData.InputData.TryGetValue(TrackedHandJoint.IndexTip, out MixedRealityPose indexTipPose))
        {
            handDicBody.Add("IndexTip", PositionArrayToString(indexTipPose));
        }

        //Middle
        if (eventData.InputData.TryGetValue(TrackedHandJoint.MiddleMetacarpal, out MixedRealityPose middleMetacarpalPose))
        {
            handDicBody.Add("MiddleMetacarpal", PositionArrayToString(middleMetacarpalPose));
        }
        if (eventData.InputData.TryGetValue(TrackedHandJoint.MiddleKnuckle, out MixedRealityPose middleKnucklePose))
        {
            handDicBody.Add("MiddleKnuckle", PositionArrayToString(middleKnucklePose));
        }
        if (eventData.InputData.TryGetValue(TrackedHandJoint.MiddleMiddleJoint, out MixedRealityPose middleMiddlePose))
        {
            handDicBody.Add("MiddleMiddle", PositionArrayToString(middleMiddlePose));
        }
        if (eventData.InputData.TryGetValue(TrackedHandJoint.MiddleDistalJoint, out MixedRealityPose middleDistalPose))
        {
            handDicBody.Add("MiddleDistal", PositionArrayToString(middleDistalPose));
        }
        if (eventData.InputData.TryGetValue(TrackedHandJoint.MiddleTip, out MixedRealityPose middleTipPose))
        {
            handDicBody.Add("MiddleTip", PositionArrayToString(middleTipPose));
        }

        //Ring
        if (eventData.InputData.TryGetValue(TrackedHandJoint.RingMetacarpal, out MixedRealityPose ringMetacarpalPose))
        {
            handDicBody.Add("RingMetacarpal", PositionArrayToString(ringMetacarpalPose));
        }
        if (eventData.InputData.TryGetValue(TrackedHandJoint.RingKnuckle, out MixedRealityPose ringKnucklePose))
        {
            handDicBody.Add("RingKnuckle", PositionArrayToString(ringKnucklePose));
        }
        if (eventData.InputData.TryGetValue(TrackedHandJoint.RingMiddleJoint, out MixedRealityPose ringMiddlePose))
        {
            handDicBody.Add("RingMiddle", PositionArrayToString(ringMiddlePose));
        }
        if (eventData.InputData.TryGetValue(TrackedHandJoint.RingDistalJoint, out MixedRealityPose ringDistalPose))
        {
            handDicBody.Add("RingDistal", PositionArrayToString(ringDistalPose));
        }
        if (eventData.InputData.TryGetValue(TrackedHandJoint.RingTip, out MixedRealityPose ringTipPose))
        {
            handDicBody.Add("RingTip", PositionArrayToString(ringTipPose));
        }

        //Pinky
        if (eventData.InputData.TryGetValue(TrackedHandJoint.PinkyMetacarpal, out MixedRealityPose pinkyMetacarpalPose))
        {
            handDicBody.Add("PinkyMetacarpal", PositionArrayToString(pinkyMetacarpalPose));
        }
        if (eventData.InputData.TryGetValue(TrackedHandJoint.PinkyKnuckle, out MixedRealityPose pinkyKnucklePose))
        {
            handDicBody.Add("PinkyKnuckle", PositionArrayToString(pinkyKnucklePose));
        }
        if (eventData.InputData.TryGetValue(TrackedHandJoint.PinkyMiddleJoint, out MixedRealityPose pinkyMiddlePose))
        {
            handDicBody.Add("PinkyMiddle", PositionArrayToString(pinkyMiddlePose));
        }
        if (eventData.InputData.TryGetValue(TrackedHandJoint.PinkyDistalJoint, out MixedRealityPose pinkyDistalPose))
        {
            handDicBody.Add("PinkyDistal", PositionArrayToString(pinkyDistalPose));
        }
        if (eventData.InputData.TryGetValue(TrackedHandJoint.PinkyTip, out MixedRealityPose pinkyTipPose))
        {
            handDicBody.Add("PinkyTip", PositionArrayToString(pinkyTipPose));
        }

        return handDicBody;
    }

    // convert position data array to json string
    private string PositionArrayToString(MixedRealityPose fingerPose)
    {
        return String.Format("[{0},{1},{2}]", fingerPose.Position.x, fingerPose.Position.y, fingerPose.Position.z);
    }

    // convert <string, string> dictionary to json string
    public string DictionaryToString(Dictionary<string, string> dictionary)
    {
        string dictionaryString = "{";
        foreach (KeyValuePair<string, string> keyValues in dictionary)
        {
            dictionaryString += "\"" + keyValues.Key + "\"" + " : " + keyValues.Value + ", ";
        }
        return dictionaryString.TrimEnd(',', ' ') + "}";
    }
}
