open System
open System.IO

//------------------------------------------
// Step 0. Get the package bootstrap. This is standard F# boiler plate for scripts that also get packages.

let packagesDir = __SOURCE_DIRECTORY__ + "/script-packages"
Directory.CreateDirectory(packagesDir)
Environment.CurrentDirectory <- packagesDir

if not (File.Exists "paket.exe") then
    let url = "https://github.com/fsprojects/Paket/releases/download/1.2.0/paket.bootstrapper.exe" in use wc = new System.Net.WebClient() in let tmp = Path.GetTempFileName() in wc.DownloadFile(url, tmp); File.Move(tmp,"paket.bootstrapper.exe"); System.Diagnostics.Process.Start("paket.bootstrapper.exe") |> ignore;;

//------------------------------------------
// Step 1. Resolve and install the Canopy package and the Chrome web driver
// You can add any additional packages you like to this step.
#r "script-packages/paket.exe"


Paket.Dependencies.Install """
    source https://nuget.org/api/v2
    nuget Canopy
    nuget Selenium.Support
    nuget Selenium.WebDriver.ChromeDriver
""";;

//------------------------------------------
// Step 2. Reference the canopy framework libraries and configure chrome
#r @"script-packages/packages/Selenium.WebDriver/lib/net40/WebDriver.dll"
#r @"script-packages/packages/Selenium.Support/lib/net40/WebDriver.Support.dll"
#r @"script-packages/packages/canopy/lib/canopy.dll"
open canopy
open runner

canopy.configuration.chromeDir <- Path.Combine(packagesDir, @"packages\Selenium.WebDriver.ChromeDriver\driver")

//------------------------------------------
// Step 3. Open Chrome and go to Dynamics NAV page

let root = "http://orangechat.azurewebsites.net"

start chrome

url root