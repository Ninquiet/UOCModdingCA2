using UnityEngine;
using System.Collections;
using TestClasses;
/// <summary>
/// Test class for generic methods
/// </summary>
public class GenericTest : MonoBehaviour {
    [CallableFunction]
    public void GenericCharacter<T>(T o) where T : CharacterTestClass
    {
        Debug.Log("GenericTest/GenericCharacter, type = " + typeof(T).Name + ", val = " + o.ToString());
    }
    [CallableFunction]
    public void GenericNoRestriction<T> (T o)
    {
        Debug.Log("GenericTest/GenericNoRestriction, type = " + typeof(T).Name + ", val = " + o.ToString());
    }
}
