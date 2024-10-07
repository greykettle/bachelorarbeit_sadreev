using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Localize
{
    public class TextHandler : MonoBehaviour
    {
        public string eng;
        public string ger;
        [HideInInspector] public TextMeshProUGUI text;

        private void Awake()
        {
            if (text == null)
            {
                text = GetComponent<TextMeshProUGUI>();
            }
        }

        public void ChangeLanguage(string lang)
        {
            if (text == null)
            {
                text = GetComponent<TextMeshProUGUI>();
            }

            if (lang == "eng")
            {
                text.text = eng;
            } else
            {
                text.text = ger;
            }
        }
    }
}