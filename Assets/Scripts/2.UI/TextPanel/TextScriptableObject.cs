using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Text", menuName = "Scriptable Object/Text", order = 0)]
public class TextScriptableObject : ScriptableObject
{
    [TextArea(5, 5)]
    public List<string> text;
}
