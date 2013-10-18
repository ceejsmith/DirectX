namespace DirectX

    open System
    open System.Windows.Forms
    open System.Drawing
    open System.Diagnostics

    open SharpDX.Windows
    open SharpDX.Direct3D11
    open SharpDX.Direct3D
    open SharpDX.DXGI
    open SharpDX.D3DCompiler
    open SharpDX

    open Windowing
    open Context
    open Projection

    module Demo =
        let run () =
            let clock = new Stopwatch()
            clock.Start()

            resize()

            RenderLoop.Run(form, fun () ->
                updateWorldView clock
                redraw()
            )

            // The process is torn down after this, but it would be good practice to dispose all the wrappers around DirectX unmanaged structs
            ()