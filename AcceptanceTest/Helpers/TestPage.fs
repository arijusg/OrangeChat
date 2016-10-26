namespace PageObjects
open canopy
open System.Configuration

module TestPageHelper =
    let IsTextDisplayed word = 
        (element "h1").Text |> contains word
        ()