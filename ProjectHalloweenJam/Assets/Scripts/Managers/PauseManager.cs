using Player.Controls;
using UnityEngine;

namespace Managers
{
    public class PauseManager : MonoBehaviour
    {

        private InputReader _inputReader;
        private bool _paused;
    
        public bool isPause => _paused;

        public static PauseManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }

            DontDestroyOnLoad(gameObject);
        }
        void Start()
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

        // Update is called once per frame
        void OnGamePause()
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
