using SubSystems.SpawnSystem;
using UnityEngine;

namespace SubSystems.Level
{
    [RequireComponent(typeof(LevelUIController))]
    [RequireComponent(typeof(SpawnSystemController))]
    public class LevelController : MonoBehaviour
    {
        public static bool MovementIsAllowed = true;
        
        private LevelUIController _levelUIController;
        private SpawnSystemController _spawnSystemController;

        public void StartGame()
        {
            MovementIsAllowed = true;    
            _levelUIController.HideSpawnUI();
        }
        
        public void SaveLevel(int slotIndex)
        {
            var currentLevel = _spawnSystemController.GetCurrentLevel();
            SaveLoadController.SaveLevel(currentLevel, slotIndex);
        }
        
        public void LoadLevel(int slotIndex)
        {
            var loadedLevel = SaveLoadController.LoadLevel(slotIndex);
            _spawnSystemController.SetLevel(loadedLevel);
        }

        public void EnterOnSpawnMode()
        {
            MovementIsAllowed = false;
            _levelUIController.ShowSpawnUI();
        }
        
        private void Awake()
        {
            _levelUIController = GetComponent<LevelUIController>();
            _spawnSystemController = GetComponent<SpawnSystemController>();
            
            LevelBlocksHandler.AllBlocksPressed += HandleAllBlocksPressed;
            EnterOnSpawnMode();
        }

        private void HandleAllBlocksPressed(bool allBlocksPressed)
        {
            if (allBlocksPressed)
            {
                Debug.Log("All blocks pressed: ");
            }
            else
            {
                Debug.Log("Not all blocks pressed: ");
            }
        }
    }
}