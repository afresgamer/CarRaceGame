public class CarGameManager : SingletonMonoBehaviour<CarGameManager>
{
    public bool IsGameStart { get; set; }
    public bool IsGoal { get; set; }
    public int LapCnt { get; set; }
    public int GoalCnt { get; set; }
    public int CheckPointCnt { get; set; }
    public float LapTime { get; set; }
    public string Rank { get; set; }

    public void Init()
    {
        IsGameStart = false;
        IsGoal = false;
        LapCnt = 0;
        GoalCnt = 0;
        CheckPointCnt = 0;
        LapTime = 0;
        Rank = "";
    }

    protected override void Awake()
    {
        base.Awake();
        Init();
    }
}
