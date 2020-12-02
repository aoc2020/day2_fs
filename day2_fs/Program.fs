// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open System.IO
open System.Text.RegularExpressions

let passwordRegex = new Regex("^([0-9]+)-([0-9]+) ([a-z]): (.*)$")

type PasswordLine(input: String) as self =
    let m = passwordRegex.Match input
    let min = m.Groups.[1].Value |> int 
    let max = m.Groups.[2].Value |> int
    let character = m.Groups.[3].Value |> char
    let password = m.Groups.[4].Value 
    // do printfn "%s %d %d %c %s" input min max character password 
    member this.valid1() =
        let keepChar c = c.Equals(character)
        let count = password |> Seq.filter keepChar |> Seq.length
        in count >= min && count <= max
    member this.valid2() =
        let c1 = password.[min-1].Equals(character)
        let c2 = password.[max-1].Equals(character)
        in (c1 || c2) && not (c1 && c2)

let readLines (filePath: String) = seq {
    use sr = new StreamReader (filePath)
    while not sr.EndOfStream do
        yield sr.ReadLine ()
}

let passwordLines(filePath: String) =
    readLines(filePath) |> Seq.map PasswordLine |> Seq.toArray                                                     

[<EntryPoint>]
let main argv =
    let pws = passwordLines "/Users/xeno/projects/aoc2020/day2_fs/day2_fs/input.txt"
    let valids1 = pws |> Seq.filter (fun p -> p.valid1()) |> Seq.length
    let valids2 = pws |> Seq.filter (fun p -> p.valid2()) |> Seq.length
    printfn "Answer 1: %d" valids1
    printfn "Answer 2: %d" valids2
    0 // return an integer exit code