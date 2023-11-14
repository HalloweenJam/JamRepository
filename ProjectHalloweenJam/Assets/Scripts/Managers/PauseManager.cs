using Player.Controls;
using UnityEngine;

namespace Managers
{
    public class PauseManager : PersistentSingleton<PauseManager>
    {
        private InputReader _inputReader;
        private bool _paused;
    
        public bool isPause => _paused;

        private void Start()
        {
            _inputReader = InputReaderManager.Instance.GetInputReader();
            _inputReader.ExitEvent += Pause;
            GameMenu.Instance.SetActive(false);
            _inputReader.SetPlayerActions();
            _paused = false;
        }

        private void Pause()
        {
            OnGamePause();
        }
        
        private void OnGamePause()
        {
            _paused = true;
            GameMenu.Instance.SetActive(true);
            _inputReader.SetUIActions();
            Time.timeScale = 0.0f;
        }

        public void OffGamePause()
        {
            _inputReader.SetPlayerActions();
            _paused = false;
            GameMenu.Instance.SetActive(false);
        
            Time.timeScale = 1.0f;
        }

        public void ExitGameMenu()
        {
            _paused = false;
            GameMenu.Instance.SetActive(false);
            Time.timeScale = 1.0f;
        } 
    }
}
