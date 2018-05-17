using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CountPLayer : NetworkBehaviour {

    [SyncVar(hook = "Printtest")]
    public int players=1;

    public int totplayers;

	// Use this for initialization
	
	
	public void addplayer()
    {
        players++;
    }

    public int Getnumplayers()
    {
        return players;
    }

    public void reset_players()
    {
        players = 1;
    }
    public void Printtest(int newplayers)
    {
        totplayers = newplayers;
        Debug.Log("sono in test -> "+ players);
    }
}
