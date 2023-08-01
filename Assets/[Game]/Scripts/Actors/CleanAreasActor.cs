using DG.Tweening;
using Game.Managers;
using TriflesGames.ManagerFramework;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Actors
{
    public class CleanAreasActor : Actor<RoomControllerManager>
    {
        public RoomControllerActor myRoomActor;

        public Image cleanTag;

        public bool bed, commode, window,serum;

        public bool go;

        public ParticleSystem bedPoof, windowPoof, commodePoof;
        
        protected override void MB_Update()
        {
            if (myRoomActor.roomStatus == RoomControllerActor.RoomStatus.RoomDirty)
            {
                if (go)
                {
                    cleanTag.fillAmount -= 1 * Time.deltaTime;

                    if (cleanTag.fillAmount <= 0)
                    {
                        go = false;

                        if (bed)
                        {
                            myRoomActor.bedCleanStatus = true;
                            myRoomActor.blanket.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, 0);

                            myRoomActor.cleanTag2.SetActive(false);
                            cleanTag.fillAmount = 1;
                            bedPoof.Play();
                        }

                        if (commode)
                        {
                            myRoomActor.commodeCleanStatus = true;
                            // myRoomActor.commode.transform.localPosition = new Vector3(0, 0.247f, 0);
                            // myRoomActor.commode.transform.localEulerAngles = new Vector3(0, 0, 0);
                            myRoomActor.commode.transform.DOLocalMove(new Vector3(0, 0.247f, 0), .5f);
                            myRoomActor.commode.transform.DOLocalRotate(new Vector3(0, 0, 0), .5f);
                            
                            
                            myRoomActor.cleanTag1.SetActive(false);
                            cleanTag.fillAmount = 1;
                            commodePoof.Play();
                        }

                        if (window)
                        {
                            myRoomActor.windowCleanStatus = true;
                            // myRoomActor.window.transform.localEulerAngles = new Vector3(0, 0, 0);
                            myRoomActor.window.transform.DOLocalRotate(new Vector3(0, 0, 0), .5f);

                            myRoomActor.cleanTag3.SetActive(false);
                            cleanTag.fillAmount = 1;
                            windowPoof.Play();
                        }

                        if (serum)
                        {
                            myRoomActor.commodeCleanStatus = true;
                            
                            myRoomActor.serum.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, 100);
                            
                            myRoomActor.cleanTag1.SetActive(false);
                            cleanTag.fillAmount = 1;
                            commodePoof.Play();
                        }
                        
                        myRoomActor.SetRoomToCleaned();
                    }
                }
            }
        }
    }
}