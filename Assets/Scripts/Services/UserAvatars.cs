using UnityEngine;

public class UserAvatars : MonoBehaviour
{
    [SerializeField] Sprite[] avatars;

    public Sprite GetAvatar(int indexer)
    {
        return avatars[indexer];
    }
}