using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class cardhandleScript : MonoBehaviour
{
    //notwendig um in UI zu deaktivieren
    private void Start(){}

    /// <summary>method <c>setActive</c> aktiviert Kind nach character in cardClass.</summary>
    public void setActive(CharacterType character)
    {
        foreach (Transform card in transform)
        {
            card.gameObject.SetActive(character == card.gameObject.GetComponent<cardClass>().character);
        }
    }
}