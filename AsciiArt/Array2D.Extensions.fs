namespace Array2D.Extensions
    module Array2D = 

        let toSequence<'p> (elements : 'p[,]) = 
            let xMax = Array2D.length1 elements - 1
            let yMax = Array2D.length2 elements - 1
            seq { 
                for x in 0..xMax do
                    for y in 0..yMax do
                        yield elements.[x, y]
            }


