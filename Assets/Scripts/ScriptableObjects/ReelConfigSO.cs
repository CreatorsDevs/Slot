using UnityEngine;

[CreateAssetMenu(fileName = "ReelConfig", menuName = "ScriptableObject/NewReelConfig")]

public class ReelConfigSO : ScriptableObject
{
    //Reels Parameters
    public int Rows;
    public int Columns;
    public float RotationSpeed;
    public bool AnticipateRotation;

    //Normal Reel Parameters
    public float ReelRotationOffset;
    public float SpinAnticipationDuration;
    public float SpinStopDelay;

    public float GetNormalShowReelDelay => SpinStopDelay + (ReelRotationOffset * Columns);
}
