public interface IMachineInterface
{
    public bool IsAvailable{get;}
    bool isTakenPlace{get;}
    bool isUnlocked{get;}
    public Food unlockedFood{get;}
    int currentLevel{get;}
    float currentCookTime { get; }
    int currentUpgradeMoney{get;}

    public void SwitchTakenPlace();
    public void SetData(int level, bool unlock);
    public SaveData<IMachineInterface> GetData();
}
