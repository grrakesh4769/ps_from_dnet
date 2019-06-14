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

            foreach (PSObject res in results)
            {
                Console.WriteLine("Res val::" + res.ToString());
            }

            Console.ReadLine();
        }
    }
}
