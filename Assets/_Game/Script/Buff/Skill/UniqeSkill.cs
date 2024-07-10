using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqeSkill : MonoBehaviour
{
    Transform tf;
    public Transform TF
    {
        get
        {
            if (tf == null)
            {
                tf = transform;
            }
            return tf;
        }
    }
    [SerializeField] GameObject thunderObj;
    [SerializeField] Transform[] defaultThunderTrans;
    [SerializeField] float delaySpawn;
    [SerializeField] List<GameObject> objSpawnToCharacter = new List<GameObject>();
    [SerializeField] Character currentChar;
    private void Start()
    {
        Invoke(nameof(SpawnThuder),delaySpawn);
    }

    void SpawnThuder()
    {
        if (LevelManager.Instance.ActiveCharacter.Count > 0)
        {
            LevelManager.Instance.KillAllEnemy(thunderObj);
            currentChar.GetKill(LevelManager.Instance.ActiveCharacter.Count);
        }
        else
        {
            for (int i = 0; i < defaultThunderTrans.Length; i++)
            {
                Instantiate(thunderObj, defaultThunderTrans[i].position, Quaternion.identity);
            }
        }
        TF.SetParent(null);
        Invoke(nameof(Despawn), 1f);
    }

    void Despawn()
    {
        for (int i = 0; i < objSpawnToCharacter.Count; i++)
        {
            Destroy(objSpawnToCharacter[i]);
        }
        objSpawnToCharacter.Clear();

        currentChar.ChangeAnim(Constant.ANIM_IDLE);

        currentChar.SetLayer(currentChar.DefaultLayer);
        currentChar.SetStop(false);

        if (currentChar.CurrentWeaponObj != null)
        {
            currentChar.CurrentWeaponObj.gameObject.SetActive(true);
        }

        Destroy(gameObject);
    }

    public void AddObjToCharacter(GameObject tmpObj)
    {
        objSpawnToCharacter.Add(tmpObj);
    }

    public void SetCharacter(Character tmpChar)
    {
        currentChar = tmpChar;
        currentChar.SetLayer(Constant.LAYER_INGORNE);
        currentChar.SetStop(true);
        currentChar.ChangeAnim(Constant.ANIM_SKILL);
        if (currentChar.CurrentWeaponObj != null)
        {
            currentChar.CurrentWeaponObj.gameObject.SetActive(false);
        }
    }
}
