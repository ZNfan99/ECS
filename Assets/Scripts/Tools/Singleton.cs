using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ÀÁººË«ÖØËøµ¥Àý
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> where T : class,new()
{
    private static T instance;
    private static readonly Object obj = new Object();

    protected Singleton()
    {

    }

    public static T GetT()
    {
        if(instance == null)
        {
            lock(obj)
            {
                if(instance == null)
                {
                    instance = new T();
                }
            }
        }
        return instance;
    }
}
