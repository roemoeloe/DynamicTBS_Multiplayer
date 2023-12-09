using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSideDisplayHandler : MonoBehaviour
{
    [SerializeField] private GameObject active_pink;
    [SerializeField] private GameObject active_blue;

    private void Awake()
    {
        GameplayEvents.OnCurrentPlayerChanged += SetActive;
    }

    private void Start()
    {
        SetActive(PlayerManager.StartPlayer[GamePhase.GAMEPLAY]);
    }

    private void SetActive(PlayerType side)
    {
        active_pink.SetActive(side == PlayerType.pink);
        active_blue.SetActive(side == PlayerType.blue);
    }

    private void OnDestroy()
    {
        GameplayEvents.OnCurrentPlayerChanged -= SetActive;
    }
}
