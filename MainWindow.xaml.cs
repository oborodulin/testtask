using log4net;
using System;
using System.ComponentModel;
using System.Windows;

namespace TestTask
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public MainWindow()
        {
            InitializeComponent();
            log.Debug("Component Initialized");
            Loaded += MainWindow_Loaded;
            Closed += MainWindow_Closed;
            log.Debug("Window events [Loaded, Closed] subscribed");
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            log.Debug("MainWindow_Loaded(...) called");
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            log.Debug("MainWindow_Closed(...) called");
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            log.Debug("OnClosing(...) called");
            base.OnClosing(e);
            MessageBoxResult result = MessageBox.Show("Закрыть приложение?", "Подверждение закрытия", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            e.Cancel = result == MessageBoxResult.Cancel;
        }
    }
}
