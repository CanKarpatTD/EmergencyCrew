using System;
using DG.Tweening;
using Game.GlobalVariables;
using Game.Managers;
using TriflesGames.ManagerFramework;
using TriflesGames.Managers;
using UnityEngine;

namespace Game.Actors
{
    public class RoomControllerActor : Actor<RoomControllerManager>
    {
        public enum RoomLevel
        {
            None, LevelOneRoom, LevelTwoRoom, LevelThreeRoom
        }
        public RoomLevel roomLevel;

        public enum RoomStatus
        {
            None,RoomEmpty,RoomUsing,RoomDirty
        }
        [Space(5)]public RoomStatus roomStatus;

        public GameObject level1Room, level2Room, level3Room;
        public GameObject myEmptyWall;
        
        public GameObject cleanTag1, cleanTag2, cleanTag3;
        
        public GameObject darkScreen;
        public GameObject cleaningSign;
        [Header("Room 1 Objects")] public GameObject bedPosition;
        public GameObject window;
        public GameObject blanket;
        public GameObject commode;
        public SkinnedMeshRenderer serum;
        public bool windowCleanStatus,bedCleanStatus,commodeCleanStatus;

        protected override void MB_Listen(bool status)
        {
            if (status)
            {
                PatientManager.Instance.Subscribe(CustomManagerEvents.PatientComingToRoom,SetRoomStatus);
                PatientManager.Instance.Subscribe(CustomManagerEvents.SetRoom,SetRoomToClean);
                
                Manager.Subscribe(CustomManagerEvents.SetRoomList, SetRoomListWithEvent);
            }
            else
            {
                PatientManager.Instance.Unsubscribe(CustomManagerEvents.PatientComingToRoom,SetRoomStatus);
                PatientManager.Instance.Unsubscribe(CustomManagerEvents.SetRoom,SetRoomToClean);
                
                Manager.Unsubscribe(CustomManagerEvents.SetRoomList, SetRoomListWithEvent);
            }
        }

        private void SetRoomListWithEvent(object[] arguments)
        {
            SetRoomList();
        }

        public void SetRoomOpen()
        {
            if (roomLevel == RoomLevel.LevelTwoRoom)
            {
                roomLevel = RoomLevel.LevelThreeRoom;
                level2Room.SetActive(false);
                
                level3Room.transform.localScale = Vector3.zero;
                level3Room.SetActive(true);

                Manager.lv1Lv2 = false;
                Manager.lv1Lv2Lv3 = true;
                
                level3Room.transform.DOScale(new Vector3(1, 1, -1), .5f).SetEase(Ease.Linear);

                if(Manager.lv2OpenRooms.Contains(this))
                    Manager.lv2OpenRooms.Remove(this);
                if(Manager.lv1OpenRooms.Contains(this))
                    Manager.lv1OpenRooms.Remove(this);
                
                if(!Manager.lv3OpenRooms.Contains(this))
                    Manager.lv3OpenRooms.Add(this);
            }

            if (roomLevel == RoomLevel.None)
            {
                roomLevel = RoomLevel.LevelTwoRoom;
                level1Room.SetActive(false);
                
                level2Room.transform.localScale = Vector3.zero;
                level2Room.SetActive(true);

                Manager.lv1 = false;
                Manager.lv1Lv2 = true;
                
                level2Room.transform.DOScale(new Vector3(1, 1, -1), .5f).SetEase(Ease.OutBack);
                
                if(Manager.lv3OpenRooms.Contains(this))
                    Manager.lv3OpenRooms.Remove(this);
                if(Manager.lv1OpenRooms.Contains(this))
                    Manager.lv1OpenRooms.Remove(this);
                
                if(!Manager.lv2OpenRooms.Contains(this))
                    Manager.lv2OpenRooms.Add(this);
                
                myEmptyWall.SetActive(false);
                //Sandalye aç
                Push(CustomManagerEvents.SetChair);
                
            }

            // if (roomLevel == RoomLevel.None)
            // {
            //     roomLevel = RoomLevel.LevelOneRoom;
            //     myEmptyWall.SetActive(false);
            //     
            //     level1Room.transform.localScale = Vector3.zero;
            //     level1Room.SetActive(true);
            //
            //     Manager.lv1 = true;
            //     
            //     level1Room.transform.DOScale(new Vector3(1, 1, -1), .5f).SetEase(Ease.OutBack);
            //
            //     if(!Manager.lv1OpenRooms.Contains(this))
            //         Manager.lv1OpenRooms.Add(this);
            // }
        }
        
