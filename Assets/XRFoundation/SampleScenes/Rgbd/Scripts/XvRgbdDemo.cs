using UnityEngine;
using UnityEngine.UI;
using XvXR.Foundation;

public class XvRgbdDemo : MonoBehaviour
{
    public RectTransform image;
    [SerializeField]
    private XvRgbdManager rgbdManager;
    public XvRgbdManager RgbdManager
    {
        get
        {

            if (rgbdManager == null)
            {
                rgbdManager = FindObjectOfType<XvRgbdManager>();
            }

            if (rgbdManager == null)
            {
                rgbdManager = new GameObject("XvRgbdManager").AddComponent<XvRgbdManager>();
            }
            return rgbdManager;

        }
    }

    [SerializeField]
    private XvCameraManager cameraManager;


    public XvCameraManager CameraManager
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

    public RawImage rawImage;
   
    private void OnEnable()
    {
        RgbdManager.StartRgbPose();

        XvCameraManager.onARCameraStreamFrameArrived.AddListener((data) => {
            rawImage.texture = data.tex;
        });
    }

    private void OnDisable()
    {
        RgbdManager.StopRgbPose();

    }

    Vector2 rgbPixelPoint = new Vector2(960, 540);


    private void Update()
    {
        Vector3 pointerPose = new Vector3();


        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rgbPixelPoint.x -= 1;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rgbPixelPoint.x += 1;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            rgbPixelPoint.y += 1;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            rgbPixelPoint.y -= 1;
        }
        image.localPosition = rgbPixelPoint;// - new Vector2(xvCameraManager.Width/2, xvCameraManager.Height/2);


        int width = cameraManager.Width;
        int height = cameraManager.Height;

        //particlesCloudPoint.gameObject.SetActive(true);
        //particlesCloudPoint.StartDraw(vecGroup);
        Vector3 screenPoint = rgbPixelPoint;

        screenPoint.x = (rgbPixelPoint.x / 1920) * width;
        screenPoint.y = (rgbPixelPoint.y / 1080) * height;

        screenPoint.y = height - screenPoint.y;

      

        if (Time.frameCount % 5 == 0)
        {
            if (RgbdManager.GetRgbPixel3DPose(screenPoint, ref pointerPose))
            {
              
                transform.position = pointerPose;
               
            }
        }
    }
}