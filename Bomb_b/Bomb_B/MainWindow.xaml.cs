using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace Bomb_b
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //@author Rafia Anwar

        //-------global vars---------------
        // to store player ships in the grid
        private int[,] userGrid = new int[10, 10];
        //to save the coordiantes of user ships,to remove the ships present on the user grid during reset
        private int[][] userBombCopy = new int[5][];
        //to save the user ship coordinates
        private int[][] userBombSpaces = new int[5][];
        //to save the user moves to check for used hit or miss cell
        private IList<int> userMoves = new List<int>();
        //to count the moves so that winner is not checked until 16th move
        private int move;
        // 0=user, 1=computer
        private int player;
        //the buttons on player's grid
        private Button[] playerButtons = new Button[100];
        //the buttons on computer grid
        private Button[] compButtons = new Button[100];
        // the user is not allowed to click on grid when the one round is over
        private Boolean allowGridClick = false;

        public MainWindow()
        {
            InitializeComponent();
            createButtons(gridPlayer);
            createButtons(gridComputer);
        }

        /**
         * when ok button for AIRCARFT carrier is clicked, it checks for user input,
         * adds the ships on the grid and respective arrays, hides the related labels
         * and textboxes, and make the button disable.
         */
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            int colVal = 0;
            int rowVal = 0;

            int dir = 0;
            if (txtRow.Text == "" || txtCol.Text == "")
            {
                labelOutOfRange.Content = "Please enter a valid value";
                labelOutOfRange.Visibility = Visibility.Visible;
            }

            if (!validateRowValue(txtRow))
                return;
            else
                rowVal = convertLetterToNum(txtRow.Text);

            if (!validateColumnValue(txtCol))
                return;
            else colVal = Int32.Parse(txtCol.Text) - 1;
            if (radioButtonHorizontal.IsChecked == false && radioButtonVertical.IsChecked == false)
            {
                labelOutOfRange.Content = "Please choose an alignment!";
                labelOutOfRange.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                if (radioButtonHorizontal.IsChecked == true)
                {
                    dir = 0;
                }
                else if (radioButtonVertical.IsChecked == true)
                {
                    dir = 1;
                }
            }

            if (!userBombAdded(rowVal, colVal, dir, 5, 0))
            {
                labelOutOfRange.Content = "The coordinates are not valid";
                labelOutOfRange.Visibility = Visibility.Visible;
                txtCol.Text = "";
                txtRow.Text = "";
            }
            else
            {
                if (dir == 0)
                {
                    placePlayerBombs(0, colVal, rowVal, "ac", 5);
                }
                else if (dir == 1)
                {
                    placePlayerBombs(1, colVal, rowVal, "acv", 5);
                }

                hideVisibility(lblCol, lblRow, txtCol, txtRow, btnOK,
                            radioButtonVertical, radioButtonHorizontal);

                btnBlackBomb.IsEnabled = false;
                if (allBombsPlaced())
                    btnStart.IsEnabled = true;

            }//end else for coords valid
        }

        /**
         * when ok button for BATTLESHIP is clicked, it checks for user input,
         * adds the ships on the grid and respective arrays, hides the related labels
         * and textboxes, and make the button disable.
         */
        private void buttonOKGrenade_Click(object sender, RoutedEventArgs e)
        {
            int colVal = 0;
            int rowVal = 0;

            int dir = 0;
            if (textBoxRowGrenade.Text == "" || textBoxColGrenade.Text == "")
            {
                labelOutOfRange.Content = "Please enter a valid value";
                labelOutOfRange.Visibility = Visibility.Visible;
            }
            if (!validateRowValue(textBoxRowGrenade))
                return;
            else
                rowVal = convertLetterToNum(textBoxRowGrenade.Text);

            if (!validateColumnValue(textBoxColGrenade))
                return;
            else colVal = Int32.Parse(textBoxColGrenade.Text) - 1;

            if (radioButtonHorizontalGrenade.IsChecked == false && radioButtonVerticalGrenade.IsChecked == false)
            {
                labelOutOfRange.Content = "Please choose an alignment!";
                labelOutOfRange.Visibility = Visibility.Visible;
                return;
            }

            else
            {
                if (radioButtonHorizontalGrenade.IsChecked == true)
                {
                    dir = 0;
                }
                else if (radioButtonVerticalGrenade.IsChecked == true)
                {
                    dir = 1;
                }
            }

            if (!userBombAdded(rowVal, colVal, dir, 4, 1))
            {
                labelOutOfRange.Content = "The coordinates are not valid";
                labelOutOfRange.Visibility = Visibility.Visible;
                textBoxColGrenade.Text = "";
                textBoxRowGrenade.Text = "";
            }
            else
            {
                if (dir == 0)
                {
                    placePlayerBombs(0, colVal, rowVal, "b2h", 4);
                }
                else if (dir == 1)
                {
                    placePlayerBombs(1, colVal, rowVal, "bv", 4);
                }

                hideVisibility(lblColGreanade, lblRowGrenade, textBoxColGrenade,
                    textBoxRowGrenade, buttonOKGrenade, radioButtonVerticalGrenade, radioButtonHorizontalGrenade);
                btnGrenade.IsEnabled = false;
                if (allBombsPlaced())
                    btnStart.IsEnabled = true;
            }
        }


        /**
        * when ok button for SUBMARINE is clicked, it checks for user input,
        * adds the ships on the grid and respective arrays, hides the related labels
        * and textboxes, and make the button disable.
        */
        private void buttonOKLandMine_Click(object sender, RoutedEventArgs e)
        {
            int colVal = 0;
            int rowVal = 0;

            int dir = 0;
            if (textBoxRowLandMine.Text == "" || textBoxColLandMine.Text == "")
            {
                labelOutOfRange.Content = "Please enter a valid value";
                labelOutOfRange.Visibility = Visibility.Visible;
            }
            if (!validateRowValue(textBoxRowLandMine))
                return;
            else
                rowVal = convertLetterToNum(textBoxRowLandMine.Text);

            if (!validateColumnValue(textBoxColLandMine))
                return;
            else colVal = Int32.Parse(textBoxColLandMine.Text) - 1;
            if (radioButtonHorizontalLandMine.IsChecked == false && radioButtonVerticalLandMine.IsChecked == false)
            {
                labelOutOfRange.Content = "Please choose an alignment!";
                labelOutOfRange.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                if (radioButtonHorizontalLandMine.IsChecked == true)
                {
                    dir = 0;
                }
                else if (radioButtonVerticalLandMine.IsChecked == true)
                {
                    dir = 1;
                }
            }

            if (!userBombAdded(rowVal, colVal, dir, 3, 2))
            {
                labelOutOfRange.Content = "The coordinates are not valid";
                labelOutOfRange.Visibility = Visibility.Visible;
                textBoxColLandMine.Text = "";
                textBoxRowLandMine.Text = "";
            }
            else
            {
                if (dir == 0)
                {
                    placePlayerBombs(0, colVal, rowVal, "sh", 3);
                }
                else if (dir == 1)
                {
                    placePlayerBombs(1, colVal, rowVal, "sv", 3);
                }

                hideVisibility(lblColLandMine, lblRowLandMine, textBoxColLandMine, textBoxRowLandMine,
                    buttonOKLandMine, radioButtonVerticalLandMine, radioButtonHorizontalLandMine);

                btnLandMine.IsEnabled = false;
                if (allBombsPlaced())
                    btnStart.IsEnabled = true;
            }
        }

        /**
        * when ok button for CRUISER is clicked, it checks for user input,
        * adds the ships on the grid and respective arrays, hides the related labels
        * and textboxes, and make the button disable.
        */
        private void buttonOKTNT_Click(object sender, RoutedEventArgs e)
        {
            int colVal = 0;
            int rowVal = 0;

            int dir = 0;
            if (textBoxRowTNT.Text == "" || textBoxColTNT.Text == "")
            {
                labelOutOfRange.Content = "Please enter a valid value";
                labelOutOfRange.Visibility = Visibility.Visible;
            }
            if (!validateRowValue(textBoxRowTNT))
                return;
            else
                rowVal = convertLetterToNum(textBoxRowTNT.Text);

            if (!validateColumnValue(textBoxColTNT))
                return;
            else colVal = Int32.Parse(textBoxColTNT.Text) - 1;

            if (radioButtonHorizontalTNT.IsChecked == false && radioButtonVerticalTNT.IsChecked == false)
            {
                labelOutOfRange.Content = "Please choose an alignment!";
                labelOutOfRange.Visibility = Visibility.Visible;
                return;
            }

            else
            {
                if (radioButtonHorizontalTNT.IsChecked == true)
                {
                    dir = 0;
                }
                else if (radioButtonVerticalTNT.IsChecked == true)
                {
                    dir = 1;
                }
            }

            if (!userBombAdded(rowVal, colVal, dir, 3, 3))
            {
                labelOutOfRange.Content = "The coordinates are not valid";
                labelOutOfRange.Visibility = Visibility.Visible;
                textBoxColTNT.Text = "";
                textBoxRowTNT.Text = "";
            }
            else
            {
                if (dir == 0)
                {
                    placePlayerBombs(0, colVal, rowVal, "ch", 3);
                }
                else if (dir == 1)
                {
                    placePlayerBombs(1, colVal, rowVal, "cv", 3);
                }
                hideVisibility(lblColTNT, lblRowTNT, textBoxColTNT, textBoxRowTNT,
                    buttonOKTNT, radioButtonVerticalTNT, radioButtonHorizontalTNT);

                btnTNT.IsEnabled = false;
                if (allBombsPlaced())
                    btnStart.IsEnabled = true;
            }
        }



        /**
        * when ok button for DESTROYER is clicked, it checks for user input,
        * adds the ships on the grid and respective arrays, hides the related labels
        * and textboxes, and make the button disable.
        */
        private void buttonOKStun_Click(object sender, RoutedEventArgs e)
        {

            int colVal = 0;
            int rowVal = 0;

            int dir = 0;
            if (textBoxRowStun.Text == "" || textBoxColStun.Text == "")
            {
                labelOutOfRange.Content = "Please enter a valid value";
                labelOutOfRange.Visibility = Visibility.Visible;
            }

            if (!validateRowValue(textBoxRowStun))
                return;
            else
                rowVal = convertLetterToNum(textBoxRowStun.Text);

            if (!validateColumnValue(textBoxColStun))
                return;
            else colVal = Int32.Parse(textBoxColStun.Text) - 1;

            if (radioButtonHorizontalStun.IsChecked == false && radioButtonVerticalStun.IsChecked == false)
            {
                labelOutOfRange.Content = "Please choose an alignment!";
                labelOutOfRange.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                if (radioButtonHorizontalStun.IsChecked == true)
                {
                    dir = 0;
                }
                else if (radioButtonVerticalStun.IsChecked == true)
                {
                    dir = 1;
                }
            }

            if (!userBombAdded(rowVal, colVal, dir, 2, 4))
            {
                labelOutOfRange.Content = "The coordinates are not valid";
                labelOutOfRange.Visibility = Visibility.Visible;
                textBoxColStun.Text = "";
                textBoxRowStun.Text = "";
            }
            else
            {
                if (dir == 0)
                {
                    placePlayerBombs(0, colVal, rowVal, "dsh", 2);
                }
                else if (dir == 1)
                {
                    placePlayerBombs(1, colVal, rowVal, "dsv", 2);
                }
                hideVisibility(lblColStun, lblRowStun, textBoxColStun, textBoxRowStun,
                    buttonOKStun, radioButtonVerticalStun, radioButtonHorizontalStun);

                btnStun.IsEnabled = false;
                if (allBombsPlaced())
                    btnStart.IsEnabled = true;
            }
        }

        /**
        * When button is clicked , it shows the related labels and textboxes
        */
        private void btnAircarftCarrier_Click(object sender, RoutedEventArgs e)
        {
            showVisibility(lblCol, lblRow, txtCol, txtRow, btnOK, radioButtonVertical,
                       radioButtonHorizontal);

        }
        /*
         * When button is clicked , it shows the related labels and textboxes
         */
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            showVisibility(lblColGreanade, lblRowGrenade, textBoxColGrenade, textBoxRowGrenade,
                buttonOKGrenade, radioButtonVerticalGrenade, radioButtonHorizontalGrenade);
        }

        /*
         * When button is clicked , it shows the related labels and textboxes
         */
        private void btnLandMine_Click(object sender, RoutedEventArgs e)
        {
            showVisibility(lblColLandMine, lblRowLandMine, textBoxColLandMine, textBoxRowLandMine,
                buttonOKLandMine, radioButtonVerticalLandMine, radioButtonHorizontalLandMine);
        }
        /*
         * When button is clicked , it shows the related labels and textboxes
         */
        private void btnTNT_Click(object sender, RoutedEventArgs e)
        {
            showVisibility(lblColTNT, lblRowTNT, textBoxColTNT, textBoxRowTNT,
                buttonOKTNT, radioButtonVerticalTNT, radioButtonHorizontalTNT);
        }
        /*
        * When button is clicked , it shows the related labels and textboxes
        */
        private void btnStun_Click(object sender, RoutedEventArgs e)
        {
            showVisibility(lblColStun, lblRowStun, textBoxColStun, textBoxRowStun,
                buttonOKStun, radioButtonVerticalStun, radioButtonHorizontalStun);

        }
        /*
        * makes the labels and textboxes visible
        */
        private void showVisibility(Label lblCol, Label lblRow, TextBox txtBoxCol, TextBox txtBoxRow,
            Button btnOk, RadioButton vertical, RadioButton horizontal)
        {
            lblCol.Visibility = Visibility.Visible;
            lblRow.Visibility = Visibility.Visible;
            txtBoxCol.Visibility = Visibility.Visible;
            txtBoxRow.Visibility = Visibility.Visible;
            btnOk.Visibility = Visibility.Visible;
            vertical.Visibility = Visibility.Visible;
            horizontal.Visibility = Visibility.Visible;
        }
        /**
        * Converts the given string alphabet to a number for row label
        * Param String letter which is user input for the row
        * Return an int number corresponding to row value
        */
        private int convertLetterToNum(String letter)
        {
            int num = -1;
            switch (letter.ToUpper())
            {
                case "A":
                    return 0;
                case "B":
                    return 1;
                case "C":
                    return 2;
                case "D":
                    return 3;
                case "E":
                    return 4;
                case "F":
                    return 5;
                case "G":
                    return 6;
                case "H":
                    return 7;
                case "I":
                    return 8;
                case "J":
                    return 9;
            }
            return num;
        }
        /**
         * Validates the given textbox value to make sure it is in range
         * Param textcol is the textbox with user input
         * Return true if the input is valid,false if it is invalid
         */
        private Boolean validateColumnValue(TextBox textCol)
        {
            Boolean colValid = Regex.IsMatch(textCol.Text, @"^([1-9]|10)$");
            if (!colValid)
            {
                textCol.Text = "";
                labelOutOfRange.Content = "Enter a valid value for column";
                labelOutOfRange.Visibility = Visibility.Visible;
                return false;
            }
            else
                return true;
        }
        /*
         * Validates the given textbox value to make sure it is in range
         * Param textrow is the textbox with user input
         * Return true if the input is valid,false if it is invalid 
         */
        private Boolean validateRowValue(TextBox textRow)
        {

            Boolean rowValid = Regex.IsMatch(textRow.Text, @"^[a-jA-J]$");
            if (!rowValid)
            {
                textRow.Text = "";
                labelOutOfRange.Content = "Enter a valid value for row";
                labelOutOfRange.Visibility = Visibility.Visible;

                return false;
            }
            else return true;
        }

        /*
         * checks if the user input is valid and add the ship on the player grid array
         * and populate the userboat array. It also checks if the space around the ship
         * is available
         * Return true if the ship can be added,otherwise it returns false
         */
        private Boolean userBombAdded(int rowValue, int colValue, int dir, int bombLength, int bombValue)
        {
            int coord = rowValue * 10 + colValue;
            if (isBombCoordsValid(userGrid, dir, coord, bombLength) &&
                    checkSpaceAroundBomb(userGrid, dir, coord, bombLength))
            {
                userBomb[bombValue] = new int[bombLength];
                addBombToGrid(userGrid, userBomb[bombValue], dir, coord, bombLength);
                if (radioButtonSimple.IsChecked == true)
                    addSpaceAroungBomb(userGrid, dir, coord, bombLength);
                else
                    addSpaceAroundUserBombHard(dir, coord, bombLength, bombValue);
                return true;
            }
            else return false;
        }
        /**
         * hides the given labels and textboxes
         */
        private void hideVisibility(Label lblCol, Label lblRow, TextBox txtBoxCol, TextBox txtBoxRow,
            Button btnOk, RadioButton vertical, RadioButton horizontal)
        {
            lblCol.Visibility = Visibility.Hidden;
            lblRow.Visibility = Visibility.Hidden;
            txtBoxCol.Visibility = Visibility.Hidden;
            txtBoxRow.Visibility = Visibility.Hidden;
            btnOk.Visibility = Visibility.Hidden;
            vertical.Visibility = Visibility.Hidden;
            horizontal.Visibility = Visibility.Hidden;
            labelOutOfRange.Visibility = Visibility.Hidden;
        }
        /**
         * verifies that user name is entered, and all the ships are disabled
         * changes the visibility of labels and textboxes, copies userBoats
         */
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (txtPlayerName.Text.Equals(""))
                MessageBox.Show("Please enter your name.");
            else if (!allBombsPlaced())
            {
                labelOutOfRange.Content = "Please place all the ships first!";
                labelOutOfRange.Visibility = Visibility.Visible;
            }
            else
            {
                hideTheControls();
                controlsVisibility();
                labelPlayerWins.Content = "\'s Wins:";
                labelPlayerWins.Content = txtPlayerName.Text + labelPlayerWins.Content;
                computerSetup();
                allowGridClick = true;
                checkPlayer();
                //copies userBoats to userBoatsCopy to remove pictureboxes 
                for (int i = 0; i < 5; i++)
                {
                    int len = userBomb[i].Length;
                    userBombCopy[i] = new int[len];
                    Array.Copy(userBomb[i], userBombCopy[i], len);
                }
            }

        }
        private void hideTheControls()
        {
            lblHelp.Visibility = Visibility.Hidden;
            lblPlayerName.Visibility = Visibility.Hidden;
            txtPlayerName.Visibility = Visibility.Hidden;
            lblLevel.Visibility = Visibility.Hidden;
            lblYourBombs.Visibility = Visibility.Hidden;
            groupboxGrenade.Visibility = Visibility.Hidden;
            btnBlackBomb.Visibility = Visibility.Hidden;
            btnGrenade.Visibility = Visibility.Hidden;
            btnLandMine.Visibility = Visibility.Hidden;
            btnTNT.Visibility = Visibility.Hidden;
            btnStun.Visibility = Visibility.Hidden;
            btnStart.Visibility = Visibility.Hidden;
            labelOutOfRange.Visibility = Visibility.Hidden;

        }
        private void controlsVisibility()
        {
            labelPlayerWins.Visibility = Visibility.Visible;
            txtPlayerWinPoints.Visibility = Visibility.Visible;
            labelPlayerLosses.Visibility = Visibility.Visible;
            txtPlayerLossPoints.Visibility = Visibility.Visible;
            buttonOKPlay.Visibility = Visibility.Visible;
            textBoxPlayerCol.Visibility = Visibility.Visible;
            textBoxPlayerRow.Visibility = Visibility.Visible;
            labelPlayerCol.Visibility = Visibility.Visible;
            labelPlayerRow.Visibility = Visibility.Visible;
            buttonReset.Visibility = Visibility.Visible;
        }
        /**
         * resets the game setup 
         */
        private void buttonReset_Click(object sender, RoutedEventArgs e)
        {
            reset();
        }
        /**
       * hides the textboxes and labels for ships placements , and shows the textboxes and labels
       * to play
       */
        private void reset()
        {
            emptyTextBoxes();
            changeControlsVisibility();

            radioButtonSimple.IsChecked = true;
            btnBlackBomb.IsEnabled = true;
            btnGrenade.IsEnabled = true;
            btnTNT.IsEnabled = true;
            btnLandMine.IsEnabled = true;
            btnStun.IsEnabled = true;

            txtPlayerWinPoints.Text = "0";
            txtPlayerLossPoints.Text = "0";

            // makes the hit and miss buttons go back to normal color
            for (int i = 0; i < playerButtons.Length; i++)
            {
                playerButtons[i].Background = Brushes.Transparent;
                compButtons[i].Background = Brushes.Transparent;
            }

            // removes the pictureboxes from player's grid
            for (int i = 0; i < userBombCopy.Length; i++)
            {
                for (int j = 0; j < userBombCopy[i].Length; j++)
                {
                    int coord = userBombCopy[i][j];
                    playerButtons[coord].Content = null;
                }
            }
            //reset variables
            userBomb = new int[5][];
            userGrid = new int[10, 10];
            userBombSpaces = new int[5][];
            userMoves = new List<int>();
            move = 0;
            mode = 0;
            level = 0;
            killStep = 0;
            jump = false;
            pattern = 0;
            allowGridClick = false;
        }
        /**
         * Sets the text of textboxes to empty string
         */
        private void emptyTextBoxes()
        {
            txtPlayerName.Text = "";
            txtCol.Text = "";
            txtRow.Text = "";
            textBoxRowGrenade.Text = "";
            textBoxColGrenade.Text = "";
            textBoxColTNT.Text = "";
            textBoxRowTNT.Text = "";
            textBoxColLandMine.Text = "";
            textBoxRowLandMine.Text = "";
            textBoxColStun.Text = "";
            textBoxRowStun.Text = "";
            textBoxPlayerCol.Text = "";
            textBoxPlayerRow.Text = "";

        }
        /**
         * changes visibilty of controls
         */
        private void changeControlsVisibility()
        {
            btnBlackBomb.Visibility = Visibility.Visible;
            btnGrenade.Visibility = Visibility.Visible;
            btnTNT.Visibility = Visibility.Visible;
            btnLandMine.Visibility = Visibility.Visible;
            btnStun.Visibility = Visibility.Visible;
            lblYourBombs.Visibility = Visibility.Visible;
            lblLevel.Visibility = Visibility.Visible;
            lblPlayerName.Visibility = Visibility.Visible;
            txtPlayerName.Visibility = Visibility.Visible;
            radioButtonSimple.Visibility = Visibility.Visible;
            radioButtonIntelligent.Visibility = Visibility.Visible;
            btnStart.Visibility = Visibility.Visible;
            groupboxGrenade.Visibility = Visibility.Visible;
            labelPlayerLosses.Visibility = Visibility.Hidden;
            txtPlayerLossPoints.Visibility = Visibility.Hidden;
            txtPlayerWinPoints.Visibility = Visibility.Hidden;
            labelPlayerWins.Visibility = Visibility.Hidden;
            buttonOKPlay.Visibility = Visibility.Hidden;
            textBoxPlayerCol.Visibility = Visibility.Hidden;
            textBoxPlayerRow.Visibility = Visibility.Hidden;
            labelPlayerCol.Visibility = Visibility.Hidden;
            labelPlayerRow.Visibility = Visibility.Hidden;
            buttonReset.Visibility = Visibility.Hidden;
            lblHelp.Visibility = Visibility.Visible;
        }


        /**
         * places the ships pictures on the grid
         */
        private void placePlayerBombs(int dir, int colVal, int rowVal, String picName, int bombLength)
        {
            String pic = picName;

            if (dir == 0)
            {
                for (int i = 1; i <= bombLength; i++)
                {
                    picName = picName + i + ".jpg";
                    Image img = new Image();
                    img.Source = new BitmapImage(new Uri(picName, UriKind.Relative));

                    Button b = playerButtons[rowVal * 10 + colVal];
                    b.Content = img;

                    b.Background = Brushes.Transparent;
                    Grid.SetColumn(b, colVal);
                    Grid.SetRow(b, rowVal);

                    colVal = colVal + 1;
                    picName = pic;
                }
            }
            else
            {
                for (int i = 1; i <= bombLength; i++)
                {
                    picName = picName + i + ".jpg";
                    Image img = new Image();
                    img.Source = new BitmapImage(new Uri(picName, UriKind.Relative));
                    Button b = playerButtons[rowVal * 10 + colVal];
                    b.Content = img;
                    b.Background = Brushes.Transparent;

                    Grid.SetColumn(b, colVal);
                    Grid.SetRow(b, rowVal);

                    rowVal = rowVal + 1;
                    picName = pic;
                }
            }
        }
        /**
          * Verifies that all 5 ships are placed before starting the game
          * Return true if all ships are placed
          */
        private Boolean allBombsPlaced()
        {
            if (btnBlackBomb.IsEnabled)
                return false;
            if (btnGrenade.IsEnabled)
                return false;
            if (btnTNT.IsEnabled)
                return false;
            if (btnStun.IsEnabled)
                return false;
            if (btnLandMine.IsEnabled)
                return false;
            return true;
        }
        /**
         * creates the buttons on both grids to display ships 
         */
        private void createButtons(Grid gridName)
        {
            for (int row = 0; row <= 9; row++)
            {
                for (int col = 0; col <= 9; col++)
                {
                    Button b = new Button();
                    b.Background = Brushes.Transparent;
                    b.Name = "btn" + row + col;
                    b.Focusable = false;
                    if (gridName == gridPlayer)
                        playerButtons[row * 10 + col] = b;
                    else
                    {
                        compButtons[row * 10 + col] = b;
                        b.Click += new RoutedEventHandler(this.buttonGrid_Click);
                    }
                    Grid.SetColumn(b, col);
                    Grid.SetRow(b, row);
                    gridName.Children.Add(b);
                }
            }
        }

        /*
		 * Accepts and validates user's choice of coordinates for computer grid
		 * Only valid coordinates will be processed.
		 */
        private void buttonOKPlay_Click(object sender, RoutedEventArgs e)
        {
            int playerCol = 0;
            int playerRow = 0;

            if ( textBoxPlayerRow.Text == "" || textBoxPlayerCol.Text == "" )
            {
                labelOutOfRange.Content = "Please enter a valid value";
                labelOutOfRange.Visibility = Visibility.Visible;
             }

            if (!validateRowValue(textBoxPlayerRow))
                return;
            else
                playerRow = convertLetterToNum(textBoxPlayerRow.Text);
            if (!validateColumnValue(textBoxPlayerCol))
                return;
            else playerCol = Int32.Parse(textBoxPlayerCol.Text) - 1;

            int coords = playerRow * 10 + playerCol;
			if (inList(userMoves, coords))
			{
				labelOutOfRange.Visibility = Visibility.Visible;
				labelOutOfRange.Content = "You've already made this move";
				textBoxPlayerRow.Text = "";
				textBoxPlayerCol.Text = "";
			}
			else
			{
				labelOutOfRange.Visibility = Visibility.Hidden;
				play(coords);
                textBoxPlayerRow.Text = "";
                textBoxPlayerCol.Text = "";
			}
        }

        /*
         * Accepts user shot when (s)he clicks on the grid.
         */
        private void buttonGrid_Click(object sender, RoutedEventArgs e)
        {
            if (allowGridClick)
            {
                Button b = (Button)sender;
                int coords = Int32.Parse(b.Name.Substring(3, 2));
                if (inList(userMoves, coords))
                {
					labelOutOfRange.Visibility = Visibility.Visible;
					labelOutOfRange.Content = "You've already made this move";
                }
				else
				{
					labelOutOfRange.Visibility = Visibility.Hidden;
					play(coords);
				}
            }
        }

        /*
		 * Processes user's move. Indicates if user hits or sinks computer's boat(s).
		 * Then generates computer's move.
		 * param coords User-chosen validated coordinates for the next move.
		 */
        private void play(int coords)
        {
            userMoves.Add(coords);
            try
            {
                //find whether user hit computer's ship
                int result = findHitBomb(compBomb, coords);
				if(result >= 0)
                {
                    isBombHit(compBomb[result], coords);
                    bool isSunk = isBombBlast(compBomb[result], result);
                    if (!isSunk)
                        MessageBox.Show("You blast my " + intToStr(result) + "!");
                    else
                    {
                        if (allBlast(0))
                            return;
                    }
                }

                int compChoice = generateCompMove();
                if (allBlast(1))
                    return;
                move++;
            }
			//defensive design
            catch (Exception e)
            {
                MessageBox.Show("Sorry! Something went wrong. Restart the game!");
				Console.Write(e.Message);
            }
        }

        /*
		 * Checks whether the computer or user sunk all of the opponents ships.
		 * Displays the appropriate message and saves the results to file.
		 * param turn Indicates a player, where 0 is the user and 1 the computer.
		 * Returns true if all ships are sunk; false otherwise.
		 */
        private bool allBlast(int turn)
        {
            string player;
            int[][] array;
            TextBox text;
            bool blast;
            if (move >= 16)
            {
                if (turn == 0)
                {
                    player = txtPlayerName.Text;
                    array = compBomb;
                    text = txtPlayerWinPoints;
                }
                else
                {
                    player = "Computer";
                    array = userBomb;
                    text = txtPlayerLossPoints;
                }

                blast = allBombsBlast(array);
                if (blast)
                {
                    MessageBox.Show(player + " is the winner!");
                    text.Text = int.Parse(text.Text) + 1 + "";
                    saveResults();
                    allowGridClick = false;
                    buttonOKPlay.Visibility = Visibility.Hidden;
                    textBoxPlayerCol.Visibility = Visibility.Hidden;
                    textBoxPlayerRow.Visibility = Visibility.Hidden;
                    labelPlayerCol.Visibility = Visibility.Hidden;
                    labelPlayerRow.Visibility = Visibility.Hidden;
                    return true;
                }
            }
            return false;
        }
        /**
         * sets the game dificulty level to simple
         */
        private void radioButtonSimple_Checked(object sender, RoutedEventArgs e)
        {
            level = 0;
        }
        /**
         * sets the game dificulty level to intelligent/hard
         */
        private void radioButtonIntelligent_Checked(object sender, RoutedEventArgs e)
        {
            level = 1;
        }
    }
}
