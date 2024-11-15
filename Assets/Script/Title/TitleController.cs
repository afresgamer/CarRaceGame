using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleController : MonoBehaviour
{
    [SerializeField, Header("オプション画面")] private GameObject optionUiObj;

    void Start()
    {
        optionUiObj.SetActive(false);
    }
}
