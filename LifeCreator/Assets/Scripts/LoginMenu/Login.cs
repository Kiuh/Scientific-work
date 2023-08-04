using EasyTransition;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LoginMenu.Managers
{
    [AddComponentMenu("LoginMenu.Managers.Login")]
    public class Login : MonoBehaviour
    {
        [SerializeField]
        private Button login;

        [SerializeField]
        private Button registration;

        [SerializeField]
        private Button forgotPassword;

        [SerializeField]
        private TMP_Text error;

        [SerializeField]
        private TMP_InputField loginEmail;

        [SerializeField]
        private TMP_InputField password;

        [SerializeField]
        private TransitionSettings transitionSettings;

        [SerializeField]
        private ForgotPassword forgotPassManager;

        private void Awake()
        {
            login.onClick.AddListener(LogIn);
            registration.onClick.AddListener(Registration);
            forgotPassword.onClick.AddListener(ForgotPassword);
            error.text = string.Empty;
        }

        private void LogIn()
        {
            loginEmail.text = "Haha";
            password.text = "Haha";
            TransitionManager.Instance().Transition("MainMenu", transitionSettings, 0);
        }

        private void Registration()
        {
            TransitionManager.Instance().Transition("Registration", transitionSettings, 0);
        }

        private void ForgotPassword()
        {
            SetInteractive(false);
            forgotPassManager.gameObject.SetActive(true);
        }

        public void SetInteractive(bool interactable)
        {
            login.interactable = interactable;
            registration.interactable = interactable;
            forgotPassword.interactable = interactable;
            loginEmail.interactable = interactable;
            password.interactable = interactable;
        }
    }
}
