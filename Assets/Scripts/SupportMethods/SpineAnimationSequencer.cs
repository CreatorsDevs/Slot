using UnityEngine;
using Spine.Unity;

public class SpineAnimationSequencer : MonoBehaviour
{
    [SpineAnimation]
    [SerializeField] private string startAnimationState;
    [SpineAnimation]
    [SerializeField] private string midAnimationState;
    [SpineAnimation]
    [SerializeField] private string exitAnimationState;

    [Space(8)]
    [SerializeField] private bool resetOnEnable;
    [SerializeField] private bool loopMidAnimationState;

    [Space(8)]
    [SerializeField] public bool playExitAnimation;
    [Tooltip("Time after with exit animation show be played (if mid is not looping)")]
    [SerializeField] public float playExitAnimationAfter;

    private SkeletonAnimation skeletonAnimation;
    private Spine.AnimationState animationState;

    private void Awake()
    {
        skeletonAnimation = gameObject.GetComponent<SkeletonAnimation>();
        animationState = skeletonAnimation.AnimationState;
    }

    void OnEnable()
    {
        if (resetOnEnable)
            ResetAnimation();

        AnimationStartSequence();
    }

    private void AnimationStartSequence()
    {
        animationState.SetAnimation(0, startAnimationState, false);

        Spine.Animation startClip = skeletonAnimation.Skeleton.Data.FindAnimation(startAnimationState);
        float addDelay = startClip.Duration;
        animationState.AddAnimation(0, midAnimationState, loopMidAnimationState, addDelay);

        if (playExitAnimation)
            animationState.AddAnimation(0, exitAnimationState, false, playExitAnimationAfter);
    }

    private void ResetAnimation()
    {
        animationState.SetEmptyAnimation(0, 0);
        animationState.ClearTracks();
    }

    public void SetExitAnimation(bool playexitanimation) => playExitAnimation = playexitanimation;
}