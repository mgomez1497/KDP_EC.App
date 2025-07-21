using KDP_EC.App.Infraestructure.Repository.SQLite;
using FarmModel = KDP_EC.Core.Models.Farms;

namespace KDP_EC.App.Views.Reports;

public partial class IndexReports : ContentPage
{
    private readonly FarmsRepository _farmsRepository;

    private FarmModel _selectedFarm;
    public IndexReports()
	{
		InitializeComponent();
        _farmsRepository = new FarmsRepository();
        LoadLocalFarms();
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_selectedFarm != null)
        {
            farmPicker.SelectedItem = _selectedFarm;
           
        }
    }  

    private async void LoadLocalFarms()
    {
        try
        {
            var farms = await _farmsRepository.GetLocalFarms();
            farmPicker.ItemsSource = farms;
            farmPicker.ItemDisplayBinding = new Binding("Name");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudieron cargar las fincas: {ex.Message}", "OK");
        }


    }

    private void OnFarmSelected(object sender, EventArgs e)
    {
        if (farmPicker.SelectedItem is FarmModel selectedFarm)
        {
            _selectedFarm = selectedFarm;
            
        }
        else
        {
            
        }
    }

    private async void OnVolumenCafeClicked(object sender, EventArgs e)
    {
        if (farmPicker.SelectedItem is KDP_EC.Core.Models.Farms selectedFarm)
        {
            await Navigation.PushAsync(new CoffeeSalesRep(selectedFarm.Id));
        }
        else
        {
            await DisplayAlert("Atención", "Seleccione una finca primero.", "OK");
        }
    }
    private async void OnProductivityReportClicked(object sender, EventArgs e)
    {
        if (farmPicker.SelectedItem is KDP_EC.Core.Models.Farms selectedFarm)
        {
            await Navigation.PushAsync(new ProductivityRepIndex(selectedFarm.Id));
        }
        else
        {
            await DisplayAlert("Atención", "Seleccione una finca primero.", "OK");
        }
    }

    private async void OnBalanceReportClicked(object sender, EventArgs e)
    {
        if (farmPicker.SelectedItem is KDP_EC.Core.Models.Farms selectedFarm)
        {
            await Navigation.PushAsync(new BalanceCostCentersReports(selectedFarm.Id));
        }
        else
        {
            await DisplayAlert("Atención", "Seleccione una finca primero.", "OK");
        }
    }
}






