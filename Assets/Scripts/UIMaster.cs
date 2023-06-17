using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMaster : MonoBehaviour
{
    public MasterScript Mscript;

    [Header("Doodle")]
    public Toggle DoodleToggle;
    public Button DoodlePanelButton;
    Coroutine DoodlePanelCloseRoutine;


    [Header("OfflineUI")]
    public Toggle SelectionPanelToggle;
    public RectTransform SelectionPanel;

    [Space(15), Header("Audio")]
    public Toggle AudioToggle;
    public Button AudioPanelButton;
    public Button LanguageSelectionToggle;

    [System.Serializable]
    public struct AudioIcons
    {
        public Sprite MuteIcon;
        public Sprite PauseIcon;
    }
    public AudioIcons _AudioIconSprites;

    public enum AudioOptions
    {
        None, Mute, Pause
    }
    public AudioOptions Type;
    public AudioSource _BGM;
    public AudioSource _IntroVO;



    Coroutine AudioPanelCloseRoutine;


    // Start is called before the first frame update
    void Start()
    {
        SetText();
        if (Type == AudioOptions.None)
        {
            Debug.LogError("Select Audio Type");
        }

        DoodleToggle.onValueChanged.AddListener(delegate { TextureToggle(); });

        SelectionPanelToggle.onValueChanged.AddListener(delegate { ToggleOfflinePanel();});
    }

    public void ToggleOfflinePanel()
    {
        if (SelectionPanelToggle.isOn)
        {
            SelectionPanel.DOAnchorPosX(135, 0.75f).SetEase(Ease.OutBack);
        }
        else
        {
            SelectionPanel.DOAnchorPosX(-155, 0.75f).SetEase(Ease.InBack);
        }
    }


    void TextureToggle()
    {
        //If No Target is scanned,Abort
        if (MasterScript.instance.CurrentModel == null)
        {
            DoodleToggle.SetIsOnWithoutNotify(true);
            return;
        }

        //Reset Panel Coroutine
        if (AudioPanelCloseRoutine != null)
        {
            StopCoroutine(AudioPanelCloseRoutine);
            AudioPanelCloseRoutine = StartCoroutine(CloseAudioPanel());
        }

        //Toggles the texture of Current model based on UI Toggle State
        MasterScript.instance.CurrentModel.GetComponent<TargetScript>().TextureToggle(DoodleToggle.isOn);
    }

    public void SetUIControl(bool ON)
    {
        DoodlePanelButton.interactable = ON;
        if(LanguageSelectionToggle)LanguageSelectionToggle.interactable= ON;
    }

    public void ToggleMute()
    {
        
        if (AudioPanelCloseRoutine != null)
        {
            StopCoroutine(AudioPanelCloseRoutine);
            AudioPanelCloseRoutine = StartCoroutine(CloseAudioPanel());
        }

        if (AudioToggle.isOn)
        {
            if (Type == AudioOptions.Mute)
            {
                AudioListener.volume = 1;
            }
            else
            {
                if (!_BGM.isPlaying)
                {
                    _BGM.Play();
                }

                if (Mscript.CurrentModel == null)
                {
                    if (!_IntroVO.isPlaying)
                    {
                        _IntroVO.Play();
                    }
                }
                else
                {
                    AudioSource AS = Mscript.CurrentModel.GetComponent<AudioSource>();
                    if (!AS.isPlaying)
                    {
                        AS.Play();
                    }
                }
            }
        }
        else
        {
            if (Type == AudioOptions.Mute)
            {
                AudioListener.volume = 0;
            }
            else
            {
                if (_BGM.isPlaying)
                {
                    _BGM.Pause();
                }


                if (Mscript.CurrentModel == null)
                {
                    if (_IntroVO.isPlaying)
                    {
                        _IntroVO.Pause();
                    }
                }
                else
                {
                    AudioSource AS = Mscript.CurrentModel.GetComponent<AudioSource>();
                    if (AS.isPlaying)
                    {
                        AS.Pause();
                    }
                }


            }
        }
    }


    public void OpenAudioPanel()
    {
        float MasterPanelX = 47;
        float PanelX = -110;
        AudioPanelCloseRoutine = StartCoroutine(CloseAudioPanel());

        Sequence SEQ = DOTween.Sequence();
        SEQ.Append(AudioPanelButton.GetComponent<RectTransform>().DOAnchorPosX(MasterPanelX, 0.5f))
                .Append(AudioToggle.transform.parent.GetComponent<RectTransform>().DOAnchorPosX(PanelX, 0.5f));
    }

    public void OpenDoodlePanel()
    {
        float MasterPanelX = 47;
        float PanelX = -110;
        DoodlePanelCloseRoutine = StartCoroutine(CloseDoodlePanel());

        Sequence SEQ = DOTween.Sequence();
        SEQ.Append(DoodlePanelButton.GetComponent<RectTransform>().DOAnchorPosX(MasterPanelX, 0.5f))
                .Append(DoodleToggle.transform.parent.GetComponent<RectTransform>().DOAnchorPosX(PanelX, 0.5f));
    }


    IEnumerator CloseAudioPanel()
    {
        float Delay = 3;
        yield return new WaitForSeconds(Delay);

        float MasterPanelX = -47;
        float PanelX = 110;
        Sequence SEQ = DOTween.Sequence();
        SEQ.Append(AudioToggle.transform.parent.GetComponent<RectTransform>().DOAnchorPosX(PanelX, 0.5f))
        .Append(AudioPanelButton.GetComponent<RectTransform>().DOAnchorPosX(MasterPanelX, 0.5f));

        AudioPanelCloseRoutine = null;
    }

    IEnumerator CloseDoodlePanel()
    {
        float Delay = 3;
        yield return new WaitForSeconds(Delay);

        float MasterPanelX = -47;
        float PanelX = 110;
        Sequence SEQ = DOTween.Sequence();
        SEQ.Append(DoodleToggle.transform.parent.GetComponent<RectTransform>().DOAnchorPosX(PanelX, 0.5f))
        .Append(DoodlePanelButton.GetComponent<RectTransform>().DOAnchorPosX(MasterPanelX, 0.5f));

        DoodlePanelCloseRoutine = null;
    }

    void SetText()
    {
        //Dpending on the Audio Stopping type, Set UI Text
        TextMeshProUGUI UIText = AudioToggle.GetComponentInChildren<TextMeshProUGUI>();
        Image MasterIcon = AudioPanelButton.GetComponent<Image>();
        if (Type == AudioOptions.Mute)
        {
            UIText.text = "Mute";
            MasterIcon.sprite = _AudioIconSprites.MuteIcon;
        }
        else
        {
            UIText.text = "Pause";
            MasterIcon.sprite = _AudioIconSprites.PauseIcon;

        }
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void CloseApplication()
    {
        Application.Quit();
        Debug.Log("Applicaton Closed");
    }
}

