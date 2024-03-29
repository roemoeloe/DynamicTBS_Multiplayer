using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionsCounterHandler : MonoBehaviour
{
    [SerializeField] private GameObject blueActionsCounter;
    [SerializeField] private GameObject pinkActionsCounter;

    private void Awake()
    {
        SubscribeEvents();
        ChangeVisibility(false);
    }

    public void UpdateActionsCounter()
    {
        CurrentActionsCounter().GetComponent<Text>().text = GameplayManager.GetRemainingActions().ToString();
    }

    private GameObject CurrentActionsCounter()
    {
        if (PlayerManager.GetCurrentPlayer().GetPlayerType() == PlayerType.blue)
        {
            return blueActionsCounter;
        }
        return pinkActionsCounter;
    }

    private void ChangeVisibility(bool active)
    {
        blueActionsCounter.SetActive(active);
        pinkActionsCounter.SetActive(active);
    }

    private void SetActive()
    {
        ChangeVisibility(true);
        GameplayEvents.OnChangeRemainingActions += UpdateActionsCounter;
    }

    #region EventsRegion

    private void SubscribeEvents()
    {
        GameplayEvents.OnGameplayPhaseStart += SetActive;
    }

    private void UnsubscribeEvents()
    {
        GameplayEvents.OnGameplayPhaseStart -= SetActive;
        GameplayEvents.OnChangeRemainingActions -= UpdateActionsCounter;
    }

    #endregion

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }
}
