using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultController : MonoBehaviour
{
    [SerializeField, Header("リザルトの結果")] TextMeshProUGUI resultText;
    [SerializeField, Header("リザルトのタイム")] TextMeshProUGUI resultTimeText;

    void Start()
    {
        resultText.text = CarGameManager.Instance.IsGoal ? "Goal" : "Failure";
        string timeString = string.Format("{0:D2}:{1:D2}:{2:D2}",
                    (int)CarGameManager.Instance.LapTime / 60,
                    (int)CarGameManager.Instance.LapTime % 60,
                    (int)(CarGameManager.Instance.LapTime * 100) % 60);
        resultTimeText.text = "Time: " + timeString;
    }
}
