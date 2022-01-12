using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRootTitle : MonoBehaviour
{
    private float remainTime = 2f;

    void Update()
    {
        remainTime -= Time.deltaTime;
        if (remainTime <= 0f)
        {
            MGScene.Instance.ChangeScene(eSceneName.Game);
        }
    }
}
