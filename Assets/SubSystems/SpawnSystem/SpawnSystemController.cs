using System.Collections.Generic;
using SubSystems.SceneObjects;
using UnityEngine;

namespace SubSystems.SpawnSystem
{
    [RequireComponent(typeof(SpawnMouseController))]
    public class SpawnSystemController : MonoBehaviour
    {
        [Header("Spawnable Objects")]
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
        
        public List<List<int>> GetCurrentLevel() => _currentLevel;
        
        public void SelectObject (SpawnableObject selectedObject)
        {
            _selectedObject = selectedObject;
        }

        public void SetLevel(List<List<int>> level)
        {
            InitializeSpawnSystem(rows, columns);
            
            // for (int i = 0; i < level.Count; i++)
            // {
            //     for (int j = 0; j < level[i].Count; j++)
            //     {
            //         if (level[i][j] == 0)
            //             continue;
            //         
            //         if (_spawnableObjects.Length <= level[i][j])
            //             continue;
            //         
            //         var objectToSpawn = _spawnableObjects[level[i][j]];
            //         if (objectToSpawn == null)
            //             continue;
            //         
            //         SpawnObjectOnCoordinate(j, i, objectToSpawn);
            //     }
            // }
        }
        
        public void SpawnSelectedObjectOnCoordinate(int column, int row)
        {
            SpawnObjectOnCoordinate(column, row, _selectedObject);
        }

        private void SpawnObjectOnCoordinate(int column, int row, SpawnableObject objectToSpawn)
        {
            if (_selectedObject == null)
                return;
            
            DestroyObjectOnCoordinate(column, row);
            
            var newObject = Instantiate(objectToSpawn, transform);
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
            // TODO: ESTO CREA NUEVOS OBJETOS DE ALGUNA FORMA, NO Sé TOCA REVISAR TENGO SUEñO
            DestroyAllSpawnedObjects();

            for (int i = 0; i < rows; i++)
            {
                _currentLevel.Add(new List<int>(columns));
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
                    SpawnableObject spawnedObject = Instantiate(_spawnableEmptyObject,transform);
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
            SpawnableObject wall = Instantiate(_spawnableWallObject, transform);
            wall.transform.localPosition = localPosition;
            wall.columnIndex = -1;
            wall.rowIndex = -1;
        }
        
        private void DestroyAllSpawnedObjects()
        {
            for (int i = _spawnedObjects.Count - 1; i >= 0; i--)
            {
                if (_spawnedObjects[i] == null)
                    continue;
                
                Destroy(_spawnedObjects[i].gameObject);
            }

            for (int i =_emptyObjects.Count - 1; i >= 0; i--)
            {
                if (_emptyObjects[i] == null)
                    continue;
                
                Destroy(_emptyObjects[i].gameObject);
            }
            
            _spawnedObjects.Clear();
            _emptyObjects.Clear();
            _currentLevel = new List<List<int>>();
        }
    }
}