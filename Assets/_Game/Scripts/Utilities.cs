using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public static class Utilities
{
    //random gia enum trong mot kieu enum
    private static readonly Random Random = new();

    //sap xep lai list
    public static List<T> SortOrder<T>(IEnumerable<T> list, int amount)
    {
        return list.OrderBy(_ => Guid.NewGuid()).Take(amount).ToList();
    }

    public static void LookTarget(Transform owner, Transform target)
    {
        owner.LookAt(target);
        owner.rotation = Quaternion.Euler(0, owner.rotation.eulerAngles.y, 0);
    }
    
    public static void LookTarget(Transform owner, Vector3 direction)
    {
        owner.LookAt(direction);
        owner.rotation = Quaternion.Euler(0, owner.rotation.eulerAngles.y, 0);
    }
    
    public static void DoAfterSeconds(ref float time, Action action, Action onWait = null)
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            onWait?.Invoke();
        }
        else
        {
            action();
            time = 0;
        }
    }
    
    public static bool CheckTime(ref float time){
        if (time > 0) time -= Time.deltaTime;
        else return true;
        return false;
    }    
    
    //lay ket qua theo ty le xac suat
    public static bool Chance(int rand, int max = 100)
    {
        return UnityEngine.Random.Range(0, max) < rand;
    }

    public static T RandomEnumValue<T>()
    {
        Array v = Enum.GetValues(typeof(T));
        return (T)v.GetValue(Random.Next(v.Length));
    }

    //random gia tri tu 1 list
    public static T RandomInMember<T>(params T[] ts)
    {
        return ts[UnityEngine.Random.Range(0, ts.Length)];
    }

    public static void ShuffleList<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }
}