[<AutoOpen>]
module Definitions 

    open System
    open System.Drawing
    open Array2D.Extensions

    type IntSize = 
        { Width : int;
          Height : int }
    
    type Array2D<'p> = 'p [,]
    
    type ConversionBehavior<'a> = 
        { processBlock : Array2D<Color> -> 'a;
          toChar : 'a -> char }
    
    let toIntSize (size : SizeF) = 
        { Width = int size.Width;
          Height = int size.Height }

    let toColorArray (bmp : Bitmap) = 
        let getColor x y = bmp.GetPixel(x, y)
        Array2D.init bmp.Width bmp.Height getColor



