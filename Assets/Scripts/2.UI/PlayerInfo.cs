using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MissionType
{
    »ç°Ç
}

[CreateAssetMenu(fileName = "Player Info", menuName = "Scriptable Object/Player Info", order = 0)]
public class PlayerInfo : ScriptableObject
{
    public int blessing;

    public ChapterProgress chapterProgress;
}

[Serializable]
public class ChapterProgress
{
    public string chapterName;
    public string missionName;
    public MissionType missionType;
}