using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class IntermediateCameraScript : MonoBehaviour
{
    public CinemachineVirtualCamera _IntermediateCamera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public async Task Int()
    {
        StartCoroutine(CameraIndexer.Instance.ChangeCamera(_IntermediateCamera));
        int BlendTime = Mathf.RoundToInt(Camera.main.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Time*1000);
        await Task.Delay(BlendTime+750);
        StartCoroutine(CameraIndexer.Instance.ChangeCamera(GetComponent<TextureGrabber>().TargetCam));
        await Task.Delay(BlendTime);

    }

    public IEnumerator GoToIntermediate()
    {
        StartCoroutine(CameraIndexer.Instance.ChangeCamera(_IntermediateCamera));
        float BlendTime = Camera.main.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Time + Time.time;
        BlendTime += 0.75f;
        while (Time.time < BlendTime)
        {
            yield return null;
        }
        StartCoroutine(CameraIndexer.Instance.ChangeCamera(GetComponent<TextureGrabber>().TargetCam));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
