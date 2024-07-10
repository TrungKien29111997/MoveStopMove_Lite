using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXSpawn : GameUnit
{
    [SerializeField] float disableTime;
    [SerializeField] AudioClip effectAudio;
    [SerializeField] float volume;
    private void OnEnable()
    {
        AudioSource.PlayClipAtPoint(effectAudio, TF.position, volume * LevelManager.Instance.MapVolume);
        Invoke(nameof(DisableVFX), disableTime);
    }

    void DisableVFX()
    {
        SimplePool.Despawn(this);
    }
}
