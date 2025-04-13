using System;
using UnityEngine;

namespace SubSystems.SceneObjects
{
    public class GameBaseValues
    {
        public static float GRID_SIZE = 0.08f;
        
        public static string PLAYER_TAG = "Player";
        public static string ENEMY_TAG = "Enemy";
        public static string WALL_TAG = "Wall";
        public static string BLOCK_TAG = "Block";
        public static string TRIGGER_TAG = "Trigger";
        
        public static int GAME_OBJECT_LAYER = 6;
        
        public static Action CheckTriggers;
        
        public static void ResetCheckTriggers()
        {
            CheckTriggers = null;
        }
        
        public static string GetTag(TagKind tagKind)
        {
            switch (tagKind)
            {
                case TagKind.Player:
                    return PLAYER_TAG;
                case TagKind.Enemy:
                    return ENEMY_TAG;
                case TagKind.Wall:
                    return WALL_TAG;
                case TagKind.Block:
                    return BLOCK_TAG;
                case TagKind.Trigger:
                    return TRIGGER_TAG;
                default:
                    return null;
            }
        }
        
        public enum TagKind
        {
            Player,
            Enemy,
            Wall,
            Block,
            Trigger
        }
    }
}