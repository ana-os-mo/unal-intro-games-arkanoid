using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class ArkanoidController : MonoBehaviour
{
    private const string BALL_PREFAB_PATH = "Prefabs/Ball";
    private readonly Vector2 BALL_INIT_POSITION = new Vector2(0, -0.86f);
    private const int PU_BALL_LIMIT = 3;

    [SerializeField]
    private GridController _gridController;

    [Space(20)]
    [SerializeField]
    private List<LevelData> _levels = new List<LevelData>();

    private Ball _ballPrefab = null;
    private List<Ball> _balls = new List<Ball>();
    
    [SerializeField]
    private Paddle _scaledPaddle;

    private int _currentLevel = 0;
    private int _totalScore = 0;

    private void Start()
    {
        ArkanoidEvent.OnBallReachDeadZoneEvent += OnBallReachDeadZone;
        ArkanoidEvent.OnBlockDestroyedEvent += OnBlockDestroyed;
        ArkanoidEvent.OnPowerUpPaddleContactEvent += OnPowerUpPaddleContact;
    }

    private void OnDestroy()
    {
        ArkanoidEvent.OnBallReachDeadZoneEvent -= OnBallReachDeadZone;
        ArkanoidEvent.OnBlockDestroyedEvent -= OnBlockDestroyed;
        ArkanoidEvent.OnPowerUpPaddleContactEvent -= OnPowerUpPaddleContact;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            InitGame();
        }
    }
    
    private void InitGame()
    {
        _currentLevel = 0;
        _totalScore = 0;
        _gridController.BuildGrid(_levels[0]);
        _scaledPaddle.ResetPaddle();
        SetInitialBall();
    }

    private void SetInitialBall()
    {
        ClearBalls();
        Ball ball = CreateBallAt(BALL_INIT_POSITION);
        ball.Init();
        _balls.Add(ball);
    }

    private Ball CreateBallAt(Vector2 position)
    {
        if (_ballPrefab == null)
        {
            _ballPrefab = Resources.Load<Ball>(BALL_PREFAB_PATH);
        }
        return Instantiate(_ballPrefab, position, Quaternion.identity);
    }

    private void ClearBalls()
    {
        for (int i = _balls.Count - 1; i >= 0; i--)
        {
            _balls[i].gameObject.SetActive(false);
            Destroy(_balls[i]);
        }
    
        _balls.Clear();
    }

    private void OnBallReachDeadZone(Ball ball)
    {
        ball.Hide();
        _balls.Remove(ball);
        Destroy(ball.gameObject);

        CheckGameOver();
    }
    
    private void CheckGameOver()
    {
        if (_balls.Count == 0)
        {
            ClearBalls();
            
            Debug.Log("Game Over: LOSE!!!");
        }
    }

    private void OnBlockDestroyed(int blockId)
    {
        BlockTile blockDestroyed = _gridController.GetBlockBy(blockId);
        if (blockDestroyed != null)
        {
            _totalScore += blockDestroyed.Score;
        }
        
        if (_gridController.GetBlocksActive() == 0)
        {
            _currentLevel++;
            if (_currentLevel >= _levels.Count)
            {
                ClearBalls();
                Debug.LogError("Game Over: WIN!!!!");
            }
            else
            {
                SetInitialBall();
                _gridController.BuildGrid(_levels[_currentLevel]);
            }
        }
    }

    private void OnPowerUpPaddleContact (PowerUps powerUp)
    {
        PowerUpKind kind = powerUp.Kind;

        if (kind == PowerUpKind.LargePaddle || kind == PowerUpKind.SmallPaddle)
        {
            // Debug.Log("Entra a la opcion de escalado de paddle");
            _scaledPaddle.ScalePaddle(kind);
        }
        else if (kind == PowerUpKind.MultiBall)
        {
            while(_balls.Count < PU_BALL_LIMIT)
            {
                SetExtraBall();
            }
        }
        else if (kind == PowerUpKind.FastBall)
        {

        }
        else
        {
            // Other PowerUps
        }
    }

    private void SetExtraBall()
    {
        Ball ballPU = CreateBallAt(BALL_INIT_POSITION);
        ballPU.Init();
        _balls.Add(ballPU);
    }
}