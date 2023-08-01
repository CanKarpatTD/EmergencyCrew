using TriflesGames.ManagerFramework;
using TriflesGames.Managers;
using UnityEngine;

namespace Game.Actors
{
    public class CameraFollowActor : Actor<LevelManager>
    {
        public Transform target;

        public bool animationMode;

        public float smoothedSpeed = 0.125f;

        protected override void MB_FixedUpdate()
        {
            if (animationMode)
            {
                Vector3 smoothedPosition = Vector3.Slerp(transform.position, target.position, smoothedSpeed);
    
                transform.position = smoothedPosition;
    
            }
        }

        protected override void MB_LateUpdate()
        {
            if (!animationMode)
            {
                Vector3 smoothedPosition = Vector3.Slerp(transform.position, target.position, smoothedSpeed);
    
                transform.position = smoothedPosition;
            }
        }
    }
}