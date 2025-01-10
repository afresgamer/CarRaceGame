using System.Linq;

public class CarGameManager : SingletonMonoBehaviour<CarGameManager>
{
    public bool IsGameStart { get; set; }
    public bool IsGoal { get; set; }
    public int LapCnt { get; set; }
    public int GoalCnt { get; set; }
    public int CheckPointCnt { get; set; }
    public float LapTime { get; set; }
    public int Ranking { get; set; }

    public void Init()
    {
        IsGameStart = false;
        IsGoal = false;
        LapCnt = 0;
        GoalCnt = 0;
        CheckPointCnt = 0;
        LapTime = 0;
        Ranking = 0;
    }

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    private static bool IsComparePlayer(CpuCar cpuCar)
    {
        var checkPointCnt = Instance.CheckPointCnt;
        var lapCnt = Instance.LapCnt;

        if (checkPointCnt == 0 && lapCnt == 0)
            return false;
        if (lapCnt < cpuCar.LapCnt)
            return false;
        if (lapCnt <= cpuCar.LapCnt && checkPointCnt <= cpuCar.CheckCnt)
            return false;

        return true;
    }

    public static int GetRanking()
    {
        var result = -1;
        var objArray = FindObjectsOfType<CpuCar>();
        var cpuCarList = objArray.OrderBy(x => x.LapCnt).ThenBy(y => y.CheckCnt).ToList();

        for (int i = 0; i < cpuCarList.Count; i++)
        {
            var cpu = cpuCarList[i];
            if (IsComparePlayer(cpu))
            {
                result = i + 1;
                break;
            }
        }

        if (result == -1) result = cpuCarList.Count + 1;
        return result;
    }
}
