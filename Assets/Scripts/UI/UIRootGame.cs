using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRootGame : MonoBehaviour
{
    [SerializeField]
    private Button nextWaveBtn;

    void Awake()
    {
        GameSceneClass.gUiRootGame = this;
    }

    private void Start()
    {
        nextWaveBtn.onClick.AddListener(() =>
        {
            
        });
    }

    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //List<string> keyList = new List<string>(Global.spritesDic.Keys);
            //int randomIdx = Random.Range(0, keyList.Count - 1);
            
            //testImage.sprite = Global.spritesDic[keyList[randomIdx]];
            //testImage.SetNativeSize();
        }
    }

    public void TestFunc()
    {
        print("call UIRootGame");
    }
}
