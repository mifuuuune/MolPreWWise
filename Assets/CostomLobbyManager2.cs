using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CostomLobbyManager2 : NetworkLobbyManager
{
    public GameObject contpl;
    private CountPLayer script;
    public GameObject playerLobby1;
    public GameObject playerLobby2;
    public GameObject playerLobby3;
    public GameObject playerLobby4;
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;
    private GameObject pg;
    private int player_num = 0;
    private int test;
    private int spawn_player = 0;

    public void Start()
    {
        script = (CountPLayer)contpl.GetComponent(typeof(CountPLayer));
    }
    public override void OnLobbyServerConnect(NetworkConnection conn)
    {

        player_num++;
        script.addplayer();
        SpawnPG();
    }

   private void SpawnPG()
    {
        if (player_num == 1)
        {
            pg = GameObject.Instantiate<GameObject>(this.playerLobby1, new Vector3(-2.141f, 0.02f, -1.639f), Quaternion.Euler(0, 180f, 0));
            Debug.Log("sono il server con il risultato->"+script.players);
            //NetworkServer.Spawn(pg);
        }
        else if (player_num == 2)
        {
            pg = GameObject.Instantiate<GameObject>(this.playerLobby2, new Vector3(-0.548f, 0.02f, -1.639f), Quaternion.Euler(0, 180f, 0));
            //NetworkServer.Spawn(pg);
        }
        else if (player_num == 3)
        {
            pg = GameObject.Instantiate<GameObject>(this.playerLobby3, new Vector3(0.921f, 0.02f, -1.639f), Quaternion.Euler(0, 180f, 0));
           // NetworkServer.Spawn(pg);
        }
        else if (player_num == 4)
        {
            pg = GameObject.Instantiate<GameObject>(this.playerLobby4, new Vector3(2.365f, 0.02f, -1.639f), Quaternion.Euler(0, 180f, 0));
            //NetworkServer.Spawn(pg);
            player_num = 0;

        }

    }
  
    public override void OnLobbyClientEnter()
    {
        Debug.Log("sto ottenendo l'oggetot in client method");
  
        Debug.Log("ottenuto l'oggetto");
        int num = script.players;
        Debug.Log("sono il client con risultato->"+ num);
        if (player_num == 0)
        {
            int x = NetworkClient.allClients.Count;
            //Debug.Log("client=" + x);
            for (int i = 0; i < x; i++)
            {
                if (i == 0)
                {
                    pg = GameObject.Instantiate<GameObject>(this.playerLobby2, new Vector3(2.365f, 0.02f, -1.639f), Quaternion.Euler(0, 180f, 0));

                }
                else if (i == 1)
                {
                    pg = GameObject.Instantiate<GameObject>(this.playerLobby3, new Vector3(-0.548f, 0.02f, -1.639f), Quaternion.Euler(0, 180f, 0));

                }
                else if (i == 2)
                {
                    pg = GameObject.Instantiate<GameObject>(this.playerLobby4, new Vector3(0.921f, 0.02f, -1.639f), Quaternion.Euler(0, 180f, 0));
                }

            }
        }
        base.OnLobbyClientEnter();
    }
    public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId)
    {

        GameObject myPlayer = null;
        spawn_player++;
        //GameObject spawnpos = GameObject.FindGameObjectWithTag("spawnpos");
        if (spawn_player == 1)
            myPlayer = Instantiate(player1, GameObject.Find("sir_bean_spwan").transform.position, Quaternion.identity) as GameObject;
        if (spawn_player == 2)
            myPlayer = Instantiate(player2, GameObject.Find("sir_eal_spawn").transform.position, Quaternion.identity) as GameObject;
        if (spawn_player == 3)
            myPlayer = Instantiate(player3, GameObject.Find("sir_loin_spawn").transform.position, Quaternion.identity) as GameObject;
        if (spawn_player == 4)
        {
            spawn_player = 0;
            myPlayer = Instantiate(player4, GameObject.Find("sir_sage_spawn").transform.position, Quaternion.identity) as GameObject;
        }
        return myPlayer;
    }
}
