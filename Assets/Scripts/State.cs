using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State
{
    public enum eState  // 가질 수 있는 상태 나열
    {
        IDLE, PATROL, PURSUE, ATTACK, DEAD, RUNAWAY
    };

    public enum eEvent  // 이벤트 나열
    {
        ENTER, UPDATE, EXIT
    };

    public eState stateName;

    protected eEvent curEvent;

    protected GameObject myObj;
    protected NavMeshAgent myAgent;
    protected Animator myAnim;
    protected Transform enemyTrm;  // 타겟팅 할 플레이어의 트랜스폼

    LayerMask whatIsEnemy = 1 << LayerMask.NameToLayer("Enemy");

    protected State nextState;  // 다음 상태를 나타냄

    float shootDist = 7.0f;

    public State(GameObject obj, Transform targetTrm)
    {
        myObj = obj;
        enemyTrm = targetTrm;

        // 최초 이벤트를 엔터로
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

    // 공격 범위 체크 로직
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
        //     // 순찰하겠다.
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
//        // curIndex = 0;   // 초기 인덱스 세팅

//        // 플레이어 추격 후 정찰모드(Patrol)로 돌아왔을 때 가장 가까운 웨이포인트로 돌아가게 해보시오.
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
//        // 체크포인트 순회 로직
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
//        // 추적 로직
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
        // 내 각도를 플레이어 방향으로 틀어줘야 함(feat. 스무스하게 돌려줘)
        //Vector3 dir = enemyTrm.position - myObj.transform.position;
        //float angle = Vector3.Angle(dir, myObj.transform.forward);
        //dir.y = 0;

        //myObj.transform.rotation = Quaternion.Slerp(myObj.transform.rotation,
        //                                            Quaternion.LookRotation(dir),
        //                                            Time.deltaTime * rotationSpeed);

        // 타겟팅된 유닛이 사라졌는지 체크 후 다시 Idle로 돌아가야 함
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