using Cinemachine;
using System.Collections;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    public Material OriginalMAT;
    public Material DoodleMAT;
    public bool HideOnStartup;
    [Space(10)]
    public bool _ChangeColor;

    [HideInInspector] public Renderer _renderer;
    AudioSource _audioSource;
    MasterScript Mscript;
    TextureGrabber _grabber;
    LetterScript Lscript;

    public enum SceneType
    {
        Phonetics, Anatomy, SolarSystem
    }

    public SceneType _SceneType;

    void Start()
    {
        Mscript = MasterScript.instance;
        _renderer = GetComponentInChildren<Renderer>();
        _audioSource = GetComponent<AudioSource>();
        if (HideOnStartup)
        {
            _renderer.enabled = false;
            if (TryGetComponent(out AdditionalRenderers AR))
            {
                AR.ToggleMeshVisibility(false);
            }
        }


        if(_ChangeColor)
        {
            
            
            if (TryGetComponent(out AdditionalRenderers AdditionalREN))
            {
                DoodleMAT = AdditionalREN.ColorMeshes[0].material;
                AdditionalREN.ToggleMaterials(OriginalMAT);
                
                Debug.Log(gameObject.name+" "+DoodleMAT.name);
            }else
            {
                DoodleMAT = _renderer.material;
            }
            _renderer.material = OriginalMAT;
        }



        _grabber = GetComponent<TextureGrabber>();
        if (TryGetComponent(out LetterScript _LetterScript))
        {
            Lscript = _LetterScript;
        }

        if (_SceneType == SceneType.SolarSystem)
        {
            _renderer.material = OriginalMAT;
        }
    }




    public IEnumerator OnActivate()
    {
        //Set doodlemat to all renderers
        _renderer.enabled = true;
        Mscript.UIScript.SetUIControl(true);


        if (OriginalMAT && _ChangeColor) _renderer.material = DoodleMAT;

        if (TryGetComponent(out AdditionalRenderers AR))
        {
             AR.ToggleMeshVisibility(true);
            if (OriginalMAT && _ChangeColor)AR.ToggleMaterials(DoodleMAT);
        }

        if (Mscript.CurrentModel != null)
        {
            if (Mscript.CurrentModel != transform)
            {
                StartCoroutine(Mscript.CurrentModel.GetComponent<TargetScript>().OnDeactivate());
            }
        }

        //Set current model in MasterScript
        Mscript.CurrentModel = transform;
        Mscript.ModelQueue.Remove(_grabber);

        if (TryGetComponent(out IntermediateCameraScript ICS))
        {
            StartCoroutine(ICS.GoToIntermediate());
            yield return new WaitForSeconds(CameraIndexer.Instance.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Time + 0.75f);
        }

        switch (_SceneType)
        {
            case SceneType.Phonetics:

                StartCoroutine(CameraIndexer.Instance.ChangeCamera(_grabber.TargetCam));
                yield return new WaitForSeconds(CameraIndexer.Instance.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Time);

                //Set and play VO Language.
                if (TryGetComponent(out LanguageSetter LS))
                {
                    LS.SetAudio();
                }

                //If Muted/Paused,Don't play audio.
                if (Mscript.UIScript.AudioToggle.isOn) _audioSource.Play();


                if (Lscript != null)
                {
                    yield return new WaitForSeconds(0.25f);
                    Lscript.DisplayUI();
                }
                //Play Animation,If any
                if (TryGetComponent(out Animator Anim))
                {
                    Anim.enabled = true;
                    Anim.SetBool("Action", true);
                    Anim.SetBool("IsActive", true);
                    Anim.SetBool("IsPlaying", true);

                    yield return new WaitForSeconds(0.1f);

                    Anim.SetBool("Action", false);
                }

                //Reset and Show Color Toggle UI
                if (_ChangeColor)
                {
                    Mscript.UIScript.DoodlePanelButton.transform.localScale = Vector3.one;
                    Mscript.UIScript.DoodleToggle.transform.parent.GetComponent<RectTransform>().localScale = Vector3.one;
                    Mscript.UIScript.DoodleToggle.SetIsOnWithoutNotify(true);
                }
                break;

            default:
                break;
        }
        yield return new WaitForSeconds(CameraIndexer.Instance.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Time);
    }

    public IEnumerator OnDeactivate()
    {

        if (Lscript != null)
        {
            //Lscript.ShowLetter(false);
            Lscript.HideUI();
        }


        switch (_SceneType)
        {
            case SceneType.Phonetics:
                _audioSource.Stop();
                yield return new WaitForSeconds(1.5f);
                _renderer.enabled = false;
                if (TryGetComponent(out AdditionalRenderers AR))
                {
                    AR.ToggleMeshVisibility(false);
                }


                if (TryGetComponent(out Animator Anim))
                {
                    Anim.SetBool("Action", false);
                    Anim.SetBool("IsPlaying", false);
                    Anim.SetBool("IsActive", false);
                    Anim.enabled = true;
                }

                if (_ChangeColor)
                {
                    Mscript.UIScript.DoodlePanelButton.transform.localScale = Vector3.zero;
                    Mscript.UIScript.DoodleToggle.transform.parent.GetComponent<RectTransform>().localScale = Vector3.zero;
                }

                break;

            default:
                break;
        }

    }

    public IEnumerator OnActivateOffline()
    {
        //Set Original to all renderers
        Mscript.CanSelect = false;
        Mscript.UIScript.SetUIControl(true);

        _renderer.enabled = true;
            if (OriginalMAT) _renderer.material = OriginalMAT;

            if (TryGetComponent(out AdditionalRenderers AR))
            {
                AR.ToggleMeshVisibility(true);
                if (OriginalMAT) AR.ToggleMaterials(OriginalMAT);
            }



        if (Mscript.CurrentModel != null)
        {
            if (Mscript.CurrentModel != transform)
            {
                StartCoroutine(Mscript.CurrentModel.GetComponent<TargetScript>().OnDeactivate());
            }

        }

        //Hide Doodle UI
        Mscript.UIScript.DoodlePanelButton.transform.localScale = Vector3.zero;
        Mscript.UIScript.DoodleToggle.transform.parent.GetComponent<RectTransform>().localScale = Vector3.zero;

        //Set current model in MasterScript
        Mscript.CurrentModel = transform;
        Mscript.ModelQueue.Remove(_grabber);

        if (TryGetComponent(out IntermediateCameraScript ICS))
        {
            StartCoroutine(ICS.GoToIntermediate());
            yield return new WaitForSeconds(CameraIndexer.Instance.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Time + 0.75f);
        }

        switch (_SceneType)
        {
            case SceneType.Phonetics:

                StartCoroutine(CameraIndexer.Instance.ChangeCamera(_grabber.TargetCam));
                yield return new WaitForSeconds(CameraIndexer.Instance.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Time);

                //Set and play VO Language.
                if (TryGetComponent(out LanguageSetter LS))
                {
                    LS.SetAudio();
                }

                //If Muted/Paused,Don't play audio.
                if (Mscript.UIScript.AudioToggle.isOn) _audioSource.Play();


                if (Lscript != null)
                {
                    yield return new WaitForSeconds(0.25f);
                    Lscript.DisplayUI();
                }
                //Play Animation,If any
                if (TryGetComponent(out Animator Anim))
                {
                    Anim.enabled = true;
                    Anim.SetBool("Action", true);
                    Anim.SetBool("IsActive", true);
                    Anim.SetBool("IsPlaying", true);

                    yield return new WaitForSeconds(0.1f);

                    Anim.SetBool("Action", false);
                }

                break;

            default:
                break;
        }
        yield return new WaitForSeconds(CameraIndexer.Instance.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Time);
        Mscript.CanSelect = true;

    }

    public void PlayAudio()
    {
        if (_audioSource != null)
        {
            _audioSource.Play();
        }
    }

    public void StopAudio()
    {
        if (_audioSource != null)
        {
            _audioSource.Stop();
        }
    }

    public void TextureToggle(bool _IsDoodle)
    {
        if (_IsDoodle)
        {
            if (TryGetComponent(out AdditionalRenderers AR))
            {
                AR.ToggleMaterials(DoodleMAT);

            }
            else
            {
                _renderer.material = DoodleMAT;
            }
        }
        else
        {
            if (TryGetComponent(out AdditionalRenderers AR))
            {
                AR.ToggleMaterials(OriginalMAT);
            }
            else
            {
                _renderer.material = OriginalMAT;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
