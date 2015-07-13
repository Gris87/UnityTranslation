using UnityEngine;
using UnityEngine.UI;
using UnityTranslation;



public class StringArrayScript : MonoBehaviour
{
    public R.array id;

    private Text mText;



    // Use this for initialization
    void Start()
    {
        mText = GetComponent<Text>();
        OnLanguageChanged();

        Translator.addLanguageChangedListener(OnLanguageChanged);
    }

    void OnDestroy()
    {
        Translator.removeLanguageChangedListener(OnLanguageChanged);
    }

    public void OnLanguageChanged()
    {
        string res = "";

        string[] value = Translator.getStringArray(id);

        for (int i = 0; i < value.Length; ++i)
        {
            if (i > 0)
            {
                res += ", ";
            }

            res += value[i];
        }

        mText.text = res;
    }
}
