module Defaults
    open System
    open System.Drawing

    open CharacterMapping
    open BitmapProcessing
    open Array2D.Extensions

    let defaultFont = makeFont "Courier New" 12
    
    let defaultLightnessMap = 
       
        let darknessValue = countPixels defaultFont measureChar
        
        let isValidChar c = Char.IsLetterOrDigit(c) || Char.IsPunctuation(c)
        
        let cons a b = a :: b

        let toKeyedPairs maxKey values = 
            let count = values |> List.length
            let slope = (float maxKey) / (float count)
            values |> List.mapi (fun i value -> (float i * slope) |> float32, value)
        
        [ 0..255 ]
        |> Seq.map char
        |> Seq.filter isValidChar
        |> Seq.groupBy darknessValue
        |> Seq.where (fun (dark, _) -> dark > 0)
        |> Seq.map (fun (dark, chars) -> (dark, Seq.head (chars)))
        |> Seq.sortBy fst
        |> Seq.map snd
        |> List.ofSeq
        |> List.rev
        |> toKeyedPairs 1.0f
        |> List.rev
        |> cons (1.0f, ' ')

    let toChar (brightness:float32) =
        let b = brightness
        let char = 
            defaultLightnessMap
            |> List.tryFind (fun (bright, _) -> bright <= b )    
            |> Option.map snd
        defaultArg char ' '    

    let averageLightness (pixelArray : Array2D<Color>) = 
        pixelArray
        |> Array2D.toSequence
        |> Seq.averageBy (fun color -> color.GetBrightness())

    let defaultConversionBehavior = 
        {
            processBlock= averageLightness;
            toChar= toChar
        }
        
    let asciiArt = processBitmap defaultConversionBehavior

    let artFromFile (file:string) = 
        new Bitmap(file) |> asciiArt