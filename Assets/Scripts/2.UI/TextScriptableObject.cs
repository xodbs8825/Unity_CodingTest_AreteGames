using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Text", menuName = "Scriptable Object/Text", order = 1)]
public class TextScriptableObject : ScriptableObject
{
    [TextArea(2, 1)]
    public List<string> title;

    [TextArea(5, 5)]
    public List<string> text;
}
