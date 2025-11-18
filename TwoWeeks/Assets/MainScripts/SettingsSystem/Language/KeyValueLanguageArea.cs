using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KeyValueLanguageArea
{
    public LanguageIndex key;
    [TextArea]
    public string value;
}

[System.Serializable]
public class KeyValueLanguage
{
    public LanguageIndex key;
    public string value;
}
