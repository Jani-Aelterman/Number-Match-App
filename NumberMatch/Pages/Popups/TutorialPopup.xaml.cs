using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NumberMatch.Pages.Popups;

public partial class TutorialPopup : Popup
{
    private int currentStep = 0;
    private readonly int rows = 4, cols = 4;

    private readonly List<TutorialStep> steps;

    // current selection
    private readonly List<(int r, int c)> selection = new();

    // current hinted pair (to clear when changing steps)
    private ((int r, int c) a, (int r, int c) b)? currentHint;

    private readonly Color inversePrimaryColor;
    private readonly Color primaryColor;
    private readonly Color backgroundColor;

    public TutorialPopup()
    {
        InitializeComponent();

        inversePrimaryColor = (Color)(Application.Current?.Resources["InversePrimary"] ?? Colors.Gray);
        primaryColor = (Color)(Application.Current?.Resources["Primary"] ?? Colors.Blue);
        backgroundColor = (Color)(Application.Current?.Resources["Background"] ?? Colors.Black);

        // define interactive steps: each step provides a grid config and instruction
        steps = new List<TutorialStep>
        {
            new TutorialStep
            {
                Title = "Horizontal match",
                Description = "Select two tiles on the same row with only empty cells between them. Values must match or sum to 10.",
                Grid = new int[,]
                {
                    {5,0,0,5},
                    {1,2,3,4},
                    {6,7,8,9},
                    {1,2,3,4}
                }
            },
            new TutorialStep
            {
                Title = "Vertical match",
                Description = "Select two tiles in the same column with only empty cells between them. Values must match or sum to 10.",
                Grid = new int[,]
                {
                    {1,7,2,3},
                    {4,0,5,6},
                    {8,0,9,1},
                    {2,7,3,4}
                }
            },
            new TutorialStep
            {
                Title = "Diagonal match",
                Description = "Select two tiles on the same diagonal with only empty tiles along the diagonal between them.",
                Grid = new int[,]
                {
                    {3,1,2,4},
                    {5,0,6,7},
                    {8,9,0,1},
                    {2,3,4,3}
                }
            },
            new TutorialStep
            {
                Title = "Row-match (adjacent rows)",
                Description = "Select two tiles on adjacent rows that match the special row-match rule (see description).",
                Grid = new int[,]
                {
                    {1,2,3,4},
                    {7,0,0,0}, // row 1 (higher), col 0
                    {0,0,0,7}, // row 2 (lower), col 3
                    {1,2,3,4}
                }
            }
        };

        // apply dynamic styling to grid children
        foreach (var child in IllustrationGrid.Children)
        {
            if (child is Button btn)
            {
                btn.SetDynamicResource(Button.BorderColorProperty, "Primary");
                btn.SetDynamicResource(Button.TextColorProperty, "Primary");
                btn.SetDynamicResource(Button.BackgroundColorProperty, "Background");
            }
        }

        LoadStep();
    }

