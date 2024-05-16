using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.IO;

public class RunCmd : MonoBehaviour
{
    // Ҫִ�е�����
    public string command = "dir"; // ���� "dir" (�г�Ŀ¼����)

    void Start()
    {
        // ����һ��ProcessStartInfo�������������������̵�һЩ����
        ProcessStartInfo psi = new ProcessStartInfo();
        psi.FileName = "cmd"; // ָ��Ҫ�����ĳ���
        psi.RedirectStandardInput = true; // �����ض����׼������
        psi.RedirectStandardOutput = true; // �����ض����׼�����
        psi.CreateNoWindow = true; // ������cmd����
        psi.UseShellExecute = false; // ��ʹ�ò���ϵͳ��ǳ�����������

        // ��������
        Process proc = new Process();
        proc.StartInfo = psi;
        proc.Start();

        // ��cmd��������
        proc.StandardInput.WriteLine(command);
        proc.StandardInput.Flush();
        proc.StandardInput.Close();

        // ��ȡcmd�����
        string output = proc.StandardOutput.ReadToEnd();
        var udpString = output.Split("UDP");
        foreach (var item in udpString)
        {
            UnityEngine.Debug.Log(item);
        }

        // UnityEngine.Debug.Log(output);

        // �ȴ�����ִ�����
        proc.WaitForExit();

        // �رս���
        proc.Close();
        var batFilePath = "C:\\Users\\002\\Desktop\\���\\run.bat";
        // ��ȡ.bat�ļ��е�����
        string[] lines = File.ReadAllLines(batFilePath);

        // �滻.bat�ļ��еĶ˿ں�Ϊ�µĶ˿ں�
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains("set port="))
            {
                lines[i] = "set port=" + 220;
                break; // �ҵ����滻���˳�ѭ��
            }
        }

        // ���޸ĺ����������д��.bat�ļ�
        File.WriteAllLines(batFilePath, lines);
        // ����һ�����̶���
        Process process = new Process();
        // �����Թ���Ա�������
        process.StartInfo.Verb = "runas";
        // ���ý���������Ϣ
        // ProcessStartInfo startInfo = new ProcessStartInfo();
        // startInfo.FileName = "cmd.exe";
        // startInfo.Arguments = "/c " + batFilePath; // ʹ�� cmd ִ�� .bat �ļ�
        // startInfo.UseShellExecute = false; // ��ʹ����ǳ�������
        //startInfo.RedirectStandardOutput = true; // �ض����׼��������Ա����ǿ��Ի�ȡ����ִ�н��
        process.StartInfo.FileName = batFilePath;
        //process.StartInfo = startInfo;

        // ��������
        process.Start();

        // ��ȡ����ִ�н��
        //string outPut = process.StandardOutput.ReadToEnd();
        //UnityEngine.Debug.Log(outPut);

        //�������༭bat
        var batFilePaths = "C:\\Users\\002\\Desktop\\���\\runss.bat";
        string batFileContent = @"
@echo off
setlocal
set port=220
netsh advfirewall firewall add rule name=""Open Port %port%"" dir=in action=allow protocol=UDP localport=%port%
echo ""�˿� %port% �ѳɹ����š�""
";
        File.WriteAllText(batFilePaths, batFileContent);
        // �ȴ����̽���
        process.WaitForExit();

        // �������ִ�н��
        // print(outPut);
    }
}
