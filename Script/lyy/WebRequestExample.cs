using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering.Universal;

struct ServerData
{
    public string areaName;
    public string instanceID;

}


public class WebRequestExample : MonoBehaviour
{
    const string url = "http://192.168.3.124/WebCloud/Client.php";
    private const int FUNCTION_BUY = 1;
    private const int FUNCTION_GETCURRINSTANCE = 2;
    private const int FUNCTION_GETALLPRICE = 4;

    private const int FUNCTION_RENEW = 7;
    private const int FUNCTION_GETHASINSTANCECOUNT = 3;
    private const int FUNCTION_SETSTART = 5;
    private const int FUNCTION_SETSTOP = 6;
    private const string Beijing = "ap-beijing";
    private const string Chengdu = "ap-chengdu";
    private const string Chongqing = "ap-chongqing";
    private const string Guangzhou = "ap-guangzhou";
    private const string Shanghai = "ap-shanghai";
    private const string Nanjing = "ap-nanjing";
    private const string Xianggang = "ap-hongkong";
    private const string BUYTIMETYPE_HOUR = "POSTPAID_BY_HOUR";
    private const string BUYTIMETYPE_DAY = "PREPAID";


    private LocationInfo locationInfo;
    private bool isFetchingLocation = false;
    private bool findCurrinstance = false;
    private int lastCurrinstanceLenth = 0;
    private bool startServerStatusCheck = false;
    private List<ServerData> ServerDataList = new List<ServerData>();

    private string newServerName;

    private string dayOrHour;
    private string internetMaxValue;

    private string cpuValue;
    private string memoryValue;

