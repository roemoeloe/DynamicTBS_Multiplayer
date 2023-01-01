using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class cardhandleScript : MonoBehaviour
{
    //notwendig um in UI zu deaktivieren
    private void Start(){}

    private void Awake()
    {
        GameplayEvents.OnCharacterSelectionChange += setActive;
    }

    /// <summary>method <c>setActive</c> aktiviert Kind nach character in cardClass.</summary>
    public void setActive(Character character)
    {
        foreach (Transform card in transform)
        {
            card.gameObject.SetActive((character != null ? character.GetCharacterType() : null) == card.gameObject.GetComponent<cardClass>().character);
        }
    }

    private void OnDestroy()
    {
        GameplayEvents.OnCharacterSelectionChange -= setActive;
    }
}
