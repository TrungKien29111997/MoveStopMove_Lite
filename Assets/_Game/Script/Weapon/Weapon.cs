using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : ItemBuff
{
    [SerializeField] Bullet bullet;
    Pool bulletPool = new Pool();

    private void Start()
    {
        bulletPool.PreLoad(bullet,0,TF);
    }

    public Bullet ShootBullet(Vector3 tmpPos, Quaternion tmpRot)
    {
        Bullet tmpBullet = bulletPool.Spawn(tmpPos, tmpRot) as Bullet;
        tmpBullet.TF.SetParent(null);
        tmpBullet.TF.localScale = Vector3.one;
        tmpBullet.SetParentPool(bulletPool);
        tmpBullet.SetParentWeapon(this);
        return tmpBullet;
    }
    public void Despawn()
    {
        bulletPool.Release();
        SimplePool.Despawn(this);
        TF.SetParent(null);
        TF.localScale = Vector3.one;
    }
}
