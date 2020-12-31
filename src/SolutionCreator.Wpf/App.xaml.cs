using SolutionCreator.Helpers;
using System;
using System.Globalization;
using System.Reflection;
using System.Windows;

namespace SolutionCreator.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var view = new MainWindow();
            Current.MainWindow = view;
            Current.MainWindow.Show();
        }

        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += OnDispatcherUnhandledException;
            //Dispatcher.UnhandledException += OnDispatcherUnhandledException;
        }

        // http://stackoverflow.com/questions/1472498/wpf-global-exception-handler#1472562
        static void OnDispatcherUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var errorMessage = string.Format("ERROR:\r\n\r\n{0}", string.Join("\r\n", ((Exception)e.ExceptionObject).GetAllMessages()));
            MessageBox.Show(errorMessage, "Unexpected error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }
}
