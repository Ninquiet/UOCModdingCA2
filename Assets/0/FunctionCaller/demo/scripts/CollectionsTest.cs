using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
/// <summary>
/// Test class for collections' method calls
/// </summary>
public class CollectionsTest : MonoBehaviour {
    
    [CallableFunction]
    public void ArrayOfInts(int[] arr)
    {
        Debug.Log("CollectionsTest/ArrayOfInts. Count = " + arr.Length + ", Sum = " + arr.Sum().ToString());
    }

    [CallableFunction]
    public void ArrayOfArraysOfInts(int[][] arr)
    {
        
        Debug.Log("CollectionsTest/ArrayOfArraysOfInts. Count = " +arr.Length + ", sum = " + (from c in arr select c.Sum()).Sum());
    }
    
    [CallableFunction]
    public void ListIfInts(List<int> list)
    {
        Debug.Log("CollectionsTest/ListOfInts. count = " + list.Count + ", sum = " + list.Sum());
    }

    [CallableFunction]
    public int[] ArrayReturnMethod()
    {
        return new int[] {1,2,3,4};
    }

    [CallableFunction]
    public List<int> ListReturnMethod()
    {
        return new List<int>(new int[] {1,2,3,4});
    }
    [CallableFunction]
    public void GiantMethod(int[] arr, List<string> lst, List<Color> colors, Vector3[] vectors)
    {
        Debug.Log("CollectionsTest/GiantMethod");
    }
}
