module NUnit.TickSpec

open System.Configuration
open System
open System.IO
open System.Text
open System.Reflection
open canopy
open canopy.configuration
open canopy.reporters
open TickSpec
open NUnit.Framework
open PageObjects

let assembly = Assembly.GetExecutingAssembly() 
let definitions = new StepDefinitions(assembly)

let baseUrl = ConfigurationManager.AppSettings.["BaseUrl"]
let logPath = Environment.CurrentDirectory + "/canopy/logs/" + DateTime.Now.ToString("MMM-d_HH-mm-ss-fff") + ".log"

let screenshot scenarioName =
    let path = Environment.CurrentDirectory + "/canopy"
    let filename = DateTime.Now.ToString("MMM-d_HH-mm-ss-fff")
    let x = screenshot path filename
    printfn "Took screenshot %s" scenarioName
    ()

/// Inherit from FeatureFixture to define a feature fixture
[<TestFixture>]
type FeatureFixture () =
    let createFeature featureFile = 
        let stream = assembly.GetManifestResourceStream featureFile
        match stream with
        | null -> raise <| FileNotFoundException featureFile
        | _ -> definitions.GenerateFeature(featureFile, stream)

    let featureFiles = 
        let features = assembly.GetManifestResourceNames() |> Seq.where (fun x -> x.EndsWith(".feature"))
        let wipFeatures = features |> Seq.where (fun x -> x.StartsWith("_"))
        if Seq.isEmpty wipFeatures then features else wipFeatures

    let features = featureFiles |> Seq.map createFeature

    member this.scenarios = 
        reporter <- new TeamCityReporter() :> IReporter
        features 
        |> Seq.map (fun x -> x.Scenarios) 
        |> Seq.concat


    [<Test>]
    [<TestCaseSource("scenarios")>]
    static member TestScenario (scenario:Scenario) =
        if scenario.Tags |> Seq.exists ((=) "ignore") then
            raise (new IgnoreException("Ignored: " + scenario.Name))
        try
            scenario.Action.Invoke()
        with
        | _ ->  
            screenshot scenario.Name
            reraise ()

        
    [<NUnit.Framework.TestFixtureSetUp>]
    static member SetUp () =
            #if INTERACTIVE
                chromeDir <- "C:\\Code\\OrangeChat\\AcceptanceTests"
            #else
                chromeDir <- Environment.CurrentDirectory
            #endif
                let chromeOptions = OpenQA.Selenium.Chrome.ChromeOptions()
                chromeOptions.Proxy <- null
                start <| ChromeWithOptions chromeOptions 
                resize (1500, 800)
                elementTimeout <- 15.0
                compareTimeout <- 15.0
                printfn "Chrome is running from %s" chromeDir

                url (baseUrl)
                ()   
            
    [<NUnit.Framework.TestFixtureTearDown>]
    static member TearDown () =
            browser.Close()
            quit chrome
