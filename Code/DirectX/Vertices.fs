namespace DirectX

    open SharpDX
    open SharpDX.Direct3D11

    open Windowing

    module Vertices =

        let createBuffer () =

            let negativeFace = [| (-1.0f, -1.0f); (-1.0f, 1.0f); (1.0f, 1.0f); (-1.0f, -1.0f); (1.0f, 1.0f); (1.0f, -1.0f) |]
            let positiveFace = [| (-1.0f, -1.0f); (1.0f, 1.0f); (-1.0f, 1.0f); (-1.0f, -1.0f); (1.0f, -1.0f); (1.0f, 1.0f) |]

            let add normal point2d =
                let x, y = point2d
                match normal with
                | (d, 0.0f, 0.0f) -> (d, x, y)
                | (0.0f, d, 0.0f) -> (x, d, y)
                | (0.0f, 0.0f, d) -> (x, y, d)
                | _ -> failwith "normals can only point along an axis"

            let negativeFacing normal =
                match normal with
                | (-1.0f, 0.0f, 0.0f)
                | (0.0f, -1.0f, 0.0f)
                | (0.0f, 0.0f, -1.0f) -> true
                | _ -> false

            let faceFor normal = if (negativeFacing normal) then negativeFace else positiveFace

            let createSide normal colour  =
                let asVector4 p =
                    let x, y, z = p
                    Vector4(x, y, z, 1.0f)
                let vertices = (faceFor normal) |> Seq.map (add normal) |> Seq.map asVector4
                let colour4 = colour |> asVector4
                seq { for vertex in vertices do
                        yield vertex
                        yield colour4
                }

            let cube = Seq.toArray <| seq {
                yield! createSide ( 0.0f,  0.0f, -1.0f) (1.0f, 0.0f, 0.0f) // Front - red
                yield! createSide ( 0.0f,  0.0f,  1.0f) (0.0f, 1.0f, 0.0f) // Back - green
                yield! createSide ( 0.0f,  1.0f,  0.0f) (0.0f, 0.0f, 1.0f) // Top - blue
                yield! createSide ( 0.0f, -1.0f,  0.0f) (1.0f, 1.0f, 0.0f) // Bottom - yellow
                yield! createSide (-1.0f,  0.0f,  0.0f) (1.0f, 0.0f, 1.0f) // Left - magenta
                yield! createSide ( 1.0f,  0.0f,  0.0f) (0.0f, 1.0f, 1.0f) // Right - cyan
            }

            // TODO: This doesn't quite correspond to the hard-coded (correct) array below

            let cube = [|             new Vector4(-1.0f, -1.0f, -1.0f, 1.0f); new Vector4(1.0f, 0.0f, 0.0f, 1.0f); // Front - red
                                      new Vector4(-1.0f,  1.0f, -1.0f, 1.0f); new Vector4(1.0f, 0.0f, 0.0f, 1.0f);
                                      new Vector4( 1.0f,  1.0f, -1.0f, 1.0f); new Vector4(1.0f, 0.0f, 0.0f, 1.0f);
                                      new Vector4(-1.0f, -1.0f, -1.0f, 1.0f); new Vector4(1.0f, 0.0f, 0.0f, 1.0f);
                                      new Vector4( 1.0f,  1.0f, -1.0f, 1.0f); new Vector4(1.0f, 0.0f, 0.0f, 1.0f);
                                      new Vector4( 1.0f, -1.0f, -1.0f, 1.0f); new Vector4(1.0f, 0.0f, 0.0f, 1.0f);

                                      new Vector4(-1.0f, -1.0f,  1.0f, 1.0f); new Vector4(0.0f, 1.0f, 0.0f, 1.0f); // Back - green
                                      new Vector4( 1.0f,  1.0f,  1.0f, 1.0f); new Vector4(0.0f, 1.0f, 0.0f, 1.0f);
                                      new Vector4(-1.0f,  1.0f,  1.0f, 1.0f); new Vector4(0.0f, 1.0f, 0.0f, 1.0f);
                                      new Vector4(-1.0f, -1.0f,  1.0f, 1.0f); new Vector4(0.0f, 1.0f, 0.0f, 1.0f);
                                      new Vector4( 1.0f, -1.0f,  1.0f, 1.0f); new Vector4(0.0f, 1.0f, 0.0f, 1.0f);
                                      new Vector4( 1.0f,  1.0f,  1.0f, 1.0f); new Vector4(0.0f, 1.0f, 0.0f, 1.0f);

                                      new Vector4(-1.0f, 1.0f, -1.0f,  1.0f); new Vector4(0.0f, 0.0f, 1.0f, 1.0f); // Top - blue
                                      new Vector4(-1.0f, 1.0f,  1.0f,  1.0f); new Vector4(0.0f, 0.0f, 1.0f, 1.0f);
                                      new Vector4( 1.0f, 1.0f,  1.0f,  1.0f); new Vector4(0.0f, 0.0f, 1.0f, 1.0f);
                                      new Vector4(-1.0f, 1.0f, -1.0f,  1.0f); new Vector4(0.0f, 0.0f, 1.0f, 1.0f);
                                      new Vector4( 1.0f, 1.0f,  1.0f,  1.0f); new Vector4(0.0f, 0.0f, 1.0f, 1.0f);
                                      new Vector4( 1.0f, 1.0f, -1.0f,  1.0f); new Vector4(0.0f, 0.0f, 1.0f, 1.0f);

                                      new Vector4(-1.0f,-1.0f, -1.0f,  1.0f); new Vector4(1.0f, 1.0f, 0.0f, 1.0f); // Bottom - yellow
                                      new Vector4( 1.0f,-1.0f,  1.0f,  1.0f); new Vector4(1.0f, 1.0f, 0.0f, 1.0f);
                                      new Vector4(-1.0f,-1.0f,  1.0f,  1.0f); new Vector4(1.0f, 1.0f, 0.0f, 1.0f);
                                      new Vector4(-1.0f,-1.0f, -1.0f,  1.0f); new Vector4(1.0f, 1.0f, 0.0f, 1.0f);
                                      new Vector4( 1.0f,-1.0f, -1.0f,  1.0f); new Vector4(1.0f, 1.0f, 0.0f, 1.0f);
                                      new Vector4( 1.0f,-1.0f,  1.0f,  1.0f); new Vector4(1.0f, 1.0f, 0.0f, 1.0f);

                                      new Vector4(-1.0f, -1.0f, -1.0f, 1.0f); new Vector4(1.0f, 0.0f, 1.0f, 1.0f); // Left - magenta
                                      new Vector4(-1.0f, -1.0f,  1.0f, 1.0f); new Vector4(1.0f, 0.0f, 1.0f, 1.0f);
                                      new Vector4(-1.0f,  1.0f,  1.0f, 1.0f); new Vector4(1.0f, 0.0f, 1.0f, 1.0f);
                                      new Vector4(-1.0f, -1.0f, -1.0f, 1.0f); new Vector4(1.0f, 0.0f, 1.0f, 1.0f);
                                      new Vector4(-1.0f,  1.0f,  1.0f, 1.0f); new Vector4(1.0f, 0.0f, 1.0f, 1.0f);
                                      new Vector4(-1.0f,  1.0f, -1.0f, 1.0f); new Vector4(1.0f, 0.0f, 1.0f, 1.0f);

                                      new Vector4( 1.0f, -1.0f, -1.0f, 1.0f); new Vector4(0.0f, 1.0f, 1.0f, 1.0f); // Right - cyan
                                      new Vector4( 1.0f,  1.0f,  1.0f, 1.0f); new Vector4(0.0f, 1.0f, 1.0f, 1.0f);
                                      new Vector4( 1.0f, -1.0f,  1.0f, 1.0f); new Vector4(0.0f, 1.0f, 1.0f, 1.0f);
                                      new Vector4( 1.0f, -1.0f, -1.0f, 1.0f); new Vector4(0.0f, 1.0f, 1.0f, 1.0f);
                                      new Vector4( 1.0f,  1.0f, -1.0f, 1.0f); new Vector4(0.0f, 1.0f, 1.0f, 1.0f);
                                      new Vector4( 1.0f,  1.0f,  1.0f, 1.0f); new Vector4(0.0f, 1.0f, 1.0f, 1.0f) |]

            Buffer.Create(device, BindFlags.VertexBuffer, cube)

        let vertices = createBuffer()

