using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;
using System;
using UnityEngine.SceneManagement;

public class MGScene : MonoBehaviour
{
    private static MGScene _instance;
    public static MGScene Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(MGScene)) as MGScene;
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.hideFlags = HideFlags.HideAndDontSave;
                    _instance = obj.AddComponent<MGScene>();
                }
            }

            return _instance;
        }
    }
    public void Generate(){}

    private StringBuilder _sb;

    public Canvas rootCvs;
    public Transform rootTrm;
    private Transform addUiTrm;
    
    public eSceneName curScene, prevScene;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        _sb = new StringBuilder();

        gameObject.hideFlags = HideFlags.HideAndDontSave;

        // 씬 매니져 호출시 UIRoot 생성
        GameObject createdGo = GameObject.Instantiate(Global.prefabsDic[ePrefabs.UIRoot]) as GameObject;
        if(createdGo != null)
        {
            print("UIRoot 생성!");
            rootCvs = createdGo.GetComponent<Canvas>();
            rootTrm = createdGo.transform;
        }
      
        addUiTrm = null;

        InitFirstScene();    
    }

    // MGScene이 처음 실행될 때 타이틀화면부터 시작
    private void InitFirstScene() 
    {
        prevScene = eSceneName.None;
        ChangeScene(eSceneName.Title);
    }

    public Canvas GetRootCanvas()
    {
        return rootCvs;
    }

    public Transform GetRootTransform()
    {
        return rootTrm;
    }

    public eSceneName GetPrevScene()
    {
        return prevScene;
    }

    public void ChangeScene(eSceneName inScene)
    {
        curScene = inScene;

        print(curScene.ToString());

        _sb.Remove(0, _sb.Length);
        _sb.AppendFormat("{0}Scene", eSceneName.Loading);

        SceneManager.LoadScene(_sb.ToString());
        
        // 로딩화면이 필요한씬과 아닌씬을 구분(타이틀을 제외한 모든 장면은 로딩을 거친다)
        if (curScene == eSceneName.Title)
            changeUi(eSceneName.Title);
        else
            changeUi(eSceneName.Loading);
    }

    private void changeUi(eSceneName inScene)
    {
        // 바꿀씬의 UI 프리팹
        _sb.Remove(0, _sb.Length);
        _sb.AppendFormat("UIRoot{0}", inScene.ToString());
        ePrefabs addUiPrefab = (ePrefabs)(Enum.Parse(typeof(ePrefabs), _sb.ToString()));

        // 기존에 생성된 UI가 있다면 초기화
        if (addUiTrm != null)
        {
            addUiTrm.SetParent(null);
            GameObject.Destroy(addUiTrm.gameObject);
        }

        // 새로운씬의 UI프리팹 생성
        GameObject go = GameObject.Instantiate(Global.prefabsDic[addUiPrefab]) as GameObject;
        addUiTrm = go.transform;
		addUiTrm.SetParent(rootTrm , false);
        addUiTrm.localPosition = Vector3.zero;
        addUiTrm.localScale = new Vector3(1, 1, 1);

        if(inScene >= eSceneName.Loading)
        {
            RectTransform rts = go.GetComponent<RectTransform>();
            rts.offsetMax = new Vector2(0, 0);
            rts.offsetMin = new Vector2(0, 0);
        }
    }

    // 로딩이 끝났을 때 처리해주는 함수
    public void LoadingDone()
    {
        prevScene = curScene;
        changeUi(curScene);

        //Util.CallStopAllCoroutine(this);
        Debug.LogWarning("loadingDone");

        if (curScene == eSceneName.Game)
        {
            GameObject.Instantiate(Global.prefabsDic[ePrefabs.MGPool]);
            GameObject.Instantiate(Global.prefabsDic[ePrefabs.MGGame]);
        }

        // if (curScene == eSceneName.Game)
        // {
        //     GameObject.Instantiate(Global._prefDic[ePrefabs.PoolManager]);
        //     GameObject.Instantiate(Global._prefDic[ePrefabs.GameManager]);
        // }
        // else if (curScene == eSceneName.Prologue)
        // {
        //     GameObject.Instantiate(Global._prefDic[ePrefabs.UIEtcPrologue]);
        // }
        // else if (curScene == eSceneName.MainMenu)
        // {
        //     GameObject.Instantiate(Global._prefDic[ePrefabs.UIEtcTitle]);
        //     GameObject.Instantiate(Global._prefDic[ePrefabs.UIEtcTitleMainCam]);
            
        //     MGLayer.instance.ClearLayerList();
        // }
    }
}