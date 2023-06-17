using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LanguageUI : MonoBehaviour
{
    [System.Serializable]
    public struct LanguageData
    {
        public Languageoption Language;
        public Toggle OptionToggle;
    }
    public LanguageData[] Languages;

    public enum Languageoption
    {
        EnglishBasic, EnglishIntermediary, EnglishAdvanced, EnglishAdvanced_01, EnglishAdvanced_02, Hindi, Marathi, Telugu,Spanish
    }

    public LanguageData _CurrentLanguage;

    public Image BGFadeUI;
    Coroutine BGFadeRoutine;

    [Header("UI")]
    public RectTransform Dialog;
    public Button Close;




    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {

        _CurrentLanguage = new LanguageData();
        _CurrentLanguage.OptionToggle = Languages[0].OptionToggle;
        _CurrentLanguage.Language = Languages[0].Language;
        _CurrentLanguage.OptionToggle.isOn = (true);


        foreach (LanguageData item in Languages)
        {
            item.OptionToggle.onValueChanged.AddListener(delegate { SetLanguage(); });
        }
        Close?.onClick.AddListener(closeDialog);
    }

    void SetLanguage()
    {
        if (MasterScript.instance == null) return;
        MasterScript Mscript = MasterScript.instance;

        foreach (LanguageData item in Languages)
        {
            if (item.OptionToggle.isOn)
            {
                Debug.Log("Selected");
                PlayerPrefs.SetString("Language", item.Language.ToString());
                _CurrentLanguage = new LanguageData();
                _CurrentLanguage.OptionToggle = item.OptionToggle;
                _CurrentLanguage.Language = item.Language;

                if (Mscript.CurrentModel!=null)
                {
                    if (Mscript.CurrentModel.TryGetComponent(out LanguageSetter _LS))
                    {
                        _LS.SetAudio();
                        if(MasterScript.instance.UIScript.AudioToggle.isOn)_LS._AS.Play();
                    }
                }

                return;
            }
        }
    }

    public void OpenDialog()
    {
        if (Dialog.localScale != Vector3.one)
        {
            Dialog.localScale = Vector3.one*0.56f;
            if (BGFadeRoutine != null) StopCoroutine(BGFadeRoutine);

            BGFadeRoutine = StartCoroutine(BGFader());
        }
    }

    void closeDialog()
    {
        Dialog.localScale = Vector3.zero;
        if (BGFadeRoutine != null) StopCoroutine(BGFadeRoutine);
        Color BGCOL2 = BGFadeUI.color;
        BGCOL2.a = 0;
        BGFadeUI.color = BGCOL2;

    }

    IEnumerator BGFader()
    {
        float FadeLimit = 0.75f;
        while (BGFadeUI.color.a < FadeLimit)
        {
            Color BGCOL = BGFadeUI.color;
            BGCOL.a += 0.01f;
            BGFadeUI.color = BGCOL;
            Debug.Log("Fading");
            yield return new WaitForSeconds(0.025f);
        }
        Color BGCOL2 = BGFadeUI.color;
        BGCOL2.a = FadeLimit;
        BGFadeUI.color = BGCOL2;
        BGFadeRoutine = null;
        Debug.Log("Complete");

    }



    // Update is called once per frame
    void Update()
    {

    }
}
