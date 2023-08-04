using EasyTransition;
using TMPro;
using UnityEngine;
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
        private TMP_InputField login;

        [SerializeField]
        private TMP_InputField email;

        [SerializeField]
        private TMP_InputField password;

        [SerializeField]
        private TMP_InputField repeatPassword;

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
            login.text = "Hehe";
            email.text = "Hehe";
            password.text = "Hehe";
            repeatPassword.text = "Hehe";
            successRegistration.gameObject.SetActive(true);
        }

        private void Back()
        {
            TransitionManager.Instance().Transition("MainMenu", transitionSettings, 0);
        }
    }
}