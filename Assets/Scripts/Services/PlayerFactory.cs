using System.Collections;
using UnityEngine;
using Fusion;

public class PlayerFactory:NetworkBehaviour
{
    [SerializeField] private Player[] playerPrefabs;
    [SerializeField] private Vector3 spawnPosition;
    [SerializeField] private int firstLane = 1;
    [SerializeField] private int secondLane = 2;

    private int localPlayerCar = -1;
    private int clientCar = -1;

    private NetworkRunner network;
    private WorldGenerator worldGenerator;

    public void Init(NetworkRunner network,WorldGenerator worldGenerator,FirebaseDatabaseService databaseService)
    {
        this.network = network;
        this.worldGenerator = worldGenerator;

        databaseService.GetSelectedCar(car => localPlayerCar = car);
    }

    public void CreatePlayerCar(bool isPlayerHost, PlayerRef player)
    {
        if (network.IsServer)
            StartCoroutine(InstantiateCar(isPlayerHost, player));
    }

    public override void Spawned()
    {
        if(network.IsServer == false)
            RPC_SendSelectedCarToServer(localPlayerCar);
    }


    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private void RPC_SendSelectedCarToServer(int selectedCar)
    {
        clientCar = selectedCar;
    }

    private IEnumerator InstantiateCar(bool isPlayerHost,PlayerRef player)
    {
        if (isPlayerHost)
        {
            yield return new WaitUntil(() => localPlayerCar != -1);

            Player car = playerPrefabs[localPlayerCar];
            float xPos = worldGenerator.LanesXs[firstLane];
            spawnPosition = new Vector3(xPos, spawnPosition.y, spawnPosition.z);
            network.Spawn(car.gameObject, spawnPosition, inputAuthority: player);
        }
        else
        {
            yield return new WaitUntil(() => clientCar != -1);

            Player car = playerPrefabs[clientCar];
            float xPos = worldGenerator.LanesXs[secondLane];
            spawnPosition = new Vector3(xPos, spawnPosition.y, spawnPosition.z);
            network.Spawn(car.gameObject, spawnPosition, inputAuthority: player);
        }
    }

}