using UnityEngine;

[CreateAssetMenu(fileName = "CarInfo", menuName = "ScriptableObject/CarInfo")]
public class CarInfo : ScriptableObject
{
    [Header("車両ID")]
    public int CarId = 0;
    [Header("移動スピード")]
    public float moveSpeed = 10f;
    [Header("回転スピード")]
    public float spinSpeed = 10f;
    [Header("重量")]
    public int weight = 1;
    [Header("加速スピード")]
    public float boostSpeed = 10;
    [Header("空気抵抗")]
    public float drag = 0;
    [Header("回転の空気抵抗")]
    public float angularDrag = 0;
}
