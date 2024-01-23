using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MusicalyAdminApp
{
    public partial class SongInfo : UserControl
    {

        public SongInfo()
        {
            InitializeComponent();
        }

        public event EventHandler SaveClicked;

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            NomInf.IsReadOnly = !NomInf.IsReadOnly;
            IdiomaInf.IsReadOnly = !IdiomaInf.IsReadOnly;
            DuracioInf.IsReadOnly = !DuracioInf.IsReadOnly;
            FormatInf.IsReadOnly = !FormatInf.IsReadOnly;
            LletraInf.IsReadOnly = !LletraInf.IsReadOnly;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveClicked?.Invoke(this, EventArgs.Empty);
        }

        public TextBox NomInfTextBox => NomInf;
        public TextBox IdiomaInfTextBox => IdiomaInf;
        public TextBox DuracioInfTextBox => DuracioInf;         
        public TextBox FormatInfTextBox => FormatInf;
        public TextBox LletraInfTextBox => LletraInf;

    } 
}
