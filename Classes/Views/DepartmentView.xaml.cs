using log4net;
using System;
using System.Windows;
using System.Windows.Controls;

namespace TestTask.Classes.Views
{
    /// <summary>
    /// Demonstrates different ways to render text in the View.
    /// </summary>
    public partial class DepartmentView : Window
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Initializes a new UserView object.
        /// </summary>
        public DepartmentView()
        {
            log4net.Config.XmlConfigurator.Configure();
            InitializeComponent();
            log.Debug("Component Initialized");
            Loaded += DepartmentView_Loaded;
            Closed += DepartmentView_Closed;
            log.Debug("Window events [Loaded, Closed] subscribed");
        }
        private void DepartmentView_Loaded(object sender, RoutedEventArgs e)
        {
            log.Debug("DepartmentView_Loaded(...) called");
        }

        private void DepartmentView_Closed(object sender, EventArgs e)
        {
            log.Debug("DepartmentView_Closed(...) called");
        }

    }
}