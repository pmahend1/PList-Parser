# PList-Parser

Plist parser in dotnet and powershell core.

## 1. Dotnet

-   Install nuget [PListParserLibrary](https://www.nuget.org/packages/PListParserLibrary)
-   Call `PListParserLibrary.Parser.Parse(string pListText)`

Look at [sample](./DotNet/PListParserLibrary.Sample) for more details

### Example

```csharp
var text = File.ReadAllText("Plist/PlistFile1.plist", System.Text.Encoding.UTF8);
var parser = new PListParserLibrary.Parser();
Dictionary<string, object> dictionaryOutput = parser.Parse(text);
Console.WriteLine("UIDeviceFamily=" + string.Join(',', dictionaryOutput["UIDeviceFamily"] as List<int>));
Console.WriteLine("MinimumOSVersion=" + dictionaryOutput["MinimumOSVersion"]);
//further use
var json = JsonConvert.SerializeObject(dictionaryOutput, Formatting.Indented);
Console.WriteLine(Environment.NewLine + "As Json" + Environment.NewLine + "----------------" + Environment.NewLine + json);
```

**Output**

```log
UIDeviceFamily=1,2
MinimumOSVersion=8.0

As Json
----------------
{
  "UIDeviceFamily": [
    1,
    2
  ],
  "UISupportedInterfaceOrientations": [
    "UIInterfaceOrientationPortrait",
    "UIInterfaceOrientationLandscapeLeft",
    "UIInterfaceOrientationLandscapeRight"
  ],
  "UISupportedInterfaceOrientations~ipad": [
    "UIInterfaceOrientationPortrait",
    "UIInterfaceOrientationPortraitUpsideDown",
    "UIInterfaceOrientationLandscapeLeft",
    "UIInterfaceOrientationLandscapeRight"
  ],
  "MinimumOSVersion": "8.0",
  "CFBundleDisplayName": "App2",
  "CFBundleIdentifier": "com.companyname.App2",
  "CFBundleVersion": "1.0",
  "UILaunchStoryboardName": "LaunchScreen",
  "CFBundleName": "App2",
  "XSAppIconAssets": "Assets.xcassets/AppIcon.appiconset",
  "Required device capabilities": {
    "telephony": "wifi",
    "wifi": "wifi",
    "sms": "sms"
  },
  "Bundle OS type code": false,
  "CustomProperty": "1.2"
}
```

## 2. Powershell

```powershell
Install-Module -Name PListParserLibrary.Powershell.PListParser
```

### Example

```powershell
Import-Module PListParserLibrary.Powershell.PlistParser
$text = Get-Content -Path .\DotNet\PListParserLibrary.Sample\Plist\PlistFile1.plist -Raw
$dictionaryText = Get-PlistFromText -Input $text
$dictionaryText
Key                                   Value
---                                   -----
UIDeviceFamily                        {1, 2}
UISupportedInterfaceOrientations      {UIInterfaceOrientationPortrait, UIInterfaceOrientationLandscapeLeft, UIInterfac…
UISupportedInterfaceOrientations~ipad {UIInterfaceOrientationPortrait, UIInterfaceOrientationPortraitUpsideDown, UIInt…
MinimumOSVersion                      8.0
CFBundleDisplayName                   App2
CFBundleIdentifier                    com.companyname.App2
CFBundleVersion                       1.0
UILaunchStoryboardName                LaunchScreen
CFBundleName                          App2
XSAppIconAssets                       Assets.xcassets/AppIcon.appiconset
Required device capabilities          {[telephony, wifi], [wifi, wifi], [sms, sms]}
Bundle OS type code                   False
CustomProperty                        1.2


$dictionaryText["UIDeviceFamily"]
1
2

dictionaryText["MinimumOSVersion"]
8.0

$dictionaryText["Required device capabilities"]
Key       Value
---       -----
telephony wifi
wifi      wifi
sms       sms
```
