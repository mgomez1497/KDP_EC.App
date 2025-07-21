using KDP_EC.App.Infraestructure.Repository.Sincronizar;
using KDP_EC.App.Infraestructure.Repository.SQLite;
using KDP_EC.Core.Models;
using FloweringModel = KDP_EC.Core.Models.FloweringRecords;

namespace KDP_EC.App.Views.FloweringRecords;

public partial class CreateFloweringRecords : ContentPage
{
    private readonly FarmsRepository _farmsRepository;
    private readonly StatesRepository _statesRepository;
    private readonly CitiesRepository _citiesRepository;
    private readonly FloweringRecordsRepository _floweringRecordsRepository;
    private readonly FloweringRecordsRepositoryAPI _floweringRecordsRepositoryAPI;

    private List<States> _todoslosdptos = new List<States>();
    private List<string> _tiposFloracion = new List<string> { "Alta", "Media", "Baja" };
    private string _base64Image = null;
    public CreateFloweringRecords()
    {
        InitializeComponent();
        _farmsRepository = new FarmsRepository();
        _floweringRecordsRepository = new FloweringRecordsRepository();
        _floweringRecordsRepositoryAPI = new FloweringRecordsRepositoryAPI();
        _statesRepository = new StatesRepository();
        _citiesRepository = new CitiesRepository();

        floweringDatePicker.Date = DateTime.Today;
        _ = InicializarDepartamentos();
        CargarTiposFloracion();
        ObtenerUbicacionAlCargar();

    }

    private void CargarTiposFloracion()
    {
        var tipos = new List<string> { "Alta", "Media", "Baja" };
        TipoFloracionPicker.ItemsSource = tipos;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        TipoFloracionPicker.ItemsSource = _tiposFloracion;
    }

    private void TiposIngresosPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (TipoFloracionPicker.SelectedIndex == -1)
            return;

