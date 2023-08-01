using System;
using System.Collections;
using System.Collections.Generic;
using Game.Actors;
using Game.GlobalVariables;
using TriflesGames.ManagerFramework;
using TriflesGames.Managers;
using UnityEngine;

namespace Game.Managers
{
    public class RoomControllerManager : Manager<RoomControllerManager>
    {
        [Space(10)][Header("* Rooms *")]
        [Space(5)]public List<RoomControllerActor> lv1OpenRooms;
        [Space(5)]public List<RoomControllerActor> lv2OpenRooms;
        [Space(5)]public List<RoomControllerActor> lv3OpenRooms;
        public bool lv1,lv1Lv2,lv1Lv2Lv3;

        public bool isRoomActive;

        protected override void MB_Listen(bool status)
        {
            if (status)
            {
                PatientManager.Instance.Subscribe(CustomManagerEvents.PatientComingToRoom,CheckRooms);
                
                GameManager.Instance.Subscribe(ManagerEvents.BtnClick_Play, TestDebug);
                GameManager.Instance.Subscribe(ManagerEvents.BtnClick_Continue, TestDebug);
                GameManager.Instance.Subscribe(ManagerEvents.BtnClick_Retry, TestDebug);
            }
            else
            {
                PatientManager.Instance.Unsubscribe(CustomManagerEvents.PatientComingToRoom,CheckRooms);
                
                GameManager.Instance.Unsubscribe(ManagerEvents.BtnClick_Play, TestDebug);
                GameManager.Instance.Unsubscribe(ManagerEvents.BtnClick_Continue, TestDebug);
                GameManager.Instance.Unsubscribe(ManagerEvents.BtnClick_Retry, TestDebug);
            }
        }

        private void TestDebug(object[] arguments)
        {
            lv1 = true;
            lv1Lv2 = false;
            lv1Lv2Lv3 = false;
            
            lv1OpenRooms.Clear();
            lv2OpenRooms.Clear();
            lv3OpenRooms.Clear();

            StartCoroutine(Ko());
        }

        IEnumerator Ko()
        {
            yield return new WaitForSeconds(.5f);
            Publish(CustomManagerEvents.SetRoomList);
        }

        private void CheckRooms(object[] arguments)
        {
            
        }
    }
}