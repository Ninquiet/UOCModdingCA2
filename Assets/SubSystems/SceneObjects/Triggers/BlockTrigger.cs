using System;
using SubSystems.Level;
using UnityEngine;

namespace SubSystems.SceneObjects
{
    public class BlockTrigger : SceneObjectTrigger, IBlockTrigger
    {
        public event Action OnBlockPressed;
        public event Action OnBlockReleased;
        
        private bool _isPressed;
        private GameObject _objectThatIsPressing;

        private void Start()
        {
            LevelBlocksHandler.SubscribeToBlockTrigger(this);
        }

        protected override void OnPressed(GameObject gameObject)
        {
            if (_isPressed)
                return;
            
            _objectThatIsPressing = gameObject;
            if (_objectThatIsPressing == null)
                return;
            
            var iNotifyWhenPressing = gameObject.GetComponent<INotifyWhenPressing>();
            if (iNotifyWhenPressing != null)
                iNotifyWhenPressing.OnPressingSomething(this.gameObject);
            
            _isPressed = true;
            OnBlockPressed?.Invoke();
        }

        protected override void OnReleased()
        {
            if (!_isPressed)
                return;

            if (_objectThatIsPressing != null)
            {
                var iNotifyWhenPressing = _objectThatIsPressing.GetComponent<INotifyWhenPressing>();
                if (iNotifyWhenPressing != null)
                    iNotifyWhenPressing.OnReleasingSomething(this.gameObject);
                
                _objectThatIsPressing = null;
            }
            
            _isPressed = false;
            OnBlockReleased?.Invoke();
        }
        
        protected override void OnDestroySecondPart()
        {
            LevelBlocksHandler.UnsubscribeFromBlockTrigger(this);
        }
    }
}