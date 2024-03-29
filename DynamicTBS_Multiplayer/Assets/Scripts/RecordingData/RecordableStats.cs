using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordableStats
{
    public int gameNumber;
    public int blueWins;

    // Records how many times a certain unit count as been drafted
    public Dictionary<(int, string), int> unitsInDraftDictionary = new Dictionary<(int, string), int>();

    // Records unique drafts as strings, how many times a draft has been played and how many times it has won
    public Dictionary<string, (int, int)> uniqueDraftsDictionary = new Dictionary<string, (int, int)>();

    public RecordableStats()
    {
        gameNumber = 0;
        blueWins = 0;

        foreach (CharacterType characterType in Enum.GetValues(typeof(CharacterType)))
        {
            for (int i = 1; i <= 7; i++)
            {
                unitsInDraftDictionary.Add((i, characterType.ToString()), 0);
            }
        }
    }
}