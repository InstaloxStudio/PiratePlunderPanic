using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState State { get; private set; }

    [RuntimeInitializeOnLoadMethod]
    private static void InitializeOnLoad()
    {
        if (Instance == null)
        {
            // Create a new GameManager object if one doesn't exist
            GameObject managerObject = new GameObject("GameManager");
            Instance = managerObject.AddComponent<GameManager>();

            // Set initial game state
            Instance.State = GameState.MainMenu;
        }
    }

    public void StartGame()
    {
        // Initialize game state, load resources, etc.
    }

    //set game state
    public void SetGameState(GameState _state)
    {
        State = _state;

        switch (State)
        {
            case GameState.MainMenu:

                break;

            case GameState.Playing:

                break;

            case GameState.Paused:

                break;

            case GameState.GameOver:

                break;
        }
    }
}
