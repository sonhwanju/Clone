using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum eLoadingStatus
{
    None,
    Unload,
    NextScene,
    Done,
}

public class UIRootLoading : MonoBehaviour
{
    private StringBuilder _sb;

    private eLoadingStatus loadingState;
    private AsyncOperation unLoadDone, loadLevelDone;
    private float loadingLimitTime;

    const float MAX_LOADING_TIME = 1.0f;    // 로딩시간은 최소 1초 이상

    void Awake()
    {
        _sb = new StringBuilder();
    }

    void Start()
    {
        loadingState = eLoadingStatus.None;
        NextState();
    }

    IEnumerator NoneState()
    {
        loadingState = eLoadingStatus.Unload;

        while (loadingState == eLoadingStatus.None)
        {

            yield return null;
        }

        NextState();
    }

    IEnumerator UnloadState()
    {
        unLoadDone = Resources.UnloadUnusedAssets();
        System.GC.Collect();

        while (loadingState == eLoadingStatus.Unload)
        {
            if (unLoadDone.isDone)
            {
                loadingState = eLoadingStatus.NextScene;
            }

            yield return null;
        }

        NextState();
    }

    IEnumerator NextSceneState()
    {
        loadLevelDone = SceneManager.LoadSceneAsync("MainScene");

        loadingLimitTime = MAX_LOADING_TIME;
        while (loadingState == eLoadingStatus.NextScene)
        {
            loadingLimitTime -= Time.deltaTime;
            if (loadLevelDone.isDone && loadingLimitTime <= 0)
            {
                loadingState = eLoadingStatus.Done;
            }

            yield return null;
        }

        NextState();
    }

    IEnumerator DoneState()
    {
        MGScene.Instance.LoadingDone();

        while (loadingState == eLoadingStatus.Done)
        {
            yield return null;
        }

        NextState();
    }

    void NextState()
    {
        _sb.Remove(0, _sb.Length);
        _sb.Append(loadingState.ToString());
        _sb.Append("State");

        MethodInfo info = GetType().GetMethod(_sb.ToString(), BindingFlags.NonPublic | BindingFlags.Instance);
        StartCoroutine((IEnumerator)info.Invoke(this, null));
    }

    void Update()
    {
        //Debug.Log(loadingState);
    }
}
