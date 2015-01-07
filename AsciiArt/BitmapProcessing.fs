module BitmapProcessing

open System
open System.Drawing
open Array2D.Extensions


let extractPixelBlock blockSize (pixelArray : Array2D<_>) xIndex yIndex = 
    let (xMin, yMin) = blockSize.Width * xIndex, blockSize.Height * yIndex
    let (xMax, yMax) = xMin + blockSize.Width - 1, yMin + blockSize.Height - 1
    pixelArray.[xMin..xMax, yMin..yMax]

let splitIntoBlocks blockSize pixelArray = 
    let xSize = Array2D.length1 pixelArray / blockSize.Width
    let ySize = Array2D.length2 pixelArray / blockSize.Height
    Array2D.init xSize ySize (fun x y -> extractPixelBlock blockSize pixelArray x y)


let charArrayToStringArray (chars) = 
    let maxRow = Array2D.length2 chars - 1
    [| for row in 0..maxRow do
           let rowChars:char[] = chars.[0.., row]
           yield new string(rowChars) |]

let processBitmap conversionBehavior (bmp : Bitmap) = 
    
    let blockSize = 
        { Width = 4;
          Height = 8; }
    bmp
    |> toColorArray
    |> splitIntoBlocks blockSize
    |> Array2D.map conversionBehavior.processBlock
    |> Array2D.map conversionBehavior.toChar
    |> charArrayToStringArray

let prettyPrint = Array.iter (fun x -> printfn "%s" x)
