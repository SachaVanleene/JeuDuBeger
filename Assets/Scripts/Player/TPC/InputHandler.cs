using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPC
{
    public class InputHandler : MonoBehaviour
    {
        [Header("Inputs")]
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

        Player player;

        [Space]
        [Header("Camera Settings")]
        #region Camera Settings
        public float normalFov = 60;
        public float aimingFov = 40;
        float targetFov;
        float curFov;

        public float cameraNormalX = 0.5f;
        public float cameraAimingX = 0;
        public float cameraNormalY = 1.5f;
        public float cameraAimingY = 0;
        public float cameraNormalZ = -6;
        public float cameraAimingZ = -0.86f;

        float targetZ;
        float curZ;
        float actualZ;
        public LayerMask shotLayerMask;
        public LayerMask camLayerMask;

        public float shakeRecoil = 0.05f;
        public float shakeMovement = 0.05f;
        public float shakeMin = 0f;
        float targetShake;
        float curShake;

        bool fenceCollision;
        GameObject fence;
        #endregion

        // Use this for initialization
        void Start()
        {
            crosshairManager = CrosshairManager.GetInstance();
            //Debug.Log(crosshairManager.activeCrosshair);
            crosshair = crosshairManager.activeCrosshair.GetComponent<Crosshair>();

            camProperties = FreeCameraLook.GetInstance();
            camPivot = camProperties.transform.GetChild(0);
            camTrans = camPivot.GetChild(0);
            shakeCam = camPivot.GetComponentInChildren<ShakeCamera>();

            states = GetComponent<StateManager>();

            shotLayerMask = ~(1 << gameObject.layer | 
                1 << LayerMask.NameToLayer("Enclos") |
                1 << LayerMask.NameToLayer("Wolf") |
                1 << LayerMask.NameToLayer("Leurre") |
                1 << LayerMask.NameToLayer("Trap")) ;

            states.shotLayerMask = shotLayerMask;

            camLayerMask = ~(1 << gameObject.layer | 
                1 << LayerMask.NameToLayer("Terrain") |
                1 << LayerMask.NameToLayer("Wolf") |
                1 << LayerMask.NameToLayer("Leurre") | 
                1 << LayerMask.NameToLayer("Trap"));

            fenceCollision = false;

            gameObject.AddComponent<HandleMovement_Player>();
            hMove = GetComponent<HandleMovement_Player>();
            player = GetComponent<Player>();
            
            states.isPlayer = true;
            states.Init();
            hMove.Init(states, this);
            player.Init();

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

            if (Physics.Raycast(ray.origin, ray.direction, out hit, 100, shotLayerMask))
            {
                states.lookHitPosition = hit.point;
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Shootable"))
                    crosshair.SetColor(Color.red);
                else
                    crosshair.SetColor(Color.white);
            }
            else
            {
                states.lookHitPosition = states.lookPosition;
                crosshair.SetColor(Color.white);
            }


            //Check for obstacles in front of camera
            CameraCollision(camLayerMask);

            //Update camera's postion
            curZ = Mathf.Lerp(curZ, actualZ, Time.deltaTime * 15);
            camTrans.localPosition = new Vector3(cameraNormalX, cameraNormalY, curZ);
        }

        void HandleInput()
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            if(!Assets.Script.Managers.GameManager.instance.IsTheSunAwakeAndTheBirdAreSinging)
            {
                mouse1 = Input.GetAxis("Fire1");
                mouse2 = Input.GetAxis("Fire2");
                middleMouse = Input.GetAxis("Mouse ScrollWheel");
                mouseX = Input.GetAxis("Mouse X");
                mouseY = Input.GetAxis("Mouse Y");
                fire3 = Input.GetAxis("Fire3");
            }
            else
            {
                mouse1 = 0f;
                mouse2 = 0f;
                middleMouse = 0f;
                mouseX = 0f;
                mouseY = 0f;
                fire3 = 0f;
            }

            jumpInput = Input.GetButtonDown(Statics.Jump);
            runInput = Input.GetButton(Statics.Run);
        }

        void UpdateStates()
        {
            states.aiming = states.onGround && (mouse2 > 0);
            states.walk = (fire3 > 0);

            states.horizontal = horizontal;
            states.vertical = vertical;
            states.moving = states.horizontal != 0 || states.vertical != 0;

            states.canRun = states.canRun = !states.aiming && states.vertical > 0 && !states.obstacleForward;

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
            transform.GetChild(0).transform.localPosition = Vector3.zero;
            transform.GetChild(0).transform.position = transform.position;
            transform.GetChild(1).transform.localPosition = Vector3.zero;
            transform.GetChild(1).transform.position = transform.position;

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

            if (canRun)
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
                crosshair.defaultSpread = 2.4f;
            }
            else
            {
                crosshair.defaultSpread = 3.6f;
            }

            if (states.shoot && states.canShoot && states.alive)
            {
                if (states.aiming)
                {
                    targetShake = shakeRecoil;
                    camProperties.WiggleCrosshairAndCamera(0.15f);
                    targetFov += 3;
                }
                else
                {
                    targetShake = shakeRecoil;
                    camProperties.WiggleCrosshairAndCamera(0.25f);
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
            if (Physics.Raycast(origin, direction, out hit,Mathf.Abs(targetZ), layerMask))
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Enclos"))
                {
                    if (!fenceCollision)
                    {
                        fence = hit.transform.gameObject;
                        StandardShaderUtils.ChangeRenderMode(fence.GetComponent<MeshRenderer>().material, StandardShaderUtils.BlendMode.Transparent);
                        fenceCollision = true;
                    }

                    if (fence != hit.transform.gameObject)
                    {
                        StandardShaderUtils.ChangeRenderMode(fence.GetComponent<MeshRenderer>().material, StandardShaderUtils.BlendMode.Opaque);
                        fenceCollision = false;
                    }
                }
                else
                {
                    //If we hit the terrain, then find that distance
                    float dist = Vector3.Distance(camPivot.position, hit.point);
                    actualZ = -dist;
                }
            }
            else
            {
                if (fenceCollision)
                {
                    StandardShaderUtils.ChangeRenderMode(fence.GetComponent<MeshRenderer>().material, StandardShaderUtils.BlendMode.Opaque);
                    fenceCollision = false;
                }
            }
        }
    }
}
