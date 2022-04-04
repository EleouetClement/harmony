using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    public Button new_game;
    public Button load_game;
    public Button extras;
    public Button settings;
    public Button quit_game;
    public CanvasGroup backgroundScreen;

    private AsyncOperation _asyncOp;

    /// <summary>
    /// references to UI elements' game object
    /// </summary>
    private GameObject _new_game_button;
    private GameObject _load_game_button;
    private GameObject _extras_button;
    private GameObject _settings_button;
    private GameObject _quit_button;
    private GameObject _loading_text;

    /// <summary>
    /// is a scene being loaded?
    /// </summary>
    private bool _loading;

    private float _elapsedTime;
    private float _fadeTime;

	private void Awake()
	{
        _loading = false;
        _new_game_button = new_game.gameObject;
        _load_game_button = load_game.gameObject;
        _extras_button = extras.gameObject;
        _settings_button = settings.gameObject;
        _quit_button = quit_game.gameObject;
        _loading_text = backgroundScreen.GetComponentInChildren<Text>().gameObject;
        _loading_text.SetActive(false);
        _fadeTime = 1f;
    }
	// Start is called before the first frame update
	void Start()
    {
        new_game.onClick.AddListener(NewGame);
    }

	private void Update()
	{
        if (_loading)
        {
            if (_asyncOp.isDone)
            {
                _elapsedTime += Time.deltaTime;
                _elapsedTime = Mathf.Clamp(_elapsedTime, 0f, _fadeTime);
                backgroundScreen.alpha = 1f - (_elapsedTime / _fadeTime);
            }
            if (_elapsedTime == _fadeTime)
            {
                GameModeSingleton.GetInstance().GetPlayerReference.GetComponent<PlayerInput>().enabled = true;
                SceneManager.UnloadSceneAsync("MainMenu");
            }
        }
	}

	private void NewGame()
    {
        LoadingScreen();
        _asyncOp = SceneManager.LoadSceneAsync("CommonScene", LoadSceneMode.Additive);
        //_asyncOp.allowSceneActivation = false;
        _elapsedTime = 0f;
        _loading = true;
    }

    private void LoadingScreen()
    {
        _new_game_button.SetActive(false);
        _load_game_button.SetActive(false);
        _load_game_button.SetActive(false);
        _extras_button.SetActive(false);
        _settings_button.SetActive(false);
        _quit_button.SetActive(false);
        _loading_text.SetActive(true);
    }
}
