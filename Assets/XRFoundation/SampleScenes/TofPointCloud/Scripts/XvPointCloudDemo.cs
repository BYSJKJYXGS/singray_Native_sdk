using UnityEngine;
using UnityEngine.UI;



namespace XvXR.Foundation.SampleScenes
{ 

public class XvPointCloudDemo : MonoBehaviour
{
    public XvParticlesCloudPoint particlesCloudPoint;
    Vector3[] vecGroup;

    

    public Text info;
    public Slider slider;


    public Text info_0;
    public Slider slider_0;

    public Text info_1;
    public Slider slider_1;

    public Text info_2;
    public Slider slider_2;
    public Text vvv;

    private int v_0 = 4;
    private int v_1 = 1;
    private int v_2 = 5;
    private float v_3 = 0.2f;



    [SerializeField]
    private XvCameraManager cameraManager;


    public XvCameraManager XvCameraManager
    {
        get
        {

            if (cameraManager == null)
            {
                cameraManager = FindObjectOfType<XvCameraManager>();
            }

            if (cameraManager == null)
            {
                cameraManager = new GameObject("XvCameraManager").AddComponent<XvCameraManager>();
            }
            return cameraManager;

        }
    }

    private void OnEnable()
    {
        slider_0.onValueChanged.AddListener(changeIpd0);
        slider_1.onValueChanged.AddListener(changeIpd1);
        slider_2.onValueChanged.AddListener(changeIpd2);
        slider.onValueChanged.AddListener(changeIpd);




    }


    private void Start()
    {

    }

    private int countTime = 0;
   


    // Update is called once per frame
    void Update()
    {
       
        countTime++;
        if (countTime == 10)
        {
            countTime = 0;

            if (XvCameraManager.GetPointCloudData(out vecGroup))
            {
                particlesCloudPoint.gameObject.SetActive(true);
                particlesCloudPoint.StartDraw(vecGroup);
            }
        }

    }
    public void  StartTofPointCloud() { 
        XvCameraManager.StartTofPointCloud();

    }

    public void StopTofPointCloud()
    {
        particlesCloudPoint.gameObject.SetActive(false);
        XvCameraManager.StopTofPointCloud();
    }
    public void SetUp()
    {
        XvCameraManager.SetTofExposure(v_0, v_1, v_2, v_3);

    }

    private void changeIpd0(float value)
    {
        v_0 = int.Parse(slider_0.value.ToString());
       
        info_0.text = v_0 + " ";
    }

    private void changeIpd1(float value)
    {
        v_1 = int.Parse(slider_1.value.ToString());
        
        info_1.text = v_1 + " ";
    }

    private void changeIpd2(float value)
    {
        v_2 = int.Parse(slider_2.value.ToString());
      
        info_2.text = v_2 + " ";
    }


    private void changeIpd(float value)
    {
        v_3 = slider.value;
       
        info.text = v_3 + " ";
    }

   

}

}
