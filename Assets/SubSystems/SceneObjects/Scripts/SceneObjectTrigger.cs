using UnityEngine;
using UnityEngine.Events;

namespace SubSystems.SceneObjects
{
    public abstract class SceneObjectTrigger : MonoBehaviour
    {
        [SerializeField]
        private GameBaseValues.TagKind[] _objectsThatCanTrigger;
        
        public UnityEvent OnObjectPressed;
        public UnityEvent OnObjectReleased;
        
        private bool _isActive = false;
        private void Awake()
        {
            GameBaseValues.CheckTriggers += CheckTrigger;
        }

        public void CheckTrigger()
        {
            var offset = new Vector3(GameBaseValues.GRID_SIZE / 2, GameBaseValues.GRID_SIZE / 2, 0);
            var position = transform.position + offset;
            
            RaycastHit2D[] hits = Physics2D.RaycastAll(position, Vector3.zero, 0f);
            
            foreach (var hit in hits)
            {
                if (hit.collider.gameObject == this.gameObject)
                    continue;
                
                if (hit.collider != null)
                {
                    foreach (var tag in _objectsThatCanTrigger)
                    {
                        if (hit.collider.CompareTag(GameBaseValues.GetTag(tag)))
                        {
                            if (!_isActive)
                            {
                                _isActive = true;
                                OnObjectPressed?.Invoke();
                            }
                            OnPressed(hit.collider.gameObject);
                            return;
                        }
                    }
                }
            }
            
            if (_isActive)
            {
                _isActive = false;
                OnObjectReleased?.Invoke();
            }
            OnReleased();
        }
        
        protected abstract void OnPressed(GameObject gameObject);
        
        protected abstract void OnReleased();

        private void OnDestroy()
        {
            GameBaseValues.CheckTriggers -= CheckTrigger;
            OnDestroySecondPart();
        }

        protected abstract void OnDestroySecondPart();
    }
}