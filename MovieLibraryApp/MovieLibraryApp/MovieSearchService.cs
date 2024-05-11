using MovieLibraryApp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MovieLibraryApp
{
    public class MovieSearchService
    {
        private string filePath;
        public MovieSearchService(string filespath)
        {
            filePath = filespath;
        }
        public List<MovieInfo> SearchMovies(string searchTerm)
        {
            var allMovies = ReadMovieInfoFromFile();
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return allMovies;
            }

            searchTerm = searchTerm.ToLower();

            List<MovieInfo> searchResults = allMovies
                .Where(movie =>
                    movie.Title != null && movie.Title.ToLower().Contains(searchTerm) ||
                    movie.Genre != null && movie.Genre.ToLower().Contains(searchTerm) ||
                    movie.Year.ToString().Contains(searchTerm) ||
                    movie.Actors != null && movie.Actors.ToLower().Contains(searchTerm) ||
                    movie.Director != null && movie.Director.ToLower().Contains(searchTerm) ||
                    movie.Writer != null && movie.Writer.ToLower().Contains(searchTerm))
                .ToList();

            return searchResults;
        }

        public List<MovieInfo> ReadMovieInfoFromFile()
        {
            List<MovieInfo> movies = new List<MovieInfo>();
            MovieInfo movie = new MovieInfo();

            try
            {
                // Read all lines from the file
                string[] lines = File.ReadAllLines(filePath);

                // Iterate through each line
                foreach (string line in lines)
                {
                    // Check if the line is empty, which indicates the end of a movie entry
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        // Add the movie to the list and create a new MovieInfo object for the next movie
                        movies.Add(movie);
                        movie = new MovieInfo();
                        continue;
                    }

                    // Split each line by ": " to separate the property name and value
                    string[] parts = line.Split(": ");

                    // Get the property name and value
                    string propertyName = parts[0].Trim();
                    string propertyValue = parts[1].Trim();

                    // Assign the property value to the corresponding property of the MovieInfo object
                    switch (propertyName)
                    {
                        case "Title":
                            movie.Title = propertyValue;
                            break;
                        case "Year":
                            movie.Year = propertyValue;
                            break;
                        case "Rated":
                            movie.Rated = propertyValue;
                            break;
                        case "Released":
                            movie.Released = propertyValue;
                            break;
                        case "Runtime":
                            movie.Runtime = propertyValue;
                            break;
                        case "Genre":
                            movie.Genre = propertyValue;
                            break;
                        case "Director":
                            movie.Director = propertyValue;
                            break;
                        case "Writer":
                            movie.Writer = propertyValue;
                            break;
                        case "Actors":
                            movie.Actors = propertyValue;
                            break;
                        case "Plot":
                            movie.Plot = propertyValue;
                            break;
                        case "Language":
                            movie.Language = propertyValue;
                            break;
                        case "Country":
                            movie.Country = propertyValue;
                            break;
                        case "Awards":
                            movie.Awards = propertyValue;
                            break;
                        case "Poster":
                            movie.Poster = propertyValue;
                            break;
                        case "Metascore":
                            // Initialize the Ratings list if it's null
                            movie.Metascore = propertyValue;
                            break;
                        case "imdbRating":
                            // Initialize the Ratings list if it's null
                            movie.imdbRating = propertyValue;
                            break;
                        case "imdbVotes":
                            // Initialize the Ratings list if it's null
                            movie.imdbVotes = propertyValue;
                            break;
                        case "imdbID":
                            // Initialize the Ratings list if it's null
                            movie.imdbID = propertyValue;
                            break;
                        case "Type":
                            // Initialize the Ratings list if it's null
                            movie.Type = propertyValue;
                            break;
                        case "DVD":
                            // Initialize the Ratings list if it's null
                            movie.DVD = propertyValue;
                            break;
                        case "BoxOffice":
                            // Initialize the Ratings list if it's null
                            movie.BoxOffice = propertyValue;
                            break;
                        case "Production":
                            // Initialize the Ratings list if it's null
                            movie.Production = propertyValue;
                            break;
                        case "Website":
                            // Initialize the Ratings list if it's null
                            movie.Website = propertyValue;
                            break;
                        case "Ratings":
                            // Initialize the Ratings list if it's null
                            movie.Ratings ??= new List<Rating>();
                            break;
                        default:
                            movie.Ratings.Add(new Rating { Source = propertyName, Value = propertyValue });
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading movie info: {ex.Message}");
            }

            return movies;
        }

        public List<string> GetGenres()
        {
            var allMovies = ReadMovieInfoFromFile();
            List<string> genres = new List<string>();

            foreach (var movie in allMovies)
            {
                if (!string.IsNullOrEmpty(movie.Genre))
                {
                    var genreWords = movie.Genre.Split(',').Select(g => g.Trim());
                    foreach (var word in genreWords)
                    {
                        if (!genres.Contains(word))
                        {
                            genres.Add(word);
                        }
                    }
                }
            }

            return genres;
        }

        public List<MovieInfo> SortedByRating(string selectedRating)
        {
            List<MovieInfo> SortedList = ReadMovieInfoFromFile();

            SortedList = SortedList.OrderByDescending(movie => GetRatingValue(movie, selectedRating)).ToList();

            return SortedList;
        }

        private double GetRatingValue(MovieInfo movie, string selectedRating)
        {
            double ratingValue = 0;

            if (movie.Ratings != null)
            {
                var rating = movie.Ratings.FirstOrDefault(r => r.Source == selectedRating);

                if (rating != null && double.TryParse(rating.Value, out ratingValue))
                {
                    if (selectedRating == "Internet Movie Database")
                    {
                        ratingValue = double.Parse(rating.Value.Split('/')[0]);
                    }
                    else if (selectedRating == "Rotten Tomatoes")
                    {
                        ratingValue = double.Parse(rating.Value.TrimEnd('%')); // Remove % sign
                    }
                    else if (selectedRating == "Metacritic")
                    {
                        ratingValue = double.Parse(rating.Value.Split('/')[0]); // Take only the first part before "/"
                    }
                }
            }

            return ratingValue;
        }



    }
}
