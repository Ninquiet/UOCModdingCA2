using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TestClasses;
/// <summary>
/// Test class with simple types' methods
/// </summary>
public class SimpleTest : MonoBehaviour
{
    public enum MyEnum
    {
        enumval1,
        enumval2,
        enumvalNum45 = 45
    }

    [CallableFunction]
    public void VoidFuntion()
    {
        Debug.Log("SimpleTest/VoidFunction(void)");
    }
    [CallableFunction]
    public void IntMethod(int i)
    {
        Debug.Log("SimpleTest/IntMethod, val = " + i);
    }
    [CallableFunction]
    public void ShortAndCharMethod(short i, char c)
    {
        Debug.Log("SimpleTest/ShortAndCharMethod, short = " + i.ToString() + ", char = " + c.ToString());
    }

    [CallableFunction]
    public void FloatAndFloat(float a, float b)
    {
        Debug.Log("SimpleTest/FloatAndFloat, a = " + a.ToString() + ", b = " + b.ToString());
    }

    [CallableFunction]
    public void LongAndUint(long a, uint b)
    {
        Debug.Log("SimpleTest/LongAndUint" + a.ToString() + "," + b.ToString());
    }

    [CallableFunction]
    public void DecimalMethod(decimal d)
    {

        Debug.Log("SimpleTest/DecimalMethod, val = " + d.ToString());
    }

   
    [CallableFunction]
    public void Vector2Method(Vector2 ve)
    {
        Debug.Log("SimpleTest/Vector2Method " + ve.ToString());
    }
    [CallableFunction]
    public void Vector3Method(Vector3 ve)
    {
        Debug.Log("SimpleTest/Vector3Method " + ve.ToString());
    }
    [CallableFunction]
    public void Vector4Method(Vector4 ve)
    {
        Debug.Log("SimpleTest/Vector4Method " + ve.ToString());
    }
    [CallableFunction]
    public void StringMethod(string str)
    {
        Debug.Log("SimpleTest/StringMethod, val = " + str);
    }

    [CallableFunction]
    public void BoolMethod(bool b)
    {
        Debug.Log("SimpleTest/BoolMethod, val = " + b.ToString());
    }
    [CallableFunction]
    public void MyEnumMethod(MyEnum va)
    {
        Debug.Log("SimpleTest/MyEnumMethod, val = " + va.ToString());
    }

    [CallableFunction]
    public string StringReturnMethod()
    {
        return "Hello here!";
    }

    [CallableFunction]
    public int IntReturnMethod()
    {
        return 1234;
    }

    [CallableFunction]
    public Vector3 VectorReturnMethod()
    {
        return transform.position;
    }
    [CallableFunction]
    public void ColorSetMethod(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
        Debug.Log("SimpleTest/ColorSetMethod, val = " + color.ToString());
    }

    [CallableFunction]
    public void SetSpriteMethod(Sprite sprite)
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
        if (sprite == null)
            Debug.Log("SimpleTest/SetSpriteMethod, val = null");
        else
            Debug.Log("SimpleTest/SetSpriteMethod, name = " + sprite.name);
    }

    [CallableFunction]
    public void SpawnSomeAround(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.Log("SimpleTest/SpawnSomeAround, val = null");
            return;
        }
        Vector2[] poses = new[] { Vector2.up * 5, Vector2.left * 5, Vector2.down * 5, Vector2.right * 5 };
        foreach (var vector2 in poses)
        {
            Instantiate(prefab, vector2, new Quaternion());
        }
        Debug.Log("SimpleTest/SpawnSomeAround, name = " + prefab.name);

    }

    [CallableFunction]
    public void SpawnTwo(GameObject a, GameObject b)
    {
        if (a != null)
            Instantiate(a, Vector2.up*5, new Quaternion());
        if (b != null)
            Instantiate(b, Vector2.down*5, new Quaternion());
    }
    [CallableFunction]
    public void MoveToTransform(Transform t)
    {
        //transform.position = t.position;
        StartCoroutine(MoveCorut(t.position));
    }

    [CallableFunction]
    public void MoveToTarget(Vector3 target)
    {
        StartCoroutine(MoveCorut(target));
    }

    [CallableFunction]
    public void RectMethod(Rect r)
    {
        Debug.Log("SimpleTest/RectMethod, val = " + r.ToString());
    }
    [CallableFunction]
    public void GiantMethod(int a, float b, string str, Color col, Vector2 v2, Vector3 v3, MyEnum enu, Sprite spr,
        GameObject ob, Transform t)
    {
        Debug.Log("SimpleTest/GiantMethod");
    }

    private IEnumerator MoveCorut(Vector3 target)
    {
        Vector3 from = transform.position;
        for (int i = 1; i <= 50; i++)
        {
            transform.position = Vector3.Lerp(from, target, i * 1.0f / 50);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
