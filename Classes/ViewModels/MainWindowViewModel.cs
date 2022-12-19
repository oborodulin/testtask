using TestTask.Classes.DataModels;
using TestTask.Classes.DataModels.Enums;
using TestTask.Classes.ViewModels.Commands;
using TestTask.Classes.ViewModels.Properties;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace TestTask.Classes.ViewModels
{
    /// <summary>
    /// Serves the MainWindow class with its data via the ViewModel property and provides custom user settings management.
    /// </summary>
    public class MainWindowViewModel : BaseViewModel
    {
        private BaseViewModel viewModel;
        private PageModel activePage = null;
        private ObservableCollection<PageModel> pages = null;

        /// <summary>
        /// Initializes a new MainWindowViewModel with default values.
        /// </summary>
        public MainWindowViewModel() : base()
        {
            InitializeData();
            ViewModel = new StartViewModel();
        }

        /// <summary>
        /// Gets or sets the BaseViewModel object that is currently displayed in the application.
        /// </summary>
        public BaseViewModel ViewModel
        {
            get { return viewModel; }
            set { if (viewModel != value) { viewModel = value; NotifyPropertyChanged(); } }
        }

        /// <summary>
        /// Gets or sets the collection of PageModel objects that relate to views to display in the application.
        /// </summary>
        public ObservableCollection<PageModel> Pages
        {
            get { return pages; }
            set { if (pages != value) { pages = value; NotifyPropertyChanged(); } }
        }

        /// <summary>
        /// Gets or sets the selected PageModel instance from the collection of PageModel objects that relate to views to display in the application.
        /// </summary>
        public PageModel ActivePage
        {
            get { return activePage; }
            set
            {
                if (activePage != value)
                {
                    activePage = value;
                    NotifyPropertyChanged();

                    if (activePage != null) ViewModel = Activator.CreateInstance(activePage.Type) as BaseViewModel;
                }
            }
        }

        /// <summary>
        /// The command used to redisplay the start view.
        /// </summary>
        public ICommand ShowStartViewCommand
        {
            get { return new ActionCommand(action => ShowStartView()); }
        }

        private void ShowStartView()
        {
            ViewModel = new StartViewModel();
            ActivePage = null;
        }

        /// <summary>
        /// Loads the default project settings.
        /// </summary>
        public void LoadSettings()
        {
            Settings.Default.Reload();
            StateManager.AreAuditFieldsVisible = Settings.Default.AreAuditFieldsVisible;
            StateManager.AreSearchTermsSaved = Settings.Default.AreSearchTermsSaved;
        }

        /// <summary>
        /// Saves the default project settings.
        /// </summary>
        public void SaveSettings()
        {
            Settings.Default.AreAuditFieldsVisible = StateManager.AreAuditFieldsVisible;
            Settings.Default.AreSearchTermsSaved= StateManager.AreSearchTermsSaved;
            Settings.Default.Save();
        }

        private void InitializeData()
        {
            pages = new ObservableCollection<PageModel>();
            pages.Add(new PageModel(typeof(DepartmentsViewModel), Page.Departments, Chapter.Four));
       }
    }
}