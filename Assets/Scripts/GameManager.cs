using System;
using System.IO;
using UnityEditor;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

public class GameManager : MonoBehaviour
{
    //Параметры:
    [SerializeField] private int width;                                      // Ширина поля
    [SerializeField] private int height;                                     // Высота поля
    [SerializeField] private Vector3[] obstacles;                            // Координаты препятствий (относительно левого нижнего угла)
    [SerializeField] private Vector3[] apples;                               // Координаты яблок (относительно левого нижнего угла)
    
    private const int _upToWall = 10;                                        // Расстояние от стены до края поля за ней
    
    [SerializeField] private Transform walkableNodePrefab;
    [SerializeField] private Transform obstacleNodePrefab;
    [SerializeField] private Transform applePrefab;
    [SerializeField] private TMP_Text startText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject restartButtonPanel;
    
    public static event Action StartMove;

    private int score = 0;
    
    private int _countDown = 3;
    private int _widthHalf;
    private int _heightHalf;
    
/*    private void Awake()
    {
        _widthHalf = width / 2;
        _heightHalf = height / 2;
    }
*/
    private void OnEnable()
    {
        SnakeColliderHandler.GameOver += OnGameOver;
        SnakeColliderHandler.ApplePicked += OnApplePicked;
        
        LoadSave.Save += OnSave;
        LoadSave.Load += OnLoad;
    }

    private void OnDisable()
    {
        SnakeColliderHandler.GameOver -= OnGameOver;
        SnakeColliderHandler.ApplePicked -= OnApplePicked;
        
        LoadSave.Save -= OnSave;
        LoadSave.Load -= OnLoad;
    }

    private void Start()
    {
        LoadSave.LoadConfig();
        
        _widthHalf = width / 2;
        _heightHalf = height / 2;

        CreateField();
        CreateWalls();
        CreateObstacles();
        CreateApples();
        
        scoreText.text = String.Format("Score: {0}", score);
        startText.text = _countDown.ToString();
        InvokeRepeating("CountDown", 1f,1f);
    }


    private void CountDown()
    {
        _countDown--;
        
        if (_countDown == 0)
        {
            startText.text = string.Empty;
            StartMove?.Invoke();
            CancelInvoke("CountDown");
        }
        else
        {
            startText.text = _countDown.ToString();
        }
    }
    

    private void CreateField()
    {
        for (int w = -_widthHalf -_upToWall; w < _widthHalf + _upToWall; w++)
        {
            for (int h = -_heightHalf - _upToWall; h < _heightHalf + _upToWall; h++)
            {
                Instantiate(walkableNodePrefab, new Vector3(w, h, 0f), transform.rotation.normalized);
            }
        }
    }

    private void CreateWalls()
    {
        var t = transform.rotation.normalized;
        for (int w = -_widthHalf; w <= _widthHalf; w++)
        {
            Instantiate(obstacleNodePrefab, new Vector3(w,  -_heightHalf, 0f), t);
            Instantiate(obstacleNodePrefab, new Vector3(w, _heightHalf, 0f), t);
        }
        for (int h = -_heightHalf; h <= _heightHalf; h++)
        {
            Instantiate(obstacleNodePrefab, new Vector3(-_widthHalf, h, 0f), t);
            Instantiate(obstacleNodePrefab, new Vector3(_widthHalf, h, 0f), t);
        }
    }

    private void CreateObstacles()
    {
        Vector3 leftLowerCorner = new Vector3(-_widthHalf, -_heightHalf, 0f);
        foreach (var v3 in obstacles)
        {
            Instantiate(obstacleNodePrefab, v3 + leftLowerCorner, transform.rotation.normalized);
        }
    }
    
    private void CreateApples()
    {
        Vector3 leftLowerCorner = new Vector3(-_widthHalf, -_heightHalf, 0f);
        foreach (var v3 in apples)
        {
            Instantiate(applePrefab, v3 + leftLowerCorner, transform.rotation.normalized);
        }
    }

    private void OnGameOver()
    {
        Time.timeScale = 0;
        startText.text = "Game Over!";
        restartButtonPanel.SetActive(true);
    }

    private void OnApplePicked()
    {
        score++;
        scoreText.text = String.Format("Score: {0}", score);
    }
    
    public void RestartPressed()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    private void OnSave()
    {
        LoadSave.SaveSnapshot.height = height;
        LoadSave.SaveSnapshot.width = width;
        LoadSave.SaveSnapshot.obstacles = obstacles;
        LoadSave.SaveSnapshot.apples = apples;
    }
    
    private void OnLoad()
    {
        height = LoadSave.SaveSnapshot.height;
        width = LoadSave.SaveSnapshot.width;
        obstacles = LoadSave.SaveSnapshot.obstacles;
        apples = LoadSave.SaveSnapshot.apples;
    }
}
