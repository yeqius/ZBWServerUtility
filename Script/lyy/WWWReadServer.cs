using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class WWWReadServer : WWWBase
{
    private const int FUNCTION_BUY = 0;
    private const int FUNCTION_GETCURRINSTANCE = 2;

    public WWWReadServer()
    {


    }

    public struct Instance
    {
        public string InstanceId;

    }

    public struct TencentCloudInstance
    {
        public string InstanceID;
        public string InstanceName;
        public string InstanceStatus;
    }
    public IEnumerator BUY(Action<Instance> onComplete)
    {
        var form = new WWWForm();
        form.AddField("function", FUNCTION_BUY);

        var www = UnityWebRequest.Post(url, form);

        yield return www.SendWebRequest();

        var error = www.error;
        if (string.IsNullOrEmpty(error))
        {
            using (var reader = new BinaryReader(new MemoryStream(www.downloadHandler.data)))
            {
                try
                {
                    Instance instance;
                    instance.InstanceId = reader.ReadString();

                    onComplete(instance);

                    yield break;
                }
                catch (Exception e)
                {
                    Debug.LogException(e.InnerException ?? e);
                }
            }
        }
        else
            Debug.LogError(error);

        onComplete(default);
    }


    public IEnumerator GetCurrinstance(Action<TencentCloudInstance> onComplete)
    {
        var form = new WWWForm();
        form.AddField("function", FUNCTION_GETCURRINSTANCE);

        var www = UnityWebRequest.Post(url, form);

        yield return www.SendWebRequest();
        int[] currinstance=new int[0];
        var error = www.error;
        if (string.IsNullOrEmpty(error))
        {
            using (var reader = new BinaryReader(new MemoryStream(www.downloadHandler.data)))
            {
                try
                {
                    var lenth = reader.ReadInt32();
                    currinstance = new int[lenth];
                    for (int i = 0; i < lenth; i++)
                    {
                        currinstance[i]= reader.ReadInt32();
                        
                    }
                    
                   // onComplete(instance);

                    yield break;
                }
                catch (Exception e)
                {
                    Debug.LogException(e.InnerException ?? e);
                }
               
            }
        }
        else
            Debug.LogError(error);

        onComplete(default);

        Debug.LogError(currinstance.Length);
    }
}
