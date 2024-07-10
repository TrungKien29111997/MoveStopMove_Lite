using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    [SerializeField] NavMeshAgent nav;
    IState<Enemy> currentState;
    [SerializeField] int randomTarget;
    [SerializeField] Vector2 delayAttack;
    [SerializeField] Vector2 freezeTime;
    float freezeTimer;
    float freezeTimeRandom;

    // timer
    CounterTime tDAttack = new CounterTime();

    protected override void Update()
    {
        if (isDead || isStop || LevelManager.Instance.EGameStateL != EGameState.GamePlay)
        {
            nav.isStopped = true;
            return;
        }
        else
        {
            nav.isStopped = false;
        }
        base.Update();
        if (currentState != null)
        {
            currentState.OnExecute(this);
        }
        tDAttack.Execute(Time.deltaTime);
    }

    public override void OnInit()
    {

        tDAttack.SetTime(-1);
        nav.isStopped = false;
        switch(Random.Range(0, weaponScObj.WeaponAnim.Length))
        {
            case (int)EItemType.Sword:
                SetWeaponType(objectPoolScObj.SwordPrefab[Random.Range(0, objectPoolScObj.SwordPrefab.Count)].PoolType);
                break;
            case (int)EItemType.Hammer:
                SetWeaponType(objectPoolScObj.HammerPrefab[Random.Range(0, objectPoolScObj.HammerPrefab.Count)].PoolType);
                break;
            case (int)EItemType.Gun:
                SetWeaponType(objectPoolScObj.GunPrefab[Random.Range(0, objectPoolScObj.GunPrefab.Count)].PoolType);
                break;
        }
        
        ChangeState(this, ref currentState, Constant.ENEMY_STATE_FIND);
        base.OnInit();
    }

    public override void EndGame()
    {
        base.EndGame();
        anim.SetFloat(Constant.ANIM_SPEED, 0);
    }

    public override void ResetTarget()
    {
        base.ResetTarget();
        FindNavTarget();
    }

    #region StateMachine
    public void EnterFindState()
    {
        nav.isStopped = false;
        FindNavTarget();
    }

    public void FindState()
    {
        if (LevelManager.Instance.ActiveCharacter.Count > 1)
        {
            NavAnimSpeed(MoveSpeed);
            nav.SetDestination(LevelManager.Instance.ActiveCharacter[randomTarget].TF.position);
        }
        else
        {
            NavAnimSpeed(0);
        }

        if (CharInRange.Count > 0)
        {
            ChangeState<Enemy>(this, ref currentState, Constant.ENEMY_STATE_ATTACK);
        }
    }

    public void EnterAttackState()
    {
        nav.isStopped = true;
        freezeTimer = 0;
        NavAnimSpeed(0);
        float delayAttackRandom = Random.Range(delayAttack.x, delayAttack.y);
        tDAttack.Start(() => base.Attack(Constant.ANIMSTATE_SHOOT, CurrentWeaponObj.ItemType, Constant.ANIM_SHOT, 0.8f), delayAttackRandom);
        freezeTimeRandom = Random.Range(delayAttackRandom, delayAttackRandom + Random.Range(freezeTime.x, freezeTime.y));
    }

    public void AttackState()
    {
        TF.LookAt(TargetPos, Vector3.up);
        freezeTimer += Time.deltaTime;
        if (freezeTimer > freezeTimeRandom)
        {
            ChangeState<Enemy>(this, ref currentState, Constant.ENEMY_STATE_FIND);
        }
    }

    #endregion

    void NavAnimSpeed(float tmpSpeed)
    {
        nav.speed = tmpSpeed;
        anim.SetFloat(Constant.ANIM_SPEED, tmpSpeed);
    }

    void FindNavTarget()
    {
        if (LevelManager.Instance.ActiveCharacter.Count > 1)
        {
            while (true)
            {
                randomTarget = Random.Range(0, LevelManager.Instance.ActiveCharacter.Count);
                if (this != LevelManager.Instance.ActiveCharacter[randomTarget])
                {
                    break;
                }
            }
        }
    }
}
