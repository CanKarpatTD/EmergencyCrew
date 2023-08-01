using System.Collections;
using System.Linq;
using DG.Tweening;
using Game.GlobalVariables;
using Game.Managers;
using TriflesGames.ManagerFramework;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Actors
{
    public class BankoControllerActor : Actor<PatientManager>
    {
        [Header("* Timer Variable *")] public Image timerImage;
        public GameObject bgImage;
        public float timerCount;
        public bool canCount;
        
        [Space(10)][Header("* Timer Variable *")]
        public GameObject activePatient;
        public PatientStatusActor psa;

        public bool isPlayerIn;

        protected override void MB_Listen(bool status)
        {
            if (status)
            {
                PatientManager.Instance.Subscribe(CustomManagerEvents.PatientWaiting,SetWaitingPatient);
                PlayerManager.Instance.Subscribe(CustomManagerEvents.PlayerIn,StartPatientActivity);
                PlayerManager.Instance.Subscribe(CustomManagerEvents.PlayerOut,StopPatientActivity);
                
                PatientManager.Instance.Subscribe(CustomManagerEvents.PatientIn,StartPatientActivity);
            }
            else
            {
                PatientManager.Instance.Unsubscribe(CustomManagerEvents.PatientWaiting,SetWaitingPatient);
                PlayerManager.Instance.Unsubscribe(CustomManagerEvents.PlayerIn,StartPatientActivity);
                PlayerManager.Instance.Unsubscribe(CustomManagerEvents.PlayerOut,StopPatientActivity);
                
                PatientManager.Instance.Unsubscribe(CustomManagerEvents.PatientIn,StartPatientActivity);
            }
        }

        protected override void MB_Update()
        {
            timerImage.fillAmount = timerCount;

            if (canCount)
            {
                timerCount += 1 * Time.deltaTime;


                if (timerCount >= 1.85f)
                {
                    StartPatientActivityCoroutine();
                    // StartCoroutine(StartPatientActivityCoroutine());
                }

                if (timerCount >= 2)
                {
                    timerCount = 0;
                }
            }

        }

        private void StartPatientActivity(object[] arguments)
        {
            //Hastanın durumunu kontrol et,
            //Timer Çalıştır,
            //Hastanın levelını kontrol et,
            //Para al,
            //Hastayı gönder,
            //Hastanın gerekliliklerini aktif et.
            var checker = (GameObject)arguments[0];
            var checkPlayer = (int)arguments[1];
            if (ReferenceEquals(gameObject, checker))
            {
                if (checkPlayer == 0)
                {
                    isPlayerIn = true; 
                }
                // isPlayerIn = true;
                
                //Boş oda varsa çalışacak. PLayer içeride durduğu sürece çalışacak.
                if (isPlayerIn)
                {
                    if (psa != null)
                    {
                        if (psa.patientLevel == PatientStatusActor.PatientLevel.LevelOnePatient)
                        {
                            if (RoomControllerManager.Instance.lv1OpenRooms.Any())
                            {
                                canCount = true;
                                bgImage.SetActive(true);
                                // psa.GiveMoney();
                            }
                        }

                        if (psa.patientLevel == PatientStatusActor.PatientLevel.LevelTwoPatient)
                        {
                            if (RoomControllerManager.Instance.lv2OpenRooms.Any())
                            {
                                canCount = true;
                                bgImage.SetActive(true);
                                // psa.GiveMoney();
                            }
                        }

                        if (psa.patientLevel == PatientStatusActor.PatientLevel.LevelThreePatient)
                        {
                            if (RoomControllerManager.Instance.lv3OpenRooms.Any())
                            {
                                canCount = true;
                                bgImage.SetActive(true);
                                // psa.GiveMoney();
                            }
                        }
                    }
                }
            }
        }

        private void StartPatientActivityCoroutine()
        {
            // yield return new WaitForSeconds(0.0f);

            if (psa != null)
            {
                if (psa.patientStatus == PatientStatusActor.PatientStatus.PatientWaiting)
                {
                    if (psa.patientLevel == PatientStatusActor.PatientLevel.LevelOnePatient)
                    {
                        psa.controllerActor.setter.target = RoomControllerManager.Instance.lv1OpenRooms[0].gameObject.transform;
                        psa.patientStatus = PatientStatusActor.PatientStatus.PatientMoving;
                        Push(CustomManagerEvents.PatientComingToRoom, RoomControllerManager.Instance.lv1OpenRooms[0].gameObject);
                    }

                    if (psa.patientLevel == PatientStatusActor.PatientLevel.LevelTwoPatient)
                    {
                        // psa.controllerActor.setter.target = RoomControllerManager.Instance.lv2OpenRooms[0].gameObject.transform;
                        
                        psa.patientStatus = PatientStatusActor.PatientStatus.PatientMoving;
                        psa.controllerActor.path.canMove = false;

                        // psa.gameObject.GetComponent<BoxCollider>().enabled = false;
                        
                        // var a = Instantiate(Manager.wheelChairObject, new Vector3(psa.gameObject.transform.position.x,psa.gameObject.transform.position.y + 0.56f,psa.gameObject.transform.position.z), Quaternion.identity,psa.gameObject.transform);
                       
                        
                        psa.gameObject.transform.DOJump(Manager.wheelChair.position, 1,1,.1f).OnComplete(() =>
                        {
                            psa.wheelChair = Manager.activeChair;
                            psa.anim.SetTrigger("Sit");
                            
                            Manager.activeChair.transform.parent = psa.gameObject.transform;
                            PlayerManager.Instance.anim.SetBool("Push",true);
                        });
                        
                        PlayerManager.Instance.player.transform.DOJump(new Vector3(Manager.wheelChair.position.x, Manager.wheelChair.position.y, Manager.wheelChair.position.z - .65f), 1, 1, .2f).OnStart(
                            () =>
                            {
                                psa.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                                PlayerManager.Instance.player.transform.DORotate(new Vector3(0, 0, 0), .2f);
                            }).OnComplete(() =>
                        {
                            psa.gameObject.transform.parent = PlayerManager.Instance.player.transform;
                            PlayerManager.Instance.isPlayerHasPatient = true;
                            PlayerManager.Instance.childPatient = psa.gameObject;

                            PlayerManager.Instance.player.transform.position = new Vector3(
                                PlayerManager.Instance.player.transform.position.x, 0,
                                PlayerManager.Instance.player.transform.position.z);
                            
                            psa = null;
                        });
                        
                        
                        Push(CustomManagerEvents.PatientComingToRoom, RoomControllerManager.Instance.lv2OpenRooms[0].gameObject);
                    }

                    if (psa.patientLevel == PatientStatusActor.PatientLevel.LevelThreePatient)
                    {
                        psa.controllerActor.setter.target =
                            RoomControllerManager.Instance.lv3OpenRooms[0].gameObject.transform;
                        psa.patientStatus = PatientStatusActor.PatientStatus.PatientMoving;
                        Push(CustomManagerEvents.PatientComingToRoom,
                            RoomControllerManager.Instance.lv3OpenRooms[0].gameObject);
                    }

                    psa.patientStatus = PatientStatusActor.PatientStatus.PatientGoingRoom;
                }
                bgImage.SetActive(false);
                canCount = false;
                timerCount = 0;
                timerImage.fillAmount = 0;
                activePatient = null;
                
                if (psa.patientLevel == PatientStatusActor.PatientLevel.LevelOnePatient)
                {
                    psa = null;
                }
            }
            
        }
        
        private void StopPatientActivity(object[] arguments)
        {
            var checker = (GameObject)arguments[0];
            if (ReferenceEquals(gameObject, checker))
            {
                isPlayerIn = false;
                print("Out");
                // canCount = false;
                // timerCount = 0;
                // timerImage.fillAmount = 0;
            }
        }

        private void SetWaitingPatient(object[] args)
        {
            var obj = (GameObject)args[0];
            var script = (PatientStatusActor) args[1];

            activePatient = obj;
            psa = script;
        }
    }
}