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
            // Merging dlls into a single .exe with wpf
            // https://stackoverflow.com/questions/1025843/merging-dlls-into-a-single-exe-with-wpf#4995039
            AppDomain.CurrentDomain.AssemblyResolve += OnResolveAssembly;

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

        private static Assembly OnResolveAssembly(object sender, ResolveEventArgs args)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            var assemblyName = new AssemblyName(args.Name);

            var path = $"{assemblyName.Name}.dll";
            if (assemblyName.CultureInfo.Equals(CultureInfo.InvariantCulture) == false) path = string.Format(@"{0}\{1}", assemblyName.CultureInfo, path);

            using (var stream = executingAssembly.GetManifestResourceStream(path))
            {
                if (stream == null) return null;

                var assemblyRawBytes = new byte[stream.Length];
                stream.Read(assemblyRawBytes, 0, assemblyRawBytes.Length);
                return Assembly.Load(assemblyRawBytes);
            }
        }

    }
}
