using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State
{
    public enum eState  // ���� �� �ִ� ���� ����
    {
        IDLE, PATROL, PURSUE, ATTACK, DEAD, RUNAWAY
    };

    public enum eEvent  // �̺�Ʈ ����
    {
        ENTER, UPDATE, EXIT
    };

    public eState stateName;

    protected eEvent curEvent;

    protected GameObject myObj;
    protected NavMeshAgent myAgent;
    protected Animator myAnim;
    protected Transform enemyTrm;  // Ÿ���� �� �÷��̾��� Ʈ������

    LayerMask whatIsEnemy = 1 << LayerMask.NameToLayer("Enemy");

    protected State nextState;  // ���� ���¸� ��Ÿ��

    float shootDist = 7.0f;

    public State(GameObject obj, Transform targetTrm)
    {
        myObj = obj;
        enemyTrm = targetTrm;

        // ���� �̺�Ʈ�� ���ͷ�
        curEvent = eEvent.ENTER;
    }

    public virtual void Enter() { curEvent = eEvent.UPDATE; }
    public virtual void Update() { curEvent = eEvent.UPDATE; }
    public virtual void Exit() { curEvent = eEvent.EXIT; }

    public State Process()
    {
        if (curEvent == eEvent.ENTER) Enter();
        if (curEvent == eEvent.UPDATE) Update();
        if (curEvent == eEvent.EXIT)
        {
            Exit();
            return nextState;
        }

        return this;
    }

    // ���� ���� üũ ����
    public bool CanAttackEnemy()
    {

        Collider[] col = Physics.OverlapSphere(myObj.transform.position, shootDist,whatIsEnemy);
        int idx = 0;
        Vector3 dir = Vector3.zero;
        if (col.Length > 0)
        {
            float lastDist = Mathf.Infinity;
            for (int i = 0; i < col.Length; i++)
            {
                
                GameObject thisWP = col[i].gameObject;
                float distance = Vector3.Distance(myObj.transform.position, thisWP.transform.position);

                if (distance < lastDist)
                {
                    idx = i;
                    lastDist = distance;
                }
            }
        }
        else
        {
            return false;
        }
        // = enemyTrm.position - myObj.transform.position;
        enemyTrm = col[idx].transform;
        return true;
    }
}

public class Idle : State
{
    public Idle(GameObject obj, Transform targetTrm)
              : base(obj, targetTrm)
    {
        stateName = eState.IDLE;
    }

    public override void Enter()
    {
        //myAnim.SetTrigger("isIdle");
        base.Enter();
    }

    public override void Update()
    {
        // if(Random.Range(0, 100) < 10)
        // {
        //     // �����ϰڴ�.
        //     nextState = new Patrol(myObj, myAgent, myAnim, playerTrm);
        //     curEvent = eEvent.EXIT;
        // }

        if (CanAttackEnemy())
        {
            nextState = new Attack(myObj, enemyTrm);
            curEvent = eEvent.EXIT;
        }
    }

    public override void Exit()
    {
        //myAnim.ResetTrigger("isIdle");
        base.Exit();
    }
}

//public class Patrol : State
//{
//    int curIndex = -1;

//    public Patrol(GameObject obj,Transform targetTrm)
//                : base(obj, targetTrm)
//    {
//        stateName = eState.PATROL;
//        //myAgent.speed = 2;
//        //myAgent.isStopped = false;
//    }

//    public override void Enter()
//    {
//        // curIndex = 0;   // �ʱ� �ε��� ����

//        // �÷��̾� �߰� �� �������(Patrol)�� ���ƿ��� �� ���� ����� ��������Ʈ�� ���ư��� �غ��ÿ�.
//        //float lastDist = Mathf.Infinity;
//        //for (int i = 0; i < GameEnvironment.Instance.checkPointList.Count; i++)
//        //{
//        //    GameObject thisWP = GameEnvironment.Instance.checkPointList[i];
//        //    float distance = Vector3.Distance(myObj.transform.position, thisWP.transform.position);

