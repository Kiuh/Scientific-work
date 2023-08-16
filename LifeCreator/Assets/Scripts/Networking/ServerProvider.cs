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
                "/Users/LogIn",
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
                "/Users",
                HttpMethod.Post,
                registrationData
            );

            yield return webRequest.SendWebRequest();

            action(webRequest);
            webRequest.Dispose();
        }
    }
}
