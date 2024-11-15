using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConst
{
    // ログの注意レベル
    // 明るい緑：正常(表示された場合は問題ない)
    // 黄色：管理方法が把握不足、必要なデータが用意されていない(改善のみでOK)
    // 赤：エラー、もしくはバグ(表示された場合は修正や改善が必要)
    public enum LogLevel { Lime, Yellow, Red }


    // タグ
    public const string PLAYER_TAG = "Player";
}
