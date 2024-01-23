using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using MusicalyAdminApp.API.APISQL;
using MusicalyAdminApp.API.APISQL.Taules;

namespace MusicalyAdminApp
{
    public partial class MainWindow : Window
    {
        private readonly Apisql apiSql;

        public MainWindow()
        {
            InitializeComponent();
            apiSql = new Apisql();
            MostrarCanciones();
        }
        /// <summary>
        /// method/task to show all the songs on the stack panel
        /// </summary>
        /// <returns></returns>
        private async Task MostrarCanciones()
        {
            try
            {
                List<Song> canciones = await apiSql.GetSongs();
                ListBoxCanciones.ItemsSource = canciones;
                ListBoxCanciones.SelectionChanged += ListBoxCanciones_SelectionChanged;
                Inf.SaveClicked += SongInfo_SaveClicked;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener y mostrar las canciones: {ex.Message}");
            }
        }
        /// <summary>
        /// method that changes the inf fields of a song with its actual data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBoxCanciones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Song cancionSeleccionada = ListBoxCanciones.SelectedItem as Song;
            if (cancionSeleccionada != null)
            {
                Inf.NomInfTextBox.Text = $"{cancionSeleccionada.Title}";
                Inf.IdiomaInfTextBox.Text = $"{cancionSeleccionada.Language}";
                Inf.DuracioInfTextBox.Text = $"{cancionSeleccionada.Duration}";
            }
        }
        /// <summary>
        /// this method is used to save the modified data on the database 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private async void SongInfo_SaveClicked(object sender, EventArgs e)
        {
            try
            {
                Song cancionSeleccionada = ListBoxCanciones.SelectedItem as Song;
                if (cancionSeleccionada != null)
                {
                    cancionSeleccionada.Title = Inf.NomInfTextBox.Text;
                    cancionSeleccionada.Language = Inf.IdiomaInfTextBox.Text;
                    await apiSql.PutSong(cancionSeleccionada);

                    await MostrarCanciones();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la canción editada: {ex.Message}");
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            apiSql.Dispose();
        }
    }
}
