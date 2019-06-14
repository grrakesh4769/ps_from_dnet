Stacktrace :
```
System.Management.Automation.CmdletInvocationException: "This parameter set requires WSMan, and no supported WSMan client library was found. WSMan is either not installed or unavailable for this system." ---> System.Exception {System.Management.Automation.Remoting.PSRemotingTransportException}: "This parameter set requires WSMan, and no supported WSMan client library was found. WSMan is either not installed or unavailable for this system." ---> System.Exception {System.DllNotFoundException}: "Unable to load shared library 'libpsrpclient' or one of its dependencies. In order to help diagnose loading problems, consider setting the DYLD_PRINT_LIBRARIES environment variable: dlopen(liblibpsrpclient, 1): image not found"
  at at System.Management.Automation.Remoting.Client.WSManNativeApi.WSManInitialize(Int32 flags, IntPtr& wsManAPIHandle)
   at System.Management.Automation.Remoting.Client.WSManClientSessionTransportManager.WSManAPIDataCommon..ctor()
  --- End of inner exception stack trace ---
  at at System.Management.Automation.Remoting.Client.WSManClientSessionTransportManager.WSManAPIDataCommon..ctor()
   at System.Management.Automation.Remoting.Client.WSManClientSessionTransportManager..ctor(Guid runspacePoolInstanceId, WSManConnectionInfo connectionInfo, PSRemotingCryptoHelper cryptoHelper, String sessionName)
   at System.Management.Automation.Runspaces.WSManConnectionInfo.CreateClientSessionTransportManager(Guid instanceId, String sessionName, PSRemotingCryptoHelper cryptoHelper)
   at System.Management.Automation.Remoting.ClientRemoteSessionDSHandlerImpl..ctor(ClientRemoteSession session, PSRemotingCryptoHelper cryptoHelper, RunspaceConnectionInfo connectionInfo, URIDirectionReported uriRedirectionHandler)
   at System.Management.Automation.Remoting.ClientRemoteSessionImpl..ctor(RemoteRunspacePoolInternal rsPool, URIDirectionReported uriRedirectionHandler)
   at System.Management.Automation.Internal.ClientRunspacePoolDataStructureHandler.CreateClientRemoteSession(RemoteRunspacePoolInternal rsPoolInternal)
   at System.Management.Automation.Internal.ClientRunspacePoolDataStructureHandler..ctor(RemoteRunspacePoolInternal clientRunspacePool, TypeTable typeTable)
   at System.Management.Automation.Runspaces.Internal.RemoteRunspacePoolInternal.CreateDSHandler(TypeTable typeTable)
   at System.Management.Automation.Runspaces.Internal.RemoteRunspacePoolInternal..ctor(Int32 minRunspaces, Int32 maxRunspaces, TypeTable typeTable, PSHost host, PSPrimitiveDictionary applicationArguments, RunspaceConnectionInfo connectionInfo, String name)
   at System.Management.Automation.Runspaces.RunspacePool..ctor(Int32 minRunspaces, Int32 maxRunspaces, TypeTable typeTable, PSHost host, PSPrimitiveDictionary applicationArguments, RunspaceConnectionInfo connectionInfo, String name)
   at System.Management.Automation.RemoteRunspace..ctor(TypeTable typeTable, RunspaceConnectionInfo connectionInfo, PSHost host, PSPrimitiveDictionary applicationArguments, String name, Int32 id)
   at Microsoft.PowerShell.Commands.NewPSSessionCommand.CreateRunspacesWhenUriParameterSpecified()
   at Microsoft.PowerShell.Commands.NewPSSessionCommand.ProcessRecord()
   at System.Management.Automation.Cmdlet.DoProcessRecord()
   at System.Management.Automation.CommandProcessor.ProcessRecord()
  --- End of inner exception stack trace ---
  at System.Management.Automation.Runspaces.PipelineBase.Invoke(IEnumerable input)
   at System.Management.Automation.Runspaces.Pipeline.Invoke()
   at System.Management.Automation.PowerShell.Worker.ConstructPipelineAndDoWork(Runspace rs, Boolean performSyncInvoke)
   at System.Management.Automation.PowerShell.Worker.CreateRunspaceIfNeededAndDoWork(Runspace rsToUse, Boolean isSync)
   at System.Management.Automation.PowerShell.CoreInvokeHelper[TInput,TOutput](PSDataCollection`1 input, PSDataCollection`1 output, PSInvocationSettings settings)
   at System.Management.Automation.PowerShell.CoreInvoke[TInput,TOutput](PSDataCollection`1 input, PSDataCollection`1 output, PSInvocationSettings settings)
   at System.Management.Automation.PowerShell.CoreInvoke[TOutput](IEnumerable input, PSDataCollection`1 output, PSInvocationSettings settings)
   at System.Management.Automation.PowerShell.Invoke(IEnumerable input, PSInvocationSettings settings)
   at System.Management.Automation.PowerShell.Invoke()
   at ps_from_dnet.Program.Main(String[] args) in /Users/rakesh/Documents/GitHub/ps_from_dnet/ps_from_dnet/ps_from_dnet/Program.cs:62

```

Reproduction code is at https://github.com/grrakesh4769/ps_from_dnet.

Relevant bits :

Program.cs :

```
using System;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Security;

namespace ps_from_dnet
{
    class Program
    {
        static void Main(string[] args)
        {
            string uname=Console.ReadLine();
            string pwd = Console.ReadLine();
            PSCommand psCommand = new PSCommand();
            psCommand.AddScript("$PSHome");
            PowerShell ps = PowerShell.Create();
            ps.Commands = psCommand;

            Collection<PSObject> results = ps.Invoke();
            Console.WriteLine("results length::" + results.Count);

            foreach (PSObject res in results)
            {
                Console.WriteLine("Res val::" + res.ToString());
            }

            Runspace runspace = RunspaceFactory.CreateRunspace();
            runspace.Open();
            ps.Runspace = runspace;

            results = ps.Invoke();
            Console.WriteLine("results length::" + results.Count);

            foreach (PSObject res in results)
            {
                Console.WriteLine("Res val::" + res.ToString());
            }

            PSCommand rmcommand = new PSCommand();
            string exchangeUri = "https://outlook.office365.com/powershell-liveid/";
            Uri uri = new Uri(exchangeUri);

            rmcommand.AddCommand("New-PSSession");
            rmcommand.AddParameter("ConfigurationName", "Microsoft.Exchange");
            rmcommand.AddParameter("ConnectionUri", uri);

            SecureString password = new SecureString();
            foreach (char c in pwd)
            {
                password.AppendChar(c);
            }

            PSCredential creds = new PSCredential(uname, password);

            rmcommand.AddParameter("Credential", creds);
            rmcommand.AddParameter("Authentication", "Basic");
            rmcommand.AddParameter("AllowRedirection");

            ps.Commands = rmcommand;

            Collection<PSObject> result = ps.Invoke();

            Console.ReadLine();
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
    <PackageReference Include="Microsoft.PowerShell.SDK" Version="6.2.1" />
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
