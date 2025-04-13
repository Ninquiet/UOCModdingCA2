using UnityEngine;

namespace SubSystems.SceneObjects
{
    public interface INotifyWhenPressing
    {
        public void OnPressingSomething(GameObject gameObject);
        public void OnReleasingSomething(GameObject gameObject);
    }
}