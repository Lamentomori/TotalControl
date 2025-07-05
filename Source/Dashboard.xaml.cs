using NetFwTypeLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
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

        private void RefreshRules_Click(object sender, RoutedEventArgs e)
        {
            string script = "Get-NetFirewallRule | Where-Object {$_.DisplayName -like '*Total Control*'} | Select-Object -ExpandProperty DisplayName";

            var ruleNames = PowershellHelper.RunWithOutput(script);

            Application.Current.Dispatcher.Invoke(() =>
            {
                RuleList.ItemsSource = null;
                RuleList.ItemsSource = ruleNames;
            });
        }

        private void RemoveRule_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag != null)
            {
                string ruleName = button.Tag.ToString();
                MessageBox.Show($"Removing rule: '{ruleName}'");
                PowershellHelper.Run($"Remove-NetFirewallRule -DisplayName '{ruleName}'");
                RefreshRules_Click(null, null); // Refresh after removal
            }
            else
            {
                MessageBox.Show("Button tag is null");
            }
        }

        private void AddRuleButton_Click(object sender, RoutedEventArgs e)
        {
            string ruleValue = RuleTextBox.Text.Trim();
            if (string.IsNullOrEmpty(ruleValue))
            {
                MessageBox.Show("Please enter a rule value.");
                return;
            }

            if (RuleTypeComboBox.SelectedItem is ComboBoxItem ruleTypeItem &&
                ActionTypeComboBox.SelectedItem is ComboBoxItem actionTypeItem)
            {
                string ruleType = ruleTypeItem.Content.ToString();
                string action = actionTypeItem.Content.ToString(); // "Allow" or "Deny"

                string ruleName = $"Total Control {action} {ruleType} {ruleValue}";

                string psScript = "";

                switch (ruleType)
                {
                    case "IP":
                        psScript = $"New-NetFirewallRule -DisplayName '{ruleName}' -Direction Outbound -RemoteAddress '{ruleValue}' -Action {action} -Enabled True -Profile Any";
                        break;

                    case "Port":
                        psScript = $"New-NetFirewallRule -DisplayName '{ruleName}' -Direction Outbound -Protocol TCP -RemotePort {ruleValue} -Action {action} -Enabled True -Profile Any";
                        break;

                    case "Application":
                        psScript = $"New-NetFirewallRule -DisplayName '{ruleName}' -Direction Outbound -Program '{ruleValue}' -Action {action} -Enabled True -Profile Any";
                        break;

                    default:
                        MessageBox.Show("Unsupported rule type.");
                        return;
                }

                PowershellHelper.Run(psScript);
                MessageBox.Show($"Rule added: {ruleName}");

                // Clear inputs
                RuleTextBox.Clear();
                RuleTypeComboBox.SelectedIndex = -1;
                ActionTypeComboBox.SelectedIndex = -1;

                // Refresh the rule list
                RefreshRules_Click(null, null);
            }
            else
            {
                MessageBox.Show("Please select both a rule type and action.");
            }
        }



        private List<string> GetTotalControlRules()
        {
            var ruleNames = new List<string>();

            using (PowerShell ps = PowerShell.Create())
            {
                ps.AddScript("Get-NetFirewallRule | Where-Object {$_.DisplayName -like '*Total Control*'} | ForEach-Object { $_.DisplayName }");

                var results = ps.Invoke();

                Console.WriteLine("Returned PowerShell Results:");
                foreach (var result in results)
                {
                    Console.WriteLine($" - {result}");
                    if (result != null && !string.IsNullOrWhiteSpace(result.ToString()))
                        ruleNames.Add(result.ToString());
                }

                if (ps.HadErrors)
                {
                    foreach (var err in ps.Streams.Error)
                        Console.WriteLine($"[ERROR]: {err}");
                }
            }

            return ruleNames;
        }



        private void RemoveFirewallRule(string ruleName)
        {
            using (PowerShell ps = PowerShell.Create())
            {
                ps.AddScript($"Remove-NetFirewallRule -DisplayName '{ruleName}'");
                ps.Invoke();
            }
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

        private void homeBTN_Click(object sender, RoutedEventArgs e)
        {
            this.Home.Visibility = Visibility.Visible;
            this.Settings.Visibility = Visibility.Hidden;
            this.manageRules.Visibility = Visibility.Hidden;
        }

        private void manageRulesBTN_Click(object sender, RoutedEventArgs e)
        {
            this.Home.Visibility = Visibility.Hidden;
            this.Settings.Visibility = Visibility.Hidden;
            this.manageRules.Visibility = Visibility.Visible;
        }

        private void settingsBTN_Click(object sender, RoutedEventArgs e)
        {
            this.Home.Visibility = Visibility.Hidden;
            this.Settings.Visibility = Visibility.Visible;
            this.manageRules.Visibility = Visibility.Hidden;
        }

        private void allowDNSRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            ToggleCoreInternetRules(shouldAdd: true);
        }

        private void allowDNSRadioBtn_Unchecked(object sender, RoutedEventArgs e)
        {
            ToggleCoreInternetRules(shouldAdd: false);
        }

        private void ToggleCoreInternetRules(bool shouldAdd)
        {
            string script = shouldAdd ? @"
if (-not (Get-NetFirewallRule -DisplayName 'Total Control Allow DNS' -ErrorAction SilentlyContinue)) {
    New-NetFirewallRule -DisplayName 'Total Control Allow DNS' -Direction Outbound -Protocol UDP -RemotePort 53 -Action Allow -Profile Any -Enabled True
}
if (-not (Get-NetFirewallRule -DisplayName 'Total Control Allow HTTP' -ErrorAction SilentlyContinue)) {
    New-NetFirewallRule -DisplayName 'Total Control Allow HTTP' -Direction Outbound -Protocol TCP -RemotePort 80 -Action Allow -Profile Any -Enabled True
}
if (-not (Get-NetFirewallRule -DisplayName 'Total Control Allow HTTPS' -ErrorAction SilentlyContinue)) {
    New-NetFirewallRule -DisplayName 'Total Control Allow HTTPS' -Direction Outbound -Protocol TCP -RemotePort 443 -Action Allow -Profile Any -Enabled True
}
" : @"
Get-NetFirewallRule -DisplayName 'Total Control Allow DNS' -ErrorAction SilentlyContinue | Remove-NetFirewallRule
Get-NetFirewallRule -DisplayName 'Total Control Allow HTTP' -ErrorAction SilentlyContinue | Remove-NetFirewallRule
Get-NetFirewallRule -DisplayName 'Total Control Allow HTTPS' -ErrorAction SilentlyContinue | Remove-NetFirewallRule
";

            PowershellHelper.Run(script);
            RefreshRules_Click(null, null); // Optional: update UI
        }

        private void allowWinStoreNmore_Checked(object sender, RoutedEventArgs e)
        {
            ToggleWinStoreRules(shouldAdd: true);
        }

        private void allowWinStoreNmore_Unchecked(object sender, RoutedEventArgs e)
        {
            ToggleWinStoreRules(shouldAdd: false);
        }

        private void ToggleWinStoreRules(bool shouldAdd)
        {
            string script = shouldAdd ? @"
# Allow Windows Store and related services

# Windows Store
if (-not (Get-NetFirewallRule -DisplayName 'Total Control Allow Windows Store' -ErrorAction SilentlyContinue)) {
    New-NetFirewallRule -DisplayName 'Total Control Allow Windows Store' -Direction Outbound -Program '%ProgramFiles%\WindowsApps\*' -Action Allow -Profile Any -Enabled True
}

# Delivery Optimization
if (-not (Get-NetFirewallRule -DisplayName 'Total Control Allow Delivery Optimization' -ErrorAction SilentlyContinue)) {
    New-NetFirewallRule -DisplayName 'Total Control Allow Delivery Optimization' -Direction Outbound -RemotePort 7680 -Protocol TCP -Action Allow -Profile Any -Enabled True
}

# Xbox Live Auth
if (-not (Get-NetFirewallRule -DisplayName 'Total Control Allow Xbox Live Auth' -ErrorAction SilentlyContinue)) {
    New-NetFirewallRule -DisplayName 'Total Control Allow Xbox Live Auth' -Direction Outbound -RemotePort 3074 -Protocol TCP -Action Allow -Profile Any -Enabled True
}
" : @"
# Remove Windows Store and related rules
Get-NetFirewallRule -DisplayName 'Total Control Allow Windows Store' -ErrorAction SilentlyContinue | Remove-NetFirewallRule
Get-NetFirewallRule -DisplayName 'Total Control Allow Delivery Optimization' -ErrorAction SilentlyContinue | Remove-NetFirewallRule
Get-NetFirewallRule -DisplayName 'Total Control Allow Xbox Live Auth' -ErrorAction SilentlyContinue | Remove-NetFirewallRule
";

            PowershellHelper.Run(script);
            RefreshRules_Click(null, null); // Optional UI update
        }

    }
}
