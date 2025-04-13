using UnityEngine;

namespace SubSystems.Level
{
    public class LevelUIController : MonoBehaviour
    {
        [SerializeField]
        CanvasGroup _spawnedObjectCanvasGroup;
        
        public void ShowSpawnUI()
        {
            _spawnedObjectCanvasGroup.alpha = 1;
            _spawnedObjectCanvasGroup.interactable = true;
        }

        public void HideSpawnUI()
        {
            _spawnedObjectCanvasGroup.alpha = 0;
            _spawnedObjectCanvasGroup.interactable = false;
        }
    }
}