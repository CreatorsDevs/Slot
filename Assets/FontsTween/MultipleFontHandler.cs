using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Globalization;

namespace CusTween
{
    public class MultipleFontHandler : MonoBehaviour
    {
        [SerializeField]
        TMP_Text[] texts;
        [SerializeField]
        bool canTweenAmount = false;

        private List<TweenFloatValue> tweenAmountList;
        private List<TweenIntValue> tweenIntAmountList;

        public TMP_Text[] Texts { get => texts; set => texts = value; }
        public bool CanTweenAmount { get => canTweenAmount; set => canTweenAmount = value; }

        public void setText(float amount)
        {
            if (amount <= 0)
            {
                foreach (var text in texts) text.text = "0.00";
                return;
            }
            if (canTweenAmount)
            {
                setTweenFunctionality();
                foreach (var tween in tweenAmountList)
                {
                    tween.Tween(amount, 100);
                }
                return;
            }
            foreach (var text in texts) text.text = amount.ToString("F2");
        }

        public void setText(int amount)
        {
            if (amount <= 0)
            {
                foreach (var text in texts) text.text = "0";
                return;
            }

            if (canTweenAmount)
            {
                setIntTweenFunctionality();
                foreach (var tween in tweenIntAmountList)
                {
                    tween.Tween(amount, 100);
                }
                return;
            }

            foreach (var text in texts) text.text = amount < 10 ? amount.ToString() : amount.ToString("0,0");
        }

        private void setTweenFunctionality()
        {
            tweenAmountList = new List<TweenFloatValue>();
            foreach (var text in texts)
            {
                float initValue = float.Parse(text.text, CultureInfo.InvariantCulture.NumberFormat);
                var a = new TweenFloatValue(text.gameObject, initValue, 1f, 1f, true,
                        (value) =>
                        {
                            text.text = value.ToString("F2");
                        });
                tweenAmountList.Add(a);
            }
        }

        private void setIntTweenFunctionality()
        {
            tweenIntAmountList = new List<TweenIntValue>();
            foreach (var text in texts)
            {
                bool success = Int32.TryParse(text.text, out int initValue);
                if (success)
                {
                    var a = new TweenIntValue(text.gameObject, initValue, 1f, 1f, true,
                            (value) =>
                            {
                                text.text = value.ToString();
                            });
                    tweenIntAmountList.Add(a);
                }
                else
                    Debug.LogWarning("Input string was not in correct format: " + text.text);
            }
        }

        public void setStringText(string stringText)
        {
            foreach (var text in texts) text.text = stringText;
        }
    }

}
