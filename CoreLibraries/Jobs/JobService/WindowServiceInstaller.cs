using System;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;
using System.Linq;
using System.Collections;
using Microsoft.Win32;

namespace MAXIMUS.Core.Services
{
    [RunInstaller(true)]
    public partial class WindowServiceInstaller : System.Configuration.Install.Installer
    {
        private ServiceInstaller serviceInstaller;
        private ServiceProcessInstaller serviceProcessInstaller;
        private string DBName;

        private void SetServicePropertiesFromCommandLine(ServiceInstaller serviceInstaller)
        {
            string[] commandlineArgs = Environment.GetCommandLineArgs();

            string servicename;
            ParseServiceNameSwitches(commandlineArgs, out servicename);

            string dbname;
            ParseDatabaseNameSwitches(commandlineArgs, out dbname);

            serviceInstaller.ServiceName = servicename;
            serviceInstaller.DisplayName = servicename;
            serviceInstaller.Description = "PDMS Job service for environment " + servicename;
            DBName = dbname;
        }

        private void ParseServiceNameSwitches(string[] commandlineArgs, out string serviceName)
        {
            var servicenameswitch = (from s in commandlineArgs where s.StartsWith("/servicename") select s).FirstOrDefault();

            if (servicenameswitch == null)
            {
                serviceName = "JobService";
                return;
            }

            if (!(servicenameswitch.Contains('=') || servicenameswitch.Split('=').Length < 2))
                throw new ArgumentNullException("The /servicename switch is malformed");

            serviceName = servicenameswitch.Split('=')[1];

            serviceName = serviceName.Trim('"');
        }

        public WindowServiceInstaller()
        {
            // InitializeComponent();
            serviceProcessInstaller = new ServiceProcessInstaller();
            serviceInstaller = new ServiceInstaller();

            //# Service Account Information
            serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
            serviceProcessInstaller.Username = null;
            serviceProcessInstaller.Password = null;

            //# Service Information
            serviceInstaller.DisplayName = "Job Service";
            serviceInstaller.Description = "This service executes Jobs for MAXIMUS applications";
            serviceInstaller.DelayedAutoStart = true;
            serviceInstaller.StartType = ServiceStartMode.Automatic;

            //# This must be identical to the WindowsService.ServiceBase name
            //# set in the constructor of WindowsService.cs
            SetServicePropertiesFromCommandLine(serviceInstaller);


            this.Installers.Add(serviceProcessInstaller);
            this.Installers.Add(serviceInstaller);
        }

        /// <summary>
        /// Modify the registry to install the new service
        /// </summary>
        /// <param name="stateServer"></param>
        public override void Install(IDictionary stateServer)
        {
            RegistryKey system,
                //HKEY_LOCAL_MACHINE\System\CurrentControlSet
                currentControlSet,
                //...\Services
                services,
                //...\<Service Name>
                service,
                //...\Parameters - this is where you can put service-specific configuration
                config;

            base.Install(stateServer);

            // Define the registry keys
            // Navigate to services
            system = Registry.LocalMachine.OpenSubKey("System");
            currentControlSet = system.OpenSubKey("CurrentControlSet");
            services = currentControlSet.OpenSubKey("Services");
            // Add the service
            service = services.OpenSubKey(this.serviceInstaller.ServiceName, true);
            // Default service description
            service.SetValue("Description", this.serviceInstaller.Description);

            // Display the assembly image path and modify to add the service name
            // The executable then strips the name out of the image
            Console.WriteLine("ImagePath: " + service.GetValue("ImagePath"));
            //Console.WriteLine("Provide the database name for the service: ");
            //string databaseName = Console.ReadLine();

            string imagePath = (string)service.GetValue("ImagePath");
            imagePath += " -d " + DBName; //databaseName
            imagePath += " -n " + @"""" + this.serviceInstaller.ServiceName + @"""";
            service.SetValue("ImagePath", imagePath);
            // Create a parameters subkey
            config = service.CreateSubKey("Parameters");
            //config.SetValue("-db ", "mainDb");

            // Close keys
            config.Close();
            service.Close();
            services.Close();
            currentControlSet.Close();
            system.Close();
        }

        /// <summary>
        /// Modify the registry to remove the service
        /// </summary>
        /// <param name="stateServer"></param>
        public override void Uninstall(IDictionary stateServer)
        {
            RegistryKey system,
                //HKEY_LOCAL_MACHINE\System\CurrentControlSet
                currentControlSet,
                //...\Services
                services,
                //...\<Service Name>
                service;
            //...\Parameters - this is where you can put service-specific configuration

            base.Uninstall(stateServer);

            // Navigate down the registry path
            system = Registry.LocalMachine.OpenSubKey("System");
            currentControlSet = system.OpenSubKey("CurrentControlSet");
            services = currentControlSet.OpenSubKey("Services");
            service = services.OpenSubKey(this.serviceInstaller.ServiceName, true);
            // Remove the parameters key
            service.DeleteSubKeyTree("Parameters");

            // Close keys
            service.Close();
            services.Close();
            currentControlSet.Close();
            system.Close();
        }

        private void ParseDatabaseNameSwitches(string[] commandlineArgs, out string DBName)
        {
            var DBNameswitch = (from s in commandlineArgs where s.StartsWith("/dbname") select s).FirstOrDefault();

            if (DBNameswitch == null)
            {
                DBName = "JobService";
                return;
            }

            if (!(DBNameswitch.Contains('=') || DBNameswitch.Split('=').Length < 2))
                throw new ArgumentNullException("The /dbname switch is malformed");

            DBName = DBNameswitch.Split('=')[1];

            DBName = DBName.Trim('"');
        }

    }
}
