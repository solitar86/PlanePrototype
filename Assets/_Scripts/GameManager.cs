using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    AudioSource _musicSource;

    float _maxTime;
    TextMeshProUGUI _timeText;
    public static bool _isGameOver = false;
    public static bool _HasGameStarted = false;

    private InputAction _restart;

    [SerializeField] private GameObject _quitCanvas;

    private void Awake()
    {
        _isGameOver = false;
        _HasGameStarted = false;
        _musicSource = GetComponentInChildren<AudioSource>();
        _timeText = GetComponentInChildren<TextMeshProUGUI>();
        _maxTime = 90f;

        _restart = InputSystem.actions.FindAction("Restart");
        _restart.Enable();

        _quitCanvas.SetActive(false);
    }

    IEnumerator Start()
    {
        yield return new WaitWhile(() => GameManager._HasGameStarted == false);
        _musicSource.PlayScheduled(AudioSettings.dspTime + 2f);
    }

    private void Update()
    {
        if(_HasGameStarted && _isGameOver == false)
        {
            _maxTime -= Time.deltaTime;

            if( _maxTime <= 0 )
            {
                // Game Over
                _isGameOver = true;
            }
        }

        UpdateUI();


        if (_restart.WasPerformedThisFrame())
        {
            if(_isGameOver )
            {
                Debug.Log("Restarting game");
                RestartGame();
            }
            else
            {
                _quitCanvas.SetActive(!_quitCanvas.activeInHierarchy);
                _quitCanvas.GetComponentInChildren<Button>().Select();
            }
        }

    }

    private void UpdateUI()
    {
        if( _isGameOver )
        {
            _timeText.text = "Game Over!\nFinal Score : " + FindFirstObjectByType<PlayerScoreManager>().CurrentScore.ToString();
            _timeText.text += "\n\nEsc / Start\nto Restart";
            _quitCanvas.SetActive(false);
        }
        else
        {
            _timeText.SetText(_maxTime.ToString("F1"));
        }
    }

    public static void RestartGame()
    {
        _isGameOver = false;
        _HasGameStarted = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void StarGame()
    {
        Debug.Log("Starting game");
        _HasGameStarted = true;
    }

    public static void SetGameOver()
    {
        _isGameOver = true;
    }
}