//        //    if (distance < lastDist)
//        //    {
//        //        curIndex = i - 1;
//        //        lastDist = distance;
//        //    }
//        //}

//        //myAnim.SetTrigger("isWalking");
//        base.Enter();
//    }

//    public override void Update()
//    {
//        // üũ����Ʈ ��ȸ ����
//        //if (myAgent.remainingDistance < 1)
//        //{
//        //    if (curIndex >= GameEnvironment.Instance.checkPointList.Count - 1)
//        //    {
//        //        curIndex = 0;
//        //    }
//        //    else
//        //    {
//        //        curIndex++;
//        //    }

//        //    myAgent.SetDestination(GameEnvironment.Instance.checkPointList[curIndex].transform.position);

//        //    if (CanSeePlayer())
//        //    {
//        //        nextState = new Pursue(myObj, myAgent, myAnim, playerTrm);
//        //        curEvent = eEvent.EXIT;
//        //    }
//        //}
//        if(CanAttackEnemy())
//        {
//            nextState = new Attack()
//        }
//    }

//    public override void Exit()
//    {
//        //myAnim.ResetTrigger("isWalking");
//        base.Exit();
//    }
//}

//public class Pursue : State
//{
//    public Pursue(GameObject obj, NavMeshAgent agent, Animator anim, Transform targetTrm)
//              : base(obj, agent, anim, targetTrm)
//    {
//        stateName = eState.PURSUE;
//        myAgent.speed = 5;
//        myAgent.isStopped = false;
//    }

//    public override void Enter()
//    {
//        myAnim.SetTrigger("isRunning");
//        base.Enter();
//    }

//    public override void Update()
//    {
//        // ���� ����
//        myAgent.SetDestination(enemyTrm.position);
//        if (myAgent.hasPath)
//        {
//            if (CanAttackEnemy())
//            {
//                nextState = new Attack(myObj, myAgent, myAnim, enemyTrm);
//                curEvent = eEvent.EXIT;
//            }
//            else if (!CanSeeEnemy())
//            {
//                nextState = new Patrol(myObj, myAgent, myAnim, enemyTrm);
//                curEvent = eEvent.EXIT;
//            }
//        }
//    }

//    public override void Exit()
//    {
//        myAnim.ResetTrigger("isRunning");
//        base.Exit();
//    }
//}

public class Attack : State
{
    float rotationSpeed = 2.0f;
    float attackTime = 0.2f;
    float curTime = 0f;
    public Attack(GameObject obj, Transform targetTrm)
              : base(obj, targetTrm)
    {
        stateName = eState.ATTACK;
    }

    public override void Enter()
    {
        //myAnim.SetTrigger("isShooting");
        //myAgent.isStopped = true;
        base.Enter();
    }

    public override void Update()
    {
        // �� ������ �÷��̾� �������� Ʋ����� ��(feat. �������ϰ� ������)
        //Vector3 dir = enemyTrm.position - myObj.transform.position;
        //float angle = Vector3.Angle(dir, myObj.transform.forward);
        //dir.y = 0;

        //myObj.transform.rotation = Quaternion.Slerp(myObj.transform.rotation,
        //                                            Quaternion.LookRotation(dir),
        //                                            Time.deltaTime * rotationSpeed);

        // Ÿ���õ� ������ ��������� üũ �� �ٽ� Idle�� ���ư��� ��
        if (enemyTrm == null)
        {
            nextState = new Idle(myObj, enemyTrm);
            curEvent = eEvent.EXIT;
        }
        else
        {
           if(curTime >= attackTime)
            {
                enemyTrm.GetComponent<CONEnemy>().Hp -= 10;
                enemyTrm = null;
                //Debug.Log("attack!");
                curTime = 0f;
            }
           else
            {
                curTime += Time.deltaTime;
            }
        }
    }

    public override void Exit()
    {
        //myAnim.ResetTrigger("isShooting");
        //shootEff.Stop();
        base.Exit();
    }
}