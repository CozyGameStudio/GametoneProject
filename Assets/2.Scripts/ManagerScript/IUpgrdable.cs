
public interface IUpgradable 
{
    public string name{get;}
    public int currentLevel{get;}
    public bool isUnlocked{get;}
    public void LevelUp();
    public void SetData();
}
