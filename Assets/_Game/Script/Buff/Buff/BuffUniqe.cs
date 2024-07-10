using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffUniqe : Buff
{
    [SerializeField] ObjectPoolScObj objectPoolScObj;
    [SerializeField] UniqeSkill uniqeSkillPrefab;
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        UniqeSkill tmpUniqeSkill = Instantiate(uniqeSkillPrefab, Cache.GetCharacter(other).TF);
        tmpUniqeSkill.TF.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        GameObject tmpObj = Instantiate(objectPoolScObj.SkillPrefab[0].gameObject, Cache.GetCharacter(other).CharInfo.RightHandPos);
        tmpUniqeSkill.AddObjToCharacter(tmpObj);
        tmpUniqeSkill.SetCharacter(Cache.GetCharacter(other));
    }
}
