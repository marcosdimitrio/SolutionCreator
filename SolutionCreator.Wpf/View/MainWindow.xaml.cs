using System;
using System.Windows;
using System.Windows.Controls;

namespace SolutionCreator.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        //http://stackoverflow.com/questions/1483892/how-to-bind-to-a-passwordbox-in-mvvm#25001115
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).Senha = ((PasswordBox)sender).SecurePassword;
            }
        }

        private void MainGrid_AccessKeyPressed(object sender, System.Windows.Input.AccessKeyPressedEventArgs e)
        {

        }

    }
}
