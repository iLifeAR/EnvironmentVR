using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SelectionMenu : MonoBehaviour
{
    public RectTransform Scroll;
    public Canvas _Canvas;
    public float ScrollMultiplier;
    public TargetScript[] ContentItems;
    public int ScrollIndex = 0;
    public float ScrollTime;
    public Vector2 ScrollBounds;
    AudioSource _AS;

    [Header("Audio")]
    public AudioClip SlideAudio;
    public AudioClip SelectAudio;

    Coroutine _IsScrolling;
    public InputActionReference _Actions;
    public Vector2 JoystickInput;

    void Start()
    {
        _AS=GetComponent<AudioSource>();
    }

    IEnumerator ScrollRoutine(int DIR)
    {
        float elapsedTime = 0;
        float StartScrollValue=Scroll.anchoredPosition.x;
        float EndScrollValue=StartScrollValue-(ScrollMultiplier*DIR);

        //Check Bounds
        if (EndScrollValue < ScrollBounds.x)
        {
            EndScrollValue = ScrollBounds.x;
        }
        else if (EndScrollValue > ScrollBounds.y)
        {
            EndScrollValue = ScrollBounds.y;
        }
        else
        {
            PlayAudio(SlideAudio);
            //If In bounds set index
            ScrollIndex += (1 * DIR);
        }

        _AS.Play();
        while (elapsedTime<=ScrollTime) 
        {
            elapsedTime += Time.deltaTime;
            float percentageCompleted = elapsedTime / ScrollTime;
            Vector2 Scrollrect = Scroll.anchoredPosition;
            Scrollrect.x = Mathf.Lerp(StartScrollValue,EndScrollValue, percentageCompleted);
            Scroll.anchoredPosition = Scrollrect;

            yield return new WaitForEndOfFrame();
        }
        Scroll.anchoredPosition =new Vector2(EndScrollValue,Scroll.anchoredPosition.y);
        
        _IsScrolling = null;
    }

    void PlayAudio(AudioClip A)
    {
        if (_AS.isPlaying)
        {
            _AS.Stop();
        }
        _AS.clip= A;
        _AS.Play();
    }

    public void SelectItem(int Index)
    {
        StartCoroutine(MasterScript.instance.Items[Index].OnActivateOffline());
    }

    // Update is called once per frame
    void Update()
    {
        JoystickInput = _Actions.action.ReadValue<Vector2>();

        if ((Input.GetKeyDown("f") || JoystickInput.x >= 0.9f) && _IsScrolling==null)
        {
            _IsScrolling = StartCoroutine(ScrollRoutine(1));
        }else if ((Input.GetKeyDown("d") || JoystickInput.x <= -0.9f) && _IsScrolling == null)
        {
            _IsScrolling = StartCoroutine(ScrollRoutine(-1));
        }
    }
}
