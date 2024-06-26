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
        public event Action MoveCancelledEvent;

        public event Action DashEvent;
        public event Action ReloadEvent;

        public event Action ShootingEvent;
        public event Action ShootingCancelledEvent;

        public event Action OpenMinimapEvent;
        public event Action InteractEvent;

        public event Action ClearProjectilesAction;
        
        public event Action ExitEvent;
        public event Action DialogueEvent;
        
        private void OnEnable()
        {
            if (_gameControls != null) 
                return;
            
            _gameControls = new GameControls();
                
            _gameControls.Gameplay.SetCallbacks(this);
            _gameControls.UI.SetCallbacks(this);
            
            SetPlayerActions();
        }

        public void Disable()
        {
            _gameControls.UI.Disable();
            _gameControls.Gameplay.Disable();
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
            
            if (context.phase == InputActionPhase.Canceled)
                MoveCancelledEvent?.Invoke();
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
        }

        public void OnMinimap(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                OpenMinimapEvent?.Invoke();
        }

        public void OnReload(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                ReloadEvent?.Invoke();
        }

        public void OnDialogue(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                DialogueEvent?.Invoke();
        }

        public void OnClearProjectiles(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                ClearProjectilesAction?.Invoke();
        }
    }
}