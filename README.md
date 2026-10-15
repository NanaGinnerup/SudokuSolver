## Sudoku solver (work in progress)
This is a simple sudoku solver. At the moment it is set to completely solve a specified sudoku from a locally saved version of this [Kaggle sudoku puzzle data set](https://www.kaggle.com/datasets/rohanrao/sudoku). 
However, the plan is to expand this code to handle an image inputted from the user, translate it to a Sudoku as defined in this program, and then give the option of completely solving the sudoku or simply filling out one of the empty cells to help the user get unstuck in their own sudoku solving.
The translation of the image to a Sudoku could be either through a CNN or by using a llm - this is still to be decided.

### How to use

> **Note:** This project is still a work in progress, so the setup process may change.

1. Download the [Sudoku dataset from Kaggle](https://www.kaggle.com/datasets/rohanrao/sudoku) and place it in the `Data` folder within the project.
2. Open a terminal and navigate to the `SudokuSolver/SudokuSolver` directory.
3. Run the program using:
   ```bash
   dotnet run
   ```

   To solve a specific Sudoku puzzle, use the `--sudokuIndex` (or `-i`) flag followed by an integer:
   ```bash
   dotnet run --sudokuIndex 10
   ```
   This example selects the Sudoku puzzle at index `10`.

As of now, the program will only output an message if the sudoku was solved or not 
