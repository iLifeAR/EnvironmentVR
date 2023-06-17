using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraIndexer : MonoBehaviour
{
    [System.Serializable]
    public struct Cameras
    {
        public CinemachineVirtualCamera IdleCam;
        public List<CinemachineVirtualCamera> OtherCams;
    }
    public Cameras LevelCameras;
    public static CameraIndexer Instance;
    public float DollySpeed;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        ChangeCamera(LevelCameras.IdleCam);
    }

    public IEnumerator ChangeCamera(CinemachineVirtualCamera Cam)
    {
        if (Cam == LevelCameras.IdleCam)
        {
            LevelCameras.IdleCam.Priority = 11;
            Debug.Log("IsIdle");
        }
        else
        {
            LevelCameras.IdleCam.Priority = 10;
            if (LevelCameras.IdleCam.TryGetComponent(out AudioSource AS))
            {
                AS.Stop();
            }
        }
        foreach (CinemachineVirtualCamera item in LevelCameras.OtherCams)
        {
            if (item == Cam)
            {
                item.Priority = 11;
            }
            else
            {
                item.Priority = 10;
            }
        }

        yield return new WaitForSeconds(GetComponent<CinemachineBrain>().m_DefaultBlend.m_Time);


        if (Cam.TryGetComponent(out CinemachineDollyCart Cart))
        {
            Cart.m_Speed = DollySpeed;

        }
        else if (Cam.TryGetComponent(out Animator Anim))
        {
            Anim.SetBool("IsActive", true);
            yield return new WaitForSeconds(0.1f);
            Anim.SetBool("IsActive", false);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
