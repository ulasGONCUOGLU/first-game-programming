﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kamerKontrol : MonoBehaviour
{
    private Vector3 offset;
    public GameObject player;
    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
