using System;
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
    public class PlayerControllerActor : Actor<PlayerManager>
    {

        public Transform level;
        public Transform astar;
        
        protected override void MB_Listen(bool status)
        {
            if (status)
            {
                GameManager.Instance.Subscribe(ManagerEvents.BtnClick_Play,SetPlayer);
                LevelManager.Instance.Subscribe(CustomManagerEvents.AddMoney,AddMoney);
            }
            else
            {
                GameManager.Instance.Unsubscribe(ManagerEvents.BtnClick_Play,SetPlayer);
                LevelManager.Instance.Unsubscribe(CustomManagerEvents.AddMoney,AddMoney);
            }
        }

        

        protected override void MB_Update()
        {
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("BankoTrigger"))
            {
                Push(CustomManagerEvents.PlayerIn,other.gameObject,0);
            }

            if (other.gameObject.CompareTag("Money"))
            {
                // PatientManager.Instance.droppedMoneyCount--;
                // other.gameObject.tag = "MoneyCollected";
                Push(CustomManagerEvents.GetMoney, other.gameObject);
            }
            
            if (other.gameObject.CompareTag("MoneyCollected"))
            {
                CurrencyManager.Instance.AddCurrency(other.gameObject.GetComponent<MoneyMovement>().value);
                Destroy(other.gameObject);
                PlayerManager.Instance.playerMoney++;
                PlayerManager.Instance.MoneyEffect();
                print("AddMoney");
            }

            if (other.gameObject.CompareTag("RoomInLeft"))
            {
                Manager.camera.DORotate(new Vector3(0, -60, 0), 2f);
            }
            if (other.gameObject.CompareTag("RoomInRight"))
            {
                Manager.camera.DORotate(new Vector3(0, 60, 0), 2f);
            }

            if (other.gameObject.CompareTag("RoomOut"))
            {
                Manager.camera.DORotate(new Vector3(0, 0, 0), 2f);
            }

            if (other.gameObject.CompareTag("CommodeCleanTag"))
            {
                if (other.gameObject.GetComponent<CleanAreasActor>().myRoomActor.roomStatus == RoomControllerActor.RoomStatus.RoomDirty)
                    other.gameObject.GetComponent<CleanAreasActor>().go = true;
            }

            if (other.gameObject.CompareTag("BedCleanTag"))
            {
                if (other.gameObject.GetComponent<CleanAreasActor>().myRoomActor.roomStatus == RoomControllerActor.RoomStatus.RoomDirty)
                    other.gameObject.GetComponent<CleanAreasActor>().go = true;
            }

            if (other.gameObject.CompareTag("WindowCleanTag"))
            {
                if (other.gameObject.GetComponent<CleanAreasActor>().myRoomActor.roomStatus == RoomControllerActor.RoomStatus.RoomDirty)
                    other.gameObject.GetComponent<CleanAreasActor>().go = true;
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("BankoTrigger"))
            {
                Push(CustomManagerEvents.PlayerOut,other.gameObject);
            }
        }
        
        private void AddMoney(object[] arguments)
        {
            var value = (int)arguments[0];
            
            Manager.playerMoney += value;
            
            CurrencyManager.Instance.AddCurrency(value);
        }
        
        private void SetPlayer(object[] arguments)
        {
            Manager.player = gameObject;
        }
    }
}