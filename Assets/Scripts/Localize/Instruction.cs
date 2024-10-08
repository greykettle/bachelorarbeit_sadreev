using UnityEngine;

[System.Serializable]
public class Instruction
{
    [TextArea]
    public string GermanText;
    [TextArea]
    public string EnglishText;

    public string GetText(string language)
    {
        switch (language)
        {
            case "eng":
                return EnglishText;
            case "de":
                return GermanText;
            default:
                return EnglishText; 
        }
    }
}
