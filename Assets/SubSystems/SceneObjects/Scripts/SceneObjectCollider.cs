using System.Collections.Generic;
using UnityEngine;

namespace SubSystems.SceneObjects
{
    public class SceneObjectCollider : MonoBehaviour
    {
        private float normalDirection = 0.08f;
        
        [SerializeField]
        private List<GameBaseValues.TagKind> collidableTags = new List<GameBaseValues.TagKind>
        {
            GameBaseValues.TagKind.Wall,
            GameBaseValues.TagKind.Block
        };
        
        public bool CanMoveTo(Vector3 targetDirection, out GameObject gameObject)
        {
            var offset = new Vector3(GameBaseValues.GRID_SIZE / 2, GameBaseValues.GRID_SIZE / 2, 0);
            var position = transform.position + offset;
            RaycastHit2D[] hits = Physics2D.RaycastAll(position, targetDirection, normalDirection);
            
            Debug.DrawRay(position, targetDirection * normalDirection, Color.red, 1f);

            foreach (var hit in hits)
            {
                if (hit.collider.gameObject == this.gameObject)
                    continue;
                
                if (hit.collider != null)
                {
                    foreach (var tag in collidableTags)
                    {
                        if (hit.collider.CompareTag(GameBaseValues.GetTag(tag)))
                        {
                            gameObject = hit.collider.gameObject;
                            return false;
                        }
                    }
                }
            }
            
            gameObject = null;
            return true;
        }
    }
}