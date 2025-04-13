using System;
using UnityEngine;

namespace SubSystems.Player
{
    public class PlayerInputsController : MonoBehaviour
    {
        public static Action OnLeftPressed;
        public static Action OnRightPressed;
        public static Action OnUpPressed;
        public static Action OnDownPressed;
        
        private void Update()
        {
            CheckInputs();
        }

        private void CheckInputs()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                OnLeftPressed?.Invoke();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                OnRightPressed?.Invoke();
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                OnUpPressed?.Invoke();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                OnDownPressed?.Invoke();
            }
        }
    }
}