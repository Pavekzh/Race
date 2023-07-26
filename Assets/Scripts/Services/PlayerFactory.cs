using UnityEngine;

public class PlayerFactory:MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Vector3 spawnPosition;

    public void CreatePlayer(Fusion.NetworkRunner network)
    {
        network.Spawn(playerPrefab, spawnPosition);
    }
}