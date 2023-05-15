using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraftManager : MonoBehaviour
{
    #region Draft Config

    public const int MaxDraftCount = 14;
    public static readonly List<int> draftOrder = new List<int>() {3, 6, 7, 9, 11, 13};
    private static List<Vector3> instantiationPositions = new List<Vector3>() {
        new Vector3(1f, 2.75f, 1f), 
        new Vector3(2f, 2.75f, 1f), 
        new Vector3(3f, 2.75f, 1f), 
        new Vector3(-1f, 1.8f, 1f), 
        new Vector3(-2f, 1.8f, 1f), 
        new Vector3(-3f, 1.8f, 1f), 
        new Vector3(1f, 0.9f, 1f), 
        new Vector3(-1f, -0.05f, 1f), 
        new Vector3(-2f, -0.05f, 1f), 
        new Vector3(1f, -0.95f, 1f), 
        new Vector3(2f, -0.95f, 1f), 
        new Vector3(-1f, -1.9f, 1f), 
        new Vector3(-2f, -1.9f, 1f), 
        new Vector3(1f, -2.85f, 1f) };

    #endregion

    private static int draftCounter;
    private static int draftOrderIndex;

    private void Awake()
    {
        draftCounter = 0;
        draftOrderIndex = 0;
    }

    private void Start()
    {
        GameManager.ChangeGamePhase(GamePhase.DRAFT);
    }

    public void CreateCharacter()
    {
        if (draftCounter >= MaxDraftCount) return;
        
        string buttonName = EventSystem.current.currentSelectedGameObject.name;

        if (!PlayerManager.IsCurrentPlayer(buttonName)) return;

        if (!PlayerManager.ClientIsCurrentPlayer())
            return;

        Enum.TryParse(buttonName.Split("_")[0], out CharacterType characterType);

        DraftCharacter(characterType, PlayerManager.GetCurrentPlayer());

        AudioEvents.PressingButton();
    }

    public static void DraftCharacter(CharacterType type, Player side)
    {
        if (draftCounter >= MaxDraftCount) return;

        Character character = CharacterFactory.CreateCharacter(type, side);
        GameObject characterGameObject = character.GetCharacterGameObject();

        characterGameObject.transform.position = instantiationPositions[draftCounter];

        DraftEvents.CharacterCreated(character);

        AdvanceDraftOrder();
    }

    public static void RandomDrafts(Player side)
    {
        int i = GetRemainingDraftCount(side);
        while (i-- > 0)
        {
            RandomDraft(side);
        }
    }

    public static void RandomDraft(Player side)
    {
        CharacterType randomCharacterType = CharacterFactory.GetRandomCharacterType();
        DraftCharacter(randomCharacterType, side);
    }

    public static int GetRemainingDraftCount(Player currentPlayer)
    {
        if(PlayerManager.GetCurrentPlayer() != currentPlayer)
        {
            return 0;
        }

        if (draftOrderIndex == draftOrder.Count)
        {
            return MaxDraftCount - draftCounter;
        }
        return draftOrder[draftOrderIndex] - draftCounter;
    }

    public static int GetCurrentDraftCount()
    {
        if(draftOrderIndex == 0)
        {
            return draftOrder[draftOrderIndex];
        } else if(draftOrderIndex == draftOrder.Count)
        {
            return MaxDraftCount - draftOrder[draftOrderIndex - 1];
        }
        return draftOrder[draftOrderIndex] - draftOrder[draftOrderIndex - 1];
    }

    private static void AdvanceDraftOrder()
    {
        draftCounter += 1;

        if (draftOrder.Contains(draftCounter))
        {
            draftOrderIndex++;
            PlayerManager.NextPlayer();
        }
            
        
        if (draftCounter >= MaxDraftCount)
        {
            DraftCompleted();
        }
    }

    private static void DraftCompleted()
    {
        GameEvents.EndGamePhase(GamePhase.DRAFT);
    }
}