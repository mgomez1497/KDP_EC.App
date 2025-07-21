using KDP_EC.App.Infraestructure.Repository.SQLite;
using FarmModel = KDP_EC.Core.Models.Farms;

namespace KDP_EC.App.Views.Farms;

public partial class EditFarms : ContentPage
{
    private readonly FarmsRepository _farmsRepository;
    private readonly VillagesRepository _villagesRepository;
    private readonly CompaniesRepository _companiesRepository;


    public EditFarms(Guid farmId)
	{
		InitializeComponent();
        _farmsRepository = new FarmsRepository();
        _villagesRepository = new VillagesRepository();
        _companiesRepository = new CompaniesRepository();
        LoadLocalFarms(farmId);
    }
    private double _latitude;
    private double _longitude;


    private async void LoadLocalFarms(Guid farmId)
    {
        try
        {
            var farms = await _farmsRepository.GetLocalFarms();
            var Farm = farms.FirstOrDefault();
            

            if (Farm != null)
            {
                BindingContext = Farm;

                var village = await _villagesRepository.GetLocalVeredas(Farm.VillageId);
                var company = await _companiesRepository.GetLocalCompanies(Farm.CompanyId);

            }
            else
            {
                await DisplayAlert("Advertencia", "Finca no encontrada.", "OK");
            }
        }
       
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudieron cargar las fincas: {ex.Message}", "OK");
        }


    }

    private async void OnAgregarCoordenadasClicked(object sender, EventArgs e)
    {
        try
        {
            var location = await Geolocation.GetLastKnownLocationAsync();

            if (location == null)
            {
                location = await Geolocation.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Medium,
                    Timeout = TimeSpan.FromSeconds(10)
                });
            }

            if (location != null)
            {
                _latitude = location.Latitude;
                _longitude = location.Longitude;

               
                if (BindingContext is FarmModel farm)
                {
                    farm.Latitude = (decimal)_latitude;
                    farm.Longitude = (decimal)_longitude;

                   
                    BindingContext = null;
                    BindingContext = farm;
                }

                await DisplayAlert("Ubicación capturada", $"Latitud: {_latitude}, Longitud: {_longitude}", "OK");
            }
            else
            {
                await DisplayAlert("Error", "No se pudo obtener la ubicación", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo obtener la ubicación: {ex.Message}", "OK");
        }
    }


    private async void OnGuardarCambiosClicked(object sender, EventArgs e)
    {
        try
        {
            if (BindingContext is FarmModel farm)
            {
                farm.UpdatedAt = DateTime.UtcNow;
                
                await _farmsRepository.UpdateFarm(farm);

                await DisplayAlert("Éxito", "Finca actualizada correctamente.", "OK");
                await Navigation.PushAsync(new IndexFarms());
            }
            else
            {
                await DisplayAlert("Error", "No se pudo obtener la información de la finca.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo actualizar la finca: {ex.Message}", "OK");
        }
    }
    
}




