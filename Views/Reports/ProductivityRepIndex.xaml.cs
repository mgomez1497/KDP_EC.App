using KDP_EC.App.Infraestructure.Repository.SQLite;

namespace KDP_EC.App.Views.Reports;

public partial class ProductivityRepIndex : ContentPage
{
    private readonly FarmsRepository _farmsRepository;
    private readonly ProductivityReportRepository _productivityReportRepository;
    private readonly Guid _farmId;

    public ProductivityRepIndex(Guid FarmId)
    {
        InitializeComponent();
        _farmsRepository = new FarmsRepository();
        _productivityReportRepository = new ProductivityReportRepository();

        _farmId = FarmId;

        List<int> años = new List<int>();
        int añoActual = DateTime.Now.Year;
        for (int año = 2016; año <= añoActual; año++)
        {
            años.Add(año);
        }
        yearPicker.ItemsSource = años;
        yearPicker.SelectedIndexChanged += YearPicker_SelectedIndexChanged;
    }

    private async void YearPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (yearPicker.SelectedIndex == -1)
            return;

        int selectedYear = (int)yearPicker.SelectedItem;
        var results = await _productivityReportRepository.GetLocalProductivityReportsByFarmIdAndYear(_farmId, selectedYear);
        if (results != null && results.Count > 0)
        {
            reportListView.ItemsSource = results;
        }
        else
        {
            await DisplayAlert("Sin datos", "No hay información para el año seleccionado.", "OK");
            reportListView.ItemsSource = null;
        }
    }
}