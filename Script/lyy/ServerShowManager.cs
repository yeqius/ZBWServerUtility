using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class ServerShowManager : MonoBehaviour
{
    public GameObject layoutGroup;
    public ServerShowStyle style;
    public WebRequestExample example;
    private List<string> myServerName = new List<string>();
    private List<ServerShowStyle> serverShowStyleList = new List<ServerShowStyle>();



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void CreateAServerStyle(string serverName,DateTime lastDateTime )
    {
        var newStyle=Instantiate(style.gameObject, layoutGroup.transform);
        myServerName.Add(serverName);
        newStyle.SetActive(true);
        var stript=newStyle.GetComponent<ServerShowStyle>();
        stript.SetServerName(serverName);
        stript.SetLastTime(lastDateTime);
        stript.SetExaple(example);
        serverShowStyleList.Add(stript);

    }
    public List<string> GetAllServerName()
    {
        return myServerName;
    }
    
}
