using Spine;
using UnityEngine;

[RequireComponent(typeof(AnimateSymbolSpineOnEnable))]
public class SymbolsAnimationController : MonoBehaviour
{
    [SerializeField] private GameObject spineGameObject;
    private TrackEntry animationTrack;


    public void ManageSymbolAnimForPaylineState(PayLineState paylinestate, bool isSpecialSymbol)
    {
        animationTrack = spineGameObject.GetComponent<AnimateSymbolSpineOnEnable>().AnimationTrack;
        if (paylinestate == PayLineState.FirstIteration && animationTrack != null)
            animationTrack.TimeScale = isSpecialSymbol ? 1f : 0f;
    }
}
