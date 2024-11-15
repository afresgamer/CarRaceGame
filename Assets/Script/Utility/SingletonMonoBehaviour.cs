using System;
using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                Type t = typeof(T);

                instance = (T)FindObjectOfType(t);
                if (instance == null)
                {
                    CustomDebugger.ColorLog(t + "がありません。", GameConst.LogLevel.Red);
                }
            }

            return instance;
        }
    }

    virtual protected void Awake()
    {
        CheckInstance();
    }

    protected bool CheckInstance()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(Instance);
            return true;
        }
        else if (Instance == this)
        {
            DontDestroyOnLoad(Instance);
            return true;
        }
        Destroy(this);
        return false;
    }
}
