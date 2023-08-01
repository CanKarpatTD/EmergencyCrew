using TriflesGames.ManagerFramework;
using TriflesGames.Managers;

namespace Game.Actors
{
    public class LevelObjSpecial : Actor<LevelManager>
    {
        protected override void MB_Start()
        {
            PlayerManager.Instance.activeLevel = gameObject;
        }
    }
}