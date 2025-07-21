using KDP_EC.App.Infraestructure.Repository.SQLite;
using KDP_EC.Core.Models;
using LotModel = KDP_EC.Core.Models.Lots;

namespace KDP_EC.App.Views.Lots;

public partial class EditLots : ContentPage
{
	private readonly LotsRepository _lotsRepository;
	private readonly Lots_TypeRepository _lots_TypeRepository;
	private readonly Lots_VarietysRepository _lots_VarietysRepository;
	private readonly Renewal_TypesRepository _renewal_TypesRepository;
	public EditLots(Guid LotId)
	{
		InitializeComponent();
		_lotsRepository = new LotsRepository();
		_lots_TypeRepository = new Lots_TypeRepository();
		_lots_VarietysRepository = new Lots_VarietysRepository();
		_renewal_TypesRepository = new Renewal_TypesRepository();
        LoadLocalLots(LotId);
    }

    private double _latitude;
    private double _longitude;

    private bool _isCafeLot = false;
    private async void LoadLocalLots(Guid LotId)
	{
		try
		{


			var lots = await _lotsRepository.GetLocalLotsById(LotId);

			var Lot = lots.FirstOrDefault();

			if (Lot != null)
			{
				BindingContext = Lot;

                var allTypes = await _lots_TypeRepository.GetAllLots_Types();
                TipoLotePicker.ItemsSource = allTypes;
                var selectedType = allTypes.FirstOrDefault(t => t.Id == Lot.TypeLotId);
                TipoLotePicker.SelectedItem = selectedType;

                var cafeTypeId = Guid.Parse("f4bf6866-7c99-4b0b-a2a4-165f9ce5f590");

                if (Lot.TypeLotId != cafeTypeId)
                {
                    
                    VariedadPicker.IsEnabled = false;
                    TipoRenovacionPicker.IsEnabled = false;
                    DensityEntry.IsEnabled = false;
                    DistanceTreesEntry.IsEnabled = false;
                    DistanceGrooveEntry.IsEnabled = false;
                    TreesNumberEntry.IsEnabled = false;
                    TipoLotePicker.IsEnabled = false;
                    VariedadPicker.IsEnabled= false;
                    TipoRenovacionPicker.IsEnabled = false;
                }

                else
                {
                    var allVarieties = await _lots_VarietysRepository.GetAllLots_Varietys();
                    VariedadPicker.ItemsSource = allVarieties;
                    var selectedVariety = allVarieties.FirstOrDefault(v => v.Id == Lot.VarietyId);
                    VariedadPicker.SelectedItem = selectedVariety;


                    var allRenewalTypes = await _renewal_TypesRepository.GetAllRenewal_Types();
                    TipoRenovacionPicker.ItemsSource = allRenewalTypes;
                    var selectedRenewalType = allRenewalTypes.FirstOrDefault(r => r.Id == Lot.TypeReknewalId);
                    TipoRenovacionPicker.SelectedItem = selectedRenewalType;
                }

            }

			else
			{
				await DisplayAlert("Error", "No se encontró el lote.", "OK");
				await Navigation.PopAsync();
			}
		}
		catch (Exception ex)
		{
			await DisplayAlert("Error", $"No se pudieron cargar los lotes: {ex.Message}", "OK");

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


    private async void OnGuardarCambiosClicked(object sender, EventArgs e)
    {
        try
        {
            if (BindingContext is LotModel lot)
            {
                // Obtener las selecciones actuales de los Pickers
                var selectedType = TipoLotePicker.SelectedItem as Lots_Type;
                var selectedVariety = VariedadPicker.SelectedItem as Lots_Varietys;
                var selectedRenewal = TipoRenovacionPicker.SelectedItem as Renewal_Types;

                // Asignar los IDs al objeto lot
                if (selectedType != null)
                    lot.TypeLotId = selectedType.Id;

                if (selectedVariety != null)
                    lot.VarietyId = selectedVariety.Id;

                if (selectedRenewal != null)
                    lot.TypeReknewalId = selectedRenewal.Id;

               
                await _lotsRepository.SaveLotsLocally(new List<LotModel> { lot });
                await DisplayAlert("Éxito", "Lote actualizado correctamente.", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "No se pudo guardar el lote.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudieron guardar los cambios: {ex.Message}", "OK");
        }
    }

}