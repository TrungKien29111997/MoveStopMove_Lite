using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using Cinemachine;

public class LevelManager : Singleton<LevelManager>
{
    [Header("GeneralSettings")]
    [SerializeField] Transform environmentTransform;
    [field: SerializeField] public EGameState EGameStateL { get; private set; }
    [field: SerializeField] public float FPS { get; private set; }
    [field: SerializeField] public Camera MainCamera { get; private set; }
    [field: SerializeField] public CameraFollow CamFollow { get; private set; }
    [field: SerializeField] public float MapVolume { get; private set; }
    [field: SerializeField] public bool AudioStatus { get; private set; }
    [field: SerializeField] public bool VibrationStatus { get; private set; }

    public Player Player { get; private set; }

    // scriptable object
    [SerializeField] ObjectPoolScObj objectPoolScObj;
    [SerializeField] CharacterScObj charScObj;

    [Header("GamePlaySettings")]
    [SerializeField] int alivePerLevel;
    [SerializeField] int minNumberNeedSpawn;
    int posEnemyCount;

    [SerializeField] float buffTimeSpawn;
    float buffTimer;
    int posBuffCount;
    bool isEmptyBuff;
    bool interruptSpawnBot;

    [Header("UISettings")]
    [SerializeField] Transform heathBarPrefab;

    [Header("LightSettings")]
    [SerializeField] Light sunLight;
    [SerializeField] Vector2 intensityDayNight;
    [SerializeField] Color dayColor;
    [SerializeField] Color nightColor;
    [SerializeField] Color ambientColor;

    [Header("ChracterPoolSetting")]
    [SerializeField] List<Character> activeCharacter;
    public List<Character> ActiveCharacter => activeCharacter;

    [Header("TargetIndicator")]
    [SerializeField] float offset;
    [field: SerializeField] public CanvasGroup IndicatorGroup { get; private set; }
    Vector3 originPoint;

    [Header("MapSettings")]
    [SerializeField] List<Map> maps;
    [SerializeField] Map currentMap;
    [field: SerializeField] public int Alive { get; private set; }
    [field: SerializeField] public int LevelCount { get; private set; }

    [Header("MapData")]
    [SerializeField] List<Transform> spawnBotTrans = new List<Transform>();
    [SerializeField] List<Transform> spawnBuffTrans = new List<Transform>();
    [field: SerializeField] public Transform SpawnPlayerTrans { get; private set; }

    [field: SerializeField] public WaitingHall WaitingPos { get; private set; }
    [field: SerializeField] public bool IsPlayerDead { get; private set; }
    [field: SerializeField] public bool IsPlayerSurvive { get; private set; }

    private void Awake()
    {
        LevelCount = 0;
        ReadMap(0, true);
    }
    void Start()
    {
        originPoint = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        UIManager.Instance.OpenUI<CanvasStartScreen>().SetNextCanvas(() => UIManager.Instance.OpenUI<CanvasMainMenu>(), 2f);
        SetAudioStatus(true);
        SetVibrationStatus(true);
    }

    private void Update()
    {
        FPS = (int)(1f / Time.unscaledDeltaTime);

        if (EGameStateL == EGameState.GamePlay)
        {
            ControlSpawnBuff();
            TargetIndicator();
            if (!interruptSpawnBot)
            {
                ControlSpawnBot();
            }
        }
    }

    public void OnInit()
    {
        // reset cam transform
        CamFollow.OnInit();

        IsPlayerDead = false;
        IsPlayerSurvive = false;
        Alive = alivePerLevel;
        posEnemyCount = 0;

        // buff spawn reset
        buffTimer = 0;
        posBuffCount = 0;
        isEmptyBuff = true;
        interruptSpawnBot = false;
    }

    public void OnDespawn()
    {
        // clear all current character on map
        for (int i = 0; i < activeCharacter.Count; i++)
        {
            activeCharacter[i].OnDespawn();
        }
        activeCharacter.Clear();

        // release all current object pooling
        SimplePool.ReleaseAll();
    }

    // sound and vibration
    public void SetAudioStatus(bool status)
    {
        AudioStatus = status;
        if (status)
        {
            MapVolume = 1;
        }
        else
        {
            MapVolume = 0;
        }
    }

