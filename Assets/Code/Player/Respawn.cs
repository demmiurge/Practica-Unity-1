using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public GameObject spawnPoint;
    public GameObject daddy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider l_Collider)
    {
        if (l_Collider.tag == "DeadZone")
        {
            daddy.transform.localPosition = spawnPoint.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
