using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace XvXR.Foundation.SampleScenes
{
    public class XvLoadScenesDemo : MonoBehaviour
    {
        public void BtClick(GameObject bt) {


            XvLoadScenesManager.Instance.LoadScenes(bt.name);
        
        }
    }
}
