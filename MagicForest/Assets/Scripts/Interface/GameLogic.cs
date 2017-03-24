using UnityEngine;
using System.Collections;
using MagicForestClient;

public class GameLogic : MonoBehaviour {

    public float maxAcceleration = 4.2f;
    public float maxSpeed = 5.0f;
    public float resistance = 0.0f;

	// Use this for initialization
	void Start ()
    {
        GameObject playerSelf = LogicInterface.Instance.PlayerSelf;
    }
	
	// Update is called once per frame
	void Update ()
    {
        UpdateParameters();
        EventDispatcher.Instance.GameLogicTick();
	}

    private void UpdateParameters()
    {
        LogicInterface.s_LogicParameters.fMaxAcceleration = maxAcceleration;
        LogicInterface.s_LogicParameters.fMaxSpeed = maxSpeed;
        LogicInterface.s_LogicParameters.fResistance = resistance;
    }
}
