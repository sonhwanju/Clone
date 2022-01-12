using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;

public class GameAwake : MonoBehaviour
{
    public static GameAwake Instance {get; private set;}

    private StringBuilder _sb;

    private bool bFirstInit = false;

    private string tempStr;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        _sb = new StringBuilder();

        gameObject.hideFlags = HideFlags.HideAndDontSave;
    }

    private void Start()
    {
        // 필요한 데이터 세팅

        // 최초 한번만 로딩
        if (!bFirstInit)
        {
            InitOnce();

            bFirstInit = true; 
        }

        // 메인씬을 로딩할 때마다 세팅할 값 또는 함수 호출

    }

    private void InitOnce()
    {
        // 게임에 필요한 리소스 및 데이터 최초 한번 로딩

        LoadPrefabDic();

        LoadSpritesDic();

        // 씬매니져 생성
        MGScene.Instance.Generate();
    }


    void LoadPrefabDic()
    {
        Global.prefabsDic = new Dictionary<ePrefabs, GameObject>();

        object[] files;

        _sb.Remove(0, _sb.Length);
        _sb.Append("Prefabs/");

        files = Resources.LoadAll(_sb.ToString());
        setPrefabDic(files);

        // 폴더별로 분리하여 저장했을 때는 개별로 불러옴
        // // ui
        // _sb.Remove(0, _sb.Length);
		// _sb.AppendFormat("Prefabs{0}UI{0}", _folderSlash);

		// files = Resources.LoadAll(_sb.ToString());
		// setPrefabDic(files);

        // // game , Bg
        // _sb.Remove(0, _sb.Length);
        // _sb.AppendFormat("Prefabs{0}Game{0}", _folderSlash);

		// files = Resources.LoadAll(_sb.ToString());
        // setPrefabDic(files);
    }

    void LoadSpritesDic()
    {
        Global.spritesDic = new Dictionary<string, Sprite>();

        Sprite[] files;

        _sb.Remove(0, _sb.Length);
        _sb.Append("Sprites/");

        files = Resources.LoadAll<Sprite>(_sb.ToString());
        setSpriteDic(files);
    }

    private void setPrefabDic(object[] files)
    {
        for (int i = 0; i < files.Length; i++)
        {
            GameObject outObj;

            tempStr = getFileName(files[i].ToString());

            if (!Global.prefabsDic.TryGetValue((ePrefabs)Enum.Parse(typeof(ePrefabs), tempStr), out outObj))
                Global.prefabsDic.Add((ePrefabs)Enum.Parse(typeof(ePrefabs), tempStr), (GameObject)files[i]);
        }
    }

    private void setSpriteDic(Sprite[] files)
    {
        for (int i = 0; i < files.Length; i++)
        {
            Sprite outObj;

            tempStr = getFileName(files[i].ToString());

            if (!Global.spritesDic.TryGetValue(tempStr, out outObj))
                Global.spritesDic.Add(tempStr, (Sprite)files[i]);
        }
    }

    private string getFileName(string objectName)
    {
        string rtn = null;

        rtn = objectName.Substring(0, objectName.IndexOf(' '));

        return rtn;
    }
}