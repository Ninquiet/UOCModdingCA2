using UnityEngine;

namespace SubSystems.SceneObjects
{
    public class BlockController : MonoBehaviour ,INotifyWhenPressing
    {
        [SerializeField]
        private Animator _animator;
        
        private GameObject _pressedObject;
        
        public void OnPressingSomething(GameObject gameObject)
        {
            _pressedObject = gameObject;
            
            _animator.SetBool("BlockIsPressed", true);
        }

        public void OnReleasingSomething(GameObject gameObject)
        {
            if (_pressedObject != null)
                if (_pressedObject != gameObject)
                    return;
                
            
            _animator.SetBool("BlockIsPressed", false);
        }
    }
}