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
