Stacktrace :
```
System.Management.Automation.Runspaces.PSSnapInException: "Cannot load PowerShell snap-in Microsoft.PowerShell.Host because of the following error: Could not load file or assembly '/Users/rakesh/Documents/GitHub/ps_from_dnet/ps_from_dnet/ps_from_dnet/bin/Debug/netcoreapp2.2/Microsoft.PowerShell'. The system cannot find the file specified.
at System.Management.Automation.Runspaces.InitialSessionState.CreateDefault()
   at System.Management.Automation.Runspaces.RunspaceFactory.CreateRunspace(PSHost host)
   at System.Management.Automation.Runspaces.RunspaceFactory.CreateRunspace()
   at System.Management.Automation.PowerShell.Worker.CreateRunspaceIfNeededAndDoWork(Runspace rsToUse, Boolean isSync)
   at System.Management.Automation.PowerShell.CoreInvokeHelper[TInput,TOutput](PSDataCollection`1 input, PSDataCollection`1 output, PSInvocationSettings settings)
   at System.Management.Automation.PowerShell.CoreInvoke[TInput,TOutput](PSDataCollection`1 input, PSDataCollection`1 output, PSInvocationSettings settings)
   at System.Management.Automation.PowerShell.CoreInvoke[TOutput](IEnumerable input, PSDataCollection`1 output, PSInvocationSettings settings)
   at System.Management.Automation.PowerShell.Invoke(IEnumerable input, PSInvocationSettings settings)
   at System.Management.Automation.PowerShell.Invoke()
   at ps_from_dnet.Program.Main(String[] args) in /Users/rakesh-3889/Documents/GitHub/ps_from_dnet/ps_from_dnet/ps_from_dnet/Program.cs:16

```

Reproduction code is at https://github.com/grrakesh4769/ps_from_dnet.

Relevant bits :

Program.cs :

```
using System;
using System.Collections.ObjectModel;
using System.Management.Automation;

namespace ps_from_dnet
{
    class Program
    {
        static void Main(string[] args)
        {
          PSCommand psCommand = new PSCommand();
          psCommand.AddScript("$PSHome");
          PowerShell ps = PowerShell.Create();
          ps.Commands = psCommand;

          Collection<PSObject> results = ps.Invoke();
          Console.WriteLine("results length::" + results.Count);
        }
    }
}
```

ps_from_dnet.csproj :
```
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>netcoreapp2.2</TargetFramework>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Microsoft.PowerShell.SDK" Version="6.2.0" />
    <PackageReference Include="Newtonsoft.Json" Version="12.0.2" />
    <PackageReference Include="Microsoft.WSMan.Management" Version="6.2.1" />
    <PackageReference Include="Microsoft.PowerShell.Commands.Diagnostics" Version="6.2.1" />
  </ItemGroup>
</Project>
```

The OS is macOS Mojave - 10.14.5.

My end goal is to establish an exchange online Powershell session from dot net core.

I can open and use Powershell from the terminal(pwsh) without any issue.

Googling only results in only