    private void LoadStep()
    {
        // clear any state from previous step
        selection.Clear();
        //ClearHint();
        NextButton.IsEnabled = false;

        var step = steps[currentStep];
        TitleLabel.Text = step.Title;
        DescriptionLabel.Text = step.Description;
        BackButton.IsVisible = currentStep > 0;
        NextButton.IsVisible = currentStep < steps.Count - 1;

        // populate illustration grid and ensure default styles
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                Button b = GetCellButton(r, c);
                int val = step.Grid[r, c];
                b.Text = val == 0 ? null : val.ToString();

                // restore default styling
                b.SetDynamicResource(Button.BackgroundColorProperty, "Background");
                b.SetDynamicResource(Button.TextColorProperty, "Primary");
                b.Opacity = 1;
            }
        }

        // show hint by highlighting one valid match in inverse primary
        var hint = FindValidMatchInGrid(step.Grid);
        if (hint != null)
        {
            currentHint = hint.Value;
            var b1 = GetCellButton(hint.Value.a.r, hint.Value.a.c);
            var b2 = GetCellButton(hint.Value.b.r, hint.Value.b.c);
            if (b1 != null)
            {
                b1.BackgroundColor = inversePrimaryColor;
                b1.TextColor = backgroundColor;
            }
            if (b2 != null)
            {
                b2.BackgroundColor = inversePrimaryColor;
                b2.TextColor = backgroundColor;
            }
        }
    }

    private void ClearHint()
    {
        if (currentHint == null) return;

        var a = currentHint.Value.a;
        var b = currentHint.Value.b;

        var btnA = GetCellButton(a.r, a.c);
        var btnB = GetCellButton(b.r, b.c);

        if (btnA != null)
        {
            btnA.SetDynamicResource(Button.BackgroundColorProperty, "Background");
            btnA.SetDynamicResource(Button.TextColorProperty, "Primary");
        }
        if (btnB != null)
        {
            btnB.SetDynamicResource(Button.BackgroundColorProperty, "Background");
            btnB.SetDynamicResource(Button.TextColorProperty, "Primary");
        }

        currentHint = null;
    }

    private ((int r, int c) a, (int r, int c) b)? FindValidMatchInGrid(int[,] grid)
    {
        int rCount = grid.GetLength(0);
        int cCount = grid.GetLength(1);

        for (int r1 = 0; r1 < rCount; r1++)
        {
            for (int c1 = 0; c1 < cCount; c1++)
            {
                int v1 = grid[r1, c1];
                if (v1 == 0) continue;

                for (int r2 = r1; r2 < rCount; r2++)
                {
                    int startC = (r2 == r1) ? c1 + 1 : 0;
                    for (int c2 = startC; c2 < cCount; c2++)
                    {
                        int v2 = grid[r2, c2];
                        if (v2 == 0) continue;

                        if (v1 == v2 || v1 + v2 == 10)
                        {
                            if (PositionMatchesRuleForGrid(grid, r1, c1, r2, c2))
                                return ((r1, c1), (r2, c2));
                        }
                    }
                }
            }
        }

        return null;
    }

    private bool PositionMatchesRuleForGrid(int[,] grid, int r1, int c1, int r2, int c2)
    {
        // Horizontal
        if (r1 == r2)
        {
            if (c2 < c1) (c1, c2) = (c2, c1);
            for (int col = c1 + 1; col < c2; col++)
                if (grid[r1, col] != 0) return false;
            return true;
        }

        // Vertical
        if (c1 == c2)
        {
            if (r2 < r1) (r1, r2) = (r2, r1);
            for (int row = r1 + 1; row < r2; row++)
                if (grid[row, c1] != 0) return false;
            return true;
        }

        // Diagonal
        if (Math.Abs(r1 - r2) == Math.Abs(c1 - c2))
        {
            int steps = Math.Abs(r1 - r2);
            int rdir = (r2 > r1) ? 1 : -1;
            int cdir = (c2 > c1) ? 1 : -1;
            for (int i = 1; i < steps; i++)
                if (grid[r1 + i * rdir, c1 + i * cdir] != 0) return false;
            return true;
        }

        // RowMatch (adjacent rows)
        if (Math.Abs(r1 - r2) == 1)
        {
            int higherRow = Math.Min(r1, r2);
            int lowerRow = Math.Max(r1, r2);
            int colHigherRow = (r1 > r2) ? c2 : c1;
            int colLowerRow = (r1 < r2) ? c2 : c1;

            for (int col = 0; col < colHigherRow; col++)
                if (grid[higherRow, col] != 0) return false;

            for (int col = colLowerRow + 1; col < cols; col++)
                if (grid[lowerRow, col] != 0) return false;

            return true;
        }

        return false;
    }

    private Button GetCellButton(int r, int c)
    {
        foreach (var child in IllustrationGrid.Children)
        {
            if (IllustrationGrid.GetRow(child) == r && IllustrationGrid.GetColumn(child) == c && child is Button btn)
                return btn;
        }
        return null;
    }

    private async void IllustrationButton_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button btn) return;

        int r = IllustrationGrid.GetRow(btn);
        int c = IllustrationGrid.GetColumn(btn);

        // ignore empty cells
        if (string.IsNullOrWhiteSpace(btn.Text)) return;

        // toggle selection
        var tuple = (r, c);
        if (selection.Contains(tuple))
        {
            // deselect
            selection.Remove(tuple);
            btn.SetDynamicResource(Button.BackgroundColorProperty, "Background");
            btn.SetDynamicResource(Button.TextColorProperty, "Primary");
            return;
        }

        // select
        selection.Add(tuple);
        btn.BackgroundColor = primaryColor;
        btn.TextColor = backgroundColor;

        // when two selected -> validate
        if (selection.Count == 2)
        {
            var a = selection[0];
            var b = selection[1];

            bool valueMatch = ValuesMatch(a, b);
            bool positionMatch = PositionMatchesRule(a, b);

            if (valueMatch && positionMatch)
            {
                // success: animate and mark cells as removed
                var firstBtn = GetCellButton(a.r, a.c);
                var secondBtn = GetCellButton(b.r, b.c);

                if (firstBtn != null) await firstBtn.ScaleTo(1.1, 80);
                if (secondBtn != null) await secondBtn.ScaleTo(1.1, 80);

                // remove (set empty)
                if (firstBtn != null) firstBtn.Text = null;
                if (secondBtn != null) secondBtn.Text = null;

                // restore all buttons to default so nothing stays selected
                ResetAllButtonsToDefault();

                // mark step complete
                NextButton.IsEnabled = true;
                DescriptionLabel.Text = "Correct! You can proceed.";
            }
            else
            {
                // wrong selection: feedback & reset selection highlight
                DescriptionLabel.Text = "That is not a valid match. Try again.";
                await ShakeButtons(GetCellButton(a.r, a.c), GetCellButton(b.r, b.c));

                // restore appearance
                var b1 = GetCellButton(a.r, a.c);
                var b2 = GetCellButton(b.r, b.c);
                if (b1 != null) { b1.SetDynamicResource(Button.BackgroundColorProperty, "Background"); b1.SetDynamicResource(Button.TextColorProperty, "Primary"); }
                if (b2 != null) { b2.SetDynamicResource(Button.BackgroundColorProperty, "Background"); b2.SetDynamicResource(Button.TextColorProperty, "Primary"); }
            }

            // clear selection and ensure no residual highlights
            selection.Clear();
        }
    }

    private bool ValuesMatch((int r, int c) a, (int r, int c) b)
    {
        var b1 = GetCellButton(a.r, a.c);
        var b2 = GetCellButton(b.r, b.c);
        if (b1 == null || b2 == null) return false;
        if (!int.TryParse(b1.Text, out int v1)) return false;
        if (!int.TryParse(b2.Text, out int v2)) return false;

        return v1 == v2 || v1 + v2 == 10;
    }

    private bool PositionMatchesRule((int r, int c) a, (int r, int c) b)
    {
        int r1 = a.r, c1 = a.c, r2 = b.r, c2 = b.c;

        // Horizontal
        if (r1 == r2)
        {
            if (c2 < c1) (c1, c2) = (c2, c1);
            for (int col = c1 + 1; col < c2; col++)
            {
                var btn = GetCellButton(r1, col);
                if (btn != null && !string.IsNullOrWhiteSpace(btn.Text)) return false;
            }
            return true;
        }

        // Vertical
        if (c1 == c2)
        {
            if (r2 < r1) (r1, r2) = (r2, r1);
            for (int row = r1 + 1; row < r2; row++)
            {
                var btn = GetCellButton(row, c1);
                if (btn != null && !string.IsNullOrWhiteSpace(btn.Text)) return false;
            }
            return true;
        }

        // Diagonal
        if (Math.Abs(r1 - r2) == Math.Abs(c1 - c2))
        {
            int steps = Math.Abs(r1 - r2);
            int rdir = (r2 > r1) ? 1 : -1;
            int cdir = (c2 > c1) ? 1 : -1;

            for (int i = 1; i < steps; i++)
            {
                var btn = GetCellButton(r1 + i * rdir, c1 + i * cdir);
                if (btn != null && !string.IsNullOrWhiteSpace(btn.Text)) return false;
            }
            return true;
        }

        // RowMatch (adjacent rows)
        if (Math.Abs(r1 - r2) == 1)
        {
            int higherRow = Math.Min(r1, r2);
            int lowerRow = Math.Max(r1, r2);
            int colHigherRow = (r1 > r2) ? c2 : c1;
            int colLowerRow = (r1 < r2) ? c2 : c1;

            // check columns before higherRow's selected column are empty
            for (int col = 0; col < colHigherRow; col++)
            {
                var btn = GetCellButton(higherRow, col);
                if (btn != null && !string.IsNullOrWhiteSpace(btn.Text)) return false;
            }

            // check columns after lowerRow's selected column are empty
            for (int col = colLowerRow + 1; col < cols; col++)
            {
                var btn = GetCellButton(lowerRow, col);
                if (btn != null && !string.IsNullOrWhiteSpace(btn.Text)) return false;
            }

            return true;
        }

        return false;
    }

    private async Task ShakeButtons(Button a, Button b)
    {
        var tasks = new List<Task>();
        if (a != null) tasks.Add(Shake(a));
        if (b != null) tasks.Add(Shake(b));
        await Task.WhenAll(tasks);
    }

    private async Task Shake(Button btn)
    {
        if (btn == null) return;
        await btn.TranslateTo(-6, 0, 40);
        await btn.TranslateTo(6, 0, 40);
        await btn.TranslateTo(-3, 0, 30);
        await btn.TranslateTo(0, 0, 20);
    }

    private void BackButton_Clicked(object sender, EventArgs e)
    {
        if (currentStep <= 0) return;
        currentStep--;
        LoadStep();
    }

    private void NextButton_Clicked(object sender, EventArgs e)
    {
        if (currentStep >= steps.Count - 1) return;
        currentStep++;
        LoadStep();
    }

    private void CloseButton_Clicked(object sender, EventArgs e)
    {
        CloseAsync();
    }

    private void ResetAllButtonsToDefault()
    {
        // clear selection and hint and restore style for all buttons
        selection.Clear();
        ClearHint();

        foreach (var child in IllustrationGrid.Children)
        {
            if (child is Button btn)
            {
                btn.SetDynamicResource(Button.BackgroundColorProperty, "Background");
                btn.SetDynamicResource(Button.TextColorProperty, "Primary");
            }
        }
    }

    private class TutorialStep
    {
        public string Title { get; init; }
        public string Description { get; init; }
        public int[,] Grid { get; init; }
    }
}