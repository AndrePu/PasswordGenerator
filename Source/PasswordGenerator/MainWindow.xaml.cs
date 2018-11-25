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

namespace PasswordGenerator
{
    public partial class MainWindow : Window
    {
        /*Needed for generation password global variables*/
        string number_symb = "0123456789";
        string lowercase_symb = "abcdefghijklmnopqrstvwxyz";
        string uppercase_symb = "ABCDEFGHIJKLMNOPQRSTVWXYZ";
        string special_symb = "!@#$%^&*()~_+=-";

        
        public MainWindow()
        {
            InitializeComponent();
            symb_amount_box.MaxLength = 4;
        }

        /// <summary>
        /// Asses password by Entropy measure.
        /// More detail info(2.1-2.2 sections): https://en.wikipedia.org/wiki/Password_strength
        /// </summary>
        /// <returns></returns>
        private double AssessPotentialPassword()
        {
            string accessible_characters = FormAccesibleSymbols();

            if (accessible_characters == null)
                return 0;

            int ac_amount = accessible_characters.Length;   // amount of accesible characters availible
            int password_length = int.Parse(symb_amount_box.Text);

            double password_strength = password_length * Math.Log(ac_amount, 2);
            return password_strength;
        }

        private void PasswordEvaluation()
        {
            double password_strength = AssessPotentialPassword();
            double[] desired_entropy = { 40, 80, 96}; // desired entropy that we set by ourself for medium, strong, very strong passwords accordingly

            if (password_strength == 0)
            {
                password_complexity.Content = "";
                progressBar1.Value = 0;
            }
            else if (password_strength < desired_entropy[0])
            {
                password_complexity.Content = "Weak";
                progressBar1.Value = 15;
            }
            else if (password_strength < desired_entropy[1])
            {
                password_complexity.Content = "Medium";
                progressBar1.Value = 45;
            }
            else if (password_strength < desired_entropy[2])
            {
                password_complexity.Content = "Strong";
                progressBar1.Value = 77;
            }
            else
            {
                password_complexity.Content = "Very Strong";
                progressBar1.Value = 99;
            }
        }

        /// <summary>
        /// Forms needed set of symbols that will be used for generating password.
        /// </summary>
        /// <returns>Returns all the availible symbols.</returns>
        private string FormAccesibleSymbols()
        {            
            string accessible_symbols = null;

            if (cb1.IsChecked == true)
                accessible_symbols += number_symb;

            if (cb2.IsChecked == true)
                accessible_symbols += lowercase_symb;

            if (cb3.IsChecked == true)
                accessible_symbols += uppercase_symb;

            if (cb4.IsChecked == true)
                accessible_symbols += special_symb;

            return accessible_symbols;
        }

        /// <summary>
        /// Checks if the user entered the correct information in our password size field
        /// </summary>
        /// <returns>Returns true if he did.</returns>
        private bool IfNumberFieldCorrect()
        {
            int sab_length = symb_amount_box.Text.Length;
            if (sab_length == 0)
                return false;

            for (int i = 0; i < sab_length; i++)
            {
                if (symb_amount_box.Text[i] < '0' || symb_amount_box.Text[i] > '9')
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Tells us if we have an opportunity to generate the password
        /// </summary>
        /// <param name="accesible_symbols"></param>
        /// <returns>Return true if we have.</returns>
        private bool CanGenerate(string accesible_symbols)
        {
            if (accesible_symbols == null)
            {
                MessageBox.Show("You must choose at least one option!", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }
            else if (symb_amount_box.Text == "")
            {
                MessageBox.Show("You must point the size of password!", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                symb_amount_warning.Content = "*needed field";
                return false;
            }
            else if (!IfNumberFieldCorrect())
            {
                MessageBox.Show("Incorect format of password size field!", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            return true;
        }
        private string GeneratePassword(string accesible_symbols)
        {
            string password = "";
            int password_size = int.Parse(symb_amount_box.Text);
            int as_amount = accesible_symbols.Length; // accessible symbols amount

            Random me = new Random();
            for (int i = 0; i < password_size; i++)
            {
                int rand_index = me.Next(0, as_amount - 1);
                password += accesible_symbols[rand_index];
            }
            return password;
        }

        /// <summary>
        /// Erases the password that was generated previous time
        /// </summary>
        private void ErasePreviousWork()
        {
            answer_box.Text = "";
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ErasePreviousWork();

            string accessible_symbols = FormAccesibleSymbols();

            if (CanGenerate(accessible_symbols))
            {
                string new_password = GeneratePassword(accessible_symbols);
                answer_box.Text = new_password;
            }

        }

        private void symb_amount_box_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (symb_amount_box.Text == "")
            {
                symb_amount_warning.Content = "";
                password_complexity.Content = "";
                progressBar1.Value = 0;
            }
            else if (IfNumberFieldCorrect())
            {
                symb_amount_warning.Foreground = Brushes.LimeGreen;
                symb_amount_warning.Content = "*Correct";

                PasswordEvaluation();
            }
            else
            {
                symb_amount_warning.Foreground = Brushes.Red;
                symb_amount_warning.Content = "*enter the number(0 - 9 symbols)";
            }
        }

        private void cb3_Unchecked(object sender, RoutedEventArgs e)
        {
            if (IfNumberFieldCorrect())
                PasswordEvaluation();
        }

        private void cb3_Checked(object sender, RoutedEventArgs e)
        {
            if (IfNumberFieldCorrect())
                PasswordEvaluation();
        }
        
        private void cb1_Checked(object sender, RoutedEventArgs e)
        {
            if (IfNumberFieldCorrect())
                PasswordEvaluation();
        }

        private void cb1_Unchecked(object sender, RoutedEventArgs e)
        {
            if (IfNumberFieldCorrect())
                PasswordEvaluation();
        }

        private void cb2_Unchecked(object sender, RoutedEventArgs e)
        {
            if (IfNumberFieldCorrect())
                PasswordEvaluation();
        }
        private void cb2_Checked(object sender, RoutedEventArgs e)
        {
            if (IfNumberFieldCorrect())
                PasswordEvaluation();
        }

        private void cb4_Checked(object sender, RoutedEventArgs e)
        {
            if (IfNumberFieldCorrect())
                PasswordEvaluation();
        }

        private void cb4_Unchecked(object sender, RoutedEventArgs e)
        {
            if (IfNumberFieldCorrect())
                PasswordEvaluation();
        }
    }
}
