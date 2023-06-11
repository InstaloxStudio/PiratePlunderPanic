using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;
using Sirenix.OdinInspector;

public class TimeOfDaySystem : MonoBehaviour
{
    [FoldoutGroup("Events")]
    public UnityEvent Dawn, Sunrise, Morning, Noon, Afternoon, Dusk, Sunset, Night;
    public TimePeriods currentTimePeriod;
    public float dayCycleInMinutes = 1;
    public const float Second = 1;
    public const float Minute = 60 * Second;
    public const float Hour = 60 * Minute;
    public const float Day = 24 * Hour;
    public List<TimePeriod> timePeriods;
    public Light _sunLight;
    public HDAdditionalLightData lightData;
    public Volume volume;
    public Fog fog;
    public float _degreeRotation;
    public float _timeOfDay;
    public int _timePeriodIndex;

    private void Start()
    {
        //switch to the time period in the list that matches the currentTimePeriod and set the index to that
        for (int i = 0; i < timePeriods.Count; i++)
        {
            if (timePeriods[i].timePeriod == currentTimePeriod)
            {
                _timePeriodIndex = i;
                break;
            }
        }


        _degreeRotation = 360 / Day;
        volume.profile.TryGet<Fog>(out fog);
        var _currentTimePeriod = timePeriods[_timePeriodIndex];
        var _nexttimePeriod = timePeriods[GetNextTimePeriodIndex()];
        UpdateLightingAndFog(_currentTimePeriod, _nexttimePeriod, _timeOfDay);

    }

    public int GetTimePeriodIndex(TimePeriods timePeriod)
    {
        for (int i = 0; i < timePeriods.Count; i++)
        {
            if (timePeriods[i].timePeriod == timePeriod)
            {
                return i;
            }
        }
        return -1;
    }

    public int GetNextTimePeriodIndex()
    {
        return (_timePeriodIndex + 1) % timePeriods.Count;
    }

    public int GetPreviousTimePeriodIndex()
    {
        return (_timePeriodIndex - 1) % timePeriods.Count;
    }

    public void SetTimeBasedOnTimePeriod(TimePeriod timePeriod)
    {

        _timePeriodIndex = GetTimePeriodIndex(timePeriod.timePeriod);
        _timeOfDay = (float)_timePeriodIndex / timePeriods.Count;
        var _currentTimePeriod = timePeriods[_timePeriodIndex];
        var _nexttimePeriod = timePeriods[GetNextTimePeriodIndex()];
        UpdateLightingAndFog(_currentTimePeriod, _nexttimePeriod, _timeOfDay);
    }

    private void UpdateLightingAndFog(TimePeriod current, TimePeriod next, float t)
    {
        lightData.color = Color.Lerp(current.lightColor, next.lightColor, t);
        lightData.intensity = Mathf.Lerp(current.lightIntensity, next.lightIntensity, t);
        fog.color.value = Color.Lerp(current.fogColor, next.fogColor, t);
        fog.baseHeight.value = Mathf.Lerp(current.fogDensity, next.fogDensity, t);
    }

    private void Update()
    {
        float timeSinceStart = Time.timeSinceLevelLoad;
        _timeOfDay = (timeSinceStart % Day) / Day;
        float rotationAmount = _degreeRotation * Time.deltaTime;
        _sunLight.transform.Rotate(Vector3.right, rotationAmount, Space.World);

        // determine the current TimePeriod based on _timeOfDay and lerp the light and fog settings
        int index = Mathf.FloorToInt(_timeOfDay * timePeriods.Count);
        TimePeriod currentPeriod = timePeriods[index];
        TimePeriod nextPeriod = timePeriods[(index + 1) % timePeriods.Count];

        float t = (_timeOfDay * timePeriods.Count) % 1;

        UpdateLightingAndFog(currentPeriod, nextPeriod, t);
        // Fire off events based on the current TimePeriod

    }
}
