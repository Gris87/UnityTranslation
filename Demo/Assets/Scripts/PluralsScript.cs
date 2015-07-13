using UnityEngine;
using UnityEngine.UI;
using UnityTranslation;



public class PluralsScript : MonoBehaviour
{
    public R.plurals id;
    public Text      text;

    private Slider mSlider;



    // Use this for initialization
    void Start()
    {
        mSlider = GetComponent<Slider>();
        text.text = Translator.GetQuantityString(id, mSlider.value, mSlider.value);

        Translator.AddLanguageChangedListener(OnLanguageChanged);
    }

    void OnDestroy()
    {
        Translator.RemoveLanguageChangedListener(OnLanguageChanged);
    }

    public void OnLanguageChanged()
    {
		text.text = Translator.GetQuantityString(id, mSlider.value, mSlider.value);
    }

    public void OnValueChanged(float newValue)
    {
        OnLanguageChanged();
    }
}
