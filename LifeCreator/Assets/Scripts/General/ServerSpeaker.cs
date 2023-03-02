using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ServerSpeaker : MonoBehaviour
{
    readonly string URI = "http://localhost:27503";
    string PublicKey = "";
    string Login = "";
    string HashedPassword = "";
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        GetPublicKey(WritePublicKey);
    }
    void WritePublicKey(PublicKeyResponse keyResponse)
    {
        PublicKey = keyResponse.pubkey;
    }
    string GetNonce()
    {
        DateTime zero = new DateTime(1970, 1, 1);
        TimeSpan span = DateTime.UtcNow.Subtract(zero);

        return Convert.ToString((long)span.TotalMilliseconds);
    }
    #region Получить публичный ключ
    public void GetPublicKey(Action<PublicKeyResponse> action)
    {
        StartCoroutine(GetPublicKeySmart(action));
    }
    IEnumerator GetPublicKeySmart(Action<PublicKeyResponse> action)
    {
        UnityWebRequest webRequest = new(URI + "/Pubkey", "GET");
        webRequest.downloadHandler = new DownloadHandlerBuffer();

        yield return webRequest.SendWebRequest();

        action(JsonUtility.FromJson<PublicKeyResponse>(webRequest.downloadHandler.text));
        webRequest.Dispose();
    }
    [Serializable]
    public class PublicKeyResponse
    {
        public string pubkey;
    }
    #endregion
    #region Вход в систему
    public class LogInData
    {
        public string login;
        public string password;
        public LogInData(string login, string password)
        {
            this.login = login;
            this.password = password;
        }
    }
    public void LogIn(LogInData data, Action<long> action)
    {
        Login = data.login;
        byte[] Data = Encoding.UTF8.GetBytes(data.password);
        HashedPassword = Convert.ToBase64String(SHA256.Create().ComputeHash(Data));
        StartCoroutine(LogInSmart(action));
    }
    IEnumerator LogInSmart(Action<long> action)
    {
        UnityWebRequest webRequest = new(URI + $"/User/{Login}", "POST");
        string nonce = GetNonce();
        webRequest.SetRequestHeader("Nonce", nonce);
        byte[] Data = Encoding.UTF8.GetBytes(Login + nonce + HashedPassword);
        webRequest.SetRequestHeader("Signature", Convert.ToBase64String(SHA256.Create().ComputeHash(Data)));

        yield return webRequest.SendWebRequest();

        action(webRequest.responseCode);
        webRequest.Dispose();
    }
    #endregion
    #region Регистрация
    [Serializable]
    public class RegistrationOpenData
    {
        public string login;
        public string email;
        public string password;
        public RegistrationOpenData(string login, string email, string password)
        {
            this.login = login;
            this.email = email;
            this.password = password;
        }
    }
    public class RegistrationData
    {
        public string login;
        public string nonce_email;
        public string nonce;
        public string password;
    }
    public void Registration(RegistrationOpenData data, Action<long> action)
    {
        RegistrationData local_data = new();
        // Login
        local_data.login = data.login;
        // Nonce
        local_data.nonce = GetNonce();
        //Email
        byte[] Data = Encoding.UTF8.GetBytes(data.email + local_data.nonce);
        local_data.nonce_email = Convert.ToBase64String(SHA256.Create().ComputeHash(Data));
        //Password
        byte[] Data1 = Encoding.UTF8.GetBytes(data.password);
        string hashed_password = Convert.ToBase64String(SHA256.Create().ComputeHash(Data1));

        RSACryptoServiceProvider rsa = RSAKeys.ImportPublicKey(PublicKey);
        RSAEncryptionPadding padding = RSAEncryptionPadding.OaepSHA256;
        local_data.password = Convert.ToBase64String(rsa.Encrypt(Encoding.UTF8.GetBytes(hashed_password), padding));

        StartCoroutine(RegistrationSmart(local_data, action));
    }
    IEnumerator RegistrationSmart(RegistrationData data, Action<long> action)
    {
        UnityWebRequest webRequest = new(URI + "/User", "POST");
        webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(JsonUtility.ToJson(data)));
        webRequest.SetRequestHeader("Content-Type", "application/json;charset=UTF-8");

        yield return webRequest.SendWebRequest();

        action(webRequest.responseCode);
        webRequest.Dispose();
    }
    #endregion
    #region Запросить смену пароля
    public class WhantChangePasswordOpenData
    {
        public string email;
        public WhantChangePasswordOpenData(string email)
        {
            this.email = email;
        }
    }
    class WhantChangePasswordData
    {
        public string nonce_email;
        public string nonce;
    }
    public void WhantChangePassword(WhantChangePasswordOpenData data, Action<long> action)
    {
        WhantChangePasswordData local_data = new();
        local_data.nonce = GetNonce();
        byte[] Data = Encoding.UTF8.GetBytes(data.email + local_data.nonce);
        local_data.nonce_email = Convert.ToBase64String(SHA256.Create().ComputeHash(Data));
        StartCoroutine(WhantChangePasswordSmart(local_data, action));
    }
    IEnumerator WhantChangePasswordSmart(WhantChangePasswordData data, Action<long> action)
    {
        UnityWebRequest webRequest = new(URI + "/User/Password", "POST");
        webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(JsonUtility.ToJson(data)));
        webRequest.SetRequestHeader("Content-Type", "application/json;charset=UTF-8");

        yield return webRequest.SendWebRequest();

        action(webRequest.responseCode);
        webRequest.Dispose();
    }
    #endregion
    #region Смена пароля
    public class ChangePasswordOpenData
    {
        public string access_code;
        public string new_password;
        public string email;
        public ChangePasswordOpenData(string access_code, string new_password, string email)
        {
            this.access_code = access_code;
            this.new_password = new_password;
            this.email = email;
        }
    }
    public class ChangePasswordData
    {
        public string access_code;
        public string new_password;
        public string nonce_email;
        public string nonce;
    }
    public void ChangePassword(ChangePasswordOpenData data, Action<long> action)
    {
        ChangePasswordData local_data = new();
        // Access code
        local_data.access_code = data.access_code;
        // Nonce
        local_data.nonce = GetNonce();
        // Nonce Email
        byte[] Data = Encoding.UTF8.GetBytes(data.email + local_data.nonce);
        local_data.nonce_email = Convert.ToBase64String(SHA256.Create().ComputeHash(Data));
        // New Password
        byte[] Data1 = Encoding.UTF8.GetBytes(data.new_password);
        string hashed_newpassword = Convert.ToBase64String(SHA256.Create().ComputeHash(Data1));

        RSACryptoServiceProvider rsa = RSAKeys.ImportPublicKey(PublicKey);
        RSAEncryptionPadding padding = RSAEncryptionPadding.OaepSHA256;
        local_data.new_password = Convert.ToBase64String(rsa.Encrypt(Encoding.UTF8.GetBytes(hashed_newpassword), padding));

        StartCoroutine(ChangePasswordSmart(local_data, action));
    }
    IEnumerator ChangePasswordSmart(ChangePasswordData data, Action<long> action)
    {
        UnityWebRequest webRequest = new(URI + "/User/Password", "PATCH");
        webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(JsonUtility.ToJson(data)));
        webRequest.SetRequestHeader("Content-Type", "application/json;charset=UTF-8");

        yield return webRequest.SendWebRequest();

        action(webRequest.responseCode);
        webRequest.Dispose();
    }
    #endregion
}