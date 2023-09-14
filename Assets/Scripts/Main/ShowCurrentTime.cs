using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Globalization;
using System;

public class ShowCurrentTime : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI currentTimeLabel;
    [SerializeField] TextMeshProUGUI nextRewardText;
    [SerializeField] TextMeshProUGUI nextExtraCoinsText;
    [SerializeField] int timeForReward;

    private void Start()
    {
        StartCoroutine(CheckRewardAvailability());
        Gamemanager.instance.ThisDay = TimerUtility.CurrentTime.Day;
    }

    void Update()
    {
        currentTimeLabel.text = TimerUtility.CurrentTime.ToString("F");
        TimeSpan timeRemaining = GetTimeUntilNext1300UTC();
        nextRewardText.text = $"Next reward: {timeRemaining.Hours}:{timeRemaining.Minutes}:{timeRemaining.Seconds}";
        nextExtraCoinsText.text = $"Next reward: {timeRemaining.Hours}:{timeRemaining.Minutes}:{timeRemaining.Seconds}";
    }

    IEnumerator CheckRewardAvailability()
    {
        Gamemanager.instance.GetReward = IsRewardTime();
        yield return new WaitForSeconds(60);
    }

    bool IsRewardTime()
    {
        DateTime currentTime = DateTime.UtcNow;
        DateTime rewardTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, timeForReward, 0, 0);
        if (currentTime >= rewardTime)
        {
            return true;
        }
        return false;
    }

    private TimeSpan GetTimeUntilNext1300UTC()
    {
        DateTime currentTime = DateTime.UtcNow;
        DateTime targetTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, timeForReward, 0, 0, DateTimeKind.Utc);

        if (currentTime >= targetTime)
        {
            targetTime = targetTime.AddDays(1);
        }

        return targetTime - currentTime;
    }

}
