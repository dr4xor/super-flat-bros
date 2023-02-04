using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODManager : Singleton<FMODManager>
{
    private FMOD.Studio.EventInstance _bgm;
    private FMOD.Studio.EventInstance _sword;

    void Start()
    {
        _bgm = FMODUnity.RuntimeManager.CreateInstance("event:/BGM");
        _sword = FMODUnity.RuntimeManager.CreateInstance("event:/Sword");
        StartBGM();
    }

    void Update()
    {
        
    }

    public void StartBGM()
    {
        _bgm.start();
    }

    public void StopBGM()
    {
        _bgm.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    public void PlaySwordSound()
    {
        _sword.start();
    }

}
