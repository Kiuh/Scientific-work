using LoginMenu.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace LoginMenu
{
    [AddComponentMenu("LoginMenu.Successful")]
    public class Successful : MonoBehaviour
    {
        [SerializeField]
        private Button close;

        [SerializeField]
        private Login login;

        private void Awake()
        {
            close.onClick.AddListener(Close);
        }

        private void Close()
        {
            login.SetInteractive(true);
            gameObject.SetActive(false);
        }
    }
}
