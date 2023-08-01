using System.Collections;
using System.Collections.Generic;
using Game.Actors;
using Game.GlobalVariables;
using Game.Helpers;
using Game.Managers;
using Pathfinding;
using TriflesGames.ManagerFramework;
using TriflesGames.Managers;
using UnityEngine;

public class PatientManager : Manager<PatientManager>
{
    [Header("* Patient Prefabs *")]
    [Space(5)]public List<GameObject> lv1Patients;
    [Space(5)]public List<GameObject> lv1Lv2Patients;
    [Space(5)]public List<GameObject> lv1Lv2Lv3Patients;
    
    [Space(10)][Header("* Patient Status&Transform Control *")]
    public List<Transform> patientTransform;
    [Space(5)]public List<PatientStatusActor> psaList;
    [Space(5)]public Transform spawnPoint;
    [HideInInspector][Space(5)]public Transform firstPoint;
    [HideInInspector]public Transform secondP,thirdP,fourthP,fifthP,sixP,sevenP,eightP,nineP,tenP;
    
    [Space(10)][Header("* Money Variables *")]
    public List<Transform> moneyPositions;
    public GameObject moneyPrefab;
    public int droppedMoneyCount;

    public bool isGameStarted;
    private float spawnTime;

    public Transform wheelChair,stretcher;
    
    public GameObject wheelChairObject, stretcherObject;
    public GameObject activeChair;

    public AstarPath pathfinder;
    
    protected override void MB_Listen(bool status)
    {
        if (status)
        {
            GameManager.Instance.Subscribe(ManagerEvents.BtnClick_Play,SetPatientSpawn);
            GameManager.Instance.Subscribe(ManagerEvents.BtnClick_Continue,SetPatientSpawn);
            GameManager.Instance.Subscribe(ManagerEvents.BtnClick_Retry,SetPatientSpawn);
            
            GameManager.Instance.Subscribe(ManagerEvents.BtnClick_Play, TestDebug);
            GameManager.Instance.Subscribe(ManagerEvents.BtnClick_Continue,TestDebug);
            GameManager.Instance.Subscribe(ManagerEvents.BtnClick_Retry,TestDebug);
            
            RoomControllerManager.Instance.Subscribe(CustomManagerEvents.SetChair,SpawnChair);
        }
        else
        {
            GameManager.Instance.Unsubscribe(ManagerEvents.BtnClick_Play, SetPatientSpawn);
            GameManager.Instance.Unsubscribe(ManagerEvents.BtnClick_Continue,SetPatientSpawn);
            GameManager.Instance.Unsubscribe(ManagerEvents.BtnClick_Retry,SetPatientSpawn);
            
            
            GameManager.Instance.Unsubscribe(ManagerEvents.BtnClick_Play, TestDebug);
            GameManager.Instance.Unsubscribe(ManagerEvents.BtnClick_Continue,TestDebug);
            GameManager.Instance.Unsubscribe(ManagerEvents.BtnClick_Retry,TestDebug);
            
            RoomControllerManager.Instance.Unsubscribe(CustomManagerEvents.SetChair,SpawnChair);
        }
    }

    private void TestDebug(object[] arguments)
    {
        foreach (var go in psaList)
        {
            Destroy(go.gameObject);
        }
        
        psaList.Clear();
        patientTransform.Clear();
        Destroy(activeChair);
        activeChair = null;
    }

    private void SpawnChair(object[] arguments)
    {
        activeChair = Instantiate(wheelChairObject, wheelChair.position, Quaternion.identity);
    }
    
    private void SetPatientSpawn(object[] arguments)
    {
        isGameStarted = true;
        
        pathfinder.Scan();
    }

    protected override void MB_Start()
    {
        spawnTime = 4;
        
        
    }

    protected override void MB_Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            LevelManager.Instance.levelActor.FinishLevel(true);
        }
        
        if (isGameStarted)
        {
            if (psaList.Count < 10)
            {
                spawnTime += 1 * Time.deltaTime;
                if (spawnTime >= 5)
                {
                    spawnTime = 0;
        
                    if (RoomControllerManager.Instance.lv1)
                    {
                        Instantiate(lv1Patients[Random.Range(0, lv1Patients.Count)], spawnPoint.transform.position,
                            Quaternion.identity);
                    }
        
                    if (RoomControllerManager.Instance.lv1Lv2)
                    {
                        Instantiate(lv1Lv2Patients[Random.Range(0, lv1Lv2Patients.Count)],
                            spawnPoint.transform.position, Quaternion.identity);
                    }
        
                    if (RoomControllerManager.Instance.lv1Lv2Lv3)
                    {
                        Instantiate(lv1Lv2Lv3Patients[Random.Range(0, lv1Lv2Lv3Patients.Count)],
                            spawnPoint.transform.position, Quaternion.identity);
                    }
                }
            }
        }
    }
    
    
}
