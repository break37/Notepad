using System.Windows;
using System.IO;
using Microsoft.Win32;

namespace Notepad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Properties
        private string currentFilePath;
        public string CurrentFilePath
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(currentFilePath)) return currentFilePath;
                else return "";
            }

            set { currentFilePath = value; }
        }

        private string WordsCount
        {
            get { return wordsCount.Text; }
            set
            {
                string lastCharacter = value[value.Length - 1].ToString();

                if (value.Equals("1")) wordsCount.Text = $"{value} znak";
                else if ("234".Contains(lastCharacter)) wordsCount.Text = $"{value} znaki";
                else wordsCount.Text = $"{value} znaków";
            }
        }

        #endregion
        private OpenFileDialog openFileDialog;
        private SaveFileDialog saveFileDialog;

        public MainWindow()
        {
            InitializeComponent();

            openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select a file";
            openFileDialog.DefaultExt = "txt";
            openFileDialog.Filter = "Wszystkie pliki (*.*)|*.*|Pliki tekstowe (*.txt)|*.txt|Pliki XML (*.xml)|*.xml|Pliki źródłowe (*.cs)|*.cs|Pliki bazy danych (*.sql)|*.sql";

            saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Zapisz jako";
            saveFileDialog.DefaultExt = "txt";
            saveFileDialog.Filter = openFileDialog.Filter;
        }

        private void MenuItem_Otworz_Click(object sender, RoutedEventArgs e)
        {
            Open();
        }

        private void MenuItem_Zapisz_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void MenuItem_Zapisz_jako_Click(object sender, RoutedEventArgs e)
        {
            SaveAs();
        }

        private void TextContent_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            UpdateWordsCounter();
        }

        private void Open()
        {
            //if exists, initialize last file in dialog
            if (!string.IsNullOrWhiteSpace(currentFilePath))
            {
                openFileDialog.InitialDirectory = Path.GetDirectoryName(currentFilePath);
                openFileDialog.FileName = Path.GetFileName(currentFilePath);
            }

            //set selected filename (also sets "" if no file was selected)

            bool? dialogResult = openFileDialog.ShowDialog();

            if (dialogResult.HasValue && dialogResult.Value)
            {
                CurrentFilePath = openFileDialog.FileName;
                textContent.Text = File.ReadAllText(CurrentFilePath);
                loadedFileName.Text = Path.GetFileName(CurrentFilePath);
            }
        }

        private void Save()
        {
            System.Console.WriteLine($"File to save: {CurrentFilePath}");

            //if existing file is opened
            if (!string.IsNullOrWhiteSpace(currentFilePath))
            {
                File.WriteAllText(CurrentFilePath, textContent.Text);
            }
            else SaveAs();
        }

        private void SaveAs()
        {
            //if exists, initialize last file in dialog
            if (!string.IsNullOrWhiteSpace(currentFilePath))
            {
                saveFileDialog.InitialDirectory = Path.GetDirectoryName(currentFilePath);
                saveFileDialog.FileName = Path.GetFileName(currentFilePath);
            }

            bool? dialogResult = saveFileDialog.ShowDialog();
            
            //save file
            if (dialogResult.HasValue && dialogResult.Value)
            {
                CurrentFilePath = saveFileDialog.FileName;
                File.WriteAllText(CurrentFilePath, textContent.Text);
                loadedFileName.Text = Path.GetFileName(currentFilePath);
            }
        }

        private void UpdateWordsCounter()
        {
            WordsCount = textContent.Text.Length.ToString();
        }
    }
}
