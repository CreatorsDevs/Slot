using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{
    [SerializeField] private Image loadingBarBackground;
    [SerializeField] private Image loadingBarFillImage;
    [SerializeField] private Image baseBackground;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private Sprite loadingImageBG;
    [SerializeField] private Sprite loadingBarBlank;
    [SerializeField] private Sprite loadingBarFull;
    [SerializeField] private TMP_FontAsset loadingFont;
    private static bool isLoadingComplete;
    private void Awake()
    {
        loadingSlider.value = 0;
        isLoadingComplete = true;
        baseBackground.sprite = loadingImageBG;
        loadingBarBackground.sprite = loadingBarBlank;
        loadingBarFillImage.sprite = loadingBarFull;
        loadingText.font = loadingFont;
    }

    public IEnumerator MoveSliderRandomly()
    {
        while (isLoadingComplete)
        {
            if (loadingSlider.value > 99)
            {
                isLoadingComplete = false;
                gameObject.SetActive(false);
                break;
            }
            else
            {
                loadingSlider.value += Random.Range(10, 21);
                loadingText.text = "Loading " + loadingSlider.value + "%";
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
}
