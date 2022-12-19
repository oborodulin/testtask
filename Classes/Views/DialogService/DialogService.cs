using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.Classes.DataModels.Enums;

namespace TestTask.Classes.Views.DialogService
{
    public class DialogService
    {
        public static MessageBoxButtonSelection OpenDialog()
        {
            DialogWindow win = new DialogWindow();
            win.ShowDialog();
            return MessageBoxButtonSelection.None;
        }
    }
}
