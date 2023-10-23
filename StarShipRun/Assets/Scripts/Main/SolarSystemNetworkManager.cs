using Characters;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections.Generic;

namespace Main
{
    public class SolarSystemNetworkManager : NetworkManager
    {
        //[SerializeField] private string playerName;
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private GameObject _inputPanel;
        [SerializeField] private int count;
        Dictionary<int, ShipController> _players = new Dictionary<int, ShipController>();

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        {
            var spawnTransform = GetStartPosition();

            var player = Instantiate(playerPrefab, spawnTransform.position, spawnTransform.rotation);
            //player.GetComponent<ShipController>().PlayerName = playerName;
            _players.Add(conn.connectionId, player.GetComponent<ShipController>());

            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        }

        public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)  //попытка фикса бага
        {
            _players.Remove(conn.connectionId);
            Debug.Log("OnServerRemovePlayer");
            base.OnServerRemovePlayer(conn, player);            
        }

        public override void OnStartServer()
        {
            base.OnStartServer();

            NetworkServer.RegisterHandler(100, ReceiveName);            
        }

        public class MessageLogin : MessageBase
        {
            public string login;

            public override void Deserialize(NetworkReader reader)
            {
                login = reader.ReadString();
            }

            public override void Serialize(NetworkWriter writer)
            {
                writer.Write(login);
            }
        }

        public override void OnClientConnect(NetworkConnection conn)
        {
            base.OnClientConnect(conn);
            MessageLogin _login = new MessageLogin();
            _login.login = _inputField.text;
            conn.Send(100,_login);
            _inputPanel.SetActive(false);
        }

        public override void OnClientDisconnect(NetworkConnection conn)
        {
            _players.Remove(conn.connectionId);
            Debug.Log("OnClientDisconnect");
            base.OnClientDisconnect(conn);            
            _inputPanel.SetActive(true);
        }

        public override void OnStopHost()
        {
            base.OnStopHost();
            Debug.Log("OnStopHost");
            _inputPanel.SetActive(true);
        }

        public void ReceiveName(NetworkMessage networkMessage)
        {
            _players[networkMessage.conn.connectionId].PlayerName = networkMessage.reader.ReadString();
            _players[networkMessage.conn.connectionId].gameObject.name = _players[networkMessage.conn.connectionId].PlayerName;
            Debug.Log(_players[networkMessage.conn.connectionId]);
        }
    }
}
