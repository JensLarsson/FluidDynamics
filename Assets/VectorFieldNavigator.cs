﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorFieldNavigator : MonoBehaviour
{

    [SerializeField] float velocityResistance = 0.1f;

    Vector2 position = new Vector2();
    Vector2 velocity = new Vector2();
    float height, width;
    float arrayScale;
    private void Start()
    {
        Vector2 topRightCorner = new Vector2(1, 1);
        Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);
        height = edgeVector.y * 2;
        width = edgeVector.x * 2;
        arrayScale = NavierFluid.Instance.Size / width;
        position = transform.position;
    }
    void Update()
    {
        if (NavierFluid.Instance != null)
        {

            int x = Mathf.FloorToInt((position.x + width * 0.5f) * arrayScale);
            int y = Mathf.FloorToInt((position.y + width * 0.5f) * arrayScale);
            //Debug.Log($"{x}:{y}");
            if (x > 1 && x < NavierFluid.Instance.Size - 1
                && y > 1 && y < NavierFluid.Instance.Size - 1)
            {
                velocity += NavierFluid.Instance.VelocityField[x, y] * Time.deltaTime * velocityResistance;
            }
        }
        position += velocity * Time.deltaTime;
        transform.position = position;
    }
}

