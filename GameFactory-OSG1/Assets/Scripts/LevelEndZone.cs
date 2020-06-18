﻿using UnityEngine;


public class LevelEndZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BallController>()!=null)
        {
            FindObjectOfType<LevelManager>().NextLevel();
        }
    }
    
}
