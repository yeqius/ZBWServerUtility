using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WWWBase : MonoBehaviour
{
    public string url;

    private bool __isWaitingForURL;

    public IEnumerator WaitingForURL()
    {
        while (__isWaitingForURL)
            yield return null;
    }

    protected IEnumerator Start()
    {
        __isWaitingForURL = string.IsNullOrEmpty(url);
        if (__isWaitingForURL)
        {
            while(!GameConstantManager.isInit)
            {
                yield return null;
            }

            url = GameConstantManager.Get(GetType());

            __isWaitingForURL = false;
        }
    }

    protected void Awake()
    {
        __isWaitingForURL = string.IsNullOrEmpty(url);
    }
}
