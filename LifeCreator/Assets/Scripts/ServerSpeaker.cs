﻿using Common;
using CryptoNet;
using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace General
{
    public class ServerSpeaker : MonoBehaviour
    {
        private string baseURI = "https://localhost:5000";
        private string jwtToken;
        private ICryptoNet rsa;

        private string Nonce => Convert.ToString(DateTimeOffset.Now.ToUnixTimeMilliseconds());

        public static ServerSpeaker Instance { get; private set; }

        private void Awake()
        {
            DontDestroyOnLoad(this);
            if (Instance == null)
            {
                Instance = this;
            }
            _ = StartCoroutine(GetPublicKey());
        }

        private IEnumerator GetPublicKey()
        {
            UnityWebRequest webRequest =
                new(baseURI + "/PublicKey", "GET")
                {
                    downloadHandler = new DownloadHandlerBuffer()
                };

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

            string bodyJson = JsonUtility.ToJson(logInData);

            UnityWebRequest webRequest =
                new(baseURI + $"/Users/LogIn", "POST")
                {
                    uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(bodyJson)),
                    downloadHandler = new DownloadHandlerBuffer()
                };
            webRequest.SetRequestHeader("Content-Type", "application/json;charset=UTF-8");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                jwtToken = webRequest.GetResponseHeader("JwtBearerToken");
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
            RegistrationData localData =
                new()
                {
                    Login = data.Login,
                    Nonce = nonce,
                    EncryptedNonceWithEmail = (data.Email + nonce).GetEncrypted(rsa),
                    EncryptedHashedPassword = data.Password.GetHash().GetEncrypted(rsa)
                };

            string localDataJson = JsonUtility.ToJson(localData);

            UnityWebRequest webRequest =
                new(baseURI + "/Users", "POST")
                {
                    uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(localDataJson)),
                    downloadHandler = new DownloadHandlerBuffer()
                };
            webRequest.SetRequestHeader("Content-Type", "application/json;charset=UTF-8");

            yield return webRequest.SendWebRequest();

            action(webRequest);
            webRequest.Dispose();
        }
    }
}
