using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class GameController : MonoBehaviour
{
    public GameSettings settings;

    private void Start()
    {
        // Get the game's difficulty from player prefs, defaulting to "normal" if no setting is found
        GameSettings.Difficulty difficulty = (GameSettings.Difficulty)PlayerPrefs.GetInt("Difficulty", (int)GameSettings.Difficulty.Normal);

        // Get the settings for the chosen difficulty
        var difficultySettings = settings.GetSettingsForDifficulty(difficulty);

        if (difficultySettings != null)
        {
            // Now you can use difficultySettings to configure your game based on the selected difficulty
            // For example, you might set the player's starting health, hunger, thirst, and other attributes
        }
        else
        {
            Debug.LogError("Invalid difficulty level: " + difficulty);
        }
    }
}
