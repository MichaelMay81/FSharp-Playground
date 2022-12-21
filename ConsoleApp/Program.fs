open System
open FSharp.Data
open System.Globalization

// get sample data
type Data = CsvProvider<"ConsumptionTestData.CSV", ";">
let data = Data.GetSample ()

// print out some of the data
printfn "Headers:    %A" data.Headers
printfn "#Columns:   %A" data.NumberOfColumns
printfn "Row0:       %A" (data.Rows |> Seq.take 1)

// helper for parsing floats
let parse (s:string) : float = Double.Parse (s, CultureInfo "de-DE")

// print value of first column in first row of data
(data.Rows
|> Seq.head
).Consumption_1
|> parse
|> printfn "TotalConsumption_1: %f"

// print min/max dates
data.Rows
|> Seq.skip 1
|> Seq.map (fun row -> row.Column1 |> DateTime.Parse)
|> fun dts -> (dts |> Seq.min), (dts |> Seq.max)
|> printfn "min/max: %A"

// YOUR TURN!
// Seq-Doc: https://fsharp.github.io/fsharp-core-docs/reference/fsharp-collections-seqmodule.html

// == print total of Consumption_1 row (hint: Seq.sum)==
data.Rows
|> Seq.skip 1
|> Seq.map (fun row -> row.Consumption_1 |> parse)
|> Seq.sum
|> printfn "total of Consumption_1: %f"

// == print number of days (hint: Seq.distinct)==
data.Rows
|> Seq.skip 1
|> Seq.map (fun row -> row.Column1 |> DateTime.Parse)
|> Seq.map (fun dt -> dt.Date)
|> Seq.distinct
|> Seq.length
|> printfn "#days: %i"

// == print average per day for the first 5 days (hint: Seq.groupBy) ==
data.Rows
|> Seq.skip 1
|> Seq.map (fun row -> (row.Column1 |> DateTime.Parse).Date, row.Consumption_1 |> parse)
|> Seq.groupBy (fun (dt, v) -> dt)
|> Seq.take 5
|> Seq.map (fun (dt, vs) ->
    dt, vs
    |> Seq.map snd
    |> Seq.average)
|> Seq.toList
|> printfn "avg/day: %A"