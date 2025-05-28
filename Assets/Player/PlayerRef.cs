// PlayerRef.cs
using UnityEngine;

public class PlayerRef : MonoBehaviour
{
    public static GameObject Instance;

    void Awake()
    {
        Instance = this.gameObject;
    }
}
