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
                lastServerTime.text = "�ѹ���";
            }
            else
            {
                lastServerTime.text = "ʣ��" + lastNumHour + "Сʱ";
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
            lastServerTime.text =  "�ѹ���";
        }
        else
        {
            lastServerTime.text = "ʣ��"+ lastNumHour + "Сʱ";
        }
        stopDateTime = value;
        startCalTime = true;

    }
    public int CalTimeDifference(DateTime localDateTime, DateTime stopDateTime)
    {
        // ��������ʱ��֮���ʱ����
        TimeSpan timeDifference = stopDateTime - localDateTime;

        // ��ȡʱ������Сʱ��
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
