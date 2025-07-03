using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using NetFwTypeLib;

namespace TotalControl
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        public static readonly DependencyProperty VulnerableProperty = DependencyProperty.Register(nameof(Vulnerable), typeof(bool), typeof(Dashboard), new PropertyMetadata(false));

        public bool Vulnerable
        {
            get => (bool)GetValue(VulnerableProperty);
            set => SetValue(VulnerableProperty, value);
        }
        public Dashboard()
        {
            InitializeComponent();
            GetFirewallStatus();
        }

        public void GetFirewallStatus()
        {
            if (this.Logo == null)
            {
                MessageBox.Show("Logo control is null!");
                return;
            }

            var fwPolicy2 = (INetFwPolicy2)Activator.CreateInstance(
                Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));

            // Use enum directly here, no cast to int
            NET_FW_PROFILE_TYPE2_ publicProfile = NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_PUBLIC;

            // action is of enum type NET_FW_ACTION_
            NET_FW_ACTION_ action = fwPolicy2.DefaultOutboundAction[publicProfile];

            // Compare enum to enum directly
            Vulnerable = (action == NET_FW_ACTION_.NET_FW_ACTION_ALLOW);


            // DEBUG : MessageBox.Show($"Public profile outbound action: {action} (vulnerable: {vulnerable})");

            if (Vulnerable == false) { 
                
                this.Logo.Source = new BitmapImage(new Uri("pack://application:,,,/image/Safe.png"));
                this.Status.Content = "Status: Secure!";
                this.Details.Content = "Your firewall is safe, press disable to open!";
                this.firewallBtn.Content = "Disable";
            }

            if (Vulnerable == true) { 

                this.Logo.Source = new BitmapImage(new Uri("pack://application:,,,/image/Vulnerable.png"));
                this.Status.Content = "Status: Vulnerable";
                this.Details.Content = "Your firewall is open, press enable to secure!";
                this.firewallBtn.Content = "Enable";
            
            }

            //string imgPath = vulnerable
            //? "pack://application:,,,/Vulnerable.png"
            //: "pack://application:,,,/Protected.png";

            //this.Logo.Source = new BitmapImage(new Uri(imgPath));

        }

        public void EnableFirewallRules()
        {
            var result = MessageBox.Show(
                "Are you sure you want to CLOSE your firewall?\nThis might prevent your device from communicating externally unless there's already a rule in place.",
                "Confirm Firewall Lockdown",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (result != MessageBoxResult.Yes)
                return;

            string script = @"
                $profiles = 'Public','Private'
                foreach ($profile in $profiles) {
                    Set-NetFirewallProfile -Profile $profile -DefaultOutboundAction Block -DefaultInboundAction Block
                }";
            PowershellHelper.Run(script);
            GetFirewallStatus();
        }
        public void DisableFirewallRules()
        {
            var result = MessageBox.Show(
                "Are you sure you want to OPEN your firewall's outbound settings?\nThis will allow most traffic unless explicitly denied.",
                "Confirm Firewall Opening",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (result != MessageBoxResult.Yes)
                return;

            string script = @"
                $profiles = 'Public','Private'
                foreach ($profile in $profiles) {
                    Set-NetFirewallProfile -Profile $profile -DefaultOutboundAction Allow -DefaultInboundAction Allow
                }";
            PowershellHelper.Run(script);
            GetFirewallStatus();
        }
        private void firewallBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Vulnerable == true) { EnableFirewallRules(); return; }

            if (Vulnerable == false) { DisableFirewallRules(); return; }
        }

        private void exitBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Environment.Exit(0);
        }

        private void minimizeBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void topBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void githubBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PowershellHelper.Run("start https://github.com/Lamentomori/TotalControl");

        }

        private void discordBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PowershellHelper.Run("start https://cyberwatch.cc/invite");

        }
    }
}
