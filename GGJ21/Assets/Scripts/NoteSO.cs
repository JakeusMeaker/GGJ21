using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Note", menuName = "Note", order = 51)]

public class NoteSO : ScriptableObject
{
    public string noteText;

    public int noteNumber;
}
