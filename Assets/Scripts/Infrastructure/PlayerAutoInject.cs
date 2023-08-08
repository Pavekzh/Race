using UnityEngine;
using Fusion;

class PlayerAutoInject:NetworkBehaviour
{
    [SerializeField] private Player player;

    public override void Spawned()
    {
        GameBootstrap.Instance.InitPlayer(player);
    }
}