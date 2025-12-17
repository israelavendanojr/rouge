using UnityEngine;

public class BossTransitionState : State
{
    private BossEnemy boss;

    private float minPhaseTime;
    private float maxPhaseTime;
    private float currentPhaseDuration;

    private bool hasEnteredSecondPhase;
    private BossPhase previousPhase = (BossPhase)(-1);

    public enum BossPhase
    {
        Follow,
        Dash,
        Shoot
    }

    public BossTransitionState(BossEnemy boss, float minTime, float maxTime) : base(boss)
    {
        this.boss = boss;
        minPhaseTime = minTime;
        maxPhaseTime = maxTime;
    }

    public override void Enter()
    {
        HandlePhaseTwo();

        currentPhaseDuration = Random.Range(minPhaseTime, maxPhaseTime);
        BossPhase nextPhase = RollPhase();
        previousPhase = nextPhase;

        Debug.Log($"Boss rolled {nextPhase} for {currentPhaseDuration:F1}s");

        switch (nextPhase)
        {
            case BossPhase.Follow:
                boss.GetFollowState().SetPhaseDuration(currentPhaseDuration);
                boss.SetState(boss.GetFollowState());
                break;

            case BossPhase.Dash:
                boss.GetDashState().SetPhaseDuration(currentPhaseDuration);
                boss.SetState(boss.GetDashState());
                break;

            case BossPhase.Shoot:
                boss.GetShootState().SetPhaseDuration(currentPhaseDuration);
                boss.SetState(boss.GetShootState());
                break;
        }
    }

    private BossPhase RollPhase()
    {
        BossPhase phase;
        do
        {
            phase = (BossPhase)Random.Range(0, 3);
        }
        while (phase == previousPhase);

        return phase;
    }

    private void HandlePhaseTwo()
    {
        float hpPercent =
            (float)boss.healthComponent.GetCurrentHealth() /
            boss.healthComponent.GetMaxHealth();

        if (!hasEnteredSecondPhase && hpPercent <= boss.GetPhaseChangeHealthPercent())
        {
            hasEnteredSecondPhase = true;
            minPhaseTime *= 0.7f;
            maxPhaseTime *= 0.7f;
            Debug.Log("Boss entered Phase 2 (faster phases)");
        }
    }
}
