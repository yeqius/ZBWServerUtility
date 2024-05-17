using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public struct serverData
{
    public string cpuValue;
    public string memoryValue;
    public string internetMaxValue;
}
public class ServerStyleData : MonoBehaviour
{
    public Image serverImage;
    public TextMeshProUGUI textMeshIntroduce;
    public TextMeshProUGUI textMeshServerName;
    public string cpuValue;
    public string memoryValue;
    public string internetMaxValue;
    private serverData data;
    // Start is called before the first frame update
    void Start()
    {
        data = new serverData();
        data.cpuValue = cpuValue;
        data.memoryValue = memoryValue;
        data.internetMaxValue = internetMaxValue;
    }
    public serverData GetServerData()
    {
        return data;
    }
   
}
