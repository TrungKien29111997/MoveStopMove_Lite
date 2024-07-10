using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] Rigidbody rb;
    [SerializeField] PlayerController controller;

    [Header("CombatSetting")]
    [SerializeField] bool isAttack;
    [SerializeField] bool isCoolDown;
    [SerializeField] float coolDownTime;
    float coolDownTimer;

    [Header("CamSetting")]
    [SerializeField] float offsetCam;
    Vector3 newPosCam;
    bool isMoveCam;
    float timerCam;

    [Header("AccessorySetting")]
    [SerializeField] ItemBuff accHat;
    [SerializeField] ItemBuff accSheild;

    //player movement
    Vector2 inputMove;
    Vector2 InputMove
    {
        get
        {
            if (controller != null)
            {
                if (controller.MobieActive)
                {
                    inputMove = new Vector2(controller.InputMobie.x, controller.InputMobie.y);
                }
                else
                {
                    inputMove = new Vector2(Input.GetAxisRaw(Constant.AXIS_HORIZONTAL), Input.GetAxisRaw(Constant.AXIS_VETICAL));
                }
            }
            return inputMove;
        }
    }

    Vector3 inputDir;
    float speed;
    const float RotationSmoothTime = 15f;
    const float SpeedChangeRate = 15f;
    float animationBlend;

    Transform camtf;
    Transform CamTF
    {
        get
        {
            if (LevelManager.Instance.MainCamera != null)
            {
                camtf = LevelManager.Instance.MainCamera.transform;
            }
            return camtf;
        }
    }

    Vector3 tmpForward;
    Vector3 TmpForward
    {
        get
        {
            if (CamTF != null)
            {
                tmpForward = new Vector3(CamTF.forward.x, 0f, CamTF.forward.z);
            }
            return tmpForward;
        }
    }

    Vector3 tmpRight;
    Vector3 TmpRight
    {
        get
        {
            if (CamTF != null)
            {
                tmpRight = new Vector3(CamTF.right.x, 0f, CamTF.right.z);
            }
            return tmpRight;
        }
    }

    Vector3 Gravity => new Vector3(0, rb.velocity.y, 0);

    private void Start()
    {
        isMoveCam = false;
        timerCam = 0;
    }

    protected override void Update()
    {
        if (isDead || isStop || LevelManager.Instance.EGameStateL != EGameState.GamePlay)
        {
            anim.SetFloat(Constant.ANIM_SPEED, 0);
            return;
        }
        base.Update();
        if (currentAnimID != Constant.ANIM_IDLE)
        {
            animationBlend = 0;
        }
        if (!isCoolDown)
        {
            Control();
        }
        AttackState();

        if (isMoveCam)
        {
            timerCam += Time.deltaTime;
            newPosCam = LevelManager.Instance.CamFollow.CamTrans.position -= LevelManager.Instance.CamFollow.CamTrans.forward * offsetCam * Time.deltaTime;
            if (timerCam > 1)
            {
                timerCam = 0;
                isMoveCam = false;
            }
        }
    }

    void FixedUpdate()
    {
        if (InputMove.sqrMagnitude > 0.1f && !isCoolDown && LevelManager.Instance.EGameStateL == EGameState.GamePlay)
        {
            if (controller != null)
            {
                if (controller.MobieActive)
                {
                    TF.forward = inputDir;
                }
                else
                {
                    TF.forward = Vector3.Slerp(TF.forward, inputDir, Time.fixedDeltaTime * RotationSmoothTime);
                }
            }
            rb.velocity = TF.forward * speed * Time.fixedDeltaTime + Gravity;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    public override void OnInit()
    {
        isAttack = false;
        isCoolDown = false;
        coolDownTimer = 0;
        rb.velocity = Vector3.zero;

        SetWeaponType(SavePlayerData.Instance.LoadData().curWeap);
        base.OnInit();
        if (accHat != null) accHat.BuffCharacter(this);
        if (accSheild != null) accSheild.BuffCharacter(this);
    }

    public override void SetInfo(EPooling tmpModelType, string tmpLayer, Material tmpMat, string name)
    {
        base.SetInfo(tmpModelType, tmpLayer, tmpMat, name);

        // set Hat
        SpawnAccessory(ref accHat, SavePlayerData.Instance.LoadData().curHat, CharInfo.HeadBone);

        // Set Sheild
        SpawnAccessory(ref accSheild, SavePlayerData.Instance.LoadData().curSheild, CharInfo.LefttHandPos);
    }

    void SpawnAccessory(ref ItemBuff accName, EPooling accType, Transform parent)
    {
        if (accName != null)
        {
            SimplePool.Despawn(accName);
            accName.TF.SetParent(null);
        }
        if (accType != EPooling.None)
        {
            ItemBuff tmpAcc = SimplePool.Spawn<ItemBuff>(accType, Vector3.zero, Quaternion.identity);
            accName = tmpAcc;
            accName.TF.SetParent(parent);
            accName.TF.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        if (accHat != null)
        {
            SimplePool.Despawn(accHat);
            accHat = null;
        }
        if (accSheild != null)
        {
            SimplePool.Despawn(accSheild);
            accSheild = null;
        }
    }

    public override void GetKill(int numKill)
    {
        base.GetKill(numKill);
        SavePlayerData.Instance.IncreaseCoin(1);
    }

    protected override void IncreasePower()
    {
        base.IncreasePower();
        isMoveCam = true;
    }

    public void SetController(PlayerController tmpController)
    {
        controller = tmpController;
    }

    void Control()
    {

        if (InputMove.sqrMagnitude < 0.1f)
        {
            speed = 0.0f;
            rb.drag = 5;
        }
        else
        {
            speed = MoveSpeed * 50f;
            rb.drag = 0;
        }
        
        inputDir = (TmpForward * InputMove.y + TmpRight * InputMove.x).normalized;

        animationBlend = Mathf.Lerp(animationBlend, speed, Time.deltaTime * SpeedChangeRate);

        if (animationBlend < 0.01f) animationBlend = 0f;
        anim.SetFloat(Constant.ANIM_SPEED, animationBlend);
    }

    void AttackState()
    {
        if (InputMove.sqrMagnitude > 0.1f)
        {
            isAttack = false;
        }
        else
        {
            if (!isAttack && !isCoolDown && CharInRange.Count > 0)
            {
                isCoolDown = true;
                TF.LookAt(TargetPos);
                base.Attack(Constant.ANIMSTATE_SHOOT, CurrentWeaponObj.ItemType, Constant.ANIM_SHOT, 0.4f);
                isAttack = true;
            }
        }

        if (isCoolDown)
        {
            coolDownTimer += Time.deltaTime;
            if (coolDownTimer > coolDownTime)
            {
                coolDownTimer = 0;
                isCoolDown = false;
            }
        }
    }
}
