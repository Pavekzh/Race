using UnityEngine;

public class PlayerFactory:MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    public void CreatePlayer(Fusion.NetworkRunner network)
    {
        network.Spawn(playerPrefab);
    }
}