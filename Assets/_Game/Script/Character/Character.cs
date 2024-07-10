using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : GameUnit
{
    // base character Setting
    protected const float BASE_MoveSpeed = 3f;
    protected const float BASE_Range = 5f;

    // character for caculate
    [field: SerializeField] public float MoveSpeed { get; protected set; }
    [field: SerializeField] public float Range { get; protected set; }
    [field: SerializeField] public ArrowIndicator ArrowInd { get; protected set; }

    [Header("GeneralSetting")]
    [SerializeField] protected Animator anim;
    protected AnimatorOverrideController animatorOverrideController;
    [field: SerializeField] public CharacterInfo CharInfo { get; protected set; }
    [SerializeField] Vector3 offsetSpawnBullet;
    Vector3 spawnPos;
    public Vector3 SpawnPos
    {
        get
        {
            if (TF != null)
            {
                spawnPos = this.TF.position + this.TF.forward + offsetSpawnBullet;
            }
            return spawnPos;
        }
    }
    [field: SerializeField] public string DefaultLayer { get; protected set; }
    [field: SerializeField] public Color CharColor { get; protected set; }

    // scriptable object
    [SerializeField] protected ObjectPoolScObj objectPoolScObj;
    [SerializeField] protected CharacterScObj charScObj;
    [SerializeField] protected WeaponScObj weaponScObj;

    [Header("AccessorySetting")]
    [SerializeField] float offsetHealthBar;
    [SerializeField] AngleRing angleRing;
    [SerializeField] Vector3 offsetAngleRing;

    [Header("CombatSetting")]
    [SerializeField] protected string charName;
    protected TargetAim targetAim;

    protected ViewRange viewRange;
    protected HealthBar healthBar;
    [field: SerializeField] public List<Character> CharInRange { get; private set; } = new List<Character>();
    [field: SerializeField] public int KillCount { get; protected set; }

    List<int> rankUpLevel = new List<int> { 2, 5, 10, 20 };
    bool levelUp;

    [Header("WeaponSetting")]
    [SerializeField] protected EPooling currentWeaponType;
    [field: SerializeField] public Weapon CurrentWeaponObj { get; protected set; }

    protected string currentAnimID;

    protected bool isDead;
    protected bool isStop;

    // timer
    CounterTime tDResetIdle = new CounterTime();
    CounterTime tDSpawnWeapon = new CounterTime();

    Vector3 targetPos;

    public Vector3 TargetPos
    {
        get
        {
            if (CharInRange.Count > 0)
            {
                targetPos = new Vector3(CharInRange[0].TF.position.x, this.TF.position.y, CharInRange[0].TF.position.z);
            }
            return targetPos;
        }
    }

    protected virtual void Update()
    {
        if (!isDead)
        {
            FindTarget();
            CheckLevel();
            tDResetIdle.Execute(Time.deltaTime);
            tDSpawnWeapon.Execute(Time.deltaTime);
        }
    }

    public virtual void SetInfo(EPooling tmpModelType, string tmpLayer, Material tmpMat, string name)
    {
        // set character color
        CharColor = tmpMat.GetColor(Constant.MAT_MAINCOLOR);

        // set characterInfo
        CharacterInfo tmpChar = SimplePool.Spawn<CharacterInfo>(tmpModelType, Vector3.zero, Quaternion.identity);
        tmpChar.TF.SetParent(this.TF);
        CharInfo = tmpChar;
        CharInfo.TF.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        // set animator
        anim = CharInfo.Anim;

        if (anim != null)
        {
            animatorOverrideController = new AnimatorOverrideController(anim.runtimeAnimatorController);
            anim.runtimeAnimatorController = animatorOverrideController;
        }

        // set default layerMask
        DefaultLayer = tmpLayer;

        // create health bar
        if (healthBar == null)
        {
            healthBar = Instantiate(charScObj.HealthBar, TF);
            healthBar.TF.localPosition = new Vector3(0, offsetHealthBar, 0);
        }
        healthBar.SetName(name, CharColor);

        // create angle ring
        if (angleRing == null)
        {
            angleRing = Instantiate(charScObj.Ring);
        }
        angleRing.TF.SetParent(CharInfo.HeadBone);
        angleRing.TF.localScale = Vector3.one;
        angleRing.TF.SetLocalPositionAndRotation(charScObj.OffsetRing, charScObj.RotateRing);
        angleRing.SetColor(tmpMat);
        angleRing.OnInit();

        // create view range
        if (viewRange == null)
        {
            viewRange = Instantiate(charScObj.RangeAttack, this.TF);
            viewRange.SetCharParent(this);
        }

        // create target aim
        if (targetAim == null)
        {
            targetAim = Instantiate(charScObj.Aim, this.TF);
        }
        //targetAim.Render.sharedMaterial 
    }

    public virtual void OnInit()
    {
        // reset animator
        ChangeAnim(Constant.ANIM_IDLE);
        anim.speed = 1f;

        // reset bool
        levelUp = false;
        isDead = false;
        isStop = false;

        KillCount = 0;
        CharInRange.Clear();
        gameObject.layer = LayerMask.NameToLayer(DefaultLayer);

        // reset default power of charater
        MoveSpeed = BASE_MoveSpeed;
        Range = BASE_Range;

        // reset timer
        tDResetIdle.SetTime(-1);
        tDSpawnWeapon.SetTime(-1);

        // reset accessory
        angleRing.OnInit();
        SetRange(Range);
        healthBar.TF.localPosition = new Vector3(0, offsetHealthBar, 0);
        targetAim.gameObject.SetActive(false);
    }

    public virtual void OnDespawn()
    {
        SetScaleModel(1);

        // remove from model accessory 
        angleRing.TF.SetParent(this.TF);

        // remove weapon from character model
        if (CurrentWeaponObj != null)
        {
            // despawn weapon
            CurrentWeaponObj.Despawn();
        }

        // remove character model from character parent
        if (CharInfo != null)
        {
            CharInfo.TF.SetParent(null);
            SimplePool.Despawn(CharInfo);
            CharInfo = null;
        }

        // then despawn
        SimplePool.Despawn(this);

        LevelManager.Instance.ResetListChar(this);
    }

    public virtual void OnDead()
    {
        isDead = true;
        gameObject.layer = LayerMask.NameToLayer(Constant.LAYER_INGORNE);
        SetTargetAim(false, this.TF);
        ChangeAnim(Constant.ANIM_DIE);
        Invoke(nameof(OnDespawn), 1f);
    }
    public virtual void EndGame()
    {
        isStop = true;
        ChangeAnim(Constant.ANIM_IDLE);
    }

    public virtual void SetStop(bool tmpBool)
    {
        isStop = tmpBool;
    }

    public virtual void SetArrowInd(ArrowIndicator tmpArrow)
    {
        ArrowInd = tmpArrow;
    }

    public virtual void GetKill(int numKill)
    {
        KillCount += numKill;
        angleRing.IncreaseStar();
    }

    public virtual void SetLayer(string nameLayer)
    {
        gameObject.layer = LayerMask.NameToLayer(nameLayer);
    }

    public void CheckLevel()
    {
        if (rankUpLevel.Contains(KillCount))
        {
            if (!levelUp)
            {
                IncreasePower();
                levelUp = true;
            }
        }
        else
        {
            levelUp = false;
        }
    }

    protected virtual void IncreasePower()
    {
        IncreaseScaleModel(charScObj.RankUpScaleModel);
        IncreaseRange(charScObj.RankUpScaleViewRange);
        SimplePool.Spawn<VFXSpawn>(EPooling.VFXLevelUp, this.TF.position, Quaternion.identity);
    }

    // Setting of scale Model ( use for buff )
    public void SetScaleModel(float tmpScale)
    {
        if (CharInfo != null)
        {
            CharInfo.TF.localScale = new Vector3(tmpScale, tmpScale, tmpScale);
        }
    }

    public void IncreaseScaleModel(float tmpScale)
    {
        CharInfo.TF.localScale += new Vector3(tmpScale, tmpScale, tmpScale);
        healthBar.TF.localPosition += new Vector3(0, tmpScale, 0);
    }

    // Setting of speed ( use for buff )
    public void SetSpeed(float tmpSpeed)
    {
        MoveSpeed = tmpSpeed;
    }

    public void IncreaseSpeed(float tmpSpeed)
    {
        MoveSpeed += tmpSpeed;
    }

    // Setting of range ( use for buff )
    public void SetRange(float tmpRange)
    {
        viewRange.TF.localScale = new Vector3(tmpRange, tmpRange, tmpRange);
        Range = tmpRange;
    }

    public void IncreaseRange(float tmpRange)
    {
        viewRange.TF.localScale += new Vector3(tmpRange, tmpRange, tmpRange);
        Range += tmpRange;
    }

    // Add and Remove List Character in range
    public void AddCharInRange(Character tmpChar)
    {
        CharInRange.Add(tmpChar);
    }

    public virtual void RemoveCharInRange(Character tmpChar)
    {
        CharInRange.Remove(tmpChar);
    }

    // Setting change anim
    public void ChangeAnim(string animID)
    {
        if (currentAnimID != animID)
        {
            anim.ResetTrigger(currentAnimID);
            currentAnimID = animID;
            anim.SetTrigger(currentAnimID);
        }
    }
    void ResetIdle()
    {
        ChangeAnim(Constant.ANIM_IDLE);
        anim.speed = 1f;
    }

    // Setting change State
    protected void ChangeState<T>(T tmpScript, ref IState<T> tmpCurrentState, IState<T> newState)
    {
        if (tmpCurrentState != null)
        {
            tmpCurrentState.OnExit(tmpScript);
        }
        tmpCurrentState = newState;

        if (tmpCurrentState != null)
        {
            tmpCurrentState.OnEnter(tmpScript);
        }
    }

    // local Target
    protected void FindTarget()
    {
        if (CharInRange.Count > 0)
        {
            SetTargetAim(true, CharInRange[0].TF);
        }
        else
        {
            SetTargetAim(false, this.TF);
        }
    }
    public virtual void ResetTarget()
    {

    }

    protected void SetTargetAim(bool active, Transform parent)
    {
        targetAim.gameObject.SetActive(active);
        targetAim.TF.SetParent(parent);
        targetAim.TF.localPosition = Vector3.zero;
    }

    // Setting spawn which type of weapon we have
    protected void Attack(string tmpAnimName, EItemType tpye, string tmpTriggerName, float endTime)
    {
        animatorOverrideController[tmpAnimName] = weaponScObj.GetAnim(tpye).CharShootClip;
        anim.speed = weaponScObj.GetAnimSpeed(tpye);
        ChangeAnim(tmpTriggerName);
        tDResetIdle.Start(() => ResetIdle(), endTime);
        tDSpawnWeapon.Start(() => SpawnWeapon(), weaponScObj.GetDelaySpawnBullet(tpye));
    }

    void SpawnWeapon()
    {
        if (CurrentWeaponObj != null)
        {
            CurrentWeaponObj.ShootBullet(SpawnPos, TF.rotation).SetOwnerChar(this);
        }
    }

    // Setting Mesh of Weapon

    public void SetWeaponType(EPooling tmpType)
    {
        currentWeaponType = tmpType;
        Weapon tmpWeap = SimplePool.Spawn<Weapon>(currentWeaponType, Vector3.zero, Quaternion.identity);
        CurrentWeaponObj = tmpWeap;
        CurrentWeaponObj.TF.SetParent(CharInfo.RightHandPos);
        CurrentWeaponObj.TF.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }
}
