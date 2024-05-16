using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.IO;

public class RunCmd : MonoBehaviour
{
    // 要执行的命令
    public string command = "dir"; // 例如 "dir" (列出目录内容)

    void Start()
    {
        // 创建一个ProcessStartInfo对象，用于设置启动进程的一些属性
        ProcessStartInfo psi = new ProcessStartInfo();
        psi.FileName = "cmd"; // 指定要启动的程序
        psi.RedirectStandardInput = true; // 允许重定向标准输入流
        psi.RedirectStandardOutput = true; // 允许重定向标准输出流
        psi.CreateNoWindow = true; // 不创建cmd窗口
        psi.UseShellExecute = false; // 不使用操作系统外壳程序启动进程

        // 启动进程
        Process proc = new Process();
        proc.StartInfo = psi;
        proc.Start();

        // 向cmd输入命令
        proc.StandardInput.WriteLine(command);
        proc.StandardInput.Flush();
        proc.StandardInput.Close();

        // 读取cmd的输出
        string output = proc.StandardOutput.ReadToEnd();
        var udpString = output.Split("UDP");
        foreach (var item in udpString)
        {
            UnityEngine.Debug.Log(item);
        }

        // UnityEngine.Debug.Log(output);

        // 等待命令执行完成
        proc.WaitForExit();

        // 关闭进程
        proc.Close();
        var batFilePath = "C:\\Users\\002\\Desktop\\快捷\\run.bat";
        // 读取.bat文件中的内容
        string[] lines = File.ReadAllLines(batFilePath);

        // 替换.bat文件中的端口号为新的端口号
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains("set port="))
            {
                lines[i] = "set port=" + 220;
                break; // 找到并替换后退出循环
            }
        }

        // 将修改后的内容重新写入.bat文件
        File.WriteAllLines(batFilePath, lines);
        // 创建一个进程对象
        Process process = new Process();
        // 设置以管理员身份运行
        process.StartInfo.Verb = "runas";
        // 设置进程启动信息
        // ProcessStartInfo startInfo = new ProcessStartInfo();
        // startInfo.FileName = "cmd.exe";
        // startInfo.Arguments = "/c " + batFilePath; // 使用 cmd 执行 .bat 文件
        // startInfo.UseShellExecute = false; // 不使用外壳程序启动
        //startInfo.RedirectStandardOutput = true; // 重定向标准输出流，以便我们可以获取命令执行结果
        process.StartInfo.FileName = batFilePath;
        //process.StartInfo = startInfo;

        // 启动进程
        process.Start();

        // 读取命令执行结果
        //string outPut = process.StandardOutput.ReadToEnd();
        //UnityEngine.Debug.Log(outPut);

        //创建并编辑bat
        var batFilePaths = "C:\\Users\\002\\Desktop\\快捷\\runss.bat";
        string batFileContent = @"
@echo off
setlocal
set port=220
netsh advfirewall firewall add rule name=""Open Port %port%"" dir=in action=allow protocol=UDP localport=%port%
echo ""端口 %port% 已成功开放。""
";
        File.WriteAllText(batFilePaths, batFileContent);
        // 等待进程结束
        process.WaitForExit();

        // 输出命令执行结果
        // print(outPut);
    }
}
