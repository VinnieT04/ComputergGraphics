using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Build.Content;
using UnityEngine;

public class GameManager: MonoBehaviour
{
    public static GameManager instance;
    public enum GameState
    {
        MENU,
        PLAYING,
        PAUSE,
        GAMEOVER,
    }
    public GameState currentState;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void SetState(GameState newState)
    {
        currentState = newState;
        
        switch (newState)
        {

            case GameState.MENU:
                break;
            case GameState.PLAYING:
                break;
            case GameState.PAUSE:
                break;
            case GameState.GAMEOVER:
                break;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetState(GameState.MENU);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
