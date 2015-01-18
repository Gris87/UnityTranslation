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

        Translator.addLanguageChangedListener(OnLanguageChanged);
    }

    void OnDestroy()
    {
        Translator.removeLanguageChangedListener(OnLanguageChanged);
    }

    public void OnLanguageChanged()
    {
        text.text = Translator.getQuantityString(id, mSlider.value, mSlider.value);
    }

    public void OnValueChanged(float newValue)
    {
        OnLanguageChanged();
    }
}
