﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player Prefers로 저장되는 데이터 , 글로벌 유저 데이터 저장
/// </summary>
public class UserData
{
    public static string token;

    private static string _username;
    public static string Username
    {
        get
        {
            return _username;
        }
        set
        {
            _username = value;
        }
    }

    public static UpArrowNotation CoreIntellect
    {
        get
        {
            double elapsedTime = (double)(DateTimeOffset.Now.ToUnixTimeMilliseconds() - LastCalcTime) / 1000f;
            UpArrowNotation intellect = Equation.GetCurrentIntellect(SingleNetworkWrapper.GetBrainDataForID(0).intellectEquation, elapsedTime);
            return intellect;
        }
    }

    private static UpArrowNotation _tp = new UpArrowNotation();
    public static UpArrowNotation TP
    {
        get
        {
            return _tp;
        }
        set
        {
            _tp = value;
            Managers.Notification.PostNotification(ENotiMessage.UPDATE_TP);
        }
    }

    private static UpArrowNotation _np = new UpArrowNotation();
    public static UpArrowNotation NP
    {
        get
        {
            return _np;
        }
        set
        {
            _np = value;
            Managers.Notification.PostNotification(ENotiMessage.UPDATE_NP);
        }
    }

    private static long _experimentStartTime = 0;
    public static long ExperimentStartTime
    {
        get
        {
            return _experimentStartTime;
        }
        set
        {
            _experimentStartTime = value;
        }
    }

    private static List<int> _resetCounts = new List<int>();
    public static List<int> ResetCounts
    {
        get
        {
            return _resetCounts;
        }
        set
        {
            _resetCounts = value;
        }
    }

    private static Dictionary<long, long> _tpUpgradeCounts = new Dictionary<long, long>();
    public static Dictionary<long, long> TPUpgradeCounts
    {
        get
        {
            return _tpUpgradeCounts;
        }
    }

    private static SingleNetworkWrapper _singleNetworkWrapper = new SingleNetworkWrapper();
    public static SingleNetworkWrapper SingleNetworkWrapper
    {
        get
        {
            return _singleNetworkWrapper;
        }
        set
        {
            _singleNetworkWrapper = value;
        }
    }


    public static long TotalBrainGenCount
    {
        get
        {
            return _singleNetworkWrapper.totalBrainGenCount;
        }
        set
        {
            _singleNetworkWrapper.totalBrainGenCount = value;
        }
    }

    public static int ExperimentLevel
    {
        get
        {
            return _singleNetworkWrapper.experimentLevel;
        }
    }

    public static long LastCalcTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();

    private static Dictionary<long, QuestAttributes> _dicQuest = new Dictionary<long, QuestAttributes>();
    public static Dictionary<long, QuestAttributes> DicQuest { get { return _dicQuest; } }

    public static void UpdateTutorialQuest(CompleteQuestResponse res)
    {
        if (res != null)
        {
            _dicQuest.Clear();
            foreach (var quest in res.questAttributes)
            {
                _dicQuest.Add(quest.questId, quest);
            }
        }
    }

    public static void UpdateTPUpgradeCounts(List<UpgradeCondition> upgradeConditions)
    {
        _tpUpgradeCounts.Clear();

        foreach (UpgradeCondition cond in upgradeConditions)
        {
            _tpUpgradeCounts.Add(cond.id, cond.upgrade);
        }
        for (int i = 1; i <= 8; i++)
        {
            if (!_tpUpgradeCounts.ContainsKey(i))
            {
                _tpUpgradeCounts.Add(i, 0);
            }
        }
        if (_tpUpgradeCounts.ContainsKey(0))
        {
            _tpUpgradeCounts.Remove(0);
        }
    }


    public static void SetString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);

        switch (key)
        {
            case "Token":
                token = value;
                break;
        }
    }

    public static void LoadAllData()
    {
        token = PlayerPrefs.GetString("Token");
        Debug.Log(token);
    }
}
