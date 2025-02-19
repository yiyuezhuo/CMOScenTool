

## Usages

### Example Project

See `ConsoleAppTest`

### .Net Interactive

```csharp
#r "C:\data\agent_ws\cmo_scen_tool\CMOScenToolSolution\ConsoleAppTest\bin\Debug\net8.0\CMOScenTool.dll"

using CMOScenToolProject;
using System.IO;
using System.Linq;

var content = File.ReadAllText(@"C:\Program Files (x86)\Steam\steamapps\common\Command - Modern Operations\Scenarios\Chains Of War\1. Blue Dawn.scen");
var scenContainer = CMOScenTool.LoadScenContainer(content);
var xml = scenContainer.GetScenarioObject_AsXML();

xml.Substring(0, 1000)
```

```xml
<ContentScenario><TimelineID>939702c2-a4b0-4371-b088-7ebc0a297975</TimelineID><ObjectID>f41bde48-d3cf-4bbc-8092-16a1fb9d359c</ObjectID><ContentTag>CHAINSOFWAR</ContentTag><CampaignID>f8691e80-1caf-4a49-a1b7-9305255a9200</CampaignID><CampaignSessionID /><CampaignScore>0</CampaignScore><Title>Blue Dawn</Title><Description /><Description_Encrypted>EAAAAK4fL7Pyrt/PGMc5BFdeJlRChR173DCj6c5wJxRc5w6LTylCo2wv3OUZt/JiWiQ3MKtn9S1j8qhuiR2d5QPHWS/pne8E7nH63PpneUa9MPDxy2hX76YO2AHCg/wjMGlo1A==</Description_Encrypted><Meta_Complexity>3</Meta_Complexity><Meta_Difficulty>1</Meta_Difficulty><Meta_ScenSetting>Yellow Sea</Meta_ScenSetting><FileName>1. Blue Dawn.scen</FileName><FileNamePath>C:\Program Files (x86)\Steam\steamapps\common\Command - Modern Operations\Scenarios\Official Scenarios\Chains Of War</FileNamePath><Time>636607296000000000</Time><ZeroHour>636607296000000000</ZeroHour><StartTime>636607296000000000</StartTime><Duration>324000000000</Duration><DaylightSavingTime>False</DaylightSavingTime><
```

```csharp
CMOScenTool.GetDecryptedDescription(xml)
```

```
<BODY scroll=auto><P>[LOADDOC]BlueDawnIntro.html[/LOADDOC]</P></BODY>
```

(Official scenario description is typically provided by an HTML file located in the same directory.)

### Python.net

