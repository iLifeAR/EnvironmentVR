using UnityEngine;

public class LanguageSetter : MonoBehaviour
{
    [HideInInspector] public AudioSource _AS;

    public enum Grade
    {
        None, Nursery, JrKG, SrKG
    }
    [System.Serializable]
    public struct LanguageInfo
    {
        public LanguageUI.Languageoption _Language;
        public AudioClip _Audio;
    }

    [System.Serializable]
    public struct GradeInfo
    {
        public Grade _Grade;
        public LanguageInfo[] _Languages;
    }

    public GradeInfo[] _LanguageClips;
    public LanguageUI _LanguageUI;
    // Start is called before the first frame update
    void Start()
    {
        _AS = GetComponent<AudioSource>();
    }

    public void SetAudio()
    {
        if (MasterScript.instance == null) return;
        MasterScript MS = MasterScript.instance;
        if (_AS.isPlaying)
        {
            _AS.Stop();
        }
        foreach (GradeInfo item in _LanguageClips)
        {
            if (item._Grade.ToString() == MS.Grade)
            {
                foreach (LanguageInfo item2 in item._Languages)
                {
                    if (item2._Language == _LanguageUI._CurrentLanguage.Language)
                    {
                        Debug.Log(item2._Language);
                        _AS.clip = item2._Audio;
                    }
                }
                break;
            }
        }
    }

    private void OnDisable()
    {
        GetComponent<TargetScript>().OnActivate();

    }

    // Update is called once per frame
    void Update()
    {

    }
}
