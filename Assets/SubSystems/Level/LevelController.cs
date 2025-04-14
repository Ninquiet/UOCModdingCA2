using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using SubSystems.SpawnSystem;
using UnityEngine;

namespace SubSystems.Level
{
    [RequireComponent(typeof(LevelUIController))]
    [RequireComponent(typeof(SpawnSystemController))]
    [RequireComponent(typeof(LevelNormalController))]
    public class LevelController : MonoBehaviour
    {
        public static bool MovementIsAllowed = true;
        public static bool IsOnEditMode = false;

        private LevelUIController _levelUIController;
        private SpawnSystemController _spawnSystemController;
        private LevelNormalController _levelNormalController;
        private List<List<int>> _currentLevel;
        private int _selectedLevelToSave;
        private int _selectedLevelToLoad;

        public Action OnLevelCompleted;

        public void GoToMainMenu()
        {
            IsOnEditMode = false;
            _spawnSystemController.Initialize();
            _levelUIController.ShowMainMenu();
            _levelNormalController.HasControl(false);
        }
        
        public void StartNormalModeGame()
        {
            IsOnEditMode = false;
            MovementIsAllowed = true;
            _levelNormalController.HasControl(true);
            _levelNormalController.StartLevel(0);
            
            _levelUIController.ShowInGameOnNormalMode();
        }
        
        public void EnterOnSpawnMode()
        {
            MovementIsAllowed = false;
            _levelUIController.ShowSpawnUI();
            IsOnEditMode = true;
        }

        public void StartEditModeInGame()
        {
            IsOnEditMode = true;
            _currentLevel = _spawnSystemController.GetCurrentLevel();
            MovementIsAllowed = true;
            _levelUIController.ShowInGameOnEdited();
        }
        
        public void SetLevelToSave(string slotIndex)
        {
            if (int.TryParse(slotIndex, out var index))
            {
                _selectedLevelToSave = index;
            }
        }
        
        public void SetLevelToLoad(string slotIndex)
        {
            if (int.TryParse(slotIndex, out var index))
            {
                _selectedLevelToLoad = index;
            }
        }

        public void SaveLevel()
        {
            var currentLevel = _spawnSystemController.GetCurrentLevel();
            SaveLoadController.SaveLevel(currentLevel, _selectedLevelToSave);
        }

        public void LoadLevel(List<List<int>> level)
        {
            SpawnLevel(level);
        }

        public void LoadLevel()
        {
            var level = SaveLoadController.LoadLevel(_selectedLevelToLoad);
            SpawnLevel(level);
        }
        
        public void ExitGame()
        {
            Application.Quit();
        }

        private void SpawnLevel(List<List<int>> level)
        {
            _currentLevel = level;
            StartCoroutine(SpawnLevelWithDelay(level));
        }
        
        private IEnumerator SpawnLevelWithDelay(List<List<int>> level)
        {
            LevelBlocksHandler.Clean();
            yield return new WaitForSeconds(0.2f);
            _spawnSystemController.Initialize();
            LevelBlocksHandler.IsCleaning = true;
            yield return new WaitForSeconds(0.1f);
            LevelBlocksHandler.IsCleaning = false;
            _spawnSystemController.SetLevel(level);
        }

        public void ResetLevel()
        {
            if (_currentLevel == null)
                return;
            
            LoadLevel(_currentLevel);
        }
        
        private void ShowVictoryScreen()
        {
            if (!IsOnEditMode)
                return;
            
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
        }
        
        private void Awake()
        {
            _levelUIController = GetComponent<LevelUIController>();
            _spawnSystemController = GetComponent<SpawnSystemController>();
            _levelNormalController = GetComponent<LevelNormalController>();
            
            LevelBlocksHandler.AllBlocksPressed += HandleAllBlocksPressed;
            
            GoToMainMenu();
        }

        private void HandleAllBlocksPressed(bool allBlocksPressed)
        {
            if (allBlocksPressed)
            {
                ShowVictoryScreen();
                OnLevelCompleted?.Invoke();
            }
        }
    }
}