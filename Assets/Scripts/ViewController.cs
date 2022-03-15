using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewController : MonoBehaviour
{
    // �Ƿ�����
    private bool bIsEnable = true;

    public float MoveSpeed = 1f;    // ����ƶ��ٶ�
    public float ZoomSpeed = 256f;  // ��������ٶ�

    public GameObject CameraToggle;

    [Header("������Χ")]
    public float MaxHorizonAxis = 396;
    public float MinHorizonAxis = 286;
    public float MaxVerticalAxis = -112;
    public float MinVerticalAxis = -174;
    public float MaxCameraHeight = 100;
    public float MinCameraHeight = 36;

    /// <summary>
    /// ����ƶ�����Ļ��Ե�Ĳ�ֵ
    /// </summary>
    private float mouseOffset = 0.01f;

    private void Awake()
    {
        var toggle = CameraToggle.GetComponent<Toggle>();
        toggle.onValueChanged.AddListener((isOn) => { bIsEnable = isOn; });
        bIsEnable = toggle.isOn;
    }

    //private void OnCameraMoveChanged(bool isOn)
    //{
    //    bIsEnable = isOn;
    //}

    private void Update()
    {
        if (bIsEnable)
        {
            mouseInScreenBoundMoveView();
            mouseScrollWheelChangeView();
        }
    }

    /// <summary>
    /// �������Ļ��Եʱ���ƶ����
    /// </summary>
    private void mouseInScreenBoundMoveView()
    {
        if (transform.position.z >= MaxHorizonAxis)
        {
            transform.Translate(new Vector3(0, 0, MaxHorizonAxis - transform.position.z), Space.World);
        }

        if (transform.position.z <= MinHorizonAxis)
        {
            transform.Translate(new Vector3(0, 0, MinHorizonAxis - transform.position.z), Space.World);
        }

        if (transform.position.x >= MaxVerticalAxis)
        {
            transform.Translate(Vector2.right * (transform.position.x - MaxVerticalAxis));
        }

        if (transform.position.x <= MinVerticalAxis)
        {
            transform.Translate(Vector2.right * (transform.position.x - MinVerticalAxis));
        }

        Vector3 v1 = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        //����
        if (v1.y >= 1 - mouseOffset)
        {
            if (transform.position.z <= MaxHorizonAxis && transform.position.z >= MinHorizonAxis)
            {
                transform.Translate(new Vector3(0, 0, -MoveSpeed), Space.World);
            }
        }

        //����
        if (v1.y <= mouseOffset)
        {
            if (transform.position.z <= MaxHorizonAxis && transform.position.z >= MinHorizonAxis)
            {
                transform.Translate(new Vector3(0, 0, MoveSpeed), Space.World);
            }
        }

        //����
        if (v1.x <= mouseOffset)
        {
            if (transform.position.x <= MaxVerticalAxis && transform.position.x >= MinVerticalAxis)
            {
                transform.Translate(Vector2.left * MoveSpeed);
            }
        }

        //����
        if (v1.x >= 1 - mouseOffset)
        {
            if (transform.position.x <= MaxVerticalAxis && transform.position.x >= MinVerticalAxis)
            {
                transform.Translate(Vector2.right * MoveSpeed);
            }
        }
    }

    /// <summary>
    /// �����ָı���Ұ��Χ
    /// </summary>
    private void mouseScrollWheelChangeView()
    {
        if (transform.position.y >= MaxCameraHeight)
        {
            transform.Translate(new Vector3(0, MaxCameraHeight - transform.position.y, 0) * Time.deltaTime, Space.World);
        }

        if (transform.position.y <= MinCameraHeight)
        {
            transform.Translate(new Vector3(0, MinCameraHeight - transform.position.y, 0) * Time.deltaTime, Space.World);
        }

        float mouse = Input.GetAxis("Mouse ScrollWheel");
        transform.Translate(new Vector3(0, (0 - mouse) * ZoomSpeed, 0) * Time.deltaTime, Space.World);
    }
}