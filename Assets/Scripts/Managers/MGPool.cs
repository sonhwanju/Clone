using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MGPool : MonoBehaviour
{
    // ePrefabs을 키값으로 하는 CONEntity리스트 딕셔너리
    private Dictionary<ePrefabs, List<CONEntity>> poolTotalDic;

    // 각 객체 타입별 리스트 및 최대갯수
    public List<GameObject> poolObjList;
    public List<int> poolObjCount;


    // 개별 오브젝트풀 관련 변수 
    public List<GameObject> poolHeroObjList;
    public List<int> poolHeroCountList;
    public int poolHeroMaxCount;

    public Transform myTrm;

    void Awake()
    {
        GameSceneClass.gMGPool = this;

        poolTotalDic = new Dictionary<ePrefabs, List<CONEntity>>();

        poolObjList = new List<GameObject>();
        poolObjCount = new List<int>();

        // 게임에서 필요한 객체타입별로 갯수 세팅
        poolHeroMaxCount = poolHeroObjList.Count;

        myTrm = this.transform;

        createPool();
    }

    private void createPool()
    {
        for (int i = 0; i < poolHeroObjList.Count; i++)
        {
            poolObjList.Add(poolHeroObjList[i]);
            poolObjCount.Add(poolHeroCountList[i]);
        }

        // 개별 객체 초기화 계속...

        
        // 객체별 최대갯수 만큼 추가
        for (int i = 0; i < poolObjList.Count; i++)
        {
            CONEntity myEn = null;
            ePrefabs myKind = ePrefabs.None;

            myEn = poolObjList[i].GetComponent<CONEntity>();

            if (myEn == null)
            {
                Debug.LogWarning(" **** Entity Controller Needed **** ");
                continue;
            }
            
            myKind = myEn.myKind;
            poolTotalDic[myKind] = new List<CONEntity>();

            for (int j = 0; j < poolObjCount[i]; j++)
            {
                myEn = (instantiateObj(myKind)).GetComponent<CONEntity>();
                myEn.SetActive(false);
                myEn.SetPosition(Vector3.zero);
                poolTotalDic[myKind].Add(myEn);
            }
        }
    }

    private GameObject instantiateObj(ePrefabs inObj)
    {
        //Debug.Log(inObj.ToString());
        GameObject myGo = GameObject.Instantiate(Global.prefabsDic[inObj]) as GameObject;

        return myGo;
    }

    public CONEntity CreateObj(ePrefabs inObj , Vector3 inPos)
    {
        CONEntity createdEn = null;
        bool bNotEnough = true;

        for (int i = 0; i < poolTotalDic[inObj].Count; i++)
        {
            if (!poolTotalDic[inObj][i].IsActive())
            {
                createdEn = poolTotalDic[inObj][i];
                createdEn.SetActive(true);
                bNotEnough = false;
                break;
            }            
        }

        if (bNotEnough)
        {
            createdEn = instantiateObj(inObj).GetComponent<CONEntity>();
            poolTotalDic[inObj].Add(createdEn);    
        }

        if (createdEn != null)
            createdEn.SetPosition(inPos);

        createdEn.myTrm.parent = null;

        return createdEn;
    }

    public CONEntity CreateObjAsChild(ePrefabs inObj, Vector3 inPos , Transform inParent)
    {
        CONEntity createdEn = null;
        bool bNotEnough = true;

        for (int i = 0; i < poolTotalDic[inObj].Count; i++)
        {
            if (!poolTotalDic[inObj][i].IsActive())
            {
                createdEn = poolTotalDic[inObj][i];
                createdEn.SetActive(true);
                bNotEnough = false;
                break;
            }
        }

        if (bNotEnough)
        {
            createdEn = instantiateObj(inObj).GetComponent<CONEntity>();
            poolTotalDic[inObj].Add(createdEn);
        }

        if (createdEn != null)
        {
            createdEn.myTrm.SetParent(null);
			createdEn.myTrm.SetParent(inParent , false);
            createdEn.SetLocalPosition(inPos);
            createdEn.SetActive(true);
        }
        else
            Debug.Log(" _____ created En (HP Gauge) is Null !!!!!  ");

        
        return createdEn;
    }

    public void DeleteObj(CONEntity inEn)
    {
        inEn.SetActive(false);
    }
}
