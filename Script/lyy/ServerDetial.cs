using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ServerDetial : MonoBehaviour
{

    public Image servrImage;
    public TextMeshProUGUI servername;
    public TextMeshProUGUI introduce;

    public TMP_Dropdown dropdown;
    public TMP_InputField inputField;
    private string optionValue;
    private serverData data;
    private int numValue;
    // Start is called before the first frame update
    void Start()
    {
        // 添加选项
        dropdown.options.Clear();
        dropdown.options.Add(new TMP_Dropdown.OptionData("小时"));
        dropdown.options.Add(new TMP_Dropdown.OptionData("月"));

        // 注册选择事件处理函数
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        optionValue = "小时";

        // 获取InputField组件
        // inputField = GetComponent<TMP_InputField>();

        // 注册输入变化事件处理函数
        inputField.onValueChanged.AddListener(OnInputValueChanged);

    }
    public void GetServerStyleData(ServerStyleData styleData)
    {
        servrImage.sprite = styleData.serverImage.sprite;
        servername.text = styleData.textMeshServerName.text;
        introduce.text = styleData.textMeshIntroduce.text;
        data = styleData.GetServerData();

    }
    void OnDropdownValueChanged(int index)
    {
        Debug.Log("Selected option: " + dropdown.options[index].text);
        optionValue = dropdown.options[index].text;

    }

    // 处理输入变化事件
    private void OnInputValueChanged(string value)
    {
        // 验证输入是否为数字
        if (!IsNumeric(value))
        {
            // 如果输入不是数字，则移除非数字字符
            inputField.text = RemoveNonNumericCharacters(value);
        }
    }

    // 验证输入是否为数字
    private bool IsNumeric(string value)
    {
        foreach (char c in value)
        {
            if (!char.IsDigit(c))
            {
                return false;
            }
        }
        return true;
    }

    // 移除非数字字符
    private string RemoveNonNumericCharacters(string value)
    {
        string result = "";
        foreach (char c in value)
        {
            if (char.IsDigit(c))
            {
                result += c;
            }
        }
        return result;
    }

    public void BuyServerButtonInput()
    {
        Debug.LogError("开始购买");
        if(inputField.text=="")
        {
            Debug.LogError("所填数字为空");
            return;
        }

        var num = int.Parse(inputField.text);
        if(num==0)
        {
            Debug.LogError("所填数字为0");
            return;
        }
        numValue = num;

        Debug.LogError("尝试购买 "+ num+ optionValue);

    }
    public int GetNumValue()
    {
        return numValue;
    }
    public string GetOptionValue()
    {
        return optionValue;
    }
    public serverData GerServerData()
    {
        return data;
    }
}