    public void SetVibrationStatus(bool status)
    {
        VibrationStatus = status;
        if(status)
        {
            Handheld.Vibrate();
        }
    }

    // GameState
    public void SetState(EGameState state)
    {
        EGameStateL = state;
    }
    // Read Infomation Map
    public void NewLevel()
    {
        OnInit();
        InstantiateCharacter();
    }

    public void ResetLevel()
    {
        OnDespawn();
        ReadMap(LevelCount, false);
        NewLevel();
    }

    public void NextLevel()
    {
        OnDespawn();
        Destroy(currentMap.gameObject);
        LevelCount++;
        if (LevelCount >= maps.Count)
        {
            LevelCount = 0;
        }
        ReadMap(LevelCount, true);
        NewLevel();
    }
    void ReadMap(int mapIndex, bool newMap)
    {
        // set light
        switch (maps[mapIndex].MapLight)
        {
            case ELight.Day:
                ChangeLight(ref sunLight, dayColor, intensityDayNight.x, ambientColor);
                break;
            case ELight.Night:
                ChangeLight(ref sunLight, nightColor, intensityDayNight.y, Color.black);
                break;
        }

        // instantiate map
        if (newMap)
        {
            currentMap = Instantiate(maps[mapIndex], environmentTransform);
        }

        // set nav mesh
        NavMesh.RemoveAllNavMeshData();
        NavMesh.AddNavMeshData(maps[mapIndex].NavMeshData);

        SpawnPlayerTrans = currentMap.PlayerTrans;
        spawnBuffTrans = currentMap.BuffTrans;
        spawnBotTrans = currentMap.BotTrans;
        WaitingPos = currentMap.WaitingPos;
    }
    void ChangeLight(ref Light tmpLight, Color tmpColor, float tmpIntensity, Color tmpAmbient)
    {
        tmpLight.color = tmpColor;
        tmpLight.intensity = tmpIntensity;
        RenderSettings.ambientLight = tmpAmbient;
    }

    void InstantiateCharacter()
    {
        for (int i = 0; i < spawnBotTrans.Count; i++)
        {
            SpawnEnemy(spawnBotTrans[i].position, Quaternion.identity);
        }
        SpawnPlayer(SpawnPlayerTrans.position, Quaternion.identity);
    }

    void SpawnPlayer(Vector3 tmpPos, Quaternion tmpRot)
    {
        Player = SimplePool.Spawn<Player>(EPooling.CombatPlayer, Vector3.zero, Quaternion.identity);
        if (Player.CharInfo != null)
        {
            SimplePool.Despawn(Player.CharInfo);
        }

        // set up character info
        Player.SetInfo(SavePlayerData.Instance.LoadData().curChar, Constant.LAYER_PLAYER, charScObj.CharacterMat[1], "Ayaya");
        SetUpChar(Player, tmpPos, tmpRot);
    }

    void SpawnEnemy(Vector3 tmpPos, Quaternion tmpRot)
    {
        Enemy tmpEnemy = SimplePool.Spawn<Enemy>(EPooling.CombatEnemy, Vector3.zero, Quaternion.identity);
        if (tmpEnemy.CharInfo == null)
        {
            tmpEnemy.SetInfo(objectPoolScObj.CharacterInfoPrefab[Random.Range(0, objectPoolScObj.CharacterInfoPrefab.Count)].PoolType, Constant.LAYER_ENEMY, charScObj.CharacterMat[Random.Range(0, charScObj.CharacterMat.Length)], Constant.ENEMY_NAME[Random.Range(0, Constant.ENEMY_NAME.Length)]);
        }
        SetUpChar(tmpEnemy, tmpPos, tmpRot);
    }

    void SetUpChar(Character tmpChar, Vector3 tmpPos, Quaternion tmpRot)
    {
        tmpChar.TF.SetPositionAndRotation(tmpPos, tmpRot);
        activeCharacter.Add(tmpChar);

        ArrowIndicator tmpArrow = SimplePool.Spawn<ArrowIndicator>(EPooling.IndicatorArrow, Vector3.zero, Quaternion.identity);
        tmpChar.SetArrowInd(tmpArrow);
        tmpArrow.ArrowImage.color = tmpChar.CharColor;

        tmpChar.OnInit();
    }

