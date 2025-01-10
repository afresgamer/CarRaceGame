using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class CpuCar : MonoBehaviour
{
    [SerializeField, Header("移動時間")]
    private float speedDuration = 10.0f;
    [SerializeField, Header("移動ルート")]
    private Transform moveTranParent;
    [SerializeField, Header("移動方法に向くスピード")]
    private float moveRotSpeed = 0.01f;
    [SerializeField, Header("周回数")]
    private int loopCnt = 2;

    public int LapCnt { get; set; }
    public int CheckCnt {  get; set; }

    void Start()
    {
        var path = EnumChildrenRecursive(moveTranParent).Select(x => x.position).ToArray();
        Tween tween = transform.DOLocalPath(path, speedDuration, PathType.CatmullRom, gizmoColor: Color.red)
            .SetOptions(true)
            .SetEase(Ease.Linear)
            .SetLookAt(moveRotSpeed, Vector3.forward)
            .SetLoops(CarGameManager.Instance.GoalCnt == 0 ? loopCnt : CarGameManager.Instance.GoalCnt, LoopType.Restart);
    }

    // parent直下の子オブジェクトLinqで再帰的に列挙する
    private IEnumerable<Transform> EnumChildrenRecursive(Transform parent)
    {
        // 親を含まない場合は親をスキップする
        return parent
            .GetComponentsInChildren<Transform>(true) // 親を含む子を再帰的に取得
            .Skip(1); // 親をスキップする
    }
}
