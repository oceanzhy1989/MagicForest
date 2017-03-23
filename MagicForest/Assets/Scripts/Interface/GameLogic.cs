using UnityEngine;
using System.Collections;
using MagicForestClient;

public class GameLogic : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        GameObject playerSelf = LogicInterface.Instance.PlayerSelf;
    }
	
	// Update is called once per frame
	void Update ()
    {
        EventDispatcher.Instance.GameLogicTick();
	}
}
