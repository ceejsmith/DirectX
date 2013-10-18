namespace DirectX

    open System
    open System.Diagnostics

    open SharpDX
    
    open Windowing
    open Context

    module Projection =
        let view = Matrix.LookAtLH(Vector3(0.0f, 0.0f, -5.0f), Vector3(0.0f, 0.0f, 0.0f), Vector3.UnitY)

        let updateWorldView (clock : Stopwatch) =

            let elapsed = clock.ElapsedMilliseconds |> float32;
            let time = elapsed / 1000.0f

            let proj = Matrix.PerspectiveFovLH((Math.PI |> float32) / 4.0f, (form.ClientSize.Width |> float32) / (form.ClientSize.Height |> float32), 0.1f, 100.0f)
            let viewProj = Matrix.Multiply(view, proj)
            let mutable worldViewProj = Matrix.RotationX(time) * Matrix.RotationY(time * 2.0f) * Matrix.RotationZ(time * 0.7f) * viewProj;
            worldViewProj.Transpose();
            context.UpdateSubresource(&worldViewProj, constantBuffer);
            ()