using System;
using UnityEngine.Networking;

namespace Common
{
    public enum HttpMethod
    {
        Get,
        Delete,
        Post,
        Put,
        Create
    }

    public static class HttpMethodTools
    {
        public static string StringValue(this HttpMethod method)
        {
            return method switch
            {
                HttpMethod.Get => UnityWebRequest.kHttpVerbGET,
                HttpMethod.Delete => UnityWebRequest.kHttpVerbDELETE,
                HttpMethod.Post => UnityWebRequest.kHttpVerbPOST,
                HttpMethod.Put => UnityWebRequest.kHttpVerbPUT,
                HttpMethod.Create => UnityWebRequest.kHttpVerbCREATE,
                _ => throw new ArgumentException()
            };
        }
    }
}
