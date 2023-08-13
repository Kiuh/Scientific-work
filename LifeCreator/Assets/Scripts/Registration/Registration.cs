using Common;
using EasyTransition;
using General;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Registration.Managers
{
    [AddComponentMenu("Registration.Managers.Registration")]
    public class Registration : MonoBehaviour
    {
        [SerializeField]
        private Button back;

        [SerializeField]
        private Button register;

        [SerializeField]
        private TMP_InputField loginField;

        [SerializeField]
        private TMP_InputField emailField;

        [SerializeField]
        private TMP_InputField passwordField;

        [SerializeField]
        private TMP_InputField repeatPasswordField;

        [SerializeField]
        private TMP_Text error;

        [SerializeField]
        private SuccessRegistration successRegistration;

        [SerializeField]
        private TransitionSettings transitionSettings;

        private void Awake()
        {
            back.onClick.AddListener(Back);
            register.onClick.AddListener(Register);
            error.text = string.Empty;
        }

        private void Register()
        {
            Result loginResult = DataValidator.ValidateLogin(loginField.text);
            if (loginResult.Failure)
            {
                error.text = loginResult.Error;
                return;
            }
            Result emailResult = DataValidator.ValidateEmail(emailField.text);
            if (emailResult.Failure)
            {
                error.text = emailResult.Error;
                return;
            }
            Result passwordResult = DataValidator.ValidatePassword(passwordField.text);
            if (passwordResult.Failure)
            {
                error.text = passwordResult.Error;
                return;
            }
            if (passwordField.text != repeatPasswordField.text)
            {
                error.text = "Passwords not match.";
                return;
            }

            _ = StartCoroutine(
                ServerSpeaker.Instance.Registration(
                    new ServerSpeaker.RegistrationOpenData(
                        loginField.text,
                        emailField.text,
                        passwordField.text
                    ),
                    RegisterEnd
                )
            );
            LoadingPause.Instance.ShowLoading("Registering");
        }

        private void RegisterEnd(UnityWebRequest webRequest)
        {
            LoadingPause.Instance.HideLoading();
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                successRegistration.gameObject.SetActive(true);
            }
            else
            {
                error.text = webRequest.error + "\n" + webRequest.downloadHandler.text;
            }
        }

        private void Back()
        {
            TransitionManager.Instance().Transition("MainMenu", transitionSettings, 0);
        }
    }
}
