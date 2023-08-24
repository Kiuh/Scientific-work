using UnityEngine;
using UnityEngine.UI;

namespace Registration.Managers
{
    [AddComponentMenu("Registration.Managers.Successfully")]
    public class Successfully : MonoBehaviour
    {
        [SerializeField]
        private Button close;

        [SerializeField]
        private Registration registration;

        private void Awake()
        {
            close.onClick.AddListener(Close);
        }

        private void Close()
        {
            registration.SetInteractive(true);
            gameObject.SetActive(false);
        }
    }
}
