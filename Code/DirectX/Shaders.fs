namespace DirectX

    open SharpDX
    open SharpDX.D3DCompiler
    open SharpDX.Direct3D11
    open SharpDX.DXGI

    open Windowing

    module Shaders =
        let vertexShaderByteCode = ShaderBytecode.CompileFromFile("MiniCube.fx", "VS", "vs_4_0")
        let vertexShader = new VertexShader(device, vertexShaderByteCode.Bytecode.Data)

        let pixelShaderByteCode = ShaderBytecode.CompileFromFile("MiniCube.fx", "PS", "ps_4_0")
        let pixelShader = new PixelShader(device, pixelShaderByteCode.Bytecode.Data)

        let signature = ShaderSignature.GetInputSignature(vertexShaderByteCode.Bytecode.Data)
        let inputElements = [| InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0, 0); InputElement("COLOR", 0, Format.R32G32B32A32_Float, 16, 0) |]
        let layout = new InputLayout(device, signature.Data, inputElements)