using KDP_EC.App.Infraestructure.Repository.SQLite;

namespace KDP_EC.App.Views.Reports;

public partial class CoffeeSalesRep : ContentPage
{
    private readonly FarmsRepository _farmsRepository;
    private readonly CoffeeSalesRepository _coffeeSalesRepository;
    private readonly Guid _farmId;

    public CoffeeSalesRep(Guid FarmId)
    {
        InitializeComponent();
        _farmsRepository = new FarmsRepository();
        _coffeeSalesRepository = new CoffeeSalesRepository();

        _farmId = FarmId;
        List<int> a�os = new List<int>();
        int a�oActual = DateTime.Now.Year;
        for (int a�o = 2016; a�o <= a�oActual; a�o++)
        {
            a�os.Add(a�o);
        }

        yearPicker.ItemsSource = a�os;
        yearPicker.SelectedIndexChanged += YearPicker_SelectedIndexChanged;
    }

    private async void YearPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (yearPicker.SelectedIndex == -1)
            return;

        int selectedYear = (int)yearPicker.SelectedItem;

        var results = await _coffeeSalesRepository.GetLocalCoffeeSalesByFarmIdAndYear(_farmId, selectedYear);

        if (results != null && results.Count > 0)
        {
            reportListView.ItemsSource = results;
        }
        else
        {
            await DisplayAlert("Sin datos", "No hay informaci�n para el a�o seleccionado.", "OK");
            reportListView.ItemsSource = null;
        }
    }

}