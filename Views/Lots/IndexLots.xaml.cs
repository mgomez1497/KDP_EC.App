using KDP_EC.App.Infraestructure.Repository.SQLite;
using KDP_EC.Core.Models;
using LotModel = KDP_EC.Core.Models.Lots;
using FarmModel = KDP_EC.Core.Models.Farms;
using KDP_EC.App.Infraestructure.Repository.Sincronizar;

namespace KDP_EC.App.Views.Lots;

public partial class IndexLots : ContentPage
{
    private readonly FarmsRepository _farmsRepository;
    private readonly LotsRepository _lotsRepository;
    private readonly LotsRepositoryAPI _lotsRepositoryAPI;

    private FarmModel _selectedFarm;

    public IndexLots()
    {
        InitializeComponent();
        _farmsRepository = new FarmsRepository();
        _lotsRepository = new LotsRepository();
        _lotsRepositoryAPI = new LotsRepositoryAPI();
        LoadLocalFarms();
    }

    private async void LoadLocalFarms()
    {
        try
        {
            var farms = await _farmsRepository.GetLocalFarms();
            farmPickerLots.ItemsSource = farms;
            farmPickerLots.ItemDisplayBinding = new Binding("Name");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudieron cargar las fincas: {ex.Message}", "OK");
        }


    }

    private void OnFarmSelected(object sender, EventArgs e)
    {
        if (farmPickerLots.SelectedItem is FarmModel selectedFarm)
        {
            _selectedFarm = selectedFarm;
            createLotButton.IsVisible = true;
        }
        else
        {
            createLotButton.IsVisible = false;
        }
    }

    private async void OnCreateLot(object sender, EventArgs e)
    {
        if (_selectedFarm != null)
        {
            await Navigation.PushAsync(new AddLots(_selectedFarm.Id));
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_selectedFarm != null)
        {
            farmPickerLots.SelectedItem = _selectedFarm; 
            var lots = await _lotsRepository.GetLocalLotsByFarmId(_selectedFarm.Id);
            lotsCollection.ItemsSource = lots;
        }
    }

    private async void OnGetLots(object sender, EventArgs e)
    {
        if (farmPickerLots.SelectedItem is KDP_EC.Core.Models.Farms selectedFarm)
        {
            var lots = await _lotsRepository.GetLocalLotsByFarmId(selectedFarm.Id);
            lotsCollection.ItemsSource = lots;
        }
    }

    private async void OnUpdateView(object sender, EventArgs e)
    {
        var button = sender as Button;
        var lot = button?.BindingContext as LotModel;

        if (lot != null)
        {
            await Navigation.PushAsync(new EditLots(lot.Id));
        }
    }

    private async void OnSincronizarClicked(object sender, EventArgs e)
    {
        if (farmPickerLots.SelectedItem is KDP_EC.Core.Models.Farms selectedFarm)
        {
            try
            {
                var farmId = selectedFarm.Id;

                
                var lots = await _lotsRepository.GetLocalLotsByFarmId(farmId);

                if (lots == null || !lots.Any())
                {
                    await DisplayAlert("Sincronización", "No hay lotes para sincronizar.", "OK");
                    return;
                }

                
                var exito = await _lotsRepositoryAPI.SincronizarLotes(lots);

                
                if (exito)
                {
                    await DisplayAlert("✅ Éxito", "Los lotes fueron sincronizados correctamente.", "OK");
                }
                else
                {
                    await DisplayAlert("❌ Error", "Hubo un problema al sincronizar los lotes.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("❌ Excepción", $"Error inesperado: {ex.Message}", "OK");
            }
        }
        else
        {
            await DisplayAlert("⚠️ Atención", "Debe seleccionar una finca.", "OK");
        }
    }

}