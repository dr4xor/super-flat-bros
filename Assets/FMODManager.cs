using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODManager : Singleton<FMODManager>
{
    private FMOD.Studio.EventInstance _bgm;

    void Start()
    {
        _bgm = FMODUnity.RuntimeManager.CreateInstance("event:/BGM");
        StartBGM();
    }

    void Update()
    {
        
    }

    void StartBGM()
    {
        _bgm.start();
    }

    void StopBGM()
    {
        _bgm.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

}
