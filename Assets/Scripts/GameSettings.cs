using UnityEngine;
[CreateAssetMenu(fileName = "GameSettings", menuName = "Game Settings")]
public class GameSettings : ScriptableObject
{
    [System.Serializable]
    public class DifficultySettings
    {
        public float playerStartingHealth;
        public float playerStartHunger;
        public float playerStartThirst;
        public float playerStartStamina;
        public float playerStartStaminaRegen;
        public float playerStartSpeed;
        public float playerStartSleep;
        public float playerStartSleepRegen;
        public float playerStartStaminaDrain;
        public float playerStartHungerDrain;
        public float playerStartThirstDrain;
        public float playerStartSleepDrain;
        public float playerStartHealthDrain;
        public float playerStartHealthRegen;

        // Add more settings as needed
    }

    public DifficultySettings EasyDifficulty;
    public DifficultySettings NormalDifficulty;
    public DifficultySettings HardDifficulty;

    // You might also want a method to get the settings based on a provided difficulty level
    public DifficultySettings GetSettingsForDifficulty(Difficulty _difficulty)
    {
        //return based on difficulty
        switch (_difficulty)
        {
            case Difficulty.Easy:
                return EasyDifficulty;
            case Difficulty.Normal:
                return NormalDifficulty;
            case Difficulty.Hard:
                return HardDifficulty;
            default:
                return NormalDifficulty;
        }
    }

    //enum for difficulty
    public enum Difficulty
    {
        Easy,
        Normal,
        Hard
    }
}