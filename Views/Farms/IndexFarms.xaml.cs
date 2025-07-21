using KDP_EC.App.Infraestructure.Repository.SQLite;


namespace KDP_EC.App.Views.Farms;


public partial class IndexFarms : ContentPage
{


    private readonly FarmsRepository _farmsRepository;
    private List<KDP_EC.Core.Models.Farms> _farmsList;
    public IndexFarms()
    {
        InitializeComponent();
        _farmsRepository = new FarmsRepository();
        LoadLocalFarms();
    }

    private double _latitude;
    private double _longitude;

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


    private async void OnUpdateView(object sender, EventArgs e)
    {
        if (farmPicker.SelectedItem is KDP_EC.Core.Models.Farms selectedFarm)
        {
            await Navigation.PushAsync(new EditFarms(selectedFarm.Id));
        }
        else
        {
            await DisplayAlert("Atención", "Seleccione una finca primero.", "OK");
        }
    }

    private async void OnSincronizarClicked(object sender, EventArgs e)
    {
        if (farmPicker.SelectedItem is KDP_EC.Core.Models.Farms selectedFarm)
        {

            var latitud = selectedFarm.Latitude;
            var longitud = selectedFarm.Longitude;
            var fechaActualizacion = selectedFarm.UpdatedAt;


            await _farmsRepository.UpdateFarm(selectedFarm);

            
            bool resultado = await _farmsRepository.ActualizarUbicacionFincaAP(selectedFarm.Id);

            if (resultado)
            {
                await DisplayAlert("Éxito", "Ubicación sincronizada correctamente.", "OK");
            }
            else
            {
                await DisplayAlert("Error", "No se pudo sincronizar la ubicación.", "OK");
            }
        }
        else
        {
            await DisplayAlert("Atención", "Seleccione una finca primero.", "OK");
        }
    }







}