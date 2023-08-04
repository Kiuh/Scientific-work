using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LoginMenu.Managers
{
    [AddComponentMenu("LoginMenu.Managers.ContinueForgotPassword")]
    public class ContinueForgotPassword : MonoBehaviour
    {
        [SerializeField]
        private Button close;

        [SerializeField]
        private TMP_InputField oneTimeKey;

        [SerializeField]
        private TMP_InputField newPassword;

        [SerializeField]
        private TMP_Text error;

        [SerializeField]
        private Button send;

        [SerializeField]
        private Successful successful;

        [SerializeField]
        private Login login;

        private void Awake()
        {
            close.onClick.AddListener(Close);
            send.onClick.AddListener(Send);
            error.text = string.Empty;
        }

        private void Close()
        {
            login.SetInteractive(true);
            gameObject.SetActive(false);
        }

        private void Send()
        {
            oneTimeKey.text = "Hehe";
            newPassword.text = "Hehe";
            successful.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
