using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RgbControl : MonoBehaviour
{
    public WifiDisplayPluginWrapper wifi;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("startwifi",8);
    }

    private void startwifi()
    {
        wifi.OnPcDisplayClick();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
