using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    private const int MaxPlacementCount = 14;
    private static List<int> placementOrder = new List<int>() { 1, 3, 5, 7, 8, 11 };
    
    private PlayerManager playerManager;
    private Board board;
    
    private int placementCount;

    private void Awake()
    {
        SubscribeEvents();
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        board = GameObject.Find("GameplayCanvas").GetComponent<Board>();
        placementCount = 0;
    }

    private void SortCharacters(List<Character> characters)
    {
        Vector3 blueStartPosition = new Vector3(-7.5f, 3, 1);
        Vector3 pinkStartPosition = new Vector3(7.5f, 3, 1);
        float verticalOffset = 1;

        foreach (Character character in characters)
        {
            if (character.GetSide().GetPlayerType() == PlayerType.blue)
            {
                character.GetCharacterGameObject().transform.position = blueStartPosition;
                blueStartPosition.y -= verticalOffset;
            }
            else
            {
                character.GetCharacterGameObject().transform.position = pinkStartPosition;
                pinkStartPosition.y -= verticalOffset;
            }
        }
    }

    private void AdvancePlacementOrder()
    {
        placementCount += 1;
        
        if (placementOrder.Contains(placementCount))
            playerManager.NextPlayer();

        if (placementCount >= MaxPlacementCount)
        {
            SpawnMasters();
            GameplayEvents.StartGameplayPhase();
        }
            
    }
    
    private void SpawnMasters()
    {
        SpawnMaster(PlayerType.blue);
        SpawnMaster(PlayerType.pink);
    }

    private void SpawnMaster(PlayerType playerType) 
    {
        Character master = CharacterFactory.CreateCharacter(CharacterType.MasterChar, playerManager.GetPlayer(playerType));

        Tile masterSpawnTile = board.FindMasterStartTile(playerType);
        Vector3 position = masterSpawnTile.GetPosition();
        master.GetCharacterGameObject().transform.position = new Vector3(position.x, position.y, 0.998f);
        masterSpawnTile.SetCurrentInhabitant(master);
        DraftEvents.CharacterCreated(master);
    }

    #region EventsRegion

    private void SubscribeEvents()
    {
        PlacementEvents.OnAdvancePlacementOrder += AdvancePlacementOrder;
        DraftEvents.OnDeliverCharacterList += SortCharacters;
    }

    private void UnsubscribeEvents()
    {
        PlacementEvents.OnAdvancePlacementOrder -= AdvancePlacementOrder;
        DraftEvents.OnDeliverCharacterList -= SortCharacters;
    }

    #endregion

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }
}