using CommunityToolkit.Maui.Views;
using System.Text;

namespace NumberMatch.Pages.Popups;

public partial class gridPopup : Popup
{
    public gridPopup(List<List<int>> grid)
    {
        InitializeComponent();

        StringBuilder sb = new();
        foreach (List<int> row in grid)
        {
            foreach (int number in row)
                sb.Append($"{number}-");
            sb.AppendLine();
        }

        lbl.Text = sb.ToString();
    }

    public gridPopup(String jsonGrid)
    {
        InitializeComponent();

        lbl.Text = jsonGrid;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Close();
    }
}