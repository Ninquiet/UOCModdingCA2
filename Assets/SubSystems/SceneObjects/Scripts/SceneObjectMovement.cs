using System;
using DG.Tweening;
using SubSystems.Level;
using UnityEngine;

namespace SubSystems.SceneObjects
{
    [RequireComponent(typeof(SceneObjectCollider))]
    public class SceneObjectMovement : MonoBehaviour
    {
        [SerializeField]
        private GameBaseValues.TagKind[] _canMoveObjects;
        [SerializeField] 
        private bool _canBeMoved;
        
        [SerializeField]
        private SceneObjectCollider _sceneObjectCollider;
        private Tween _movementTween;
        private bool _objectCanMove = true;
        
        public bool CanBeMoved => _canBeMoved;
        
        private void Awake()
        {
            _sceneObjectCollider = GetComponent<SceneObjectCollider>();
            AfterAwake();
        }
        
        protected virtual void AfterAwake(){}
        
        protected bool TryPerformMovementTo(Vector3 direction)
        {
            if (!_objectCanMove || !_canBeMoved || !LevelController.MovementIsAllowed)
                return false;
            
            var canMove =_sceneObjectCollider.CanMoveTo(direction, out var collidedObject);

            if (!canMove)
            {
                canMove = TryMoveObject(collidedObject, direction);

                if (!canMove)
                    return false;
            }
            
            _ = MoveTo(direction);
            return true;
        }

        private bool TryMoveObject(GameObject collidedObject, Vector3 direction)
        {
            foreach (var tag in _canMoveObjects)
            {
                if (collidedObject.CompareTag(GameBaseValues.GetTag(tag)))
                {
                    var sceneObjectMovement = collidedObject.GetComponent<SceneObjectMovement>();
                    if (sceneObjectMovement == null) 
                        continue;
                    
                    if (!sceneObjectMovement.CanBeMoved)
                        return false;
                        
                    var canMoveObject = sceneObjectMovement.TryPerformMovementTo(direction);
                    return canMoveObject;
                }
            }
            
            return false;
        }
        
        protected Tween MoveTo(Vector3 direction)
        {
            _movementTween?.Kill();
            var sequence = DOTween.Sequence();
            _objectCanMove = false;
            
            sequence.Append(transform.DOMove(transform.position+(direction * GameBaseValues.GRID_SIZE), 0.3f).SetEase(Ease.InSine));
            sequence.OnComplete(() => {
                _objectCanMove = true;
                OnMovementDone();
            });
            
            _movementTween = sequence;
            return _movementTween;
        }

        protected virtual void OnMovementDone()
        {
        }
    }
}