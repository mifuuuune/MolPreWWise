using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Prototype.NetworkLobby
{
    public class CostomLobbyManager2 : NetworkLobbyManager
    {

        static public CostomLobbyManager2 s_Singleton;

        

        [Header("Playable Prefabs")]
        public GameObject player1;
        public GameObject player2;
        public GameObject player3;
        public GameObject player4;
        public CountPLayer x;
        private GameObject pg;
        private int player_num = 0;
        private int test = 0;
        private int spawn_player = 0;
        private int mole;

        public void Awake()
        {
            s_Singleton = this;
            mole = 1;//UnityEngine.Random.Range(1, 4);
        }

        public override void OnLobbyServerConnect(NetworkConnection conn)
        {
            player_num++;
            SpawnPG();
        }

        private void SpawnPG()
        {
            try {

                CountPLayer._instance.SpawnLobbyPlayer(player_num);

            } catch(Exception e) {

                Debug.Log(e);

            }

        }
        

        public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId)
        {
            
            GameObject myPlayer = null;
            spawn_player++;
            if (spawn_player == 3)
            {
                myPlayer = Instantiate(player1, GameObject.Find("sir_bean_spawn").transform.position, Quaternion.identity) as GameObject;
                if (mole == 3)
                {
                    myPlayer.GetComponent<SirBeanController>().SetMole(true);
                }
            }

            if (spawn_player == 2)
            {
                myPlayer = Instantiate(player2, GameObject.Find("sir_eal_spawn").transform.position, Quaternion.identity) as GameObject;
                if (mole == 2)
                {
                    myPlayer.GetComponent<SirEalController>().SetMole(true);
                }
            }
            if (spawn_player == 1)
            {
                myPlayer = Instantiate(player3, GameObject.Find("sir_loin_spawn").transform.position, Quaternion.identity) as GameObject;
                //Debug.Log("ora setto la mole" + mole);
                if (mole == 1)
                {
                    //Debug.Log("ora setto la mole");
                    myPlayer.GetComponent<SirLoinController>().SetMole(true);
                }
            }
            if (spawn_player == 4)
            {
                spawn_player = 0;
                myPlayer = Instantiate(player4, GameObject.Find("sir_sage_spawn").transform.position, Quaternion.identity) as GameObject;
                if (mole == 4)
                {
                    myPlayer.GetComponent<SirSageController>().SetMole(true);
                }
            }
            return myPlayer;
        }

        public override void OnLobbyStopHost()
        {
            //base.OnLobbyServerDisconnect(conn);
            player_num=0;
        }

        public override void OnLobbyServerDisconnect(NetworkConnection conn)
        {
            try { CountPLayer._instance.DespawnLobbyPlayer(player_num); } catch (Exception e) { Debug.Log(e); }
            player_num--;
        }

    }

    
}

