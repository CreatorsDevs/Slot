using UnityEngine;

[CreateAssetMenu(fileName = "SystemConfig", menuName = "ScriptableObject/NewSystemConfig")]

public class SystemConfigSO : ScriptableObject
{
    public float ShowPaylineDuration;
    public float ShowFlashPaylineDuration;
    public bool ShowPaylinesInLoop;
    public int ScatterId;
    public bool AnticipateScatter;
    public float AnticipationDuration;
    public int BonusId;
}
