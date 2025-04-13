using UnityEngine;
using System.Collections;

public class TagAndAccessTest : MonoBehaviour {

    #region public
    [CallableFunction("Tag1")]
    public void Tag1PublicFunction()
    {
        Debug.Log("TagAndAccessTest/Tag1PublicFunction");
    }
    [CallableFunction("Tag2")]
    public void Tag2PublicFunction()
    {
        Debug.Log("TagAndAccessTest/Tag2PublicFunction");
    }
    [CallableFunction("Tag1", "Tag2")]
    public void Tag1Tag2PublicFunction()
    {
        Debug.Log("TagAndAccessTest/Tag1Tag2PublicFunction");
    }
    [CallableFunction]
    public void NoTagPublicFunction()
    {
        Debug.Log("TagAndAccessTest/NoTagPublicFunction");
    }

    public void NotMarkedPublicFunction()
    {
        Debug.Log("TagAndAccessTest/NotMarkedPublicFunction");
    }
    #endregion
    #region non public
    [CallableFunction("Tag1")]
    private void Tag1PrivateFunction()
    {
        Debug.Log("TagAndAccessTest/Tag1PrivateFunction");
    }
    [CallableFunction("Tag2")]
    protected void Tag2ProtectedFunction()
    {
        Debug.Log("TagAndAccessTest/Tag2ProtectedFunction");
    }
    [CallableFunction("Tag1", "Tag2")]
    internal void Tag1Tag2InternalFunction()
    {
        Debug.Log("TagAndAccessTest/Tag1Tag2InternalFunction");
    }
    [CallableFunction]
    private void NoTagPrivateFunction()
    {
        Debug.Log("TagAndAccessTest/NoTagPrivateFunction");
    }
    protected void NotMarkedProtectedFunction()
    {
        Debug.Log("TagAndAccessTest/NotMarkedProtectedFunction");
    }
    #endregion
    #region static
    [CallableFunction("Tag1")]
    private static void Tag1PrivateStaticFunction()
    {
        Debug.Log("TagAndAccessTest/Tag1PrivateStaticFunction");
    }
    [CallableFunction("Tag2")]
    public static void Tag2PublicStaticFunction()
    {
        Debug.Log("TagAndAccessTest/Tag2PublicStaticFunction");
    }
    [CallableFunction("Tag1", "Tag2")]
    public static void Tag1Tag2PublicStaticFunction()
    {
        Debug.Log("TagAndAccessTest/Tag1Tag2PublicStaticFunction");
    }
    [CallableFunction]
    private static void NoTagInternalStaticFunction()
    {
        Debug.Log("TagAndAccessTest/NoTagInternalStaticFunction");
    }
    private static void NotMarkedPrivateStaticFunction()
    {
        Debug.Log("TagAndAccessTest/NotMarkedPrivateStaticFunction");
    }
    #endregion

}