    private string FileString = "https://mycos-1325475732.cos.ap-beijing.myqcloud.com/1/ins-123456/2024-05-16-15-26-02?sign=q-sign-algorithm%3Dsha1%26q-ak%3DAKIDxnSHxb3II6iEsQ2b4c8b0ELF8ycLxSlk%26q-sign-time%3D1715844335%3B1715844995%26q-key-time%3D1715844335%3B1715844995%26q-header-list%3Dhost%26q-url-param-list%3D%26q-signature%3D41aa54786da749d7be59cfade07cfcbb80efff30&";
    public ServerShowManager serverShowManager;
    public ServerDetial serverDetial;
    private string savePath = "D:\\chajian";
    public void Start()
    {
        
        StartCoroutine(StartDownLoad());


    }
    IEnumerator StartDownLoad()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(FileString))
        {
            yield return www.SendWebRequest();
            Debug.LogError("进入");
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("File download failed: " + www.error);
            }
            else
            {
                // 将下载的文件内容保存到持久数据路径中
                string filePath = Path.Combine(Application.persistentDataPath, "example.txt");
                File.WriteAllBytes(filePath, www.downloadHandler.data);

                Debug.Log("File downloaded to: " + filePath);
            }
            Debug.LogError("结束");
        }
    }
    public void Update()
    {
        /*if (Input.location.isEnabledByUser && !isFetchingLocation)
        {
            
            // 开始获取位置信息
            StartCoroutine(FetchLocation());
        }*/
    }
    IEnumerator FetchLocation()
    {
        isFetchingLocation = true;

        // 等待位置信息的有效性
        while (Input.location.status == LocationServiceStatus.Initializing)
        {
            yield return null;
        }

        // 检查位置信息的获取状态
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogError("Failed to get location info.");
            yield break;
        }

        // 获取用户当前位置信息
        locationInfo = Input.location.lastData;

        // 输出用户位置信息
        Debug.Log("Latitude: " + locationInfo.latitude);
        Debug.Log("Longitude: " + locationInfo.longitude);
        Debug.Log("Altitude: " + locationInfo.altitude);

        isFetchingLocation = false;
    }
    void OnDestroy()
    {
        // 停止位置服务
        Input.location.Stop();
    }
    public void BuyAndGetCurrinstance()
    {
        StartCoroutine(FindCurrentServerCount());

        StartCoroutine(BUY());



    }
    IEnumerator BUY()
    {
        var optionValue = serverDetial.GetOptionValue();
        var optionBuyType = String.Empty;
        Debug.LogError(optionValue);
        switch (optionValue)
        {
            case "小时":
                optionBuyType = BUYTIMETYPE_HOUR;

                break;
            case "月":
                optionBuyType = BUYTIMETYPE_DAY;

                break;

            default:
                break;
        }
        var serverData = serverDetial.GerServerData();
        var form = new WWWForm();
        form.AddField("function", FUNCTION_BUY);
        form.AddField("userId", 1);

        // form.AddField("zone", Beijing);
        form.AddField("buyType", optionBuyType);//包年或小时POSTPAID_BY_HOUR\PREPAID
        form.AddField("buyTime", serverDetial.GetNumValue());
        form.AddField("internetMaxValue", serverData.internetMaxValue);//
        form.AddField("name", "1");
        form.AddField("password", "wwww2222....");
        form.AddField("cpu", serverData.cpuValue);//
        form.AddField("memory", serverData.memoryValue);//



        var www = UnityWebRequest.Post(url, form);
        // 允许不安全的连接
        //ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

        yield return www.SendWebRequest();
        string buyInstance = "";
        var error = www.error;
        if (string.IsNullOrEmpty(error))
        {
            using (var reader = new BinaryReader(new MemoryStream(www.downloadHandler.data)))
            {
                try
                {
                    var boolValue = reader.ReadBoolean();
                    Debug.LogError(boolValue);

                    findCurrinstance = boolValue;
                    // onComplete(instance);
                    if (findCurrinstance)
                    {
                        Debug.LogError("查询");
                        StartCoroutine(KeepSerrchCurrinstance());
                        findCurrinstance = false;
                    }
                    Debug.LogError("退出了购买");
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

        Debug.LogError(buyInstance);


    }

    IEnumerator TEST()
    {
        var form = new WWWForm();
        form.AddField("function", 3);


        var www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();
        //string buyInstance = "";
        var error = www.error;
        if (string.IsNullOrEmpty(error))
        {
            using (var reader = new BinaryReader(new MemoryStream(www.downloadHandler.data)))
            {
                try
                {
                    var temp = String.Empty;
                    var length = reader.ReadInt32();
                    Debug.LogError(length);
                    var temp1 = reader.ReadChars(length);
                    for (int i = 0; i < length; i++)
                    {
                        temp += temp1[i];
                    }
                    Debug.LogError(temp);

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
    }

    IEnumerator GetCurrinstance()
    {
        var form = new WWWForm();
        form.AddField("function", FUNCTION_GETCURRINSTANCE);
        form.AddField("userId", 1);


        var www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();
        //string buyInstance = "";
        var error = www.error;
        if (string.IsNullOrEmpty(error))
        {
            using (var reader = new BinaryReader(new MemoryStream(www.downloadHandler.data)))
            {
                try
                {
                  
                    var length = reader.ReadInt32();
                    Debug.LogError(length);
                    // var temp1 = reader.ReadChars(length);
                    for (int i = 0; i < length; i++)
                    {
                        var instanceID = String.Empty;
                        var instanceeName = String.Empty;
                        var InstanceStatus = String.Empty;
                        var InstanceStopTime = String.Empty;


                        var instanceIDLenth = reader.ReadInt32();

                        var tempID = reader.ReadChars(instanceIDLenth);
                        foreach (var item in tempID)
                        {
                            instanceID += item;
                        }
                        Debug.LogError(instanceID);

                     


                        var instanceNameLenth = reader.ReadInt32();

                        var tempName = reader.ReadChars(instanceNameLenth);
                        foreach (var item in tempName)
                        {
                            instanceeName += item;
                        }
                        Debug.LogError(instanceeName);

                        var instanceStatusLenth = reader.ReadInt32();

                        var tempStatus = reader.ReadChars(instanceStatusLenth);
                        foreach (var item in tempStatus)
                        {
                            InstanceStatus += item;
                        }
                        Debug.LogError(InstanceStatus);


                        var instanceStopTimeLenth = reader.ReadInt32();

                        var tempStopTime = reader.ReadChars(instanceStopTimeLenth);
                        foreach (var item in tempStopTime)
                        {
                            InstanceStopTime += item;
                        }
                        Debug.LogError(InstanceStopTime);


                        var stopDateTime=turnLongToDateTime(InstanceStopTime);
                       // DateTime currentDateTime = DateTime.Now;
                        //var lastTimeNumHour=CalTimeDifference(currentDateTime, stopDateTime);
                        //Debug.LogError("剩余"+ lastTimeNumHour+"小时");


                        ServerData serverData = new ServerData();
                        serverData.instanceID = instanceID;
                        ServerDataList.Add(serverData);
                        serverShowManager.CreateAServerStyle(instanceID, stopDateTime);

                    }



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
    }

    IEnumerator KeepSerrchCurrinstance()
    {
        do
        {
            Debug.LogError("开始查询");

            var form = new WWWForm();
            form.AddField("function", FUNCTION_GETCURRINSTANCE);
            form.AddField("userId", 1);


            var www = UnityWebRequest.Post(url, form);
            yield return www.SendWebRequest();
            //string buyInstance = "";
            var error = www.error;
            if (string.IsNullOrEmpty(error))
            {
                using (var reader = new BinaryReader(new MemoryStream(www.downloadHandler.data)))
                {
                    try
                    {
                        var length = reader.ReadInt32();
                        Debug.LogError(length+"个数");

                        if (lastCurrinstanceLenth != length - 1)
                        {
                            Debug.LogError("循环");
                            continue;
                        }


                        // var temp1 = reader.ReadChars(length);
                        for (int i = 0; i < length; i++)
                        {
                            var instanceID = String.Empty;
                            var instanceeName = String.Empty;
                            var InstanceStatus = String.Empty;
                            var InstanceStopTime = String.Empty;


                            var instanceIDLenth = reader.ReadInt32();
                            Debug.LogError(instanceIDLenth+" id长度");
                            var tempID = reader.ReadChars(instanceIDLenth);
                            foreach (var item in tempID)
                            {
                                instanceID += item;
                            }
                            Debug.LogError(instanceID);
                            var isNewName = true;
                            foreach (var item in ServerDataList)
                            {
                                if (item.instanceID.Equals(instanceID))
                                {
                                    isNewName = false;
                                    break;
                                }
                            }

                          

                            Debug.LogError(ServerDataList.Count);
                            var instanceNameLenth = reader.ReadInt32();
                            Debug.LogError(instanceNameLenth + " 名字长度");
                            var tempName = reader.ReadChars(instanceNameLenth);
                            foreach (var item in tempName)
                            {
                                instanceeName += item;
                            }
                            Debug.LogError(instanceeName);

                            var instanceStatusLenth = reader.ReadInt32();
                            Debug.LogError(instanceStatusLenth + " 状态长度");
                            var tempStatus = reader.ReadChars(instanceStatusLenth);
                            foreach (var item in tempStatus)
                            {
                                InstanceStatus += item;
                            }
                            Debug.LogError(InstanceStatus);


                            var instanceStopTimeLenth = reader.ReadInt32();

                            var tempStopTime = reader.ReadChars(instanceStopTimeLenth);
                            foreach (var item in tempStopTime)
                            {
                                InstanceStopTime += item;
                            }
                            Debug.LogError(InstanceStopTime);

                            var stopDateTime = turnLongToDateTime(InstanceStopTime);
                            //DateTime currentDateTime = DateTime.Now;
                           // var lastTimeNumHour = CalTimeDifference(currentDateTime, stopDateTime);
                           // Debug.LogError("剩余" + lastTimeNumHour + "小时");
                            if (isNewName)
                            {
                                Debug.LogError("新");
                                ServerData serverData = new ServerData();
                                serverData.instanceID = instanceID;
                                ServerDataList.Add(serverData);
                                serverShowManager.CreateAServerStyle(instanceID, stopDateTime);

                            }
                            else
                            {
                                Debug.LogError("旧");
                            }
                        }


                        Debug.LogError("退出了Keep查询");
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

        } while (true);
    }

    //查询目前有几个服务器
    IEnumerator FindCurrentServerCount()
    {
        var form = new WWWForm();
        form.AddField("function", FUNCTION_GETHASINSTANCECOUNT);
        form.AddField("userId", 1);


        var www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();
        //string buyInstance = "";
        var error = www.error;
        if (string.IsNullOrEmpty(error))
        {
            using (var reader = new BinaryReader(new MemoryStream(www.downloadHandler.data)))
            {
                try
                {
                    var value = reader.ReadInt32();
                    lastCurrinstanceLenth = value;
                    Debug.LogError(value+"退出了查询");

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
    }

    public void StartServerAndSerchInstance(string instanceID)
    {
        StartCoroutine(StartServer(instanceID));
        StartCoroutine(loopSerchServer(instanceID));
    }
    IEnumerator loopSerchServer(string instanceID)
    {

        do
        {
            if (startServerStatusCheck)
            {
                startServerStatusCheck = false;


                yield break;
            }
            Debug.LogError("WaitForSecond");
            yield return new WaitForSeconds(10);
            Debug.LogError(" ");
            StartCoroutine(TargetSerchCurrinstanceStatus(instanceID));

            yield return null;


        } while (true);
    }
    IEnumerator TargetSerchCurrinstanceStatus(string targetInstanceName)
    {
        var form = new WWWForm();
        form.AddField("function", FUNCTION_GETCURRINSTANCE);
        form.AddField("userId", 1);


        var www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();
        //string buyInstance = "";
        var error = www.error;
        if (string.IsNullOrEmpty(error))
        {
            using (var reader = new BinaryReader(new MemoryStream(www.downloadHandler.data)))
            {
                try
                {
                   
                    var checkStart = false;

                    var length = reader.ReadInt32();
                    Debug.LogError(length);
                    // var temp1 = reader.ReadChars(length);
                    for (int i = 0; i < length; i++)
                    {
                        var instanceID = String.Empty;
                        var instanceeName = String.Empty;
                        var InstanceStatus = String.Empty;
                        var InstanceStopTime = String.Empty;

                        var instanceIDLenth = reader.ReadInt32();

                        var tempID = reader.ReadChars(instanceIDLenth);
                        foreach (var item in tempID)
                        {
                            instanceID += item;
                        }
                        Debug.LogError(instanceID);

                        if (targetInstanceName.Equals(instanceID))
                        {
                            checkStart = true;
                        }


                        var instanceNameLenth = reader.ReadInt32();

                        var tempName = reader.ReadChars(instanceNameLenth);
                        foreach (var item in tempName)
                        {
                            instanceeName += item;
                        }
                        Debug.LogError(instanceeName);

                        var instanceStatusLenth = reader.ReadInt32();

                        var tempStatus = reader.ReadChars(instanceStatusLenth);
                        foreach (var item in tempStatus)
                        {
                            InstanceStatus += item;
                        }
                        Debug.LogError(InstanceStatus);

                        var instanceStopTimeLenth = reader.ReadInt32();

                        var tempStopTime = reader.ReadChars(instanceStopTimeLenth);
                        foreach (var item in tempStopTime)
                        {
                            InstanceStopTime += item;
                        }
                        Debug.LogError(InstanceStopTime);

                        if (checkStart)
                        {
                            checkStart = false;
                            if (InstanceStatus.Equals("Start"))
                            {
                                startServerStatusCheck = true;
                                yield break;
                            }

                        }

                    }



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
    }

    IEnumerator StartServer(string instanceID)
    {
        var form = new WWWForm();
        form.AddField("function", FUNCTION_SETSTART);
        form.AddField("userId", 1);
        form.AddField("instanceId", instanceID);
        //form.AddField("zone", Beijing);



        var www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();
        //string buyInstance = "";
        var error = www.error;
        if (string.IsNullOrEmpty(error))
        {
            using (var reader = new BinaryReader(new MemoryStream(www.downloadHandler.data)))
            {
                try
                {
                    Debug.LogError("开服");
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
    }


    public void StopServerData(string instanceID)
    {
        StartCoroutine(StopServer(instanceID));
    }
    IEnumerator StopServer(string instanceID)
    {
        var form = new WWWForm();
        form.AddField("function", FUNCTION_SETSTOP);
        form.AddField("userId", 1);
        form.AddField("instanceId", instanceID);
        //form.AddField("zone", Beijing);



        var www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();
        //string buyInstance = "";
        var error = www.error;
        if (string.IsNullOrEmpty(error))
        {
            using (var reader = new BinaryReader(new MemoryStream(www.downloadHandler.data)))
            {
                try
                {
                    Debug.LogError("关服");
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
    }

   

    IEnumerator GetAllPrice()
    {
        var form = new WWWForm();
        form.AddField("function", FUNCTION_GETALLPRICE);
        form.AddField("BuyTimeType", "Hour");
        form.AddField("InternetType", "Hour");
        form.AddField("InternetMaxValue", 1);
        form.AddField("InstanceName", "Id");
        form.AddField("InstancePassword", "123");
        form.AddField("Cpu", "2");
        form.AddField("Memory", "4");

    var www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();
        //string buyInstance = "";
        var error = www.error;
        if (string.IsNullOrEmpty(error))
        {
            using (var reader = new BinaryReader(new MemoryStream(www.downloadHandler.data)))
            {
                try
                {
                    var temp = String.Empty;
                    var valieOfOur = reader.ReadDouble();
                    Debug.LogError(valieOfOur);
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
    }


    IEnumerator DownloadFile(string url)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("File download failed: " + www.error);
            }
            else
            {
                // 将下载的文件内容保存到本地
                string filePath = Application.persistentDataPath + "/example.txt";
                System.IO.File.WriteAllText(filePath, www.downloadHandler.text);

                Debug.Log("File downloaded to: " + filePath);
            }
        }
    }

    IEnumerator RenewServer(string instanceId)
    {
     

        var form = new WWWForm();
        form.AddField("function", FUNCTION_RENEW);
        form.AddField("instanceId", instanceId);
        form.AddField("buyTime", serverDetial.GetNumValue());

        var www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();
        //string buyInstance = "";
        var error = www.error;
        if (string.IsNullOrEmpty(error))
        {
            using (var reader = new BinaryReader(new MemoryStream(www.downloadHandler.data)))
            {
                try
                {
                    var temp = String.Empty;
                    var valieOfOur = reader.ReadDouble();
                    Debug.LogError(valieOfOur);
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
    }

    public void GetStringValue()
    {
        /* var temp = String.Empty;
         var length = reader.ReadInt32();
         Debug.LogError(length);
         var temp1 = reader.ReadChars(length);
         for (int i = 0; i < length; i++)
         {
             temp += temp1[i];
         }
         Debug.LogError(temp);*/
    }


    public DateTime turnLongToDateTime(string longValue)
    {
        // 假设有一个时间戳（精确到秒）
        long timestampSeconds = long.Parse(longValue); // 假设时间戳为 2021-03-16 00:00:00

        // 将时间戳转换为DateTime对象
        DateTime dateTime = DateTimeOffset.FromUnixTimeSeconds(timestampSeconds).LocalDateTime;

        // 将DateTime对象格式化为字符串表示日期
        string formattedDate = dateTime.ToString("yyyy-MM-dd HH:mm:ss");

        // 输出转换后的日期
        Debug.Log("Converted date: " + formattedDate);
        return dateTime;
    }

    public int CalTimeDifference(DateTime localDateTime, DateTime stopDateTime)
    {
        // 计算两个时间之间的时间间隔
        TimeSpan timeDifference = stopDateTime-localDateTime;

        // 获取时间间隔的小时数
        int hoursDifference = (int)Math.Floor(timeDifference.TotalHours);


        return hoursDifference;
    }
}




