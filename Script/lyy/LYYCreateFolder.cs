using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Entities.UniversalDelegates;
using UnityEngine;

public class LYYCreateFolder : MonoBehaviour
{

    private int num = 0;
    public int targetNum = 1000;
    // Start is called before the first frame update
    void Start()
    {
        string path = "D:\\frp";
        string targetPath = path;
        var withGangPath = "D:\\frp\\createFolderWith-.bat";
        // 读取.bat文件中的内容
        string[] withGangLines = System.IO.File.ReadAllLines(withGangPath);
        for (int i = 0; i < withGangLines.Length; i++)
        {
            if (withGangLines[i].Contains("set \"basePath="))
            {
                withGangLines[i] = "set \"basePath=D:frp\"";
            }
            if (withGangLines[i].Contains("for /l %%i in "))
            {
                withGangLines[i] = "for /l %%i in (1," + "100," + "1000)do(";
            }
            if (withGangLines[i].Contains("    set /a endNumber="))
            {
                withGangLines[i] = "    set /a endNumber=%%i+99";
            }
        }
        System.IO.File.WriteAllLines(withGangPath, withGangLines);
        Process processWtihGang = new Process();
        processWtihGang.StartInfo.FileName = withGangPath;

        // 启动进程
        processWtihGang.Start();//1-100;
        // 等待进程结束
        processWtihGang.WaitForExit();
        for (int j = 1; j <= targetNum; j+=100)
        {
            var endJ = j + 99;
            for (int i = 0; i < withGangLines.Length; i++)
            {
               
                if (withGangLines[i].Contains("set \"basePath="))
                {
                    withGangLines[i] = "set \"basePath=D:frp\\"+j+"-"+ endJ + "\"";
                }
                if (withGangLines[i].Contains("for /l %%i in "))
                {
                    withGangLines[i] = "for /l %%i in ("+j+",10," + endJ + ")do(";
                }
                if (withGangLines[i].Contains("    set /a endNumber="))
                {
                    withGangLines[i] = "    set /a endNumber=%%i+99";
                }
            }
            System.IO.File.WriteAllLines(withGangPath, withGangLines);
            Process processWtihGang10 = new Process();
            processWtihGang10.StartInfo.FileName = withGangPath;

            // 启动进程
            processWtihGang10.Start();//1-10;
                                      // 等待进程结束
            processWtihGang10.WaitForExit();
        }
        var noGangPath = "D:\\frp\\createFolderOne.bat";
        string[] noGangLines = System.IO.File.ReadAllLines(noGangPath);
        for (int i = 1; i <= targetNum; i+=100)
        {
            var endI = i + 99;
            for (int j = i; j <= endI; j+=10)
            {
                var endJ = j + 9;
                for (int k = 0; k < noGangLines.Length; k++)
                {
                    if (noGangLines[k].Contains("set \"basePath="))
                    {
                        noGangLines[k] = "set \"basePath=D:\\frp\\"+i+"-"+endI+"\\"+j+"-"+endJ+"\"";
                    }
                    if (noGangLines[k].Contains("for /l %%i in ("))
                    {
                        noGangLines[k] = "for /l %%i in (" + j + ", 1, " + endJ + ") do (";
                    }
                }
                System.IO.File.WriteAllLines(noGangPath, noGangLines);
                Process processNoGang = new Process();
                processNoGang.StartInfo.FileName = noGangPath;

                // 启动进程
                processNoGang.Start();//1;
                                      // 等待进程结束
                processNoGang.WaitForExit();

            }
        }


    }


}
