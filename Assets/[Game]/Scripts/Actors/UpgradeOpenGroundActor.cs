using System;
using System.Collections;
using Assets._Game_.Scripts.Entities;
using DG.Tweening;
using Game.Managers;
using TMPro;
using TriflesGames.ManagerFramework;
using TriflesGames.Managers;
using UnityEngine;

namespace Game.Actors
{
    public class UpgradeOpenGroundActor : Actor<RoomControllerManager>
    {
        public RoomControllerActor myRoomActor;
        public bool openRoom;
        public bool upgradeRoom;

        public int money;
        public int droppedMoney;
        public bool giving;
        public float timer;
        public GameObject my;
        public TextMeshPro moneyText;
        
        protected override void MB_Update()
        {
            if (droppedMoney != 0)
            {
                if (PlayerManager.Instance.playerMoney >= money)
                {
                    if (giving)
                    {
                        // StartCoroutine(StartGiving(my));
                        timer += 1 * Time.deltaTime;
                        
                        if (timer >= .01f)
                        {
                            timer = 0;
                            
                            var pos = new Vector3(my.gameObject.transform.position.x, my.gameObject.transform.position.y + 1, my.gameObject.transform.position.z);
                            var obj = Instantiate(PatientManager.Instance.moneyPrefab, pos, Quaternion.identity);

                            obj.GetComponent<BoxCollider>().enabled = false;
                            
                            obj.transform.DOJump(gameObject.transform.position, 1, 1, .5f).OnComplete(
                                () => { Destroy(obj); });

                            PlayerManager.Instance.MoneyEffect();
                            
                            droppedMoney--;
                            moneyText.text = droppedMoney.ToString();
                            moneyText.gameObject.transform.DOScale(1.2f, .1f).OnComplete(() =>
                            {
                                moneyText.gameObject.transform.DOScale(1f, .1f);
                            });
                            PlayerManager.Instance.playerMoney--;

                            if (droppedMoney <= 0)
                            {
                                myRoomActor.SetRoomOpen();

                                // if (upgradeRoom)
                                // {
                                //     droppedMoney = 50;
                                //     money = 50;
                                //
                                //     moneyText.text = money.ToString();
                                //     upgradeRoom = false;
                                // }
                                // else
                                // {
                                    gameObject.SetActive(false);
                                // }
                            }
                        }
                    }
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                giving = true;
                my = other.gameObject;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                giving = false;
                timer = 0;
                my = null;
            }
        }
    }
}