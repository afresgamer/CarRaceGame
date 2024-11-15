using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    [SerializeField, Header("ラップ数")] private TextMeshProUGUI lapCntText;
    [SerializeField, Header("経過時間")] private TextMeshProUGUI nowTime;
    [SerializeField, Header("チェックポイント数")] private int currentCheckPointCnt;
    [SerializeField, Header("ゴール時の周回数")] private int goalLapCnt = 2;
    [SerializeField, Header("リザルト画面")] private GameObject resultUI;

    private List<CheckPoint> checkPointList = new List<CheckPoint>();
    private int checkPointCnt = 0;
    private float lapTime = 0;

    void Start()
    {
        var pointArray = FindObjectsOfType<CheckPoint>();
        checkPointList.AddRange(pointArray);
        if (pointArray.Length >= currentCheckPointCnt) checkPointCnt = currentCheckPointCnt;
        else if (currentCheckPointCnt > pointArray.Length) checkPointCnt = pointArray.Length;
        else if (currentCheckPointCnt == 0) checkPointCnt = pointArray.Length;

        lapCntText.text = "Lap: " + CarGameManager.Instance.LapCnt;
        var initTimeSpan = new TimeSpan(0, 0, 0);
        nowTime.text = "Time: " + initTimeSpan.ToString(@"hh\:mm\:ss");

        resultUI.SetActive(false);
    }

    void Update()
    {
        if (!CarGameManager.Instance.IsGameStart) return;

        lapCntText.text = "Lap: " + CarGameManager.Instance.LapCnt;

        if (CarGameManager.Instance.IsGoal) return;
        lapTime += Time.deltaTime;
        CarGameManager.Instance.LapTime = lapTime;
        string timeString = string.Format("{0:D2}:{1:D2}:{2:D2}",
            (int)CarGameManager.Instance.LapTime / 60,
            (int)CarGameManager.Instance.LapTime % 60,
            (int)(CarGameManager.Instance.LapTime * 100) % 60);
        nowTime.text = "Time: " + timeString;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameConst.PLAYER_TAG))
        {
            if (checkPointCnt == CarGameManager.Instance.CheckPointCnt)
            {
                CarGameManager.Instance.CheckPointCnt = 0;
                CarGameManager.Instance.LapCnt++;
                if (CarGameManager.Instance.LapCnt == goalLapCnt) 
                {
                    SetGoal();
                    return;
                }

                var pointArray = FindObjectsOfType<CheckPoint>();
                foreach (var point in pointArray)
                {
                    var ps = point.gameObject.GetComponentInChildren<ParticleSystem>();
                    if (ps != null) ps.Play();
                    var collider = point.gameObject.GetComponent<BoxCollider>();
                    if (collider != null) collider.enabled = true;
                }
            }
        }
    }

    private void SetGoal()
    {
        string timeString = string.Format("{0:D2}:{1:D2}:{2:D2}",
            (int)CarGameManager.Instance.LapTime / 60,
            (int)CarGameManager.Instance.LapTime % 60,
            (int)(CarGameManager.Instance.LapTime * 100) % 60);
        nowTime.text = "Time: " + timeString;
        CarGameManager.Instance.IsGoal = true;
        resultUI.SetActive(true);
    }
}
