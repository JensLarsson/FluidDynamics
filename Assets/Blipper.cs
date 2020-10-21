﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;


public class Blipper : MonoBehaviour
{
    public ComputeShader renderShader;
    private RenderTexture _target;

    ComputeBuffer intBuffer;
    ComputeBuffer quantityBuffer;
    ComputeBuffer velocityBuffer;

    SandSimulation simulation;

    NavierFluid fluid;
    int size;
    private void Start()
    {
        size = NavierFluid.Instance.Size;
        velocityBuffer = new ComputeBuffer(size * size, sizeof(float) * 2);
        quantityBuffer = new ComputeBuffer(size * size, sizeof(float));
        quantityBuffer.SetData(NavierFluid.Instance.Quantity);
        velocityBuffer.SetData(NavierFluid.Instance.VelocityField);
        renderShader.SetBuffer(0, "Quantity", quantityBuffer);
        renderShader.SetBuffer(0, "Velocity", velocityBuffer);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Render(destination);
    }

    private void Render(RenderTexture destination)
    {
        velocityBuffer.SetData(NavierFluid.Instance.VelocityField);
        quantityBuffer.SetData(NavierFluid.Instance.Quantity);


        // Make sure we have a current render target
        InitRenderTexture();
        // Set the target and dispatch the compute shader
        renderShader.SetTexture(0, "Result", _target);
        renderShader.SetInt("Width", size);
        renderShader.SetInt("Height", size);

        renderShader.Dispatch(0, size-1, size-1, 1);
        // Blit the result texture to the screen
        Graphics.Blit(_target, destination);
    }

    private void InitRenderTexture()
    {
        if (_target == null || _target.width != Screen.width || _target.height != Screen.height)
        {
            // Release render texture if we already have one
            if (_target != null)
                _target.Release();
            // Get a render target for Ray Tracing
            _target = new RenderTexture(Screen.width, Screen.height, 0,
                RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            _target.enableRandomWrite = true;
            _target.Create();
        }
    }
}
