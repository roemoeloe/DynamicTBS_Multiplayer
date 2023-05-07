using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    #region Gameplay Config

    public const int maxActionsPerRound = 2;

    #endregion

    private static int remainingActions;

    private static Dictionary<Character, List<ActionType>> actionsPerCharacterPerTurn = new Dictionary<Character, List<ActionType>>();

    private static bool hasGameStarted;

    public static bool gameIsPaused;

    private void Awake()
    {
        UnsubscribeEvents();
        SubscribeEvents();
        ResetStates();
        hasGameStarted = false;
        gameIsPaused = false;
        ActionRegistry.RemoveAll();
    }

    public static int GetRemainingActions()
    {
        return remainingActions;
    }

    public static bool ActionAvailable(Character character, ActionType actionType)
    {
        if(actionsPerCharacterPerTurn.ContainsKey(character) && actionsPerCharacterPerTurn[character].Contains(actionType))
        {
            return false;
        }
        return true;
    }

    private void ToggleGameIsPaused(Player player, UIAction uIAction)
    {
        if(uIAction == UIAction.PAUSE_GAME || uIAction == UIAction.UNPAUSE_GAME)
        {
            gameIsPaused = uIAction == UIAction.PAUSE_GAME;
            GameplayEvents.PauseGame(gameIsPaused);
        }
    }

    private void ResetStates() 
    {
        SetRemainingActions(maxActionsPerRound);
        actionsPerCharacterPerTurn.Clear();
    }

    private void OnActionFinished(ActionMetadata actionMetadata) 
    {
        SetRemainingActions(remainingActions - actionMetadata.ActionCount);
        if (remainingActions <= 0)
        {
            HandleNoRemainingActions();
        } else if(actionMetadata.CharacterInAction != null)
        {
            if(actionsPerCharacterPerTurn.ContainsKey(actionMetadata.CharacterInAction))
            {
                actionsPerCharacterPerTurn[actionMetadata.CharacterInAction].Add(actionMetadata.ExecutedActionType);
            } else
            {
                actionsPerCharacterPerTurn.Add(actionMetadata.CharacterInAction, new List<ActionType>() { actionMetadata.ExecutedActionType });
            }

            if(!actionMetadata.ExecutingPlayer.HasAvailableAction())
            {
                GameplayEvents.AbortCurrentPlayerTurn(remainingActions, AbortTurnCondition.NO_AVAILABLE_ACTION);
            }
        }
    }

    private void SetRemainingActions(int newRemainingActions)
    {
        remainingActions = newRemainingActions;
        GameplayEvents.RemainingActionsChanged();
    }

    private void HandleNoRemainingActions()
    {
        PlayerManager.NextPlayer();
        ResetStates();
    }

    private void OnPlayerTurnEnded(Player player)
    {
        // Check if other player can perform any action (move/attack/ActiveAbility) -> if not, player wins
        Player otherPlayer = PlayerManager.GetOtherPlayer(player);

        if (!otherPlayer.HasAvailableAction())
        {
            Debug.Log("Player " + otherPlayer.GetPlayerType() + " lost because player can not perform any action this turn.");
            GameplayEvents.GameIsOver(player.GetPlayerType(), GameOverCondition.NO_AVAILABLE_ACTION);
        }
    }

    private void AbortTurn()
    {
        SetRemainingActions(0);
        HandleNoRemainingActions();
    }

    private void AbortTurn(int remainingActions, AbortTurnCondition abortTurnCondition)
    {
        AbortTurn();
    }

    public static bool HasGameStarted()
    {
        return hasGameStarted;
    }

    private void SubscribeToGameplayEvents(GamePhase gamePhase)
    {
        if (gamePhase != GamePhase.PLACEMENT)
            return;

        GameManager.ChangeGamePhase(GamePhase.GAMEPLAY);

        GameplayEvents.OnFinishAction += OnActionFinished;
        GameplayEvents.OnPlayerTurnEnded += OnPlayerTurnEnded;
        GameplayEvents.OnPlayerTurnAborted += AbortTurn;
        hasGameStarted = true;
    }

    #region EventSubscriptions

    private void SubscribeEvents()
    {
        GameEvents.OnGamePhaseEnd += SubscribeToGameplayEvents;
        GameplayEvents.OnExecuteUIAction += ToggleGameIsPaused;
    }

    private void UnsubscribeEvents()
    {
        GameEvents.OnGamePhaseEnd -= SubscribeToGameplayEvents;
        GameplayEvents.OnFinishAction -= OnActionFinished;
        GameplayEvents.OnPlayerTurnEnded -= OnPlayerTurnEnded;
        GameplayEvents.OnPlayerTurnAborted -= AbortTurn;
        GameplayEvents.OnExecuteUIAction -= ToggleGameIsPaused;
    }

    #endregion

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }
}