using SubSystems.SceneObjects;
using UnityEngine;

namespace SubSystems.SpawnSystem
{
    [RequireComponent(typeof(SpawnSystemController))]
    public class SpawnMouseController : MonoBehaviour
    {
        private SpawnSystemController _spawnSystemController;
        private int _mouseHoveringColumnIndex = -1;
        private int _mouseHoveringRowIndex = -1;
        private Camera _mainCamera;
     
        private void Awake()
        {
            _spawnSystemController  = GetComponent<SpawnSystemController>(); 
        }

        private void Update()
        {
            GetMouseCoordinates();
            CheckIfMousePressed();
        }
        
        private void CheckIfMousePressed()
        {
            if ( Input.GetMouseButtonDown(0))
            {
                if (_mouseHoveringColumnIndex == -1 || _mouseHoveringRowIndex == -1)
                    return;
                
                _spawnSystemController.SpawnSelectedObjectOnCoordinate(_mouseHoveringColumnIndex, _mouseHoveringRowIndex);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                if (_mouseHoveringColumnIndex == -1 || _mouseHoveringRowIndex == -1)
                    return;

                _spawnSystemController.DestroyObjectOnCoordinate(_mouseHoveringColumnIndex, _mouseHoveringRowIndex);
            }
        }
        
        private void GetMouseCoordinates()
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(GetMainCamera().ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            foreach (var hit in hits)
            {
                if (hit.collider.gameObject == this.gameObject)
                    continue;

                if (hit.collider == null) 
                    continue;

                if (hit.collider.gameObject.layer != GameBaseValues.GAME_OBJECT_LAYER) 
                    continue;
                
                var spawnableObject = hit.collider.GetComponent<SpawnableObject>();
                if (spawnableObject == null)
                    continue;
                
                _mouseHoveringColumnIndex = spawnableObject.columnIndex;
                _mouseHoveringRowIndex = spawnableObject.rowIndex;
                
                return;
            }
            
            _mouseHoveringColumnIndex = -1;
            _mouseHoveringRowIndex = -1;
        }
        
        private Camera GetMainCamera()
        {
            if (_mainCamera == null)
                _mainCamera = Camera.main;
            
            return _mainCamera;
        }
    }
}