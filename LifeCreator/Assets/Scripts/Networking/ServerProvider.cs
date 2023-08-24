using Common;
using CryptoNet;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Networking
{
    public class ServerProvider : MonoBehaviour
    {
        private UnityWebRequestBuilder requestBuilder;
        private ICryptoNet rsa;

        private string Nonce => Convert.ToString(DateTimeOffset.Now.ToUnixTimeMilliseconds());

        public static ServerProvider Instance { get; private set; }

        private void Awake()
        {
            requestBuilder = new("https://localhost:5000");
            DontDestroyOnLoad(this);
            if (Instance == null)
            {
                Instance = this;
            }
            _ = StartCoroutine(GetPublicKey());
        }

        private IEnumerator GetPublicKey()
        {
            UnityWebRequest webRequest = requestBuilder.CreateRequest("/PublicKey", HttpMethod.Get);

            yield return webRequest.SendWebRequest();

            rsa = new CryptoNetRsa(webRequest.downloadHandler.text);
            webRequest.Dispose();
        }

        public class PublicKeyResponse
        {
            public string PublicKey;
        }

        [Serializable]
        public class LogInOpenData
        {
            public string Login;
            public string Password;

            public LogInOpenData(string login, string password)
            {
                Login = login;
                Password = password;
            }
        }

        private class LogInData
        {
            public string Signature;
            public string Nonce;
        }

        public IEnumerator LogIn(LogInOpenData data, Action<UnityWebRequest> action)
        {
            string nonce = Nonce;
            LogInData logInData =
                new()
                {
                    Signature = (data.Login + nonce + data.Password.GetHash()).GetHash(),
                    Nonce = nonce
                };

            UnityWebRequest webRequest = requestBuilder.CreateRequest(
                "/Authorization/LogIn",
                HttpMethod.Post,
                logInData
            );

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                requestBuilder.SetToken(webRequest.GetResponseHeader("JwtBearerToken"));
            }

            action(webRequest);
            webRequest.Dispose();
        }

        public class RegistrationOpenData
        {
            public string Login;
            public string Email;
            public string Password;

            public RegistrationOpenData(string login, string email, string password)
            {
                Login = login;
                Email = email;
                Password = password;
            }
        }

        [Serializable]
        public class RegistrationData
        {
            public string Login;
            public string EncryptedNonceWithEmail;
            public string Nonce;
            public string EncryptedHashedPassword;
        }

        public IEnumerator Registration(RegistrationOpenData data, Action<UnityWebRequest> action)
        {
            string nonce = Nonce;
            RegistrationData registrationData =
                new()
                {
                    Login = data.Login,
                    Nonce = nonce,
                    EncryptedNonceWithEmail = (data.Email + nonce).GetEncrypted(rsa),
                    EncryptedHashedPassword = data.Password.GetHash().GetEncrypted(rsa)
                };

            UnityWebRequest webRequest = requestBuilder.CreateRequest(
                "/Authorization/Registration",
                HttpMethod.Post,
                registrationData
            );

            yield return webRequest.SendWebRequest();

            action(webRequest);
            webRequest.Dispose();
        }

        public class ResendEmailVerificationOpenData
        {
            public string Email;

            public ResendEmailVerificationOpenData(string email)
            {
                Email = email;
            }
        }

        [Serializable]
        public class ResendEmailVerificationData
        {
            public string EncryptedNonceWithEmail;
            public string Nonce;
        }

        public IEnumerator ResendEmailVerification(
            ResendEmailVerificationOpenData data,
            Action<UnityWebRequest> action
        )
        {
            string nonce = Nonce;
            ResendEmailVerificationData resendEmailVerificationData =
                new()
                {
                    Nonce = nonce,
                    EncryptedNonceWithEmail = (data.Email + nonce).GetEncrypted(rsa),
                };

            UnityWebRequest webRequest = requestBuilder.CreateRequest(
                "/Authorization/ResendEmailVerification",
                HttpMethod.Post,
                resendEmailVerificationData
            );

            yield return webRequest.SendWebRequest();

            action(webRequest);
            webRequest.Dispose();
        }

        public class ForgotPasswordOpenData
        {
            public string Email;

            public ForgotPasswordOpenData(string email)
            {
                Email = email;
            }
        }

        [Serializable]
        public class ForgotPasswordData
        {
            public string EncryptedNonceWithEmail;
            public string Nonce;
        }

        public IEnumerator ForgotPassword(
            ForgotPasswordOpenData data,
            Action<UnityWebRequest> action
        )
        {
            string nonce = Nonce;
            ForgotPasswordData forgotPasswordData =
                new()
                {
                    Nonce = nonce,
                    EncryptedNonceWithEmail = (data.Email + nonce).GetEncrypted(rsa),
                };

            UnityWebRequest webRequest = requestBuilder.CreateRequest(
                "/Authorization/ForgotPassword",
                HttpMethod.Post,
                forgotPasswordData
            );

            yield return webRequest.SendWebRequest();

            action(webRequest);
            webRequest.Dispose();
        }

        public class RecoverPasswordOpenData
        {
            public int AccessCode;
            public string Password;

            public RecoverPasswordOpenData(int accessCode, string password)
            {
                AccessCode = accessCode;
                Password = password;
            }
        }

        [Serializable]
        public class RecoverPasswordData
        {
            public int AccessCode;
            public string EncryptedHashedPassword;
        }

        public IEnumerator RecoverPassword(
            RecoverPasswordOpenData data,
            Action<UnityWebRequest> action
        )
        {
            RecoverPasswordData recoverPasswordData =
                new()
                {
                    AccessCode = data.AccessCode,
                    EncryptedHashedPassword = data.Password.GetHash().GetEncrypted(rsa),
                };

            UnityWebRequest webRequest = requestBuilder.CreateRequest(
                "/Authorization/RecoverPassword",
                HttpMethod.Post,
                recoverPasswordData
            );

            yield return webRequest.SendWebRequest();

            action(webRequest);
            webRequest.Dispose();
        }
    }
}
