using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    protected override void Awake()
    {
        base.Awake();
    }

    public void TestGameManager()
    {
        Debug.Log("Game Manager Instance is working!");
    }
}
