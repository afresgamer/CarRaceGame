using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    [SerializeField, Header("ラップ数")] private TextMeshProUGUI lapCntText;
    [SerializeField, Header("経過時間")] private TextMeshProUGUI nowTime;
    [SerializeField, Header("順位")] private TextMeshProUGUI rankingText;
    [SerializeField, Header("チェックポイント数")] private int currentCheckPointCnt;
    [SerializeField, Header("ゴール時の周回数")] private int goalLapCnt = 2;
    [SerializeField, Header("リザルト画面")] private GameObject resultUI;

    private List<CheckPoint> checkPointList = new List<CheckPoint>();
    private int checkPointCnt = 0;
    private float lapTime = 0;
    private List<GameObject> cpuCarList;

    void Start()
    {
        // ゲームルール初期化
        CarGameManager.Instance.Init();
        CarGameManager.Instance.GoalCnt = goalLapCnt;

        // チェックポイント初期化
        var pointArray = FindObjectsOfType<CheckPoint>();
        checkPointList.AddRange(pointArray);
        if (pointArray.Length >= currentCheckPointCnt) checkPointCnt = currentCheckPointCnt;
        else if (currentCheckPointCnt > pointArray.Length) checkPointCnt = pointArray.Length;
        else if (currentCheckPointCnt == 0) checkPointCnt = pointArray.Length;
        
        // UI初期化
        lapCntText.text = "Lap: " + CarGameManager.Instance.LapCnt;
        var initTimeSpan = new TimeSpan(0, 0, 0);
        nowTime.text = "Time: " + initTimeSpan.ToString(@"hh\:mm\:ss");
        cpuCarList = GameObject.FindGameObjectsWithTag(GameConst.CPUCAR_TAG).ToList();
        rankingText.text = $"Ranking: {cpuCarList.Count + 1} " + $"/ {cpuCarList.Count + 1}";

        // リザルト画面初期化
        resultUI.SetActive(false);
    }

    void Update()
    {
        // フラグ判定
        if (!CarGameManager.Instance.IsGameStart) return;
        if (CarGameManager.Instance.IsGoal) return;

        // UI更新
        lapCntText.text = "Lap: " + CarGameManager.Instance.LapCnt;
        lapTime += Time.deltaTime;
        CarGameManager.Instance.LapTime = lapTime;
        string timeString = string.Format("{0:D2}:{1:D2}:{2:D2}",
            (int)CarGameManager.Instance.LapTime / 60,
            (int)CarGameManager.Instance.LapTime % 60,
            (int)(CarGameManager.Instance.LapTime * 100) % 60);
        nowTime.text = "Time: " + timeString;
        rankingText.text = $"Ranking: {CarGameManager.GetRanking()} " + $"/ {cpuCarList.Count + 1}";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameConst.PLAYER_TAG)) // プレイヤーの場合
        {
            if (checkPointCnt == CarGameManager.Instance.CheckPointCnt)
            {
                CarGameManager.Instance.CheckPointCnt = 0;
                CarGameManager.Instance.LapCnt++;
                if (CarGameManager.Instance.LapCnt == CarGameManager.Instance.GoalCnt) 
                {
                    SetGoal();
                    return;
                }

                var pointArray = FindObjectsOfType<CheckPoint>();
                foreach (var point in pointArray)
                {
                    var ps = point.gameObject.GetComponentInChildren<ParticleSystem>();
                    ps?.Play();
                }
            }
        }
        if (other.CompareTag(GameConst.CPUCAR_TAG)) // CPUの場合
        {
            if (!other.TryGetComponent<CpuCar>(out var cpuCar)) return;

            if (cpuCar.CheckCnt == checkPointCnt)
            {
                cpuCar.CheckCnt = 0;
                cpuCar.LapCnt++;
            }
        }
    }

    private void SetGoal()
    {
        CustomDebugger.ColorLog(CarGameManager.Instance.LapTime.ToString(), GameConst.LogLevel.Lime);
        string timeString = string.Format("{0:D2}:{1:D2}:{2:D2}",
            (int)CarGameManager.Instance.LapTime / 60,
            (int)CarGameManager.Instance.LapTime % 60,
            (int)(CarGameManager.Instance.LapTime * 100) % 60);
        nowTime.text = "Time: " + timeString;
        CarGameManager.Instance.IsGoal = true;
        resultUI.SetActive(true);
    }
}
