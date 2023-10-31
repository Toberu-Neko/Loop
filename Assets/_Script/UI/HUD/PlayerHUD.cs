using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private GameObject playerStatusObj;
    private HUDStatus playerStatus;

    private void Awake()
    {
        playerStatus = playerStatusObj.GetComponent<HUDStatus>();
    }

}