    public void EndGame()
    {
        for (int i = 0; i < activeCharacter.Count; i++)
        {
            activeCharacter[i].EndGame();
        }
    }
    void DespawnCharacter(Character tmpChar)
    {
        SimplePool.Despawn(tmpChar.ArrowInd);
        activeCharacter.Remove(tmpChar);
        Alive--;
    }
    public void ResetListChar(Character deadChar)
    {
        DespawnCharacter(deadChar);
        if (deadChar == Player)
        {
            IsPlayerDead = true;
            EndGame();
        }
        else
        {
            for (int i = 0; i < activeCharacter.Count; i++)
            {
                activeCharacter[i].ResetTarget();
                if (activeCharacter[i].CharInRange.Contains(deadChar))
                {
                    activeCharacter[i].RemoveCharInRange(deadChar);
                }
            }
        }
    }

    public void KillAllEnemy(GameObject tmpEffect)
    {
        for (int i = 0; i < activeCharacter.Count; i++)
        {
            if (activeCharacter[i] != Player)
            {
                Instantiate(tmpEffect, activeCharacter[i].TF.position, Quaternion.identity);
                activeCharacter[i].OnDead();
            }
        }
        interruptSpawnBot = true;
        Invoke(nameof(ResetSpawnBot), 2f);
    }

    void ResetSpawnBot()
    {
        interruptSpawnBot = false;
    }

    // Control Spawn enemy in map
    void ControlSpawnBot()
    {
        if (Alive > activeCharacter.Count && activeCharacter.Count < minNumberNeedSpawn)
        {
            SpawnEnemy(spawnBotTrans[posEnemyCount].position, Quaternion.identity);
            if (posEnemyCount < spawnBotTrans.Count - 1)
            {
                posEnemyCount++;
            }
            else
            {
                posEnemyCount = 0;
            }
        }
        if (Alive == 1 && !IsPlayerDead)
        {
            IsPlayerSurvive = true;
            Player.ChangeAnim(Constant.ANIM_WIN);
        }
    }

    // Control Spawn Buff
    void ControlSpawnBuff()
    {
        if (isEmptyBuff)
        {
            buffTimer += Time.deltaTime;
            if (buffTimer > buffTimeSpawn)
            {
                SimplePool.Spawn<GameUnit>(objectPoolScObj.SkillBuffPrefab[Random.Range(0, objectPoolScObj.SkillBuffPrefab.Count)].PoolType, spawnBuffTrans[posBuffCount].position, Quaternion.identity);
                if (posBuffCount < spawnBuffTrans.Count - 1)
                {
                    posBuffCount++;
                }
                else
                {
                    posBuffCount = 0;
                }
                buffTimer = 0;
                isEmptyBuff = false;
            }
        }
    }
    public void PlayerHadBuff()
    {
        isEmptyBuff = true;
    }

    public void TargetIndicator()
    {
        if (activeCharacter.Count > 0)
        {
            for (int i = 0; i < activeCharacter.Count; i++)
            {
                Vector3 targetPoint = MainCamera.WorldToScreenPoint(activeCharacter[i].TF.position);
                if (targetPoint.x < Screen.width && targetPoint.x > 0 && targetPoint.y < Screen.height && targetPoint.y > 0)
                {
                    activeCharacter[i].ArrowInd.ArrowImage.enabled = false;
                    activeCharacter[i].ArrowInd.KillText.color = Vector4.zero;
                }
                else
                {
                    activeCharacter[i].ArrowInd.ArrowImage.enabled = true;
                    activeCharacter[i].ArrowInd.KillText.color = Vector4.one;

                    if (targetPoint.x > Screen.width - offset)
                    {
                        targetPoint.x = Screen.width - offset;
                    }
                    else if (targetPoint.x < offset)
                    {
                        targetPoint.x = offset;
                    }
                    if (targetPoint.y > Screen.height - offset)
                    {
                        targetPoint.y = Screen.height - offset;
                    }
                    else if (targetPoint.y < offset)
                    {
                        targetPoint.y = offset;
                    }
                }
                activeCharacter[i].ArrowInd.TF.position = targetPoint;
                activeCharacter[i].ArrowInd.KillText.text = activeCharacter[i].KillCount.ToString();
            }
        }
    }
}
