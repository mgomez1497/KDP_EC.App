using KDP_EC.App.Infraestructure.Repository.SQLite;
using KDP_EC.Core.Interfaces;
using KDP_EC.Core.Models;
using incModel = KDP_EC.Core.Models.Incomes;

namespace KDP_EC.App.Views.Incomes;

public partial class CreateIncomes : ContentPage
{
    private readonly FarmsRepository _farmsRepository;
    private readonly IncomesRepository _incomesRepository;
    private readonly IncomesTypesRepository _incomesTypesRepository;

    private List<KDP_EC.Core.Models.IncomesTypes> _todosLosTiposIngresos;
    public CreateIncomes(Guid farmId)
	{
		InitializeComponent();

        _farmsRepository = new FarmsRepository();
        _incomesRepository = new IncomesRepository();
        _incomesTypesRepository = new IncomesTypesRepository();
        BindingContext = new KDP_EC.Core.Models.Incomes
        {
            FarmId = farmId,
            Date = DateTime.Today
        };
        FechaDatePicker.Date = DateTime.Today;
        _ = InicializarTiposIngresosAsync();
    }

    private async Task InicializarTiposIngresosAsync()
    {
        _todosLosTiposIngresos = await _incomesTypesRepository.GetLocalIncomesTypes();
        if (_todosLosTiposIngresos != null && _todosLosTiposIngresos.Count > 0)
        {
            TiposIngresosPicker.ItemsSource = _todosLosTiposIngresos;
        }
        else
        {
            await DisplayAlert("Error", "No se encontraron tipos de ingresos locales.", "OK");
        }
    }

    private async void TiposIngresosPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
       var selectedType = (KDP_EC.Core.Models.IncomesTypes)TiposIngresosPicker.SelectedItem;
        
        
        if (selectedType.Id == Guid.Parse("edd4cca8-a8f5-4196-b566-989f442f69f5") || selectedType.Id == Guid.Parse("fe0541b2-3d87-4105-9bad-3bc53ae185d6"))
        {
            KilogramosLabel.IsVisible = false;
            KilogramosEntry.IsVisible = false;
            ValorPorKgLabel.IsVisible = false;
            ValorPorKgEntry.IsVisible = false;
            FactorRendimientoLabel.IsVisible = false;
            FactorRendimientoEntry.IsVisible = false;
            AlmendraSanaLabel.IsVisible = false;
            AlmendraSanaEntry.IsVisible = false;
            PasillaLabel.IsVisible = false;
            PasillaEntry.IsVisible = false;
            MermaLabel.IsVisible = false;
            MermaEntry.IsVisible = false;

        }

        else
        {
            ValorPorKgEntry.IsReadOnly = false;
            FactorRendimientoEntry.IsReadOnly = false;
            AlmendraSanaEntry.IsReadOnly = false;
            PasillaEntry.IsReadOnly = false;
            MermaEntry.IsReadOnly = false;
        }


    }

    private void TotalValueEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        bool esValido = !string.IsNullOrWhiteSpace(TotalValueEntry.Text)
                        && decimal.TryParse(TotalValueEntry.Text, out decimal valor)
                        && valor > 0;

        GuardarButton.IsEnabled = esValido;
        GuardarButton.BackgroundColor = esValido
            ? Color.FromArgb("#e7763d") 
            : Color.FromArgb("#80e7763d"); 
    }


    private async void GuardarButton_Clicked(object sender, EventArgs e)
    {
       if(TiposIngresosPicker.SelectedItem is IncomesTypes selectedIncomesTypes)
        {
            var incomes = new incModel
            {
                Id = Guid.NewGuid(),
                TypeId = selectedIncomesTypes.Id,
                Date = FechaDatePicker.Date,
                TotalValue = string.IsNullOrWhiteSpace(TotalValueEntry.Text) ? 0 : decimal.Parse(TotalValueEntry.Text),
                KgSold = string.IsNullOrEmpty(KilogramosEntry.Text) ? 0 : decimal.Parse(KilogramosEntry.Text),
                HealthyAlmondPercent = string.IsNullOrWhiteSpace(AlmendraSanaEntry.Text) ? 0 : decimal.Parse(AlmendraSanaEntry.Text),
                PoorCoffeePercent = string.IsNullOrWhiteSpace(PasillaEntry.Text) ? 0 : decimal.Parse(PasillaEntry.Text),
                DecreasePercent = string.IsNullOrWhiteSpace(MermaEntry.Text) ? 0 : decimal.Parse(MermaEntry.Text),
                PerformanceFactor = string.IsNullOrWhiteSpace(FactorRendimientoEntry.Text) ? 0 : decimal.Parse(FactorRendimientoEntry.Text),
                FarmId = ((incModel)BindingContext).FarmId,
                CreatedAt = DateTime.Now,
                UpdatedAt = null,
                DeletedAt = null,
                PercentageKg = string.IsNullOrEmpty(ValorPorKgEntry.Text) ? 0 : decimal.Parse(ValorPorKgEntry.Text),
                Invoice = FacturaEntry.Text,
                


            };

            await _incomesRepository.SaveIncomesLocally(new List<incModel> { incomes });

            var guaradado = await _incomesRepository.GetLocalIncomesByFarmIdAndYear(incomes.FarmId, incomes.Date.Value.Year);

            if (guaradado.Any(i => i.Id == incomes.Id))
            {
                await DisplayAlert("Éxito", "Ingreso guardado correctamente.", "OK");
                Preferences.Set("SelectedFarmId", incomes.FarmId.ToString());
                Preferences.Set("SelectedYear", incomes.Date.Value.Year.ToString());
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "No se pudo guardar el ingreso. Inténtalo de nuevo.", "OK");
            }
        }
    }

    private async void Cancelar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync(); // Esto vuelve a la página anterior
    }



}