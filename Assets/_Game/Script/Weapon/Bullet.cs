using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : GameUnit
{
    protected Pool parentPool;
    protected Weapon parentWeapon;
    [field: SerializeField] public Character OwnerChar { get; private set; }
    [SerializeField] float moveSpeed;
    [SerializeField] float timeDestroy;
    float timer;
    [SerializeField] AudioClip effectAudio;
    [SerializeField] AudioClip hitAudio;
    [SerializeField] float volume;
    private void OnEnable()
    {
        if (effectAudio != null)
        {
            AudioSource.PlayClipAtPoint(effectAudio, TF.position, volume * LevelManager.Instance.MapVolume);
        }
    }

    private void Start()
    {
        timer = 0;
    }
    public void SetParentPool(Pool tmpPool)
    {
        parentPool = tmpPool;
    }

    public void SetParentWeapon(Weapon tmpWeapon)
    {
        parentWeapon = tmpWeapon;
    }

    public void SetOwnerChar(Character tmpChar)
    {
        OwnerChar = tmpChar;
    }

    protected virtual void Update()
    {
        if (LevelManager.Instance.EGameStateL == EGameState.GamePlay)
        {
            timer += Time.deltaTime;
            if (timer > timeDestroy)
            {
                OnDespawn();
            }
            TF.position += TF.forward * Time.deltaTime * moveSpeed;
        }
        else
        {
            OnDespawn();
        }
    }

    public virtual void OnDespawn()
    {
        if (hitAudio != null)
        {
            AudioSource.PlayClipAtPoint(hitAudio, TF.position, volume * LevelManager.Instance.MapVolume);
        }
        parentPool.Despawn(this);
        if (parentWeapon != null)
        {
            TF.SetParent(parentWeapon.TF);
        }
        timer = 0;
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (OwnerChar != null)
        {
            OwnerChar.GetKill(1);
        }

        OnDespawn();
        Cache.GetCharacter(other).OnDead();
    }
}
