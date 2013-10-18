namespace DirectX

    open SharpDX
    open SharpDX.DXGI
    open SharpDX.Direct3D
    open SharpDX.Direct3D11

    open Windowing
    open Shaders
    open Vertices

    module Context =
        let constantBuffer = new Buffer(device, Utilities.SizeOf<Matrix>(), ResourceUsage.Default, BindFlags.ConstantBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0)

        let context = device.ImmediateContext
        context.InputAssembler.InputLayout <- layout
        context.InputAssembler.PrimitiveTopology <- PrimitiveTopology.TriangleList
        context.InputAssembler.SetVertexBuffers(0, VertexBufferBinding(vertices, Utilities.SizeOf<Vector4>() * 2, 0))
        context.VertexShader.SetConstantBuffer(0, constantBuffer)
        context.VertexShader.Set(vertexShader)
        context.PixelShader.Set(pixelShader)

        let mutable backBuffer : Texture2D = null
        let mutable renderView : RenderTargetView = null
        let mutable depthBuffer : Texture2D = null
        let mutable depthView : DepthStencilView = null
        
        let resize () = 
            Utilities.Dispose(&backBuffer)
            Utilities.Dispose(&renderView)
            Utilities.Dispose(&depthBuffer)
            Utilities.Dispose(&depthView)

            swapChain.ResizeBuffers(swapChainDescription.BufferCount, form.ClientSize.Width, form.ClientSize.Height, Format.Unknown, SwapChainFlags.None)

            backBuffer <- Texture2D.FromSwapChain<Texture2D>(swapChain, 0)
            renderView <- new RenderTargetView(device, backBuffer)

            let depthBufferDescription =
                new Texture2DDescription(
                    Format = Format.D32_Float_S8X24_UInt,
                    ArraySize = 1,
                    MipLevels = 1,
                    Width = form.ClientSize.Width,
                    Height = form.ClientSize.Height,
                    SampleDescription = SampleDescription(1, 0),
                    Usage = ResourceUsage.Default,
                    BindFlags = BindFlags.DepthStencil,
                    CpuAccessFlags = CpuAccessFlags.None,
                    OptionFlags = ResourceOptionFlags.None
                )
            depthBuffer <- new Texture2D(device, depthBufferDescription)
            depthView <- new DepthStencilView(device, depthBuffer)

            context.Rasterizer.SetViewport(new ViewportF(0.0f, 0.0f, form.ClientSize.Width |> float32, form.ClientSize.Height |> float32, 0.0f, 1.0f))
            context.OutputMerger.SetTargets(depthView, renderView)

        let redraw () =
            context.ClearDepthStencilView(depthView, DepthStencilClearFlags.Depth, 1.0f, 0uy)
            context.ClearRenderTargetView(renderView, Color4.Black)
            context.Draw(36,0)
            swapChain.Present(0, PresentFlags.None)