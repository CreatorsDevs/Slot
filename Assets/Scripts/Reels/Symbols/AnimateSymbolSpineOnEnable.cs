using UnityEngine;
using Spine.Unity;
using Spine;

public class AnimateSymbolSpineOnEnable : MonoBehaviour
{
    [SerializeField] private GameObject spineGameObject;

    [Space(8)]
    [SpineAnimation][SerializeField] string startAnimation;
    [SerializeField] bool isLooping = false;
    private SkeletonAnimation skeletonAn;
    private Spine.AnimationState animationState;
    private TrackEntry animationTrack;
    public TrackEntry AnimationTrack { get { return animationTrack; } }

    void Awake()
    {
        skeletonAn = spineGameObject.GetComponent<SkeletonAnimation>();
        animationState = skeletonAn.AnimationState;
    }

    void OnEnable()
    {
        animationState.ClearTracks();
        animationTrack = animationState.SetAnimation(0, startAnimation, isLooping);
    }
}
