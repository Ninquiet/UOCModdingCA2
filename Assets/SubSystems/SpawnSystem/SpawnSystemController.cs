using System.Collections.Generic;
using SubSystems.SceneObjects;
using UnityEngine;

namespace SubSystems.SpawnSystem
{
    [RequireComponent(typeof(SpawnMouseController))]
    public class SpawnSystemController : MonoBehaviour
    {
        [Header("Spawnable Objects")]
        [SerializeField]
        private SpawnableObject[] _spawnableObjects;
        
        [Header("References")]
        [SerializeField]
        private SpawnableObject _spawnableEmptyObject;
        [SerializeField]
        private SpawnableObject _spawnableWallObject;
        [SerializeField] 
        private int rows;
        [SerializeField]
        private int columns;

        private List<List<int>> _currentLevel;
        private List<SpawnableObject> _spawnedObjects = new List<SpawnableObject>();
        private List<SpawnableObject> _emptyObjects = new List<SpawnableObject>();
        private SpawnableObject _selectedObject;
        private Transform _levelRoot;
        
        public List<List<int>> GetCurrentLevel() => _currentLevel;
        
        public void SelectObject (SpawnableObject selectedObject)
        {
            _selectedObject = selectedObject;
        }

        public void Initialize()
        {
            InitializeSpawnSystem(rows, columns);
        }

        public void SetLevel(List<List<int>> level)
        {
            InitializeSpawnSystem(rows, columns);
            
            for (int i = 0; i < level.Count; i++)
            {
                for (int j = 0; j < level[i].Count; j++)
                {
                    if (level[i][j] == 0)
                        continue;
                    
                    if (_spawnableObjects.Length <= level[i][j])
                        continue;
                    
                    var objectToSpawn = _spawnableObjects[level[i][j]];
                    if (objectToSpawn == null)
                        continue;
                    
                    SpawnObjectOnCoordinate(j, i, objectToSpawn);
                }
            }
        }
        
        public void SpawnSelectedObjectOnCoordinate(int column, int row)
        {
            if (_selectedObject == null)
                return;
            
            SpawnObjectOnCoordinate(column, row, _selectedObject);
        }

        private void SpawnObjectOnCoordinate(int column, int row, SpawnableObject objectToSpawn)
        {
            DestroyObjectOnCoordinate(column, row);
            
            var newObject = Instantiate(objectToSpawn, GetLocalRoot());
            newObject.transform.localPosition =
                new Vector3(column * GameBaseValues.GRID_SIZE, row * GameBaseValues.GRID_SIZE, 0);
            
            newObject.columnIndex = column;
            newObject.rowIndex = row;
            _spawnedObjects.Add(newObject);
            _currentLevel[row][column] = newObject.spawnableObjectIndex;
        }

        public void DestroyObjectOnCoordinate(int column, int row)
        {
            foreach (var spawnedObject in _spawnedObjects)
            {
                if (spawnedObject.rowIndex == row && spawnedObject.columnIndex == column)
                {
                    Destroy(spawnedObject.gameObject);
                    _spawnedObjects.Remove(spawnedObject);
                    return;
                }
            }
            
            _currentLevel[row][column] = 0;
        }

        private void Start()
        {
            InitializeSpawnSystem(rows, columns);
        }

        private void InitializeSpawnSystem(int rows, int columns)
        {
            DestroyAllSpawnedObjects();

            for (int i = 0; i < columns; i++)
            {
                _currentLevel.Add(new List<int>(rows));
            }
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    _currentLevel[i].Add(0);
                }
            }
            
            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < columns; y++)
                {
                    SpawnableObject spawnedObject = Instantiate(_spawnableEmptyObject,GetLocalRoot());
                    spawnedObject.transform.localPosition =
                        new Vector3(x * GameBaseValues.GRID_SIZE, y * GameBaseValues.GRID_SIZE, 0);
                    
                    spawnedObject.columnIndex = x;
                    spawnedObject.rowIndex = y;
                    _emptyObjects.Add(spawnedObject);
                }
            }
            
            SpawnWallsAroundGrid();
        }
        
        private void SpawnWallsAroundGrid()
        {
            float gridSize = GameBaseValues.GRID_SIZE;

            for (int i = 0; i < rows; i++)
            {
                SpawnWall(new Vector3(i * gridSize, -gridSize, 0));
                SpawnWall(new Vector3(i * gridSize, columns * gridSize, 0));
            }

            for (int i = 0; i < columns; i++)
            {
                SpawnWall(new Vector3(-gridSize, i * gridSize, 0));
                SpawnWall(new Vector3(rows * gridSize, i * gridSize, 0));
            }
        }

        private void SpawnWall(Vector3 localPosition)
        {
            SpawnableObject wall = Instantiate(_spawnableWallObject, GetLocalRoot());
            wall.transform.localPosition = localPosition;
            wall.columnIndex = -1;
            wall.rowIndex = -1;
        }
        
        private void DestroyAllSpawnedObjects()
        {
            ResetRoot();
            
            _spawnedObjects.Clear();
            _emptyObjects.Clear();
            _currentLevel = new List<List<int>>();
        }

        private void ResetRoot()
        {
            GameObject oldRootGO = null;
            if (_levelRoot != null)
            {
                _levelRoot.name = "oldRoot";
                oldRootGO = _levelRoot.gameObject;
                _levelRoot = null;
            }

            GetLocalRoot();
            
            if (oldRootGO != null)
                Destroy(oldRootGO);
        }

        private Transform GetLocalRoot()
        {
            if (_levelRoot == null)
            {
                _levelRoot = new GameObject().transform;
                _levelRoot.name = "LevelRoot";
                _levelRoot.parent = this.transform;
                _levelRoot.localPosition = Vector3.zero;
            }

            return _levelRoot;
        }
    }
}