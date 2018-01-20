using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Contains Methods for various HTTP requests
public class Http : MonoBehaviour
{
    //Dummy GameObject to start coroutine
    private static Http instance;

    //Constants
    const float timeout = 500000;

    void Awake()
    {
        instance = this;
    }

    //Synchronous Http methods to prevent race-through
    public static class Sync
    {
        //Synchronous request handler
        private static WWW Resolve(WWW request)
        {
            float timer = 0;

            while (!request.isDone && timer < timeout)
            {
                timer += Time.deltaTime;
            }

            if (timer >= timeout)
                throw new System.TimeoutException("HTTP Request Timed Out");
            else if (!String.IsNullOrEmpty(request.error)) {
                Debug.Log(request.error);
                throw new System.OperationCanceledException(request.error);
            } else
                return request;
        }

		//Synchronous GET Request
		public static WWW GET(string URL)
		{
			WWW request = new WWW(URL);
			return Resolve(request);
		}

		//Synchronous GET Request with header
		public static WWW GET(string URL, Dictionary<string, string> HEADER)
		{
			WWW request = new WWW(URL, null, HEADER);
			return Resolve(request);
		}

        //Synchronous POST request
        public static WWW POST(string URL, WWWForm FORM)
        {
            WWW request = new WWW(URL, FORM);
            return Resolve(request);
        }

        //Synchronous POST request with header 
        public static WWW POST(string URL, byte[] FORMDATA, Dictionary<string, string> HEADER)
        {
            WWW request = new WWW(URL, FORMDATA, HEADER);
            return Resolve(request);
        }
    }

    //Asynchronous Http methods for speed
    public static class Async
    {
        //Asynchronous request handler
        private static IEnumerator Resolve(WWW request, Action<WWW> callback)
        {
            float timer = 0;

            while (!request.isDone && timer < timeout)
            {
                yield return null;
                timer += Time.deltaTime;
            }

            if (timer >= timeout)
                throw new System.TimeoutException("HTTP Request Timed Out");
            else if (!String.IsNullOrEmpty(request.error))
            {
                throw new System.OperationCanceledException(request.error);
            }
            else
                callback(request);
        }

        //Asynchronous GET Request
        public static void GET(string URL, Action<WWW> callback)
        {
            WWW request = new WWW(URL);
            instance.StartCoroutine(Resolve(request, callback));
        }

        //Asynchronous GET Request with header
        public static void GET(string URL, Dictionary<string, string> HEADER, Action<WWW> callback)
        {
            WWW request = new WWW(URL, null, HEADER);
            instance.StartCoroutine(Resolve(request, callback));
        }

        //Asynchronous POST Request
        public static void POST(string URL, WWWForm FORM, Action<WWW> callback)
        {
            WWW request = new WWW(URL, FORM);
            instance.StartCoroutine(Resolve(request, callback));
        }

        //Asynchronous POST Request with header
        public static void POST(string URL, byte[] FORMDATA, Dictionary<string, string> HEADER, Action<WWW> callback)
        {
            WWW request = new WWW(URL, FORMDATA, HEADER);
            instance.StartCoroutine(Resolve(request, callback));
        }
    }

    //Build a Query String
    public class Query
    {
        public string Encoded { get { return Uri.EscapeUriString(queryString).Replace("+", "%20"); } }

        private string queryString = "?";

        public void AddField(string key, string value)
        {
            if (this.queryString.EndsWith("?"))
                this.queryString += key + "=" + value;
            else
                this.queryString += "&" + key + "=" + value;
        }
    }
}
