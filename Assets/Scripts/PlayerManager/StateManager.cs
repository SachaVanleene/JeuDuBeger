using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour {

    [Header("Info")]
    public GameObject modelPrefab;
    public bool inGame;
    public bool isPlayer;

    [Header("Stats")]
    public float groundDistance = 0.6f;
    public float groundOffset = 0.2f;
    public float distanceToCheckForward = 1.3f;
    public float runSpeed = 6;
    public float walkSpeed = 4;
    public float jumpForce = 15;
    public float airTimeTreshold = 0.8f;

    [Header("Inputs")]
    public float horizontal;
    public float vertical;
    public bool jumpInput;
    public bool runInput;

    [Header("States")]
    public bool obstacleForward;
    public bool groundForward;
    public float groundAngle;
    public bool moving;

    #region StateRequests
    [Header("Movement State Requests")]
    public CharStates curState;
    public bool onGround;
    public bool run;
    public bool walk;
    public bool onLocomotion;
    public bool inAngle_MoveDir;
    public bool jumping;
    public bool canJump;

    [Header("Shoot State Requests")]
    public bool aiming;
    public bool canRun;
    public bool shoot;
    public bool canShoot;
    public bool reloading;
    #endregion

    #region References
    GameObject activeModel;
    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public Rigidbody rb;
    #endregion

    #region Variables
    [HideInInspector]
    public Vector3 moveDirection;
    [HideInInspector]
    public Vector3 aimPosition;
    Transform aimHelper;
    float curX;
    float curY;
    public float airTime;
    [HideInInspector]
    public bool prevGround;
    #endregion

    
    public Vector3 lookPosition;
    public Vector3 lookHitPosition;
    public LayerMask layerMask;

    LayerMask ignoreLayers;

    //public CharacterAudioManager audioManager;

    [HideInInspector]
    public HandleShooting handleShooting;

    [HideInInspector]
    public HandleAnimations handleAnim;

    public enum CharStates
    {
        idle,moving,onAir,hold
    }

    // Use this for initialization
    void Start () {
        handleShooting = GetComponent<HandleShooting>();
        handleAnim = GetComponent<HandleAnimations>();
        
	}
	
	void FixedUpdate () {
        onGround = IsOnGround();	
	}

    #region Init Phase
    public void Init()
    {
        inGame = true;
        activeModel = gameObject.transform.GetChild(1).gameObject;
        SetupAnimator();
        AddControllerReferences();
        canJump = true;

        gameObject.layer = 8;
        ignoreLayers = ~(1 << 3 | 1 << 8);

    }

    void CreateModel()
    {
        activeModel = Instantiate(modelPrefab) as GameObject;
        activeModel.transform.parent = this.transform;
        activeModel.transform.localPosition = Vector3.zero;
        activeModel.transform.localEulerAngles = Vector3.zero;
        activeModel.transform.localScale = Vector3.one;
        
    }

    void SetupAnimator()
    {
        anim = GetComponent<Animator>();
        Animator childAnim = activeModel.GetComponent<Animator>();
        if (childAnim != null)
        {
            anim.avatar = childAnim.avatar;
            Destroy(childAnim);
        }
    }

    void AddControllerReferences()
    {
        gameObject.AddComponent<Rigidbody>();
        rb = GetComponent<Rigidbody>();
        rb.angularDrag = 999;
        rb.drag = 4;
        rb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
    }
    #endregion

    public void RegularTick()
    {
        onGround = IsOnGround();
    }

    public void FixedTick()
    {
        obstacleForward = false;
        groundForward = false;
        onGround = IsOnGround();

        if (onGround)
        {
            Vector3 origin = transform.position;
            //Clear forward
            origin += Vector3.up * 0.75f;
            IsClear(origin, transform.forward, distanceToCheckForward, ref obstacleForward);
            if (!obstacleForward)
            {
                //Is ground forward ?
                origin += transform.forward * 0.6f;
                IsClear(origin, -Vector3.up, groundDistance * 3, ref groundForward);
            }
            else
            {
                if (Vector3.Angle(transform.forward, moveDirection) > 30)
                {
                    obstacleForward = false;
                }
            }
        }

        
    }

    void UpdateState()
    {
        if (curState == CharStates.hold)
            return;

        if (moving)
            curState = CharStates.moving;
        else
            curState = CharStates.idle;

        if (!onGround)
            curState = CharStates.onAir;
    }

    bool IsOnGround()
    {
        bool retVal = false;

        if (curState == CharStates.hold)
            return false;

        Vector3 origin = transform.position + Vector3.up * 0.8f;
        RaycastHit hit = new RaycastHit();

        bool isHit = false;

        FindGround(origin, ref hit, ref isHit);

        if (!isHit)
        {
            for (int i = 0; i < 4; i++)
            {
                Vector3 newOrigin = origin;

                switch (i)
                {
                    case 0: //forward
                        newOrigin += Vector3.forward / 3;
                        break;

                    case 1: //backwards
                        newOrigin -= Vector3.forward / 3;
                        break;

                    case 2: //left
                        newOrigin -= Vector3.right/ 3;
                        break;

                    case 3: //right
                        newOrigin += Vector3.right/ 3;
                        break;

                    default:
                        break;
                }

                FindGround(newOrigin, ref hit, ref isHit);

                if (isHit == true)
                    break;
            }
        }

        retVal = isHit;

        if (retVal != false)
        {
            Vector3 targetPosition = transform.position;
            targetPosition.y = hit.point.y + groundOffset;
            transform.position = targetPosition;
        }

        return retVal;
    }

    void FindGround(Vector3 origin, ref RaycastHit hit, ref bool isHit)
    {
        Debug.DrawRay(origin, -Vector3.up * 0.7f, Color.red);
        if (Physics.Raycast(origin, -Vector3.up, out hit, 1, ignoreLayers))
        {
            isHit = true;
        }
    }

    void IsClear(Vector3 origin, Vector3 direction, float distance, ref bool isHit)
    {
        RaycastHit hit;
        Debug.DrawRay(origin, direction * distance, Color.green);
        if (Physics.Raycast(origin, direction, out hit, distance, ignoreLayers))
        {
            isHit = true;
        }
        else
        {
            isHit = false;
        }

        if (obstacleForward)
        {
            Vector3 incomingVect = hit.point - origin;
            Vector3 reflectVect = Vector3.Reflect(incomingVect, hit.normal);
            float angle = Vector3.Angle(incomingVect, reflectVect);

            if (angle < 70)
            {
                isHit = false;
            }
        }

        if (groundForward)
        {
            if (moving)
            {
                Vector3 p1 = transform.position;
                Vector3 p2 = hit.point;
                float diffY = p1.y - p2.y;
                groundAngle = diffY;
            }

            float targetIncline = 0;

            if (Mathf.Abs(groundAngle) > 0.3f)
            {
                if (groundAngle < 0)
                    targetIncline = 1;
                else
                    targetIncline = -1;
            }

            if (groundAngle == 0)
                targetIncline = 0;

            anim.SetFloat(Statics.incline, targetIncline, 0.3f, Time.deltaTime);
        }
    }

    void MonitorAirTime()
    {
        if (!jumping)
            anim.SetBool(Statics.onAir, !onGround);

        if (onGround)
        {
            if (prevGround != onGround)
            {
                anim.SetInteger(Statics.jumpType,
                    (airTime > airTimeTreshold) ?
                    (moving) ?
                    2 : 1 : 0);
            }

            airTime = 0;
        }
        else
        {
            airTime += Time.deltaTime;
        }

        prevGround = onGround;
    }

    public void LegFront()
    {
        Vector3 ll = anim.GetBoneTransform(HumanBodyBones.LeftFoot).position;
        Vector3 rl = anim.GetBoneTransform(HumanBodyBones.RightFoot).position;
        Vector3 rel_ll = transform.InverseTransformPoint(ll);
        Vector3 rel_rl = transform.InverseTransformPoint(rl);

        bool left = rel_ll.z > rel_rl.z;
        anim.SetBool(Statics.mirrorJump, left);
    }
}