        string tipoSeleccionado = TipoFloracionPicker.SelectedItem.ToString();
    }

    private async Task InicializarDepartamentos()
    {
        _todoslosdptos = await _statesRepository.GetLocalStates();

        if (_todoslosdptos != null && _todoslosdptos.Count > 0)
        {
            DepartamentosPicker.ItemsSource = _todoslosdptos;
        }
        else
        {
            await DisplayAlert("Error", "No se encontraron departamentos locales.", "OK");
        }
    }

    private async void ImagenButton_Clicked(object sender, EventArgs e)
    {
        string accion = await DisplayActionSheet("Selecciona una opción", "Cancelar", null, "Tomar Foto", "Elegir de Galería");

        FileResult file = null;

        if (accion == "Tomar Foto")
        {
            file = await MediaPicker.CapturePhotoAsync();
        }
        else if (accion == "Elegir de Galería")
        {
            file = await MediaPicker.PickPhotoAsync();
        }

        if (file != null)
        {
            var stream = await file.OpenReadAsync();
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            byte[] imageBytes = ms.ToArray();
            _base64Image = Convert.ToBase64String(imageBytes);

            await DisplayAlert("Imagen cargada", "Imagen convertida correctamente.", "OK");
        }
    }

    private async void OnDepartamentoSelected(object sender, EventArgs e)
    {
        if (DepartamentosPicker.SelectedIndex == -1)
            return;

        var departamentoSeleccionado = (States)DepartamentosPicker.SelectedItem;

        if (departamentoSeleccionado != null)
        {
            var ciudades = await _citiesRepository.GetLocalCitiesByState(departamentoSeleccionado.Id);

            if (ciudades != null && ciudades.Count > 0)
            {
                MunicipioPicker.ItemsSource = ciudades;
            }
            else
            {
                await DisplayAlert("Aviso", "No se encontraron ciudades para este departamento.", "OK");
                MunicipioPicker.ItemsSource = null;
            }
        }
    }


    //private async void OnAgregarCoordenadasClicked(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        var location = await Geolocation.GetLastKnownLocationAsync();

    //        if (location == null)
    //        {
    //            location = await Geolocation.GetLocationAsync(new GeolocationRequest
    //            {
    //                DesiredAccuracy = GeolocationAccuracy.Medium,
    //                Timeout = TimeSpan.FromSeconds(10)
    //            });
    //        }

    //        if (location != null)
    //        {
    //            _latitude = location.Latitude.ToString();
    //            _longitude = location.Longitude.ToString();


    //            if (BindingContext is FloweringModel flowering)
    //            {
    //                flowering.latitude = _latitude;
    //                flowering.longitude = _longitude;


    //                BindingContext = null;
    //                BindingContext = flowering;
    //            }

    //            await DisplayAlert("Ubicación capturada", $"Latitud: {_latitude}, Longitud: {_longitude}", "OK");
    //        }
    //        else
    //        {
    //            await DisplayAlert("Error", "No se pudo obtener la ubicación", "OK");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        await DisplayAlert("Error", $"No se pudo obtener la ubicación: {ex.Message}", "OK");
    //    }
    //}

    private async void ObtenerUbicacionAlCargar()
    {
        try
        {
            var location = await Geolocation.GetLastKnownLocationAsync();

            if (location == null)
            {
                location = await Geolocation.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.High,
                    Timeout = TimeSpan.FromSeconds(10)
                });
            }

            if (location != null)
            {
                var latitude = location.Latitude.ToString("F6");
                var longitude = location.Longitude.ToString("F6");
                var elevation = location.Altitude.HasValue ? location.Altitude.Value.ToString("F2") : "0.00";


                LatitudeEntry.Text = latitude;
                LongitudeEntry.Text = longitude;
                ElevationEntry.Text = elevation;


                if (BindingContext is FloweringModel flowering)
                {
                    flowering.latitude = latitude;
                    flowering.longitude = longitude;
                    flowering.elevation = elevation;


                    BindingContext = null;
                    BindingContext = flowering;
                }
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

    private async void GuardarButton_Clicked(object sender, EventArgs e)
    {
        if (DepartamentosPicker.SelectedItem is not States SelectedStates)
        {
            await DisplayAlert("Error", "Seleccione un departamento.", "OK");
            return;
        }

        if (MunicipioPicker.SelectedItem is not Cities SelectedCities)
        {
            await DisplayAlert("Error", "Seleccione un municipio.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(LatitudeEntry.Text) ||
            string.IsNullOrWhiteSpace(LongitudeEntry.Text) ||
            string.IsNullOrWhiteSpace(ElevationEntry.Text) ||
            TipoFloracionPicker.SelectedItem is null)
        {
            await DisplayAlert("Error", "Complete todos los campos requeridos.", "OK");
            return;
        }

        try
        {
            var userIdStr = Preferences.Get("UserId", Guid.Empty.ToString());
            var userId = Guid.TryParse(userIdStr, out var parsedId) ? parsedId : Guid.NewGuid();

            var flowering = new FloweringModel
            {
                Id = Guid.NewGuid(),
                floweringDate = floweringDatePicker.Date,
                Department = SelectedStates.Id,
                Municipality = SelectedCities.Id,
                Village = null,
                latitude = LatitudeEntry.Text,
                longitude = LongitudeEntry.Text,
                elevation = ElevationEntry.Text,
                file = _base64Image,
                floweringType = TipoFloracionPicker.SelectedItem?.ToString(),
                User = userId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                DeletedAt = null
            };

            await _floweringRecordsRepository.SaveFloweringRecordsLocally(new List<FloweringModel> { flowering });

            await DisplayAlert("Éxito", "Floración guardada correctamente.", "OK");


            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo guardar la floración: {ex.Message}", "OK");
        }
    }

    private async void OnSincronizarClicked(object sender, EventArgs e)
    {
        try
        {
            var registros = await _floweringRecordsRepository.GetLocalFloweringRecords();

            if (registros == null || !registros.Any())
            {
                await DisplayAlert("Aviso", "No hay floraciones locales para sincronizar.", "OK");
                return;
            }

            var exito = await _floweringRecordsRepositoryAPI.sincronizarFloraciones(registros);

            if (exito)
                await DisplayAlert("Éxito", "Floraciones sincronizadas correctamente con el servidor.", "OK");
            else
                await DisplayAlert("Error", "Ocurrió un error al sincronizar las floraciones.", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Fallo al sincronizar: {ex.Message}", "OK");
        }
    }


}

