using System;
using System.Collections.Generic;
using System.Linq;
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
using PasswordGenerator.Interfaces;
using PasswordGenerator.Helpers;
using PasswordGenerator.Enums;

namespace PasswordGenerator
{
    public partial class MainWindow : Window
    {
        private string numberSymbols = "0123456789";
        private string lowercaseSymbols = "abcdefghijklmnopqrstvwxyz";
        private string uppercaseSymbols = "ABCDEFGHIJKLMNOPQRSTVWXYZ";
        private string specialSymbols = "!@#$%^&*()~_+=-";
        
        private readonly IPasswordGenerator passwordGenerator;
        private readonly IRandomPasswordEvaluator randomPasswordEvaluator;
        private readonly IPasswordSaver passwordSaver;

        public MainWindow()
        {
            InitializeComponent();
            
            passwordGenerator = new GeneralPasswordGenerator();
            randomPasswordEvaluator = new EntropyRandomPasswordEvaluator();
            passwordSaver = new DialogPasswordSaver();

            passwordSize_textbox.MaxLength = 4;
        }

        private void PasswordEvaluation()
        {
            int passwordSize = int.Parse(passwordSize_textbox.Text);
            string accessibleSymbols = GetAccesibleSymbols();

            if (accessibleSymbols == null || passwordSize == 0)
            {
                passwordComplexity_label.Content = String.Empty;
                progressBar.Value = 0;
                return;
            }

            PasswordRating passwordStrength = randomPasswordEvaluator.Evaluate(passwordSize, accessibleSymbols);
            
            switch (passwordStrength)
            {
                case PasswordRating.Weak:
                    {
                        passwordComplexity_label.Content = "Weak";
                        progressBar.Value = 15;
                        break;
                    }
                case PasswordRating.Medium:
                    {
                        passwordComplexity_label.Content = "Medium";
                        progressBar.Value = 45;

                        break;
                    }
                case PasswordRating.Strong:
                    {
                        passwordComplexity_label.Content = "Strong";
                        progressBar.Value = 77;

                        break;
                    }
                case PasswordRating.VeryStrong:
                    {
                        passwordComplexity_label.Content = "Very Strong";
                        progressBar.Value = 99;

                        break;
                    }
            }
            
        }
        
        private string GetAccesibleSymbols()
        {            
            string accessibleSymbols = null;

            if (numberSymbols_checkbox.IsChecked.Value)
                accessibleSymbols += numberSymbols;

            if (lowercaseSymbols_checkbox.IsChecked.Value)
                accessibleSymbols += lowercaseSymbols;

            if (uppercaseSymbols_checkbox.IsChecked.Value)
                accessibleSymbols += uppercaseSymbols;

            if (specialSymbols_checkbox.IsChecked.Value)
                accessibleSymbols += specialSymbols;

            return accessibleSymbols;
        }

        private bool IsPasswordSizeFieldEmpty()
        {
            return passwordSize_textbox.Text.Length == 0;
        }
        
        private bool CanGenerate(string accessibleSymbols, out string errorText)
        {
            if (accessibleSymbols == null)
            {
                errorText = "You must choose at least one option!";
                return false;
            }
            else if (passwordSize_textbox.Text == String.Empty)
            {
                errorText = "You must point the size of password!";
                return false;
            }

            errorText = null;

            return true;
        }
               

        private void PasswordSize_textbox_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key < Key.D0 || e.Key > Key.D9) && e.Key != Key.Back)
            {
                e.Handled = true;
            }
        }

        private void passwordSize_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            if (IsPasswordSizeFieldEmpty())
            {
                passwordComplexity_label.Content = String.Empty;
                progressBar.Value = 0;
            }
            else
            {
                PasswordEvaluation();
            }
        }


        private void GeneratePassword_Click(object sender, RoutedEventArgs e)
        {
            string accessibleSymbols = GetAccesibleSymbols();
            string errorText;

            if (CanGenerate(accessibleSymbols, out errorText))
            {
                int passwordSize = int.Parse(passwordSize_textbox.Text);

                password_textbox.Text = passwordGenerator.Generate(passwordSize, accessibleSymbols);
            }
            else
            {
                MessageBox.Show(errorText, "Message", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (password_textbox.Text == String.Empty)
            {
                MessageBox.Show("You must generate password first!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                passwordSaver.Save(password_textbox.Text);
            }
        }

        private void checkbox_StateChanged(object sender, RoutedEventArgs e)
        {

            if (!IsPasswordSizeFieldEmpty())
            {
                PasswordEvaluation();
            }
        }


        private void Window_Closed(object sender, EventArgs e)
        {
            App.Current.Shutdown();
        }
    }
}
