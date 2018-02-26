using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {

    #region Inputs
    public float horizontal;
    public float vertical;
    public float mouse1;
    public float mouse2;
    public float fire3;
    public float middleMouse;
    public float mouseX;
    public float mouseY;
    public bool jumpInput;
    public bool runInput;
    #endregion

    [HideInInspector]
    public FreeCameraLook camProperties;
    [HideInInspector]
    public Transform camPivot;
    [HideInInspector]
    public Transform camTrans;

    CrosshairManager crosshairManager;
    [HideInInspector]
    public Crosshair crosshair;

    ShakeCamera shakeCam;
    StateManager states;

    HandleMovement_Player hMove;

    #region Camera Settings
    public float normalFov = 60;
    public float aimingFov = 40;
    float targetFov;
    float curFov;

    public float cameraNormalX = 0.5f;
    public float cameraAimingX = 0;
    public float cameraNormalY = 1.5f ;
    public float cameraAimingY = 0;
    public float cameraNormalZ = -6;
    public float cameraAimingZ = -0.86f;
    
    float targetZ;
    float curZ;
    float actualZ;
    LayerMask layerMask;

    public float shakeRecoil = 0.05f;
    public float shakeMovement = 0.05f;
    public float shakeMin = 0f;
    float targetShake;
    float curShake;
    #endregion

    // Use this for initialization
    void Start ()
    {
        crosshairManager = CrosshairManager.GetInstance();
        crosshair = crosshairManager.activeCrosshair.GetComponent<Crosshair>();
        camProperties = FreeCameraLook.GetInstance();
        camPivot = camProperties.transform.GetChild(0);
        camTrans = camPivot.GetChild(0);
        shakeCam = camPivot.GetComponentInChildren<ShakeCamera>();

        states = GetComponent<StateManager>();

        layerMask = ~(1 << gameObject.layer);
        states.layerMask = layerMask;

        gameObject.AddComponent<HandleMovement_Player>();
        hMove = GetComponent<HandleMovement_Player>();

        states.isPlayer = true;
        states.Init();
        Debug.Log(states.anim.name + states.anim.avatar);
        hMove.Init(states, this);

        FixPlayerMeshes();
	}

    void Update()
    {
        states.RegularTick();
       
    }

    void FixedUpdate()
    {
        states.FixedTick();
        HandleInput();
        UpdateStates();
        hMove.Tick();
        HandleShake();

        //Find where camera is looking
        Ray ray = new Ray(camTrans.position, camTrans.forward);
        states.lookPosition = ray.GetPoint(20);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction, Color.blue);

        if (Physics.Raycast(ray.origin, ray.direction, out hit, 100, layerMask))
        {
            states.lookHitPosition = hit.point;
        }
        else
        {
            states.lookHitPosition = states.lookPosition;
        }


        //Check for obstacles in front of camera
        CameraCollision(layerMask);

        //Update camera's postion
        curZ = Mathf.Lerp(curZ, actualZ, Time.deltaTime * 15);
        camTrans.localPosition = new Vector3(cameraNormalX, cameraNormalY, curZ);
    }

    void HandleInput()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        mouse1 = Input.GetAxis("Fire1");
        mouse2 = Input.GetAxis("Fire2");
        middleMouse = Input.GetAxis("Mouse ScrollWheel");
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        fire3 = Input.GetAxis("Fire3");

        jumpInput = Input.GetButtonDown(Statics.Jump);
        runInput = Input.GetButton(Statics.Run);
    }

    void UpdateStates()
    {
        states.aiming = states.onGround && (mouse2 > 0);
        states.canRun = !states.aiming;
        states.walk = (fire3 > 0);

        states.horizontal = horizontal;
        states.vertical = vertical;
        states.moving = states.horizontal != 0 || states.vertical != 0;

        Vector3 h = camTrans.right * horizontal;
        Vector3 v = camTrans.forward * vertical;

        h.y = 0;
        v.y = 0;

        Vector3 moveDir = (h + v).normalized;
        states.moveDirection = moveDir;
        states.inAngle_MoveDir = InAngle(states.moveDirection, 90);

        if (states.walk && horizontal != 0 || states.walk && vertical != 0)
        {
            states.inAngle_MoveDir = true;
        }

        states.onLocomotion = states.anim.GetBool(Statics.onLocomotion);
        HandleRun();
        states.jumpInput = jumpInput;

        if (states.aiming)
        {
            targetZ = cameraAimingZ;
            targetFov = aimingFov;
        }
        else
        {
            targetZ = cameraNormalZ;
            targetFov = normalFov;
        }

        if (mouse1 > 0.5 && !states.reloading)
            states.shoot = true;
        else
            states.shoot = false;

    }

    void FixPlayerMeshes()
    {
        SkinnedMeshRenderer[] skinned = GetComponentsInChildren<SkinnedMeshRenderer>();
        for (int i = 0; i < skinned.Length; i++)
        {
            skinned[i].updateWhenOffscreen = true;
        }
    }

    bool InAngle(Vector3 targetDir, float angleTreshold)
    {
        bool retVal = false;
        float angle = Vector3.Angle(transform.forward, targetDir);

        if (angle < angleTreshold)
            retVal = true;

        return retVal;
    }

    void HandleRun()
    {
        bool canRun = states.canRun;

        if (runInput && canRun && !states.aiming)
        {
            states.walk = false;
            states.run = true;
        }
        else
        {
            states.walk = true;
            states.run = false;
        }

        if ((horizontal != 0 || vertical != 0) && !states.aiming)
        {
            states.run = runInput;
            states.anim.SetInteger(Statics.specialType, 
                Statics.GetAnimSpecialType(AnimSpecials.run));
        }
        else
        {
            if (states.run)
                states.run = false;
        }

        if (!states.inAngle_MoveDir && hMove.doAngleCheck)
            states.run = false;

        if (states.run == false)
        {
            states.anim.SetInteger(Statics.specialType, 
                Statics.GetAnimSpecialType(AnimSpecials.runToStop));
        }
    }

    void HandleShake()
    {
        if (states.aiming)
        {
            crosshair.defaultSpread = 0.6f;
        }
        else
        {
            crosshair.defaultSpread = 1.2f;
        }

        if (states.shoot && states.canShoot)
        {
            if (states.aiming)
            {
                targetShake = shakeRecoil;
                camProperties.WiggleCrosshairAndCamera(0.05f);
                targetFov += 3;
            }
            else
            {
                states.anim.SetTrigger("Fire");
                targetShake = shakeRecoil;
                camProperties.WiggleCrosshairAndCamera(0.1f);
                targetFov += 5;
            }
        }
        else
        {
            if (states.vertical != 0)
            {
                targetShake = shakeMovement;
            }
            else
            {
                targetShake = shakeMin;
            }
        }

        curShake = Mathf.Lerp(curShake, targetShake, Time.deltaTime * 10);
        shakeCam.positionShakeSpeed = curShake;

        curFov = Mathf.Lerp(curFov, targetFov, Time.deltaTime * 5);
        Camera.main.fieldOfView = curFov;
    }

    void CameraCollision(LayerMask layerMask)
    {
        //Do a raycast from the pivot to the camera
        Vector3 origin = camPivot.TransformPoint(Vector3.zero);
        Vector3 direction = camTrans.TransformPoint(Vector3.zero) - camPivot.TransformPoint(Vector3.zero);
        RaycastHit hit;

        //The distance of the raycast is controlled by if we are aiming or not
        actualZ = targetZ;

        //If an obstacle is found
        if (Physics.Raycast(origin, direction, out hit, Mathf.Abs(targetZ), layerMask))
        {
            //If we hit something, then find that distance
            float dist = Vector3.Distance(camPivot.position, hit.point);
            actualZ = -dist;
        }
    }
}
