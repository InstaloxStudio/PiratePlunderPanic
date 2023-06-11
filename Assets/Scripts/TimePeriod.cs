using UnityEngine;

[CreateAssetMenu(fileName = "TimePeriod", menuName = "Time of Day/Time Period")]
public class TimePeriod : ScriptableObject
{
    public TimePeriods timePeriod;
    public Color lightColor;
    public float lightTemperature;
    public float lightIntensity;
    public float fogDensity;
    public Color fogColor;
}


public enum TimePeriods
{
    Dawn,
    Sunrise,
    Morning,
    Noon,
    Afternoon,
    Dusk,
    Sunset,
    Night
}