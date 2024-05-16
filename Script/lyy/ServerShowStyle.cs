using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ServerShowStyle : MonoBehaviour
{
    public TextMeshProUGUI name;
    public TextMeshProUGUI lastServerTime;
    private WebRequestExample example;
    private string instanceName;
    private  DateTime stopDateTime;
    private bool startCalTime = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public IEnumerator Wait()
    {
        yield return new WaitForSeconds(180);
    }
    private void Update()
    {
        if(startCalTime)
        {
            DateTime currentDateTime = DateTime.Now;
            var lastNumHour = CalTimeDifference(currentDateTime, stopDateTime);
            if (lastNumHour < 0)
            {
                lastServerTime.text = "已过期";
            }
            else
            {
                lastServerTime.text = "剩余" + lastNumHour + "小时";
            }
        }
    }
    public void SetServerName(string value)
    {
        name.text = value;
        instanceName = value;
    }
    public void SetInstanceName(string value)
    {
        instanceName = value;
    }
    public void SetExaple(WebRequestExample example)
    {
        this.example = example; 
    }
    public void SetLastTime(DateTime value)
    {
        DateTime currentDateTime = DateTime.Now;
        var lastNumHour=CalTimeDifference(currentDateTime, value);
        if (lastNumHour < 0)
        {
            lastServerTime.text =  "已过期";
        }
        else
        {
            lastServerTime.text = "剩余"+ lastNumHour + "小时";
        }
        stopDateTime = value;
        startCalTime = true;

    }
    public int CalTimeDifference(DateTime localDateTime, DateTime stopDateTime)
    {
        // 计算两个时间之间的时间间隔
        TimeSpan timeDifference = stopDateTime - localDateTime;

        // 获取时间间隔的小时数
        int hoursDifference = (int)Math.Floor(timeDifference.TotalHours);


        return hoursDifference;
    }

    public void Open()
    {
        example.StartServerAndSerchInstance(instanceName);
    }
    public void Close()
    {
        example.StopServerData(instanceName);
    }
}
