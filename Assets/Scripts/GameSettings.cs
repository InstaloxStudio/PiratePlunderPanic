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

        //resources variable integers
        public int StartWood;
        public int StartStone;
        public int StartFood;
        public int StartWater;
        public int StartRum;
        public int StartGold;
        public int StartCannonBalls;
        
        //world settings
        public float WorldTime;
        public float WorldTimeSpeed;
        public int Day = 1;

        public bool isDay = true;
        public bool isRaining = false;
        public bool isStorming = false;
        public bool isFoggy = false;
        public bool isSnowing = false;
        public bool isSunny = true;
        
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
