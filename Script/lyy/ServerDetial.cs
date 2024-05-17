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
        // ���ѡ��
        dropdown.options.Clear();
        dropdown.options.Add(new TMP_Dropdown.OptionData("Сʱ"));
        dropdown.options.Add(new TMP_Dropdown.OptionData("��"));

        // ע��ѡ���¼�������
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        optionValue = "Сʱ";

        // ��ȡInputField���
        // inputField = GetComponent<TMP_InputField>();

        // ע������仯�¼�������
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

    // ��������仯�¼�
    private void OnInputValueChanged(string value)
    {
        // ��֤�����Ƿ�Ϊ����
        if (!IsNumeric(value))
        {
            // ������벻�����֣����Ƴ��������ַ�
            inputField.text = RemoveNonNumericCharacters(value);
        }
    }

    // ��֤�����Ƿ�Ϊ����
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

    // �Ƴ��������ַ�
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
        Debug.LogError("��ʼ����");
        if(inputField.text=="")
        {
            Debug.LogError("��������Ϊ��");
            return;
        }

        var num = int.Parse(inputField.text);
        if(num==0)
        {
            Debug.LogError("��������Ϊ0");
            return;
        }
        numValue = num;

        Debug.LogError("���Թ��� "+ num+ optionValue);

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
