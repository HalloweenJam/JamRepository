using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Controls
{
    [CreateAssetMenu(menuName = "Input Reader")]
    public class InputReader : ScriptableObject, GameControls.IGameplayActions, GameControls.IUIActions
    {
        private GameControls _gameControls;

        public event Action<float> MouseWheelScrollEvent; 
        
        public event Action<Vector2> MoveEvent;
        public event Action<Vector2> MousePosition;

        public event Action DashEvent;

        public event Action ShootingEvent;
        public event Action ShootingCancelledEvent;

        public event Action InteractEvent;
        
        public event Action ExitEvent;
        
        private void OnEnable()
        {
            if (_gameControls != null) 
                return;
            
            _gameControls = new GameControls();
                
            _gameControls.Gameplay.SetCallbacks(this);
            _gameControls.UI.SetCallbacks(this);
            
            SetPlayerActions();
        }
        
        public void SetPlayerActions()
        {
            _gameControls.Gameplay.Enable();
            _gameControls.UI.Disable();
        }

        public void SetUIActions()
        {
            _gameControls.Gameplay.Disable();
            _gameControls.UI.Enable();
        }
        
        public void OnMovement(InputAction.CallbackContext context)
        {
            MoveEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                DashEvent?.Invoke();
            }
        }

        public void OnShoot(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                ShootingEvent?.Invoke();

            if (context.phase == InputActionPhase.Canceled)
                ShootingCancelledEvent?.Invoke();
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                ExitEvent?.Invoke();
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                InteractEvent?.Invoke();
        }

        public void OnMouseWheelScroll(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
                MouseWheelScrollEvent?.Invoke(context.ReadValue<float>());
        }

        public void OnMousePosition(InputAction.CallbackContext context)
        {
                MousePosition?.Invoke(context.ReadValue<Vector2>());
        }

    }
}