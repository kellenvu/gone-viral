using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallGroup : MonoBehaviour
{
    public static BallGroup instance;

    public bool shouldPlayMarble = false;
    public bool shouldPlaySquish = false;

    private void Awake() {
        instance = this;
    }

    public float GetTotalRollingVolume() {
        var players = GetComponentsInChildren<BallController>();
        float totalVolume = 0f;
        foreach(BallController player in players) {
            totalVolume += player.GetRollingVolume();
        }
        return totalVolume;
    }

}
