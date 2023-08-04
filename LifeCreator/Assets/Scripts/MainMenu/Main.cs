using EasyTransition;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu.Managers
{
    [AddComponentMenu("MainMenu.Managers.Main")]
    public class Main : MonoBehaviour
    {
        [SerializeField]
        private Button quit;

        [SerializeField]
        private Button generations;

        [SerializeField]
        private Button cells;

        [SerializeField]
        private Button info;

        [SerializeField]
        private Button createGeneration;

        [SerializeField]
        private TransitionSettings transitionSettings;

        private void Awake()
        {
            quit.onClick.AddListener(Quit);
            generations.onClick.AddListener(Generations);
            cells.onClick.AddListener(Cells);
            info.onClick.AddListener(Info);
            createGeneration.onClick.AddListener(CreateGeneration);
        }

        private void Quit()
        {
            TransitionManager.Instance().Transition("Login", transitionSettings, 0);
        }

        private void Generations()
        {

        }

        private void Cells()
        {

        }

        private void Info()
        {

        }

        private void CreateGeneration()
        {

        }
    }
}
