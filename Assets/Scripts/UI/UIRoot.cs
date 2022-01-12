using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class UIRoot : MonoBehaviour
{
    public Image blackPannel;

    private CanvasScaler rootCanvas;

    private float nowRatio;

    public bool isPaused = false;

    void Awake()
    {
        rootCanvas = this.gameObject.GetComponent<CanvasScaler>();

        Global.referenceResolution = new Vector2();
        Global.referenceResolution = rootCanvas.referenceResolution;
        Global.blackPannel = blackPannel;

        nowRatio = Convert.ToSingle((double)Screen.height / (double)Screen.width);
        Debug.LogFormat("해상도{0}x{1} 비율 : {2:F6}, dpi:{3}", Screen.height, Screen.width, nowRatio, Screen.dpi);

        rootCanvas.referenceResolution = Global.referenceResolution;
    }

   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 안드로이드 기기에서 Back버튼 처리
        }
    }

     void OnApplicationFocus(bool pauseStatus)
    {
        isPaused = pauseStatus;

        if (!isPaused)
        {
            // 세이브 데이터 저장 처리 등
        }
    }

    void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            // 세이브 데이터 저장 처리 등
        }
        else
        {
            Screen.fullScreen = true;
        }
    }

    void OnApplicationQuit()
    {
        // 세이브 데이터 저장 처리 등
    }
}