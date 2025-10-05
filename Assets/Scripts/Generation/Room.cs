using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Tooltip("1: Right, 2: Left, 3: Up, 4: Down")]
    public GameObject[] doors;

    public Transform[] chestLocations;

    // Start is called before the first frame update
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
