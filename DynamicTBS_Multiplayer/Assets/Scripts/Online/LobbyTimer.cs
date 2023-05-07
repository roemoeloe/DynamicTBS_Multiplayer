using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyTimer
{
    #region Helper classes
    class PlayerTime
    {
        public float timeLeft;
        public int debuff = 0;
    }

    #endregion

    private float draftAndPlacementTime;
    private float gameplayTime;

    private PlayerType currentPlayer = PlayerManager.StartPlayer[GamePhase.DRAFT];
    private Dictionary<PlayerType, PlayerTime> timePerPlayer = new Dictionary<PlayerType, PlayerTime>();

    private TimerType currentTimerType = TimerType.DRAFT_AND_PLACEMENT;
    private GamePhase currentGamePhase = GamePhase.DRAFT;

    public GamePhase CurrentGamePhase { get { return currentGamePhase; } }

    public LobbyTimer(float draftAndPlacementTime, float gameplayTime)
    {
        this.draftAndPlacementTime = draftAndPlacementTime;
        this.gameplayTime = gameplayTime;

        timePerPlayer.Add(PlayerType.pink, new PlayerTime { timeLeft = draftAndPlacementTime });
        timePerPlayer.Add(PlayerType.blue, new PlayerTime { timeLeft = draftAndPlacementTime });
    }

    public void UpdateGameInfo(PlayerType currentPlayer, GamePhase gamePhase)
    {
        this.currentPlayer = currentPlayer;
        this.currentTimerType = gamePhase == GamePhase.GAMEPLAY ? TimerType.GAMEPLAY : TimerType.DRAFT_AND_PLACEMENT;

        if (currentGamePhase != gamePhase)
        {
            this.currentGamePhase = gamePhase;
            float newTime = currentTimerType == TimerType.GAMEPLAY ? gameplayTime : draftAndPlacementTime;

            timePerPlayer[PlayerType.pink].timeLeft = newTime;
            timePerPlayer[PlayerType.blue].timeLeft = newTime;
        }

        if(gamePhase == GamePhase.GAMEPLAY)
        {
            timePerPlayer[currentPlayer].timeLeft = gameplayTime * Mathf.Pow(1 - Timer.debuffRate, timePerPlayer[currentPlayer].debuff);
        }
    }

    public void UpdateTime(int lobbyId)
    {
        if(timePerPlayer[currentPlayer].timeLeft > 0)
        {
            timePerPlayer[currentPlayer].timeLeft--;
        } else
        {
            if (currentTimerType == TimerType.GAMEPLAY)
            {
                timePerPlayer[currentPlayer].debuff++;
            }

            OnlineServer.Instance.Broadcast(new MsgServerNotification
            {
                serverNotification = ServerNotification.TIMEOUT,
                gamePhase = currentGamePhase,
                currentPlayer = currentPlayer,
                currentPlayerTimerDebuff = timePerPlayer[currentPlayer].debuff
            }, lobbyId);
        }

        BroadcastTimerInfo(lobbyId);
    }

    public void BroadcastTimerInfo(int lobbyId)
    {
        OnlineServer.Instance.Broadcast(new MsgSyncTimer
        {
            pinkTimeLeft = timePerPlayer[PlayerType.pink].timeLeft,
            blueTimeLeft = timePerPlayer[PlayerType.blue].timeLeft,
            pinkDebuff = timePerPlayer[PlayerType.pink].debuff,
            blueDebuff = timePerPlayer[PlayerType.blue].debuff,
        }, lobbyId);
    }
}