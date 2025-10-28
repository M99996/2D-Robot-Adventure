using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LeaderboardEntry
{
    public string name;
    public float time;
}

[Serializable]
class LeaderboardData
{
    public List<LeaderboardEntry> entries = new List<LeaderboardEntry>();
}

public static class LeaderboardManager
{
    private const string Key = "leaderboard";

    static LeaderboardData Load()
    {
        string json = PlayerPrefs.GetString(Key, "");
        if (string.IsNullOrEmpty(json)) return new LeaderboardData();
        return JsonUtility.FromJson<LeaderboardData>(json) ?? new LeaderboardData();
    }

    static void Save(LeaderboardData data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(Key, json);
        PlayerPrefs.Save();
    }

    public static void AddEntry(string name, float timeSeconds, int maxCount = 10)
    {
        var data = Load();
        data.entries.Add(new LeaderboardEntry { name = name, time = timeSeconds });

        data.entries.Sort((a, b) => a.time.CompareTo(b.time));

        if (data.entries.Count > maxCount)
            data.entries = data.entries.GetRange(0, maxCount);

        Save(data);
    }

    public static List<LeaderboardEntry> GetTop(int maxCount = 10)
    {
        var data = Load();
        data.entries.Sort((a, b) => a.time.CompareTo(b.time));
        if (data.entries.Count > maxCount)
            return data.entries.GetRange(0, maxCount);
        return data.entries;
    }

    public static void ClearAll()
    {
        PlayerPrefs.DeleteKey(Key);
        PlayerPrefs.Save();
    }

    public static string FormatTime(float t)
    {
        int minutes = Mathf.FloorToInt(t / 60f);
        float seconds = t - minutes * 60f;
        return $"{minutes:00}:{seconds:00.000}";
    }
}