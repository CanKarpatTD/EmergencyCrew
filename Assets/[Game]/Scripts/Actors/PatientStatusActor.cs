using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game.GlobalVariables;
using Game.Managers;
using TriflesGames.ManagerFramework;
using TriflesGames.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Actors
{
    public class PatientStatusActor : Actor<PatientManager>
    {
        public Animator anim;
        
        [Header("* Patient Meshes *")]
        public List<GameObject> patientMeshes;
        
        [Space(10)][Header("* Patient Target Setter Script *")]
        public PatientMovementControllerActor controllerActor;
        
        
        public enum PatientLevel
        {
            None, LevelOnePatient, LevelTwoPatient, LevelThreePatient
        }
        [Space(10)][Header("* Patient Status *")]public PatientLevel patientLevel;

        public enum PatientStatus
        {
            None, PatientMoving, PatientWaiting, PatientGoingRoom, PatientInRoom
        }
        [Space(5)]public PatientStatus patientStatus;

        [Space(10)] [Header("* Money Variables *")]
        public int moneyDrop;
        public int moneyValue;

        private GameObject activeRoom;
        private bool cureOnFire;
        private float cureTime;
        public GameObject wheelChair;

        public GameObject healthy;
        public bool healty;
        public int meshCount;

        public ParticleSystem sickEmoji;
        public ParticleSystem happyEmoji;

        public float waitTime;
        
        protected override void MB_Start()
        {
            if (!healty)
                meshCount = Random.Range(0, patientMeshes.Count);
            
            patientMeshes[meshCount].SetActive(true);
            
            if(!Manager.psaList.Contains(this))
                Manager.psaList.Add(this);
            
            if(!Manager.patientTransform.Contains(gameObject.transform))
                Manager.patientTransform.Add(gameObject.transform);

            if (patientLevel == PatientLevel.LevelOnePatient)
                cureTime = 7;
            if (patientLevel == PatientLevel.LevelTwoPatient)
                cureTime = 10;
            if (patientLevel == PatientLevel.LevelThreePatient)
                cureTime = 15;
        }
        
        protected override void MB_Listen(bool status)
        {
            if (status)
            {
                RoomControllerManager.Instance.Subscribe(CustomManagerEvents.PatientInThisRoom,PatientRoomActivity);
            }
            else
            {
                RoomControllerManager.Instance.Unsubscribe(CustomManagerEvents.PatientInThisRoom,PatientRoomActivity);
            }
        }

        private void PatientRoomActivity(object[] arguments)
        {
            var checker = (GameObject) arguments[1];
            var bedPosition = (GameObject) arguments[2];
            
            if (checker == gameObject)
            {
                activeRoom = (GameObject) arguments[0];
                
                gameObject.tag = "DonePatient";
                
                patientStatus = PatientStatus.PatientInRoom;
                gameObject.transform.DORotate(new Vector3(0,180,0), .5f);
                StartCure(bedPosition);
            }
        }

        private void StartCure(GameObject bed)
        {
            controllerActor.path.canMove = false;

            anim.SetTrigger("LayingDown");
            transform.DOMove(bed.transform.position, .1f);
            cureOnFire = true;
        }

        private void StopCure()
        {
            if (patientLevel == PatientLevel.LevelTwoPatient)
            {
                sickEmoji = null;
                
                var a = Instantiate(healthy, transform.position, Quaternion.Euler(0,180,0), transform);
                a.GetComponent<PatientStatusActor>().meshCount = meshCount;
                
                if(!healty)
                    patientMeshes[meshCount].SetActive(false);
                
                a.GetComponent<Animator>().SetTrigger("LayingDown");
                a.GetComponent<Animator>().SetTrigger("HappyWalk");
                a.GetComponent<PatientStatusActor>().controllerActor.path.canMove = false;
            }

            anim.SetTrigger("HappyWalk");
            if (patientLevel == PatientLevel.LevelOnePatient)
                healty = true;
            
            Manager.transform.DOMoveX(0, 6f).OnComplete(() =>
            {
                controllerActor.path.canMove = true;
                controllerActor.setter.target = Manager.spawnPoint;
                Push(CustomManagerEvents.SetRoom,activeRoom);
            });
            
            Manager.transform.DOMoveX(0, 8f).OnComplete(() =>
            {
                GiveMoney();
            });
        }

        protected override void MB_Update()
        {
            if (cureOnFire)
            {
                cureTime -= 1 * Time.deltaTime;

                if (cureTime <= 0)
                {
                    cureOnFire = false;
                    cureTime = 99;
                    StopCure();
                    
                    //Cure done
                    //TODO: bittiği zaman hastayı gönder. DarkScreeni aç. Oda temizleme ibaresini aç. Oda temizliği bittikten sonra odayı "OpenRooms" listesine geri ekle.
                }
            }

            waitTime += 1 * Time.deltaTime;

            if (waitTime >= 3)
            {
                if (!healty)
                {
                    if (sickEmoji != null)
                        sickEmoji.Play();
                }

                if (healty)
                {
                    happyEmoji.Play();
                }
                
                waitTime = 0;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("BankoTrigger"))
            {
                patientStatus = PatientStatus.PatientWaiting;
                controllerActor.outLine = true;
                Push(CustomManagerEvents.PatientWaiting,gameObject,this);
                
                GameManager.Instance.gameObject.transform.DOMoveX(0, .5f).OnComplete(() =>
                {
                    Push(CustomManagerEvents.PatientIn,other.gameObject,1);
                });
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("BankoTrigger"))
            {
                controllerActor.outLine = false;
                if(Manager.psaList.Contains(this))
                    Manager.psaList.Remove(this);
            
                if(Manager.patientTransform.Contains(gameObject.transform))
                    Manager.patientTransform.Remove(gameObject.transform);

                Push(CustomManagerEvents.SetNewPositions);
            }
        }

        public void GiveMoney()
        {
            StartCoroutine(GiveMoneyCoroutine());
        }

        private IEnumerator GiveMoneyCoroutine()
        {
            for (int i = 0; i < moneyDrop; i++)
            {
                var pos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1, gameObject.transform.position.z);
                var obj = Instantiate(Manager.moneyPrefab, pos , Quaternion.identity);//
                obj.GetComponent<MoneyMovement>().value = moneyValue;
                
                Manager.droppedMoneyCount++;

                obj.transform.DOJump(Manager.moneyPositions[Manager.droppedMoneyCount - 1].position, 1, 1, .5f).OnComplete(
                    () =>
                    {
                        obj.transform.DORotate(new Vector3(0,0,0), 0.1f);
                        // obj.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 1f);
                    });
                yield return new WaitForSeconds(.1f);
            }
        }
    }
}