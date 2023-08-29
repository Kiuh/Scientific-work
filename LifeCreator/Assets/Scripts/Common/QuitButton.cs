using EasyTransition;
using UnityEngine;

namespace Common
{
    [AddComponentMenu("Common.QuitButton")]
    public class QuitButton : MonoBehaviour
    {
        [SerializeField]
        private TransitionSettings transitionSettings;

        public void Quit(string targetScene)
        {
            TransitionManager.Instance().Transition(targetScene, transitionSettings, 0);
        }
    }
}
