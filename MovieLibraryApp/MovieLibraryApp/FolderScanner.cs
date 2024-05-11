using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows;

namespace MovieLibraryApp
{
    public class FolderScanner
    {
        public static async Task<List<MovieInfo>> GetMovieInfo(List<string> movieNames)
        {
            List<MovieInfo> movieInfoList = new List<MovieInfo>();

            using (HttpClient client = new HttpClient())
            {
                foreach (string movieName in movieNames)
                {
                    string apiUrl = $"http://www.omdbapi.com/?t={movieName}&apikey=74e2408b"; // Replace YOUR_API_KEY with your actual IMDb API key

                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"{movieName} found!");
                        string jsonString = await response.Content.ReadAsStringAsync();
                        MovieInfo ?movieInfo = JsonSerializer.Deserialize<MovieInfo>(jsonString);
                        if (movieInfo.Response == "True")
                        {
                            movieInfoList.Add(movieInfo);
                            Console.WriteLine($"{movieName} got!");
                        }
                    }
                }
            }

            return movieInfoList;
        }

        public static void WriteMovieInfoToFile(List<MovieInfo> movieInfoList, string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (MovieInfo movieInfo in movieInfoList)
                    {
                        writer.WriteLine($"Title: {movieInfo.Title}");
                        writer.WriteLine($"Year: {movieInfo.Year}");
                        writer.WriteLine($"Rated: {movieInfo.Rated}");
                        writer.WriteLine($"Released: {movieInfo.Released}");
                        writer.WriteLine($"Runtime: {movieInfo.Runtime}");
                        writer.WriteLine($"Genre: {movieInfo.Genre}");
                        writer.WriteLine($"Director: {movieInfo.Director}");
                        writer.WriteLine($"Writer: {movieInfo.Writer}");
                        writer.WriteLine($"Actors: {movieInfo.Actors}");
                        writer.WriteLine($"Plot: {movieInfo.Plot}");
                        writer.WriteLine($"Language: {movieInfo.Language}");
                        writer.WriteLine($"Country: {movieInfo.Country}");
                        writer.WriteLine($"Awards: {movieInfo.Awards}");
                        writer.WriteLine($"Poster: {movieInfo.Poster}");
                        writer.WriteLine($"Ratings: ");
                        foreach (var rating in movieInfo.Ratings)
                        {
                            writer.WriteLine($"  {rating.Source}: {rating.Value}");
                        }
                        writer.WriteLine($"Metascore: {movieInfo.Metascore}");
                        writer.WriteLine($"imdbRating: {movieInfo.imdbRating}");
                        writer.WriteLine($"imdbVotes: {movieInfo.imdbVotes}");
                        writer.WriteLine($"imdbID: {movieInfo.imdbID}");
                        writer.WriteLine($"Type: {movieInfo.Type}");
                        writer.WriteLine($"DVD: {movieInfo.DVD}");
                        writer.WriteLine($"BoxOffice: {movieInfo.BoxOffice}");
                        writer.WriteLine($"Production: {movieInfo.Production}");
                        writer.WriteLine($"Website: {movieInfo.Website}");
                        writer.WriteLine();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }
        public static string GetAvailableFileName(string folderPath, string fileName)
        {
            string filePath = Path.Combine(folderPath, fileName);
            if (!File.Exists(filePath))
                return filePath;

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            string fileExtension = Path.GetExtension(fileName);
            int count = 1;
            string newFileName = $"{fileNameWithoutExtension}({count}){fileExtension}";
            while (File.Exists(Path.Combine(folderPath, newFileName)))
            {
                count++;
                newFileName = $"{fileNameWithoutExtension}({count}){fileExtension}";
            }
            return Path.Combine(folderPath, newFileName);
        }
        public static List<string> ScanFolderForMovies(string folderPath)
        {
            List<string> filePaths = new List<string>();

            try
            {
                // Get all files in the folder and subfolders
                string[] files = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories);

                // Check each file for movie extensions
                foreach (string file in files)
                {
                    string extension = Path.GetExtension(file).ToLower();
                    if (extension == ".mkv" || extension == ".mp4" || extension == ".avi")
                    {
                        filePaths.Add(file);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error scanning folder: " + ex.Message);
            }

            return filePaths;
        }
        public static List<string> GetMovieTitles(List<string> movieFilePaths)
        {
            List<string> movieTitles = new List<string>();

            foreach (string filePath in movieFilePaths)
            {
                string fileName = Path.GetFileName(filePath);
                string? movieTitle = ExtractMovieTitle(fileName);
                if (!string.IsNullOrEmpty(movieTitle))
                {
                    movieTitles.Add(movieTitle);
                }
            }

            return movieTitles;
        }
        public static string? ExtractMovieTitle(string fileName)
        {
            string[] parts = fileName.Split('\\');
            string movieName = parts[parts.Length - 1];

            // Replace dots with spaces
            movieName = movieName.Replace(".", " ");

            // Split the string by spaces
            string[] nameParts = movieName.Split(' ');

            // Find the year (4 digit number)
            int yearIndex = FindYear(nameParts);
            if (yearIndex != -1)
            {
                // Remove the year and everything after it
                string[] movieNameParts = new string[yearIndex];
                Array.Copy(nameParts, movieNameParts, yearIndex);
                return string.Join(" ", movieNameParts);
            }
            else
            {
                Console.WriteLine($"Error: Unable to extract movie name from '{fileName}'.");
                return null;
            }
        }
        public static int FindYear(string[] words)
        {
            string pattern = @"\(?(\b\d{4}\b)\)?";  // Matches (xxxx) or xxxx
            Regex rgx = new Regex(pattern);

            for (int i = 0; i < words.Length; i++)
            {
                if (rgx.IsMatch(words[i]))
                {
                    return i;  // Return the index of the word
                }
            }
            return -1;  // Return -1 if no year is found
        }
    }

}
