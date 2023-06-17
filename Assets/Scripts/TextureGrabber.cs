using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;


public class TextureGrabber : MonoBehaviour
{
    public CinemachineVirtualCamera TargetCam;
    public string TextureName;
    Renderer _Renderer;
    MasterScript Mscript;
    TargetScript Tscript;
    [HideInInspector] public bool IsActive;

    public float TargetStayTime;


    private void Awake()
    {
        if (TryGetComponent(out AdditionalRenderers AR))
        {
            _Renderer = AR.ColorMeshes[0];

        }
        else
        {
            _Renderer = GetComponentInChildren<Renderer>();
        }
        GetComponent<TargetScript>()._renderer = _Renderer;
    }

    // Start is called before the first frame update
    void Start()
    {
        Mscript = MasterScript.instance;
        Tscript = GetComponent<TargetScript>();
        //StartCoroutine(GrabTexture(0));
    }



/*    IEnumerator GrabTexture(float Delay)
    {

        yield return new WaitForSeconds(Delay);
        bool GotTexture = false;
        string OTP = Mscript.OTP;
        Debug.Log("Checking for TEX:" +gameObject.name);
        while (!GotTexture)
        {
            
            if (Mscript.AvailableFilesNames.Contains(TextureName))
            {
                GotTexture = true;
                int FileIndex = Mscript.AvailableFilesNames.IndexOf(TextureName);


                PNM.pubNubOBJ.GetFileURL().
               Channel(OTP).
               ID(Mscript.AvailableFilesIds[FileIndex]).
               Name(Mscript.AvailableFilesNames[FileIndex]).
               Async((result, status) =>
               {

                   if (!status.Error)
                   {
                       StartCoroutine(DownloadImage(result.URL));
                       Debug.Log("Downloading IMG");

                   }
                   else
                   {
                       Debug.LogError("Error getting URL of " + TextureName);
                   }
               });
            }

            yield return new WaitForEndOfFrame();

        }
    }*/

    

    IEnumerator DownloadImage(string MediaUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);

        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
            Debug.LogError(MediaUrl);
        }
        else
        {
            if (GetComponent<TargetScript>()._ChangeColor)
            {
                Tscript.DoodleMAT.mainTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            }


            Debug.Log("Found");
            AddToQueue();
            //DeleteSource();

        }
    }

    void AddToQueue()
    {
        Mscript.ModelQueue.Add(this);
    }

    public IEnumerator Activate()
    {
        IsActive = true;
        StartCoroutine(Tscript.OnActivate());


        yield return new WaitForSeconds(TargetStayTime);

        IsActive = false;
    }

/*    void DeleteSource()
    {
        int Index = Mscript.AvailableFilesNames.IndexOf(TextureName);
        string ID = Mscript.AvailableFilesIds[Index];
        PubnubMaster PNM = Mscript.PNMaster;
        //PNM.DeleteFile(ID, Mscript.OTP, TextureName);

        Mscript.PNMaster.pubNubOBJ.DeleteFile().
        Channel(Mscript.OTP).
        ID(ID).
        Name(TextureName).
        Async((result, status) =>
        {
            if (!status.Error)
            {
                Debug.Log("Deleted:" + TextureName);
                StartCoroutine(GrabTexture(1.5f));
            }
            else
            {
                Debug.Log(status.ErrorData.Info);
                
            }
        });

        
    }*/




    // Update is called once per frame
    void Update()
    {

    }
}
