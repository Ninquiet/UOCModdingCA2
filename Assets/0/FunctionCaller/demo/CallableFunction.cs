using System;
using UnityEngine;
using System.Collections;
//uncomment namespace declaration, if you want to move attribute to namespace
//namespace YetAnotherTools.FunctionCaller
//{
    /// <summary>
    /// Attribute to mark callable functions
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class CallableFunctionAttribute : System.Attribute
    {
        /// <summary>
        /// Tags of given method
        /// </summary>
        private string[] tags;
        /// <summary>
        /// Tags of given method
        /// </summary>
        public string[] Tags
        {
            get { return tags; }
        }

        public CallableFunctionAttribute(params string[] tags)
        {
            this.tags = tags;
        }

        public CallableFunctionAttribute()
        {
            tags = new string[0];
        }
    }
//}
