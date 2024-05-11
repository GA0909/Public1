using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Shapes;

namespace MovieLibraryApp
{
    public partial class MainWindow : Window
    {
        private string? folderPath;
        private string? librayFilePath;
        private string scanningCount = "?";
        private string foundCount = "?";
        private string extractedCount = "?";
        private string receivedCount = "?";
        private string createdCount = "?";

        public MainWindow()
        {
            InitializeComponent();
        }
        private async Task LaunchScan_ClickAsync(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(folderPath))
            {
                await CreateFile(folderPath);
            }
        }

        private void SelectFolder_Click(object sender, RoutedEventArgs e)
        {
            string? currentFolderPath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string realPath = System.IO.Path.Combine(currentFolderPath.Substring(0, currentFolderPath.IndexOf("MovieLibraryApp")), "MovieInfoCollection");
            var dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                InitialDirectory = realPath
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                folderPath = dialog.FileName;
                SelectFolder.Content = System.IO.Path.GetFileName(folderPath);

            }

        }

        private void LaunchMovieLibrary_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(librayFilePath))
            {
                MovieLibraryWindow movieLibraryWindow = new MovieLibraryWindow(librayFilePath);
                movieLibraryWindow.Show();
            }

        }
        private void Launch_Click(object sender, RoutedEventArgs e)
        {
            string? currentFolderPath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string realPath = System.IO.Path.Combine(currentFolderPath.Substring(0, currentFolderPath.IndexOf("MovieLibraryApp") + "MovieLibraryApp".Length), "MovieInfoCollection");
            var dialog = new CommonOpenFileDialog
            {
                IsFolderPicker = false,
                InitialDirectory = realPath
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                librayFilePath = dialog.FileName;
                SelectLibrary.Content = System.IO.Path.GetFileName(librayFilePath);
            }

        }
        private void UpdateTextBlocks()
        {
            ScanningTextBlock.Text = $"Scanning: {scanningCount}";
            FoundTextBlock.Text = $"Found: {foundCount}";
            ExtractedTextBlock.Text = $"Extracted: {extractedCount}";
            ReceivedTextBlock.Text = $"Received: {receivedCount}";
            CreatedTextBlock.Text = $"Created: {createdCount}";
        }

        public void UpdateCounts(string? scanning, string? found, string? extracted, string? received, string? created)
        {
            if (!string.IsNullOrEmpty(scanning)) { scanningCount = scanning; }
            if (!string.IsNullOrEmpty(found)) { foundCount = found; }
            if (!string.IsNullOrEmpty(extracted)) { extractedCount = extracted; }
            if (!string.IsNullOrEmpty(received)) { receivedCount = received; }
            if (!string.IsNullOrEmpty(created)) { createdCount = created; }
            UpdateTextBlocks();
        }

        public string GiveFilePath()
        {
            string? currentFolderPath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string? realPath = System.IO.Path.Combine(currentFolderPath.Substring(0, currentFolderPath.IndexOf("MovieLibraryApp") + "MovieLibraryApp".Length), "MovieInfoCollection");
            return FolderScanner.GetAvailableFileName(realPath, System.IO.Path.GetFileName(folderPath).ToString() + ".txt");

        }

        public async Task CreateFile(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                UpdateCounts(path, null, null, null, null);
                List<string> movieFilePaths = FolderScanner.ScanFolderForMovies(path);
                if (movieFilePaths.Count > 0)
                {
                    UpdateCounts(null, $"{movieFilePaths.Count}", null, null, null);
                }
                else
                {
                    UpdateCounts(null, null, null, null, null);
                }
                List<string> movieTitles = FolderScanner.GetMovieTitles(movieFilePaths);
                if (movieTitles.Count > 0)
                {
                    UpdateCounts(null, null, $"{movieTitles.Count}", null, null);
                }
                else
                {
                    UpdateCounts(null, null, null, null, null);
                }

                List<MovieInfo> movieInfos = await FolderScanner.GetMovieInfo(movieTitles);
                if (movieInfos.Count > 0)
                {
                    UpdateCounts(null, null, null, $"{movieInfos.Count}", null);
                }
                else
                {
                    UpdateCounts(null, null, null, null, null);
                }
                // Write movieInfoList to a text file
                FolderScanner.WriteMovieInfoToFile(movieInfos, GiveFilePath());

                UpdateCounts(null, null, null, null, GiveFilePath());

            }
        }

    }
}

