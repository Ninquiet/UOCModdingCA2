using System;
using DG.Tweening;
using SubSystems.SceneObjects;
using UnityEngine;

namespace SubSystems.Player
{
    [RequireComponent(typeof(SceneObjectCollider))]
    public class PlayerMovementController : SceneObjectMovement
    {
        private bool _playerCanMove = true;
        
        protected override void AfterAwake()
        {
            PlayerInputsController.OnRightPressed += TryMoveRight;
            PlayerInputsController.OnLeftPressed += TryMoveLeft;
            PlayerInputsController.OnUpPressed += TryMoveUp;
            PlayerInputsController.OnDownPressed += TryMoveDown;
        }

        private void TryMoveDown()
        {
            TryPerformMovementTo(Vector3.down);
        }

        private void TryMoveUp()
        {
            TryPerformMovementTo(Vector3.up);
        }

        private void TryMoveLeft()
        {
            TryPerformMovementTo(Vector3.left);
        }

        private void TryMoveRight()
        {
            TryPerformMovementTo(Vector3.right);
        }

        protected override void OnMovementDone()
        {
            GameBaseValues.CheckTriggers?.Invoke();
        }

        private void OnDestroy()
        {
            PlayerInputsController.OnRightPressed -= TryMoveRight;
            PlayerInputsController.OnLeftPressed -= TryMoveLeft;
            PlayerInputsController.OnUpPressed -= TryMoveUp;
            PlayerInputsController.OnDownPressed -= TryMoveDown;
        }
    }
}