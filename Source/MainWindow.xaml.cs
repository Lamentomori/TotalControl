﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TotalControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        public async void Init()
        {
            this.Status.Content = "";

            await IntroAnimation();

            String userName = Environment.UserName;

            // Read the environment variable
            string envValue = Environment.GetEnvironmentVariable("CyberWatch-Status", EnvironmentVariableTarget.User);

            // Check if it's set to "1"
            if (envValue == "1")
            {
                // Open Dashboard
                await Task.Delay(1200);
                this.Status.Content = $"Welcome Back {userName}";
                Animation.FadeIn(this.Status, 1);
                await Task.Delay(1000);
                Animation.FadeOut(this.Status, 1.5);

                await Task.Delay(1000);
                Dashboard dashboard = new Dashboard();
                this.Hide();
                dashboard.Show();

            }
            else
            {
                var result = MessageBox.Show(
                "Total Control needs to make Changes to your Firewall. \n Configuration is needed for this application to work as intended. \n Warning: You may experience the inability communicate externally unless you already have a firewall rule in place. \n Do you authorize these changes? We offer support..?",
                "Total Control | Confirm Changes",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

                if (result != MessageBoxResult.Yes)
                    Environment.Exit(0);

                // Proceed with logic (e.g., configure firewall)
                await Configure();

                Dashboard dashboard = new Dashboard();
                this.Hide();
                dashboard.Show();

            }
        }

        public async Task IntroAnimation()
        {
            Animation.SlideAndFadeIn(this.Logo, -100, 0, 0, 0, 1);
            await Task.Delay(100);
            Animation.FadeIn(this.Title, 3);
            Animation.FadeIn(this.Status, 5);
            await Task.Delay(500);
        }

        public async Task Configure()
        {
            // Boilerplate Animation for Configuring the firewall.

            await Task.Delay(500);
            this.Status.Content = "Configuring Firewall";
            await Task.Delay(500);
            this.Status.Content = "Configuring Firewall.";
            await Task.Delay(500);
            this.Status.Content = "Configuring Firewall..";
            await Task.Delay(500);
            this.Status.Content = "Configuring Firewall...";
            await Task.Delay(500);
            this.Status.Content = "Configuring Firewall";
            await Task.Delay(500);
            this.Status.Content = "Configuring Firewall.";
            await Task.Delay(500);
            this.Status.Content = "Configuring Firewall..";
            await Task.Delay(500);
            this.Status.Content = "Configuring Firewall...";

            await Task.Delay(750);
            this.Status.Content = "Complete!";
            Animation.FadeIn(this.Status, 5);


            string configureFirewall = @"
# Ensure script runs as administrator
if (-NOT ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole] 'Administrator')) {
    Write-Error 'This script requires administrator privileges.'
    exit
}

# Set default inbound and outbound policies to block
Set-NetFirewallProfile -Profile Domain,Private,Public -DefaultInboundAction Block -DefaultOutboundAction Block

# Allow loopback traffic
New-NetFirewallRule -DisplayName 'Allow Loopback' -Direction Inbound -InterfaceAlias 'Loopback*' -Action Allow
New-NetFirewallRule -DisplayName 'Allow Loopback Outbound' -Direction Outbound -InterfaceAlias 'Loopback*' -Action Allow

# Optionally allow DHCP and DNS (necessary for network function)
New-NetFirewallRule -DisplayName 'Allow DHCP' -Direction Outbound -Protocol UDP -RemotePort 67 -Action Allow
New-NetFirewallRule -DisplayName 'Allow DNS' -Direction Outbound -Protocol UDP -RemotePort 53 -Action Allow

# Set log parameters
$logFilePath = ""$env:SystemRoot\System32\LogFiles\Firewall\pfirewall.log""
$logMaxSize = 500,000  # Size in KB (4MB)

# Enable and configure logging for each firewall profile
$profiles = @('Domain', 'Private', 'Public')
foreach ($profile in $profiles) {
    Set-NetFirewallProfile -Profile $profile `
        -LogFileName $logFilePath `
        -LogMaxSizeKilobytes $logMaxSize `
        -LogAllowed True `
        -LogBlocked True
}
";

            PowershellHelper.Run(configureFirewall);


            // Whitelist Essential Services but leave no holes..

            // After everything trigger the Env Variable creation
            Environment.SetEnvironmentVariable("CyberWatch-Config", "1");
        }
    }
}
