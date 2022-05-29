using UnityEngine;
using TMPro;

public class CoordinatesUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI numbersText;
    [SerializeField] private TextMeshProUGUI charactersText;

    private const string letters = "aabcdefgh";

    public void SwitchDisplay (PieceColor color)
    {
        string primaryColorHEX = "#" + ColorUtility.ToHtmlStringRGBA(ChessUI.Singleton.Skin.PrimaryColor);
        string secondaryColorHEX = "#" + ColorUtility.ToHtmlStringRGBA(ChessUI.Singleton.Skin.SecondaryColor);

        string numberCoords = "";
        string charCoords = "";

        if (color == PieceColor.BLACK)
        {
            // Numbers
            for (int i = 1; i <= 8; i++)
            {
                if (i % 2 != 0)
                {
                    numberCoords += $"<color={primaryColorHEX}>";
                }
                else
                {
                    numberCoords += $"<color={secondaryColorHEX}>";
                }

                numberCoords += i + "</color>";

                if (i != 8)
                {
                    numberCoords += "\n";
                }
            }

            // Letters
            for (int i = 8; i >= 1; i--)
            {
                if (i % 2 == 0)
                {
                    charCoords += $"<color={secondaryColorHEX}>{letters[i]}</color>";
                }
                else
                {
                    charCoords += $"<color={primaryColorHEX}>{letters[i]}</color>";
                }

                if (i != 1)
                {
                    charCoords += " ";
                }
            }
        }
        else
        {
            // Numbers
            for (int i = 8; i >= 1; i--)
            {
                if (i % 2 == 0)
                {
                    numberCoords += $"<color={primaryColorHEX}>";
                }
                else
                {
                    numberCoords += $"<color={secondaryColorHEX}>";
                }

                numberCoords += i + "</color>";

                if (i != 1)
                {
                    numberCoords += "\n";
                }
            }

            // Letters
            for (int i = 1; i <= 8; i++)
            {
                if (i % 2 != 0)
                {
                    charCoords += $"<color={secondaryColorHEX}>{letters[i]}</color>";
                }
                else
                {
                    charCoords += $"<color={primaryColorHEX}>{letters[i]}</color>";
                }

                if (i != 8)
                {
                    charCoords += " ";
                }
            }
        }

        numbersText.text = numberCoords;
        charactersText.text = charCoords;
    }
}
