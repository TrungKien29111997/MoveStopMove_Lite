using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Map : MonoBehaviour
{
    [field: SerializeField] public NavMeshData NavMeshData { get; private set; }

    [field: SerializeField] public Transform PlayerTrans { get; private set; }

    [field: SerializeField] public List<Transform> BotTrans { get; private set; }
    [field: SerializeField] public List<Transform> BuffTrans { get; private set; }
    [field: SerializeField] public WaitingHall WaitingPos { get; private set; }

    [field: SerializeField] public ELight MapLight { get; private set; }
}
