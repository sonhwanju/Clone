using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;


[CustomEditor(typeof(MGPool))]
public class EDMGPool : Editor
{
    private MGPool myMGPool;

    private int prevPoolHeroCount;

    private List<GameObject> removeObjList;
    private List<int> removeCountList;

    void Awake()
    {
        removeObjList = new List<GameObject>();
        removeCountList = new List<int>();
    }

    void OnEnable()
    {
        myMGPool = (MGPool)target;

        prevPoolHeroCount = myMGPool.poolHeroMaxCount;
    }

    public override void OnInspectorGUI()
    {
        if (myMGPool.poolHeroObjList == null)
            myMGPool.poolHeroObjList = new List<GameObject>();

        if (myMGPool.poolHeroCountList == null)
            myMGPool.poolHeroCountList = new List<int>();

        generateList();
    }

    public void generateList()
    {
        ///////////////////////////////////////////////////////
        GUILayout.Space(15);
        GUILayout.Label(" Hero Pool");
        createInspectorUI(ref prevPoolHeroCount, ref myMGPool.poolHeroMaxCount, myMGPool.poolHeroObjList, myMGPool.poolHeroCountList);
    }

    private void createInspectorUI(ref int inPrev , ref int inNow , List<GameObject> inObjList , List<int> inCountList)
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Pool Object", GUILayout.Width(150)))
            inNow++;
        else if (GUILayout.Button("Remove Pool Object", GUILayout.Width(150)) && inNow > 0)
            inNow--;
        EditorGUILayout.EndHorizontal();

        if (inPrev != inNow)
        {
            int gap = 0;

            if (inPrev > inNow)
            {
                gap = inPrev - inNow;

                for (int i = 0; i < gap; i++)
                {
                    inObjList.Remove(inObjList[inObjList.Count - 1]);
                    inCountList.Remove(inCountList[inCountList.Count - 1]);
                }
            }
            else if (inPrev < inNow)
            {
                gap = inNow - inPrev;

                for (int i = 0; i < gap; i++)
                {
                    inObjList.Add(null);
                    inCountList.Add(0);
                }
            }

            inPrev = inNow;
        }

        for (int i = 0; i < inNow; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Prefab", GUILayout.Width(50));
            inObjList[i] = EditorGUILayout.ObjectField(inObjList[i], typeof(GameObject), true) as GameObject;
            inCountList[i] = EditorGUILayout.IntField(inCountList[i], GUILayout.Width(30));

            if (GUILayout.Button("Remove", GUILayout.Width(60)) && inNow > 0)
            {
                removeObjList.Add(inObjList[i]);
                removeCountList.Add(inCountList[i]);
            }
            EditorGUILayout.EndHorizontal();
        }

        for (int i = 0; i < removeObjList.Count; i++)
        {
            inObjList.Remove(removeObjList[i]);
            inCountList.Remove(removeCountList[i]);
            inNow--;
        }

        if (removeObjList.Count > 0)
        {
            removeObjList.Clear();
            removeCountList.Clear();
            inPrev = inNow;
        }
    }
}
