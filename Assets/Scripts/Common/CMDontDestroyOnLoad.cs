using UnityEngine;
using System.Collections;

public class CMDontDestroyOnLoad : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
