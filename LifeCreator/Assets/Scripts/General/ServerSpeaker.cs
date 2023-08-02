using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ServerSpeaker : MonoBehaviour
{
    //private readonly string uri = "http://localhost:27503";
    private readonly string uri = "http://185.6.27.49:27504";
    private string publicKey = "";
    private string login = "";
    private string hashedPassword = "";

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        GetPublicKey((PublicKeyResponse keyResponse) => publicKey = keyResponse.PublicKey);
    }

    private string GetNonce()
    {
        DateTime zero = new(1970, 1, 1);
        TimeSpan span = DateTime.UtcNow.Subtract(zero);
        return Convert.ToString((long)span.TotalMilliseconds);
    }

    public void GetPublicKey(Action<PublicKeyResponse> action)
    {
        _ = StartCoroutine(GetPublicKeyRoutine(action));
    }

    private IEnumerator GetPublicKeyRoutine(Action<PublicKeyResponse> action)
    {
        UnityWebRequest webRequest =
            new(uri + "/Pubkey", "GET") { downloadHandler = new DownloadHandlerBuffer() };

        yield return webRequest.SendWebRequest();

        action(JsonUtility.FromJson<PublicKeyResponse>(webRequest.downloadHandler.text));
        webRequest.Dispose();
    }

    [Serializable]
    public class PublicKeyResponse
    {
        public string PublicKey;
    }

    public class LogInData
    {
        public string Login;
        public string Password;

        public LogInData(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }

    public void LogIn(LogInData data, Action<long> action)
    {
        login = data.Login;
        byte[] _data = Encoding.UTF8.GetBytes(data.Password);
        hashedPassword = Convert.ToBase64String(SHA256.Create().ComputeHash(_data));
        _ = StartCoroutine(LogInRoutine(action));
    }

    private IEnumerator LogInRoutine(Action<long> action)
    {
        UnityWebRequest webRequest = new(uri + $"/User/{login}", "POST");
        string nonce = GetNonce();
        webRequest.SetRequestHeader("Nonce", nonce);
        byte[] Data = Encoding.UTF8.GetBytes(login + nonce + hashedPassword);
        webRequest.SetRequestHeader(
            "Signature",
            Convert.ToBase64String(SHA256.Create().ComputeHash(Data))
        );

        yield return webRequest.SendWebRequest();

        action(webRequest.responseCode);
        webRequest.Dispose();
    }

    [Serializable]
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

    public class RegistrationData
    {
        public string Login;
        public string NonceWithEmail;
        public string Nonce;
        public string Password;
    }

    public void Registration(RegistrationOpenData data, Action<long> action)
    {
        RegistrationData local_data = new() { Login = data.Login, Nonce = GetNonce() };
        //Email
        byte[] Data = Encoding.UTF8.GetBytes(data.Email + local_data.Nonce);
        local_data.NonceWithEmail = Convert.ToBase64String(SHA256.Create().ComputeHash(Data));
        //Password
        byte[] Data1 = Encoding.UTF8.GetBytes(data.Password);
        string hashed_password = Convert.ToBase64String(SHA256.Create().ComputeHash(Data1));

        //RSACryptoServiceProvider rsa = RSAKeys.ImportPublicKey(publicKey);
        //RSAEncryptionPadding padding = RSAEncryptionPadding.OaepSHA256;
        //local_data.Password = Convert.ToBase64String(
        //    rsa.Encrypt(Encoding.UTF8.GetBytes(hashed_password), padding)
        //);

        _ = StartCoroutine(RegistrationRoutine(local_data, action));
    }

    private IEnumerator RegistrationRoutine(RegistrationData data, Action<long> action)
    {
        UnityWebRequest webRequest =
            new(uri + "/User", "POST")
            {
                uploadHandler = new UploadHandlerRaw(
                    Encoding.UTF8.GetBytes(JsonUtility.ToJson(data))
                )
            };
        webRequest.SetRequestHeader("Content-Type", "application/json;charset=UTF-8");

        yield return webRequest.SendWebRequest();

        action(webRequest.responseCode);
        webRequest.Dispose();
    }

    public class RequestToChangePasswordOpenData
    {
        public string Email;

        public RequestToChangePasswordOpenData(string email)
        {
            Email = email;
        }
    }

    private class RequestToChangePasswordData
    {
        public string NonceWithEmail;
        public string Nonce;
    }

    public void RequestToChangePassword(RequestToChangePasswordOpenData data, Action<long> response)
    {
        RequestToChangePasswordData localData = new() { Nonce = GetNonce() };
        byte[] _data = Encoding.UTF8.GetBytes(data.Email + localData.Nonce);
        localData.NonceWithEmail = Convert.ToBase64String(SHA256.Create().ComputeHash(_data));
        _ = StartCoroutine(RequestToChangePasswordRoutine(localData, response));
    }

    private IEnumerator RequestToChangePasswordRoutine(
        RequestToChangePasswordData data,
        Action<long> responce
    )
    {
        UnityWebRequest webRequest =
            new(uri + "/User/Password", "POST")
            {
                uploadHandler = new UploadHandlerRaw(
                    Encoding.UTF8.GetBytes(JsonUtility.ToJson(data))
                )
            };
        webRequest.SetRequestHeader("Content-Type", "application/json;charset=UTF-8");

        yield return webRequest.SendWebRequest();

        responce(webRequest.responseCode);
        webRequest.Dispose();
    }

    public class ChangePasswordOpenData
    {
        public string AccessCode;
        public string NewPassword;
        public string Email;

        public ChangePasswordOpenData(string accessCode, string newPassword, string email)
        {
            AccessCode = accessCode;
            NewPassword = newPassword;
            Email = email;
        }
    }

    public class ChangePasswordData
    {
        public string AccessCode;
        public string NewPassword;
        public string NonceWithEmail;
        public string Nonce;
    }

    public void ChangePassword(ChangePasswordOpenData data, Action<long> action)
    {
        ChangePasswordData localData = new() { AccessCode = data.AccessCode, Nonce = GetNonce() };

        byte[] _data = Encoding.UTF8.GetBytes(data.Email + localData.Nonce);
        localData.NonceWithEmail = Convert.ToBase64String(SHA256.Create().ComputeHash(_data));

        byte[] _data1 = Encoding.UTF8.GetBytes(data.NewPassword);
        string hashedNewPassword = Convert.ToBase64String(SHA256.Create().ComputeHash(_data1));

        //RSACryptoServiceProvider rsa = RSAKeys.ImportPublicKey(publicKey);
        //RSAEncryptionPadding padding = RSAEncryptionPadding.OaepSHA256;
        //localData.NewPassword = Convert.ToBase64String(
        //    rsa.Encrypt(Encoding.UTF8.GetBytes(hashedNewPassword), padding)
        //);

        _ = StartCoroutine(ChangePasswordRoutine(localData, action));
    }

    private IEnumerator ChangePasswordRoutine(ChangePasswordData data, Action<long> action)
    {
        UnityWebRequest webRequest =
            new(uri + "/User/Password", "PATCH")
            {
                uploadHandler = new UploadHandlerRaw(
                    Encoding.UTF8.GetBytes(JsonUtility.ToJson(data))
                )
            };
        webRequest.SetRequestHeader("Content-Type", "application/json;charset=UTF-8");

        yield return webRequest.SendWebRequest();

        action(webRequest.responseCode);
        webRequest.Dispose();
    }
}
