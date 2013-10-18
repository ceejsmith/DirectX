namespace DirectX

    open System
    open System.Windows.Forms

    open SharpDX.Windows    
    open SharpDX.DXGI
    open SharpDX.Direct3D
    open SharpDX.Direct3D11

    module Windowing =
        let form = new RenderForm("MiniCube")
        form.KeyUp.Add(fun e -> if e.KeyCode = Keys.Escape then form.Close())
        
        let modeDescription = ModeDescription(form.ClientSize.Width, form.ClientSize.Height, Rational(60, 1), Format.R8G8B8A8_UNorm)

        let swapChainDescription =
            new SwapChainDescription(
                BufferCount = 1,
                ModeDescription = modeDescription,
                IsWindowed = SharpDX.Bool(true),
                OutputHandle = form.Handle,
                SampleDescription = SampleDescription(1, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput
            )

        let mutable deviceOut : SharpDX.Direct3D11.Device = null
        let mutable swapChainOut : SwapChain = null
        Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.None, swapChainDescription, &deviceOut, &swapChainOut)
        // Once we have a device and a swap chain, we remove mutability, since we cannot capture mutable values in a closure
        let device = deviceOut
        let swapChain = swapChainOut

        let factory = swapChain.GetParent<Factory>()
        factory.MakeWindowAssociation(form.Handle, WindowAssociationFlags.IgnoreAll)