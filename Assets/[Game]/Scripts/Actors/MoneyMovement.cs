using DG.Tweening;
using Game.GlobalVariables;
using TriflesGames.ManagerFramework;
using TriflesGames.Managers;
using UnityEngine;

namespace Game.Actors
{
    public class MoneyMovement : Actor<LevelManager>
    {
        public int value;
        
        public bool goUp,goPlayer;
        public BoxCollider trigger,trigger2;

        private float set1,set2;
        protected override void MB_Listen(bool status)
        {
            if (status)
            {
                PlayerManager.Instance.Subscribe(CustomManagerEvents.GetMoney,StartMovement);
            }
            else
            {
                PlayerManager.Instance.Unsubscribe(CustomManagerEvents.GetMoney,StartMovement);
            }
        }

        private void StartMovement(object[] arguments)
        {
            var obj = (GameObject)arguments[0];
            if (obj == gameObject)
            {
                goUp = true;

                gameObject.transform.DOLocalRotate(new Vector3(Random.Range(180,360),Random.Range(180,360),Random.Range(180,360)), 1f,RotateMode.FastBeyond360).SetLoops(-1);
                
                set1 = Random.Range(-1.25f, 1.25f);
                set2 = Random.Range(-1.25f, 1.25f);
                Destroy(trigger);
                trigger2.isTrigger = true;
            }
        }

        protected override void MB_Update()
        {
            if (goUp)
            {
                gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, new Vector3(gameObject.transform.position.x + set1, 2f, gameObject.transform.position.z + set2), 0.075f);
                
                if (gameObject.transform.position.y >= 1.7f)
                {
                    GetComponent<Rigidbody>().isKinematic = false;
                    goUp = false;
                    goPlayer = true;
                    
                    PatientManager.Instance.droppedMoneyCount--;
                    gameObject.tag = "MoneyCollected";
                }
            }

            if (goPlayer)
            {
                gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, Vector3.zero, 0.025f);
                
                gameObject.transform.position = Vector3.Lerp(gameObject.transform.position,
                    new Vector3(PlayerManager.Instance.player.transform.position.x,
                        PlayerManager.Instance.player.transform.position.y + 1,
                        PlayerManager.Instance.player.transform.position.z), 0.075f);
            }
        }
    }
}