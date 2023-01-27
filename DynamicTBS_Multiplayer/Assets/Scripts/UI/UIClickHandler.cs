using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIClickHandler : MonoBehaviour
{
    private Camera currentCamera;
    public GameObject cardhandler;
    //public GameObject uibutton;

    private void Update()
    {
        // In multiplayer mode only listen to clicks of current player
        if (GameManager.gameType == GameType.multiplayer && (Client.Instance.role != ClientType.player || Client.Instance.side != PlayerManager.GetCurrentPlayer().GetPlayerType()))
            return;

        if (!currentCamera)
        {
            currentCamera = Camera.main;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 clickPosition = Input.mousePosition;
            HandleClick(clickPosition);
        }
    }

    private void HandleClick(Vector3 clickPosition)
    {
        // First check whether click was onto an action field (like move destination or attack target)
        // If yes, execute that action
        bool actionExecuted = ActionUtils.ExecuteAction(currentCamera.ScreenPointToRay(clickPosition));

        // If not, check whether click was onto a character
        // If yes, create action destinations for this character
        if (!actionExecuted) {
            List<GameObject> charactersOfPlayer = CharacterHandler.GetAllLivingCharacters()
                .FindAll(character => character.isClickable && character.GetSide() == PlayerManager.GetCurrentPlayer())
                .ConvertAll(character => character.GetCharacterGameObject());

            Ray click = currentCamera.ScreenPointToRay(clickPosition);
            GameObject characterGameObject = UIUtils.FindGameObjectByRay(charactersOfPlayer, click);

            if (characterGameObject == null)
            {
                // Check if click is on UI Element (like active ability button)
                if (!UIUtils.IsHit())
                {
                    // If not
                    GameplayEvents.ChangeCharacterSelection(null);
                }
                return;
            }

            Character character = CharacterHandler.GetCharacterByGameObject(characterGameObject);
            ActionUtils.InstantiateAllActionPositions(character);
            
            GameplayEvents.ChangeCharacterSelection(character);
        } else
        {
            GameplayEvents.ChangeCharacterSelection(null);
        }
    }

    private void HandleKeyInputs()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameplayEvents.ChangeCharacterSelection(null);
        }

        // TODO: Show complete patterns, not just legal moves of...
        // - Attack while pressing A.
        // - Active Ability while pressing S (for special).
        // - Movement while pressing M.
    }
}
