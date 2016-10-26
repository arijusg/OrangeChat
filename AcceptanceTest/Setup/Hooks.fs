module Connect.Hooks

open System
open TickSpec
open canopy
open PageObjects

let [<AfterScenario>] After () = 
    ()