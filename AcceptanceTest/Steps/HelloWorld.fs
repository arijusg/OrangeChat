module HelloWorldTestSteps

open TickSpec
open canopy
open NUnit.Framework
open System.Configuration
open PageObjects

let [<Then>] ``I see '(.*)'`` text =
    TestPageHelper.IsTextDisplayed text
    