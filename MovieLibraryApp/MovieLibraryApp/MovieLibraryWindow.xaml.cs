using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Brushes = System.Windows.Media.Brushes;
using Image = System.Windows.Controls.Image;

namespace MovieLibraryApp
{
    public partial class MovieLibraryWindow : Window
    {
        private MovieSearchService movieSearchService;
        private string emptyPictureLink = "https://img.freepik.com/free-vector/question-mark-sign-brush-stroke-trash-style-typography-vector_53876-140880.jpg";
        private List<MovieInfo> displayedMovies;
        private bool titleSortAscending = true;
        private bool yearSortAscending = true;

        public MovieLibraryWindow(string filePath)
        {
            InitializeComponent();
            movieSearchService = new MovieSearchService(filePath);
            displayedMovies = movieSearchService.ReadMovieInfoFromFile();
            DisplayMovies(movieSearchService.ReadMovieInfoFromFile());
            FillGenreComboBox();

        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchTerm = SearchTextBox.Text;
            displayedMovies = movieSearchService.SearchMovies(searchTerm);
            SortAndDisplayMovies();
        }

        private void TitleSortButton_Click(object sender, RoutedEventArgs e)
        {
            titleSortAscending = !titleSortAscending;
            displayedMovies = titleSortAscending ? displayedMovies.OrderBy(movie => movie.Title).ToList() : displayedMovies.OrderByDescending(movie => movie.Title).ToList();
            UpdateSortButtonText(TitleSortButton, titleSortAscending);
            DisplayMovies(displayedMovies);
        }

        private void YearSortButton_Click(object sender, RoutedEventArgs e)
        {
            yearSortAscending = !yearSortAscending;
            displayedMovies = yearSortAscending ? displayedMovies.OrderBy(movie => movie.Year).ToList() : displayedMovies.OrderByDescending(movie => movie.Year).ToList();
            UpdateSortButtonText(YearSortButton, yearSortAscending);
            DisplayMovies(displayedMovies);
        }



        private void UpdateSortButtonText(Button button, bool ascending)
        {
            if (button.Name == "TitleSortButton")
            {
                button.Content = ascending ? "Title ▲" : "Title ▼";
            }
            else if (button.Name == "YearSortButton")
            {
                button.Content = ascending ? "Year ▲" : "Year ▼";
            }
        }

        private void SortAndDisplayMovies()
        {
            DisplayMovies(displayedMovies);
        }

        private void DisplayMovies(List<MovieInfo> movies)
        {
            MoviesWrapPanel.Children.Clear();

            if (movies != null && movies.Any())
            {
                foreach (MovieInfo movie in movies)
                {
                    if (movie.Title != null)
                    {
                        WrapPanel movieWrapPanel = new WrapPanel();
                        movieWrapPanel.Width = 300;
                        movieWrapPanel.Height = 450;
                        movieWrapPanel.Margin = new Thickness(10);

                        Image posterImage = new Image();
                        posterImage.Width = 300;
                        posterImage.Height = 375;
                        posterImage.Stretch = System.Windows.Media.Stretch.Uniform;

                        string? posterUrl = movie.Poster;
                        if (!string.IsNullOrEmpty(posterUrl))
                        {
                            BitmapImage bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.UriSource = new Uri(posterUrl);
                            bitmap.EndInit();
                            posterImage.Source = bitmap;
                        }
                        else
                        {
                            BitmapImage bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.UriSource = new Uri(emptyPictureLink);
                            bitmap.EndInit();
                            posterImage.Source = bitmap;
                        }

                        Label titleLabel = new Label();

                        titleLabel.Content = movie.Title;
                        titleLabel.FontSize = 20;
                        titleLabel.Foreground = Brushes.White;
                        titleLabel.FontWeight = FontWeights.Bold;
                        titleLabel.HorizontalContentAlignment = HorizontalAlignment.Center;
                        titleLabel.VerticalContentAlignment = VerticalAlignment.Center;
                        titleLabel.Width = 300;

                        movieWrapPanel.Children.Add(posterImage);
                        movieWrapPanel.Children.Add(titleLabel);
                        //MessageBox.Show($"Added title: {movie.Title}");
                        MoviesWrapPanel.Children.Add(movieWrapPanel);
                        //MessageBox.Show($"Added movieWrapPanel with title: {movie.Title}");
                    }
                }
                //MessageBox.Show("All movies added.");
            }
            else
            {
                Console.WriteLine("No movies found.");
            }

        }
        private void RatingComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string? selectedRating = ((ComboBoxItem)RatingComboBox.SelectedItem)?.Content.ToString();

            if (selectedRating == "Ratings")
            {
                // Do nothing
            }
            else
            {
                if (selectedRating == "Internet Movie Database")
                {
                    displayedMovies = displayedMovies.OrderByDescending(movie => GetRatingValue(movie, "Internet Movie Database")).ToList();
                    SortAndDisplayMovies();
                }
                else if (selectedRating == "Rotten Tomatoes")
                {
                    displayedMovies = displayedMovies.OrderByDescending(movie => GetRatingValue(movie, "Rotten Tomatoes")).ToList();
                    SortAndDisplayMovies();
                }
                else if (selectedRating == "Metacritic")
                {
                    displayedMovies = displayedMovies.OrderByDescending(movie => GetRatingValue(movie, "Metacritic")).ToList();
                    SortAndDisplayMovies();
                }


            }
        }

        private double GetRatingValue(MovieInfo movie, string source)
        {
            double ratingValue = 0;

            if (movie.Ratings != null)
            {
                var rating = movie.Ratings.FirstOrDefault(r => r.Source == source);

                if (rating != null)
                {
                    if (source == "Internet Movie Database")
                    {
                        ratingValue = double.Parse(rating.Value.Split('/')[0]);
                    }
                    else if (source == "Rotten Tomatoes")
                    {
                        ratingValue = double.Parse(rating.Value.TrimEnd('%')); // Remove % sign
                    }
                    else if (source == "Metacritic")
                    {
                        ratingValue = double.Parse(rating.Value.Split('/')[0]); // Take only the first part before "/"
                    }
                }
            }

            return ratingValue;
        }

        private void GenreComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string? selectedGenre = ((ComboBoxItem)GenreComboBox.SelectedItem)?.Content.ToString();
            if (selectedGenre == "Genres")
            {
                displayedMovies = movieSearchService.ReadMovieInfoFromFile();
            }
            else
            {
                displayedMovies = movieSearchService.ReadMovieInfoFromFile().Where(movie => movie.Genre.Contains(selectedGenre)).ToList();
            }
            SortAndDisplayMovies();
        }

        private void FillGenreComboBox()
        {
            List<string> genres = movieSearchService.GetGenres().Distinct().ToList();
            genres.Insert(0, "Genres");
            foreach (var genre in genres)
            {
                GenreComboBox.Items.Add(new ComboBoxItem { Content = genre });
            }
            GenreComboBox.SelectedIndex = 0;
        }
    }
}

