using TMPro;
using UnityEngine;

namespace Extensions
{
    [CreateAssetMenu(fileName = "InputField/" + nameof(BoolValidator), menuName = nameof(BoolValidator))]
    public class BoolValidator : TMP_InputValidator
    {
        public override char Validate(ref string text, ref int pos, char ch)
        {
            // Если текст уже полный "true" или "false" - запрещаем ввод новых символов
            if (text == "true" || text == "false")
                return '\0';

            // Обработка первого символа 't' или 'f' с автодополнением
            if (text.Length == 0)
            {
                ch = char.ToLower(ch);

                if (ch == 't')
                {
                    text = "true";
                    pos = 4; // Устанавливаем позицию после "true"
                    return '\0';
                }
                else if (ch == 'f')
                {
                    text = "false";
                    pos = 5; // Устанавливаем позицию после "false"
                    return '\0';
                }
                else
                {
                    return '\0'; // Запрещаем любой другой первый символ
                }
            }

            // Проверяем, что текущий текст соответствует началу "true" или "false"
            if (!IsValidPrefix(text))
                return '\0';

            // Проверяем вводимый символ
            ch = char.ToLower(ch);

            // Получаем ожидаемый символ для текущей позиции
            char? expectedChar = GetExpectedChar(text, pos);

            if (expectedChar.HasValue && ch == expectedChar.Value)
            {
                // Если вводимый символ совпадает с ожидаемым - разрешаем ввод
                return ch;
            }

            return '\0';
        }

        private bool IsValidPrefix(string text)
        {
            text = text.ToLower();

            // Проверяем, является ли текст префиксом "true" или "false"
            if ("true".StartsWith(text))
                return true;

            if ("false".StartsWith(text))
                return true;

            return false;
        }

        private char? GetExpectedChar(string currentText, int position)
        {
            currentText = currentText.ToLower();

            // Определяем, к какому слову ближе текущий текст
            int trueDistance = LevenshteinDistance(currentText, "true".Substring(0, Mathf.Min(currentText.Length, 4)));
            int falseDistance = LevenshteinDistance(currentText, "false".Substring(0, Mathf.Min(currentText.Length, 5)));

            string targetWord = trueDistance <= falseDistance ? "true" : "false";

            // Если позиция выходит за пределы целевого слова
            if (position >= targetWord.Length)
                return null;

            return targetWord[position];
        }

        private int LevenshteinDistance(string a, string b)
        {
            if (string.IsNullOrEmpty(a))
                return string.IsNullOrEmpty(b) ? 0 : b.Length;

            if (string.IsNullOrEmpty(b))
                return a.Length;

            int[,] d = new int[a.Length + 1, b.Length + 1];

            for (int i = 0; i <= a.Length; i++)
                d[i, 0] = i;

            for (int j = 0; j <= b.Length; j++)
                d[0, j] = j;

            for (int i = 1; i <= a.Length; i++)
            {
                for (int j = 1; j <= b.Length; j++)
                {
                    int cost = (a[i - 1] == b[j - 1]) ? 0 : 1;

                    d[i, j] = Mathf.Min(
                        Mathf.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }

            return d[a.Length, b.Length];
        }
    }
}