namespace KDP_EC.App.Views.Lots;

using KDP_EC.App.Infraestructure.Repository.SQLite;
using LotModel = KDP_EC.Core.Models.Lots;
public partial class AddLots : ContentPage
{
    private readonly LotsRepository _lotsRepository;
    private readonly Lots_TypeRepository _lots_TypeRepository;
    private readonly Lots_VarietysRepository _lots_VarietysRepository;
    private readonly Renewal_TypesRepository _renewal_TypesRepository;

    public AddLots(Guid farmId)
	{
		InitializeComponent();
        _lotsRepository = new LotsRepository();
        _lots_TypeRepository = new Lots_TypeRepository();
        _lots_VarietysRepository = new Lots_VarietysRepository();
        _renewal_TypesRepository = new Renewal_TypesRepository();

        BindingContext = new LotModel
        {
           
            FarmId = Guid.Parse(farmId.ToString())
            
        };


        LoadPickerData();

    }

    private double _latitude;
    private double _longitude;

    private readonly Guid CafeTypeId = Guid.Parse("f4bf6866-7c99-4b0b-a2a4-165f9ce5f590");
    private async void LoadPickerData()
    {
        try
        {
            var allTypes = await _lots_TypeRepository.GetAllLots_Types();
            TipoLotePicker.ItemsSource = allTypes;

            TipoLotePicker.SelectedIndexChanged += OnTipoLoteChanged;
            var allVarieties = await _lots_VarietysRepository.GetAllLots_Varietys();
            VariedadPicker.ItemsSource = allVarieties;

            var allRenewalTypes = await _renewal_TypesRepository.GetAllRenewal_Types();
            TipoRenovacionPicker.ItemsSource = allRenewalTypes;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudieron cargar los datos: {ex.Message}", "OK");
        }
    }

    private void OnTipoLoteChanged(object sender, EventArgs e)
    {
        if (TipoLotePicker.SelectedItem is not null && BindingContext is LotModel lot)
        {
            var selectedType = (KDP_EC.Core.Models.Lots_Type)TipoLotePicker.SelectedItem;
            lot.TypeLotId = selectedType.Id;

            bool isCafe = selectedType.Id == CafeTypeId;

            
            VariedadPicker.IsEnabled = isCafe;
            TreesDistanceEntry.IsEnabled = isCafe;
            GrooveDistanceEntry.IsEnabled = isCafe;
            DensityEntry.IsEnabled = isCafe;
            TreesNumberEntry.IsEnabled = isCafe;
            TipoRenovacionPicker.IsEnabled = isCafe;
            StemsByPlantsEntry.IsEnabled = isCafe;
            TotalStemsEntry.IsEnabled = isCafe;
            WorkDatePicker.IsEnabled = isCafe;

            
            if (!isCafe)
            {
                DisplayAlert("Advertencia", "Este tipo de lote no es Café. Solo puedes ingresar las hectáreas.", "OK");
            }
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


                if (BindingContext is LotModel lot)
                {
                    lot.Latitude = _latitude.ToString();
                    lot.Longitude = _longitude.ToString();


                    BindingContext = null;
                    BindingContext = lot;
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



    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        try
        {
            var lot = (LotModel)BindingContext;

           

            if (lot != null)
            {
               
                var cafeTypeId = Guid.Parse("f4bf6866-7c99-4b0b-a2a4-165f9ce5f590");

                
                if (lot.TypeLotId != cafeTypeId)
                {
                    lot.VarietyId = Guid.Parse("bddc39e5-e450-4bc0-8567-3ed4567b1db0");
                    lot.TypeReknewalId = Guid.Parse("b1e3c704-e5f7-4ff2-a4f1-34b7dc70c103");
                    lot.TreesDistance = 0;
                    lot.GrooveDistance = 0;
                    lot.Density = 0;
                    lot.TreesNumber = 0;
                    lot.StemsByPlants = 0;
                    lot.TotalStems = 0;
                    lot.Latitude = "0";
                    lot.Longitude = "0";
                    lot.WorkDate = null;
                }

                if (lot.Id == Guid.Empty)
                {
                    lot.Id = Guid.NewGuid();
                }

                var lotsRepository = new LotsRepository();


                await lotsRepository.SaveLotsLocally(new List<LotModel> { lot });

                await DisplayAlert("Éxito", "Lote guardado correctamente.", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "Por favor completa los campos requeridos.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Ocurrió un error al guardar el lote: {ex.Message}", "OK");
        }
    }
}
