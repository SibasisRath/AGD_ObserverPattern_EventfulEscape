using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AchievementSciprtableObject", menuName = "ScriptableObjects/AchievementSciprtableObject", order = 2)]
public class AchievementSO : ScriptableObject
{
    public AchievementTypes achievementType;
    public string achievement;
    public int DisplayDuration;
    public int WaitToTriggerDuration = 0;
}
