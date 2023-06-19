using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MasterScript : MonoBehaviour
{
    public static MasterScript instance;

    public string OTP;
    public string Grade;
    public List<string> AvailableFilesNames;
    [HideInInspector] public List<string> AvailableFilesIds;
    public List<TextureGrabber> ModelQueue;
    public Transform CurrentModel;

    [Space(15)]
    public UIMaster UIScript;

    public bool CanSelect;
    public TargetScript TargetScript;

    private void Awake()
    {
        instance = this;
        CanSelect = true;
        Grade = PlayerPrefs.GetString("Grade");
        //OTP = OTPCarrier.Instance.OTP;
    }
    void Start()
    {
        if (OTP != string.Empty)
        {
            //StartCoroutine(CheckForFiles());
            UIScript.SelectionPanel.gameObject.SetActive(false);
        }
        else
        {
            UIScript.DoodlePanelButton.gameObject.SetActive(false);
        }
        

        StartCoroutine(QueueControl());

        UIScript = FindObjectOfType<Canvas>().GetComponent<UIMaster>();
        UIScript.SetUIControl(false);

    }


    IEnumerator Startup()
    {
        while (OTP == string.Empty)
        {
            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(QueueControl());
        //StartCoroutine(CheckForFiles());

    }

    public void AddToQueue(TextureGrabber TS)
    {
        ModelQueue.Add(TS);
    }



    public void GetOTP(string _OTP)
    {
        OTP = _OTP;
    }

    public void GetGrade(string grade)
    {
        Grade = grade;
    }
/*    IEnumerator CheckForFiles()
    {
        while (true)
        {

            PNMaster.pubNubOBJ.ListFiles().Channel(OTP).Async((result, status) =>
            {
                AvailableFilesIds.Clear();
                AvailableFilesNames.Clear();
                foreach (PNFileInfo pnFileInfo in result.Data)
                {
                    {
                        if (!AvailableFilesNames.Contains(pnFileInfo.Name))
                        {
                            AvailableFilesNames.Add(pnFileInfo.Name);
                            AvailableFilesIds.Add(pnFileInfo.ID);
                            //PNMaster.StatusUI.text += pnFileInfo.Name + " ";
                        }

                    }

                }
            });
            yield return new WaitForSeconds(1);
        }
    }*/

    IEnumerator QueueControl()
    {
        TextureGrabber ActiveModel;
        while (true)
        {
            if (ModelQueue.Count != 0)
            {

                ActiveModel = ModelQueue[0];
                UIScript.SelectionPanelToggle.isOn = false;
                yield return new WaitForSeconds(ActiveModel.TargetStayTime);
                StartCoroutine(ActiveModel.Activate());
                
                ActiveModel = null;
            }
            yield return null;
        }

    }



    public void SelectOffline(TargetScript TS)
    {
        if (CurrentModel == TS.transform || !CanSelect) return;
        StartCoroutine(TS.OnActivateOffline());
        UIScript.SelectionPanelToggle.isOn = false;
    }

    public void BackToMenu()
    {
        UIScript.SelectionPanelToggle.isOn = false;
        SceneManager.LoadScene("MainMenu");
    }



    // Update is called once per frame
    void Update()
    {

    }
}
