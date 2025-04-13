using System;
using SubSystems.SceneObjects;
using UnityEngine;

namespace SubSystems.Level
{
    public static class LevelBlocksHandler
    {
        public static Action<bool> AllBlocksPressed;
        
        public static bool IsCleaning;
        
        private static int _currentActiveTriggerBlocks;
        private static int _maxTriggerBlocks;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Clean()
        {
            _currentActiveTriggerBlocks = 0;
            _maxTriggerBlocks = 0;
            IsCleaning = true;
        }
        
        public static void SubscribeToBlockTrigger(IBlockTrigger blockTrigger)
        {
            if (IsCleaning)
                return;
            
            _maxTriggerBlocks ++;
            blockTrigger.OnBlockPressed += HandleBlockPressed;
            blockTrigger.OnBlockReleased += HandleBlockReleased;
        }
        
        public static void UnsubscribeFromBlockTrigger(IBlockTrigger blockTrigger)
        {
            if (IsCleaning)
                return;
            
            _maxTriggerBlocks -= 1;
            if (_maxTriggerBlocks < 0)
                _maxTriggerBlocks = 0;
            
            blockTrigger.OnBlockPressed -= HandleBlockPressed;
            blockTrigger.OnBlockReleased -= HandleBlockReleased;
        }

        private static void HandleBlockReleased()
        {
            if (IsCleaning)
                return;
            
            if (_currentActiveTriggerBlocks >= _maxTriggerBlocks)
            {
                AllBlocksPressed?.Invoke(false);
            }
            
            _currentActiveTriggerBlocks -= 1;
            if (_currentActiveTriggerBlocks < 0)
                _currentActiveTriggerBlocks = 0;
        }

        private static void HandleBlockPressed()
        {
            if (IsCleaning)
                return;
            
            _currentActiveTriggerBlocks += 1;
             if (_currentActiveTriggerBlocks >= _maxTriggerBlocks)
            {
                AllBlocksPressed?.Invoke(true);
            }
        }
    }
}