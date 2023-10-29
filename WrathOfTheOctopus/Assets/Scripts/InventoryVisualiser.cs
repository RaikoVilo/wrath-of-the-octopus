using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryVisualiser : MonoBehaviour
{
    private bool active = true;
    public GameObject inventory;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("pressed");
            if (!active) active = true;
            else active = false;
        }

        if (active) inventory.SetActive(true);
        else inventory.SetActive(false);
    }
}
