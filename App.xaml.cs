using log4net;
using System;
using System.Configuration;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using TestTask.Classes.DataModels.Enums;
using TestTask.Classes.ViewModels;
using TestTask.Classes.ViewModels.Interfaces;
using TestTask.DataAccess;
using TestTask.Managers;
using TestTask.Managers.Interfaces;

namespace TestTask
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly string DB_CONNECTION = "DbTaskConnection";
        public static IDBManager? _dbManager = null;

        private static readonly ILog log = LogManager.GetLogger(typeof(App));
        private Exception _lastError;

        public static IDBManager DBManager
        {
            get
            {
                if (_dbManager != null) return _dbManager;
                _dbManager = DependencyManager.Instance.Resolve<IDBManager>(DB_CONNECTION);
                return _dbManager;
            }
        }

        public Exception LastError
        {
            get { return _lastError; }
            set { _lastError = value; }
        }

        /// <summary>
        /// Initializes a new WPF application.
        /// </summary>
        public App()
        {
            StateManager.Instance.RenderingTier = (RenderingTier)(RenderCapability.Tier >> 16);
            //FrameworkCompatibilityPreferences.KeepTextBoxDisplaySynchronizedWithTextProperty = false;
        }

        /// <summary>
        /// Handles the Application.Startup event and is the startup method of the application - the first to be called.
        /// </summary>
        /// <param name="sender">The Application object that represents the application.</param>
        /// <param name="e">The StartupEventArgs object containing application startup arguments if any are used.</param>
        public void App_Startup(object sender, StartupEventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Info("        =============  Started Logging  =============        ");
            string filePath = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;

            RegisterDependencies();
            //base.OnStartup(e);
            new MainWindow().Show();
        }

        private void RegisterDependencies()
        {
            log.Debug("RegisterDependencies() called");
            DependencyManager.Instance.ClearRegistrations();
            //DependencyManagerAdvanced.Instance.RegisterAllInterfacesInAssemblyOf<IDataProvider>();
            //DependencyManagerAdvanced.Instance.RegisterAllInterfacesInAssemblyOf<IDataAsynchronyManager>();
            //DependencyManagerAdvanced.Instance.RegisterAllInterfacesInAssemblyOf<IUserViewModel>();
            DependencyManager.Instance.Register<IDBManager, DBManager>();
            DependencyManager.Instance.Register<IUiThreadManager, UiThreadManager>();
            DependencyManager.Instance.Register<IDepartmentsViewModel, DepartmentsViewModel>();
            DependencyManager.Instance.Register<IDepartmentViewModel, DepartmentViewModel>();
            DependencyManager.Instance.Register<IEmployeesViewModel, EmployeesViewModel>();
            DependencyManager.Instance.Register<IEmployeeViewModel, EmployeeViewModel>();
        }

        void Failure(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            log.Fatal(e.Exception);
            e.Handled = true;
            // кэшировать последнюю возникшую в приложении ошибку
            //this.Properties["LastError"] = e.Exception; //требуется приведение типа
            this.LastError = e.Exception;
            MessageBox.Show("Произошла ошибка, отправьте, пожалуйста, системному администратору содержимое файла 'TestTask.log'");
        }
    }
}
