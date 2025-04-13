using UnityEngine;

namespace SubSystems.Level
{
    public class LevelUIController : MonoBehaviour
    {
        [SerializeField]
        CanvasGroup _mainMenuCanvasGroup;
        [SerializeField]
        CanvasGroup _spawnedObjectCanvasGroup;
        [SerializeField]
        CanvasGroup _inGameOnEditedCanvasGroup;
        [SerializeField] 
        private CanvasGroup _inGameOnNormalMode;
        
        [Header("SecondLevel")]
        [SerializeField]
        CanvasGroup _victoryScreenCanvasGroup;
        
        public void ShowMainMenu()
        {
            HideInGameOnNormalMode();
            HideInGameOnEdited();
            HideSpawnUI();
            
            EnableCanvasGroup(_mainMenuCanvasGroup,true);
        }
        
        public void ShowInGameOnNormalMode()
        {
            HideMainMenu();
            HideSpawnUI();
            HideInGameOnEdited();
            
            EnableCanvasGroup(_inGameOnNormalMode,true);
        }
        
        public void HideInGameOnNormalMode()
        {
            EnableCanvasGroup(_inGameOnNormalMode,false);
        }
        
        public void HideMainMenu()
        {
            EnableCanvasGroup(_mainMenuCanvasGroup,false);
        }
        
        public void ShowSpawnUI()
        {
            HideMainMenu();
            HideInGameOnEdited();
            EnableCanvasGroup(_spawnedObjectCanvasGroup,true);
        }

        public void HideSpawnUI()
        {
            EnableCanvasGroup(_spawnedObjectCanvasGroup,false);
        }

        public void ShowInGameOnEdited()
        {
            HideSpawnUI();
            EnableCanvasGroup(_inGameOnEditedCanvasGroup,true);
        }
        
        public void ShowVictoryScreen()
        {
            EnableCanvasGroup(_victoryScreenCanvasGroup,true);
        }
        
        public void HideVictoryScreen()
        {
            EnableCanvasGroup(_victoryScreenCanvasGroup,false);
        }

        public void HideInGameOnEdited()
        {
            EnableCanvasGroup(_inGameOnEditedCanvasGroup,false);
        }
        
        private void EnableCanvasGroup(CanvasGroup canvasGroup , bool enable)
        {
            canvasGroup.alpha = enable?1:0;
            canvasGroup.interactable = enable;
            canvasGroup.blocksRaycasts = enable;
        }
    }
}