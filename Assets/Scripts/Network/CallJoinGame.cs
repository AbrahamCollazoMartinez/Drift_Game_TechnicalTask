using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallJoinGame : MonoBehaviour
{
    public void JoinGame()
    {
    	NetworkManager.Instance.JoinOrCreateRoom();
    }
}
