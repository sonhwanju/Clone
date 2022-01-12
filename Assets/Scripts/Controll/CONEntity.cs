using UnityEngine;
using System.Collections;

public class CONEntity : MonoBehaviour
{
    public ePrefabs myKind;
    
    protected GameObject myObj;
    public Transform myTrm;
    
    public Vector3 myVelocity;

    protected bool bFirstUpdate;

    public virtual void Awake()
    {
        myTrm = this.transform;
        myObj = this.gameObject;
    }

    public virtual void OnEnable()
    {
        bFirstUpdate = false;
    }

    public virtual void OnDisable()
    {

    }

    public virtual void Start()
    {

    }

    public virtual void OnDrawGizmos()
    {

    }

    protected virtual void firstUpdate()
    {
        bFirstUpdate = true;
    }

    public void SetActive(bool bValue)
    {
        cleanUpOnDisable();

        myObj.SetActive(bValue);

        if (!bValue)
        {
            myTrm.position = Vector3.zero;
            myTrm.SetParent(GameSceneClass.gMGPool.myTrm, false);

            if (GameSceneClass.gMGGame == null)
                return;

            //GameSceneClass.gMGGame._gTeamManager.removeTeam(this, _myTeam);
        }
    }

    protected virtual void cleanUpOnDisable()
    {

    }

    public void SetPosition(Vector3 inVec)
    {
        myTrm.position = inVec;
    }

    public void SetLocalPosition(Vector3 inVec)
    {
        myTrm.localPosition = inVec;
    }

    public bool IsActive()
    {
        return myObj.activeInHierarchy;
    }

    public virtual void Update()
    {
        if (bFirstUpdate)
            return;

        firstUpdate();
    }

    public virtual void LateUpdate()
    {
        float nextYpos = myTrm.position.y + myVelocity.y * Time.deltaTime;

        myTrm.position = new Vector3(myTrm.position.x + myVelocity.x * Time.deltaTime, nextYpos, myTrm.position.z);
    }
}
