using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ControlNPCGuard : MonoBehaviour
{
    public List<GameObject> wayPoints = new List<GameObject>();
    int wayPointIndex = 0;

    [HideInInspector]
    public Animator anim;
    AnimatorStateInfo info;

    public enum GUARD_TYPE { IDLE = 0, PATROLLER = 1, CHASER = 2 };

    [SerializeField]
    public GUARD_TYPE guardType;

    [SerializeField] 
    public GameObject player;

    bool playerActivated;

    [SerializeField] 
    bool canHearPlayer;

    private void Start()
    {
        anim = GetComponent<Animator>();

        if (guardType == GUARD_TYPE.IDLE)
        {
            anim.SetBool("isIdleGuard", true);
        }
        else if (guardType == GUARD_TYPE.PATROLLER)
        {
            anim.SetBool("isPatrolGuard", true);
        }
        else if (guardType == GUARD_TYPE.CHASER)
        {
            anim.SetBool("isChaserGuard", true);
        }
    }

    private void Update()
    {
        if (!playerActivated)
        {
            player = GameObject.Find("Player");
            playerActivated = true;
        }

        info = anim.GetCurrentAnimatorStateInfo(0);

        if (info.IsName("Idle") || info.IsName("Patrol"))
        {
            if (isNearPlayer() && canHearPlayer)
            {
                SetGuardType(GUARD_TYPE.CHASER);
            }
        }

        if (isNearPlayer())
        {
            anim.SetBool("isWithinAttackingRange", true);
        }
        else
        {
            anim.SetBool("isWithinAttackingRange", false);
        }

        //Sets patrol route
        if (info.IsName("Patrol"))
        {
            GetComponent<NavMeshAgent>().isStopped = false;
            if (Vector3.Distance(transform.position, wayPoints[wayPointIndex].transform.position) < 1.5f)
            {
                wayPointIndex++;
            }

                if (wayPointIndex > wayPoints.Count + 1)
                {
                    wayPointIndex = 0;
                }

            GetComponent<NavMeshAgent>().SetDestination(wayPoints[wayPointIndex].transform.position);
        }

        if (info.IsName("Chase"))
        {
            GetComponent<NavMeshAgent>().speed = 2.5f;
            GetComponent<NavMeshAgent>().isStopped = false;
            GetComponent<NavMeshAgent>().SetDestination(player.transform.position);
        }
        else if (info.IsName("Attack"))
        {
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
            GetComponent<NavMeshAgent>().isStopped = true;
            if (info.normalizedTime % 1.0f >= 0.98f && isVeryNearPlayer())
            {
                player.GetComponent<ControlPlayer>().DecreaseHealth(10);
            }
        }

        /*
        if (info.IsName("Idle"))
        {
            if (isNearPlayer())
            {
                SetGuardType(GUARD_TYPE.CHASER);
            }
        }
        */
    }

    bool isNearPlayer()
    {

        if (Vector3.Distance(transform.position, player.transform.position) < 2.0f)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public void Dies()
    {

        anim.SetTrigger("isDying");

    }

    bool isVeryNearPlayer()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < 1.5f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetGuardType(GUARD_TYPE newType)
    {
        guardType = newType;
        anim.SetBool("isIdleGuard", false);
        anim.SetBool("isPatrolGuard", false);
        anim.SetBool("isChaserGuard", false);
        switch (guardType)
        {
            case GUARD_TYPE.IDLE: anim.SetBool("isIdleGuard", true);
                break;

            case GUARD_TYPE.PATROLLER: anim.SetBool("isPatrolGuard", true);
                break;
            
            case GUARD_TYPE.CHASER: anim.SetBool("isChaserGuard", true);
                break;

            default: anim.SetBool("isIdleGuard", true);
                break;
        }
    }
}
