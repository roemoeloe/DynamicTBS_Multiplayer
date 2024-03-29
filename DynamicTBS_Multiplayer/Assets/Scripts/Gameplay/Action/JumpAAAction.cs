using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAAAction : MonoBehaviour, IAction
{
    [SerializeField]
    private GameObject jumpPrefab;

    public ActionType ActionType { get { return ActionType.ActiveAbility; } }

    private List<GameObject> jumpTargets = new List<GameObject>();
    public List<GameObject> ActionDestinations { get { return jumpTargets; } }

    private Character characterInAction = null;
    public Character CharacterInAction { get { return characterInAction; } }

    private List<GameObject> patternTargets = new List<GameObject>();

    private void Awake()
    {
        GameplayEvents.OnGameplayPhaseStart += Register;
    }

    public void ShowActionPattern(Character character)
    {
        List<Vector3> patternPositions = FindMovePositions(character, true);

        if (patternPositions != null)
        {
            patternTargets = ActionUtils.InstantiateActionPositions(patternPositions, jumpPrefab);
        }
    }

    public void HideActionPattern()
    {
        ActionUtils.Clear(patternTargets);
    }

    public int CountActionDestinations(Character character)
    {
        List<Vector3> movePositions = FindMovePositions(character);

        if (movePositions != null)
        {
            return movePositions.Count;
        }

        return 0;
    }

    public void CreateActionDestinations(Character character)
    {
        List<Vector3> movePositions = FindMovePositions(character);

        if (movePositions != null && movePositions.Count > 0)
        {
            jumpTargets = ActionUtils.InstantiateActionPositions(movePositions, jumpPrefab);
            characterInAction = character;
        }
    }

    public void ExecuteAction(GameObject actionDestination)
    {
        Tile tile = Board.GetTileByPosition(actionDestination.transform.position);
        if (tile != null)
        {
            Vector3 oldPosition = characterInAction.GetCharacterGameObject().transform.position;
            characterInAction.GetCharacterGameObject().transform.position = actionDestination.transform.position;
            Board.UpdateTilesAfterMove(oldPosition, characterInAction);
        }

        AbortAction();
    }

    public void AbortAction()
    {
        ActionUtils.Clear(jumpTargets);
        characterInAction = null;
        ActionRegistry.Remove(this);
    }

    private List<Vector3> FindMovePositions(Character character, bool pattern = false)
    {
        Tile characterTile = Board.GetTileByPosition(character.GetCharacterGameObject().transform.position);

        List<Tile> moveTiles = Board.GetTilesOfDistance(characterTile, JumpAA.movePattern, JumpAA.distance);

        List<Vector3> movePositions = moveTiles
            .FindAll(tile => tile.IsAccessible() || pattern)
            .ConvertAll(tile => tile.GetPosition());

        return movePositions;
    }

    private void Register()
    {
        ActionRegistry.RegisterPatternAction(this);
    }

    private void OnDestroy()
    {
        GameplayEvents.OnGameplayPhaseStart -= Register;
    }
}
