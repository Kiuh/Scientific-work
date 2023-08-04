using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LoginMenu.Managers
{
    [AddComponentMenu("LoginMenu.Managers.ForgotPassManager")]
    public class ForgotPassword : MonoBehaviour
    {
        [SerializeField]
        private Button close;

        [SerializeField]
        private TMP_InputField email;

        [SerializeField]
        private Button send;

        [SerializeField]
        private Login login;

        [SerializeField]
        private ContinueForgotPassword successForgotPassManager;

        [SerializeField]
        private TMP_Text error;

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
            email.text = "Hehe";
            gameObject.SetActive(false);
            successForgotPassManager.gameObject.SetActive(true);
        }
    }
}
