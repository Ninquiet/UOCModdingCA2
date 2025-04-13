using DG.Tweening;
using UnityEngine;

namespace SubSystems.Level
{
    [RequireComponent(typeof(LevelController))]
    [RequireComponent(typeof(LevelUIController))]
    public class LevelNormalController : MonoBehaviour
    {
        [SerializeField]
        private TextAsset[] _levelJsons;
        
        private bool _hasControl;
        private LevelController _levelController;
        private LevelUIController _levelUIController;
        private int _currentLevelIndex;

        public void HasControl(bool hasControl)
        {
            if (_hasControl == hasControl)
                return;

            if (hasControl)
            {
                _levelController.OnLevelCompleted += OnLevelCompleted;
                Initialize();
            }
            else
            {
                _levelController.OnLevelCompleted -= OnLevelCompleted;
            }
        }
        
        private void Awake()
        {
            _levelController = GetComponent<LevelController>();
            _levelUIController = GetComponent<LevelUIController>();
        }

        private void OnLevelCompleted()
        {
            if (!_hasControl)
                return;
            
            if (_currentLevelIndex + 1 >= _levelJsons.Length)
            {
                ShowVictoryScreen();
                return;
            }
            
            _currentLevelIndex++;
            StartLevel(_currentLevelIndex);
            
        }
        
        private void ShowVictoryScreen()
        {
            var sequence = DOTween.Sequence();

            sequence.AppendCallback(() =>
            {
                _levelUIController.ShowVictoryScreen();
            });
            sequence.AppendInterval(0.5f);
            sequence.AppendCallback(() =>
            {
                _levelUIController.HideVictoryScreen();
            });
            sequence.AppendInterval(0.2f);
            sequence.AppendCallback(() =>
            {
                _levelController.GoToMainMenu();
            });
        }

        private void Initialize()
        {
        }
        
        public void StartLevel(int levelIndex)
        {
            _currentLevelIndex = levelIndex;
            string json = _levelJsons[levelIndex].text;
            var level = SaveLoadController.ParseJsonToLevel(json);
            _levelController.LoadLevel(level);
        }

        private void OnDestroy()
        {
            if (_levelController != null)
                _levelController.OnLevelCompleted -= OnLevelCompleted;
        }
    }
}