
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [SerializeField] private Vector3 _startPosition;
    [SerializeField] private GameObject _prefabGenerateWall;
    [SerializeField] private GameObject _player;
    [SerializeField] private Enemy _prefabEnemy;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _winPanel;


    private List<Enemy> _enemys = new List<Enemy>();
    private List<GameObject> _generateWalls = new List<GameObject>();
    private List<Vector3> _pathCoordinates = new List<Vector3>();
    private MazeConstructor _mazeConstructor = new MazeConstructor();
    private NavMeshSurface _navMesh = new NavMeshSurface();
    private int[,] _maze;   
    private Vector3[,] _cellCoordinates;  
    private int _width = 10;
    private int _height = 10;

    private void Awake()
    {
        _cellCoordinates = new Vector3[_width, _height];
        _navMesh = GetComponent<NavMeshSurface>();
    }

    private void Start()
    {
        StartGame();               
    }   

    private void Update()
    {
        CheckWin();
    }
    private void StartGame()
    {
        Time.timeScale = 1;
        CreateMaze();
        CreateEnemy();       
        _player.transform.position = _startPosition;
    }  

    private void CreateMaze()
    {
        _maze = _mazeConstructor.GenerateNewMaze(_width, _height);
        _cellCoordinates = GenerateCellCoordinates(_width, _height);

        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                if (_maze[i, j] == 1)
                {
                    _generateWalls.Add(Instantiate(_prefabGenerateWall, _cellCoordinates[i, j], Quaternion.identity));
                }
                else
                {
                    _pathCoordinates.Add(_cellCoordinates[i, j]);
                }
            }
        }

        _navMesh.BuildNavMesh();
    }

    private void CreateEnemy()
    {
        int enemysCount = 0;
        while (true)
        {
            int posX = Random.Range(0, _cellCoordinates.GetUpperBound(0));
            int posZ = Random.Range(5, _cellCoordinates.GetUpperBound(1));

            if (_maze[posX, posZ] == 0)
            {
                _enemys.Add(Instantiate(_prefabEnemy, _cellCoordinates[posX, posZ], Quaternion.identity));
                enemysCount++;
                if (enemysCount == 2)
                {
                    break;
                }
            }
        }

            foreach (var enemy in _enemys)
            {
                enemy._enemyPath = _pathCoordinates;
                enemy.SetPath();
            }
        
    }

    private Vector3[,] GenerateCellCoordinates(int width, int height)
    {

        float posX = _startPosition.x;
        float posZ = _startPosition.z;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                _cellCoordinates[i, j] = new Vector3(posX + (j * 2), 2, posZ + (i * 2));
            }
        }
        return _cellCoordinates;
    }   

    public void Alarm()
    {
        Camera.main.backgroundColor = Color.red;
        foreach (var enemy in _enemys)
        {
            enemy.Alarm();
        }
    }

    private void CheckWin()
    {
        if (Vector3.Distance(_player.transform.position, _cellCoordinates[9,9]) < 2)
        {
            Win();
        }
    }

    public void GameOver()
    {
        _gameOverPanel.SetActive(true);
        Time.timeScale = 0.01f;
    }

    private void Win()
    {
        _winPanel.SetActive(true);
        Time.timeScale = 0.01f;
    }

    public void MenuButton()
    {
        SceneManager.LoadScene(0);
    }
}
