module CharacterMapping 
    open System
    open System.Drawing
    open Array2D.Extensions
    
    let makeFont (fontFamily : string) pointSize = 
        let family = new FontFamily(fontFamily)
        new Font(family, pointSize |> single)

    let inline getGraphics< ^a when ^a : (static member op_Explicit : ^a -> int)> (x : ^a) (y : ^a) = 
        Graphics.FromImage(new Bitmap(int x, int y))

    let measureChar (font : Font) (c : char) = 
        use g = getGraphics 1 1
        let text = sprintf "%c" c
        g.MeasureString(text, font)
    
    let getPixels = toColorArray >> Array2D.toSequence
    
    let countPixels font (measureChar : Font -> char -> SizeF) (c : char) = 
        let size = measureChar font c |> toIntSize
        let b = new Bitmap(size.Width, size.Height)
        use g = Graphics.FromImage(b)
        let text = sprintf "%c" c
        g.DrawString(text, font, new SolidBrush(Color.Black), 0.0f, 0.0f)
        b
        |> getPixels
        |> Seq.filter (fun color -> color.A <> 0uy)
        |> Seq.length
    

        

