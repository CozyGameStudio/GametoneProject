using UnityEngine;

public abstract class TutorialBase : MonoBehaviour
{
    public abstract void Enter();
    public abstract void Execute(TutorialManagerForStage2 manager);
    public abstract void Exit();
}