        private void SetRoomStatus(object[] arguments)
        {
            var checker = (GameObject)arguments[0];

            if (checker == gameObject)
            {
                roomStatus = RoomStatus.RoomUsing;

                if (roomLevel == RoomLevel.LevelOneRoom)
                {
                    if (Manager.lv1OpenRooms.Contains(this))
                        Manager.lv1OpenRooms.Remove(this);
                }

                if (roomLevel == RoomLevel.LevelTwoRoom)
                {
                    if (Manager.lv2OpenRooms.Contains(this))
                        Manager.lv2OpenRooms.Remove(this);
                }

                if (roomLevel == RoomLevel.LevelThreeRoom)
                {
                    if (Manager.lv3OpenRooms.Contains(this))
                        Manager.lv3OpenRooms.Remove(this);
                }
            }
        }
        
        private void SetRoomToClean(object[] arguments)
        {
            var checker = (GameObject)arguments[0];

            if (checker == gameObject)
            {
                roomStatus = RoomStatus.RoomDirty;

                if (roomLevel == RoomLevel.LevelOneRoom)
                {
                    window.transform.localEulerAngles = new Vector3(0, 120, 0);
                    blanket.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, 100);
                    commode.transform.localPosition = new Vector3(0, 0.247f, 0.27f);
                    commode.transform.localEulerAngles = new Vector3(8, 0, 0);
                }
                else if (roomLevel == RoomLevel.LevelTwoRoom)
                {
                    window.transform.localEulerAngles = new Vector3(0, 120, 0);
                    blanket.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, 100);
                    serum.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, 0);
                }

                cleanTag1.SetActive(true);
                cleanTag2.SetActive(true);
                cleanTag3.SetActive(true);

                cleaningSign.transform.DOScale(1, 1f).SetEase(Ease.OutBack);
                darkScreen.transform.DOScaleZ(0, 1f);
            }
        }
        
        public void SetRoomToCleaned()
        {
            if (bedCleanStatus && windowCleanStatus && commodeCleanStatus)
            {
                roomStatus = RoomStatus.RoomEmpty;

                if (roomLevel == RoomLevel.LevelOneRoom)
                {
                    if(!Manager.lv1OpenRooms.Contains(this))
                        Manager.lv1OpenRooms.Add(this);
                }
            
                if (roomLevel == RoomLevel.LevelTwoRoom)
                {
                    if(!Manager.lv2OpenRooms.Contains(this))
                        Manager.lv2OpenRooms.Add(this);
                }
            
                if (roomLevel == RoomLevel.LevelThreeRoom)
                {
                    if(!Manager.lv3OpenRooms.Contains(this))
                        Manager.lv3OpenRooms.Add(this);
                }
                
                cleanTag1.SetActive(false);
                cleanTag2.SetActive(false);
                cleanTag3.SetActive(false);

                bedCleanStatus = false;
                windowCleanStatus = false;
                commodeCleanStatus = false;
                
                cleaningSign.transform.DOScale(0, 1f).SetEase(Ease.InBack);
            }
        }

        protected override void MB_Start()
        {
            // SetRoomList();
        }

        public void SetRoomList()
        {
            if (roomLevel == RoomLevel.LevelOneRoom)
            {
                if(!Manager.lv1OpenRooms.Contains(this))
                    Manager.lv1OpenRooms.Add(this);
            }
            
            if (roomLevel == RoomLevel.LevelTwoRoom)
            {
                if(!Manager.lv2OpenRooms.Contains(this))
                    Manager.lv2OpenRooms.Add(this);
            }
            
            if (roomLevel == RoomLevel.LevelThreeRoom)
            {
                if(!Manager.lv3OpenRooms.Contains(this))
                    Manager.lv3OpenRooms.Add(this);
            }
            
            roomStatus = RoomStatus.RoomEmpty;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Patient"))
            {
                darkScreen.transform.DOScaleZ(1, 1f);
                Push(CustomManagerEvents.PatientInThisRoom, gameObject,other.gameObject,bedPosition);

                if (other.gameObject.transform.parent != null)
                {
                    other.gameObject.transform.parent = null;
                }
                
                if(other.gameObject.GetComponent<PatientStatusActor>().wheelChair != null)
                    Destroy(other.gameObject.GetComponent<PatientStatusActor>().wheelChair);
            }

            if (other.gameObject.CompareTag("Player"))
            {
                if (PlayerManager.Instance.isPlayerHasPatient)
                {
                    print("Hastayla geldi");
                    PlayerManager.Instance.childPatient.transform.parent = null;
                    PlayerManager.Instance.anim.SetBool("Push",false);
                    Push(CustomManagerEvents.SetChair);
                }
            }
        }
    }
}