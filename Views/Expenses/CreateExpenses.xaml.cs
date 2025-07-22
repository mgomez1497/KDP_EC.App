using KDP_EC.App.Infraestructure.Repository.SQLite;
using KDP_EC.Core.Models;
using ExpModel = KDP_EC.Core.Models.Expenses;

namespace KDP_EC.App.Views.Expenses;

public partial class CreateExpenses : ContentPage
{
    private readonly FarmsRepository _farmsRepository;
    private readonly ExpensesRepository _expensesRepository;
    private readonly CostCenterRepository _costCenterRepository;
    private readonly ActivitiesRepository _activitiesRepository;

    private List<KDP_EC.Core.Models.CostCenter> _todosLosCostos;
    private List<KDP_EC.Core.Models.Activities> _todasLasActividades;

    private readonly string stageProduccion = "5292f503-d891-4ffc-baaf-e2518735a3b6";
    private readonly string stageRenovacion = "80298b3b-9285-46c1-99ed-7a6776bcebb7";

    public CreateExpenses(Guid farmId)
    {
        InitializeComponent();

        _farmsRepository = new FarmsRepository();
        _expensesRepository = new ExpensesRepository();
        _costCenterRepository = new CostCenterRepository();
        _activitiesRepository = new ActivitiesRepository();

        BindingContext = new ExpModel
        {
            FarmId = farmId,
            Date = DateTime.Today
        };

        FechaDatePicker.Date = DateTime.Today;
        // Carga inicial asíncrona
        _ = InicializarCentrosCostosAsync();
    }

    private async Task InicializarCentrosCostosAsync()
    {
        _todosLosCostos = await _costCenterRepository.GetLocalCostCenters();
    }

    private async Task InicializarActividades()
    {
        _todasLasActividades = await _activitiesRepository.GetLocalActivities();
    }
    private void OnProduccionTapped(object sender, EventArgs e)
    {
        // Activar Producción
        ProduccionBorder.Background = new SolidColorBrush(Colors.Black);
        ProduccionBorder.Stroke = new SolidColorBrush(Colors.Black);

        // Desactivar Renovación
        RenovacionBorder.Background = new SolidColorBrush(Color.FromArgb("#e7763d"));
        RenovacionBorder.Stroke = new SolidColorBrush(Color.FromArgb("#e7763d"));

        CentroCostosPicker.SelectedItem = null;
        ActividadesPicker.SelectedItem = null;
        ActividadesPicker.ItemsSource = null;

        var centrosFiltrados = _todosLosCostos
            .Where(c => c.StageOfCultId.ToString() == stageProduccion)          
            .ToList();

        CentroCostosPicker.ItemsSource = centrosFiltrados;
    }

    private void OnRenovacionTapped(object sender, EventArgs e)
    {
        // Activar Renovación
        RenovacionBorder.Background = new SolidColorBrush(Colors.Black);
        RenovacionBorder.Stroke = new SolidColorBrush(Colors.Black);

        // Desactivar Producción
        ProduccionBorder.Background = new SolidColorBrush(Color.FromArgb("#e7763d"));
        ProduccionBorder.Stroke = new SolidColorBrush(Color.FromArgb("#e7763d"));

        CentroCostosPicker.SelectedItem = null;
        ActividadesPicker.SelectedItem = null;
        ActividadesPicker.ItemsSource = null;

        var centrosFiltrados = _todosLosCostos
            .Where(c => c.StageOfCultId.ToString() == stageRenovacion)
            .ToList();

        CentroCostosPicker.ItemsSource = centrosFiltrados;
    }

    private async void CentroCostosPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedCostCenter = CentroCostosPicker.SelectedItem as CostCenter;
        if (selectedCostCenter == null) return;

        
        var actividades = await _activitiesRepository.GetActivitiesByCostCenterId(selectedCostCenter.Id);

       
        ActividadesPicker.ItemsSource = actividades;
    }

    private void ActividadesPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedActivity = ActividadesPicker.SelectedItem as Activities;
        if (selectedActivity == null) return;

        // Ocultar todo inicialmente
        JornalesFamiliaresLabel.IsVisible = false;
        JornalesFamiliaresEntry.IsVisible = false;
        JornalesContratadosLabel.IsVisible = false;
        JornalesContratadosEntry.IsVisible = false;
        CantidadAplicadaLabel.IsVisible = false;
        CantidadAplicadaEntry.IsVisible = false;
        UnidadMedidaLabel.IsVisible = false;
        UnidadMedidaPicker.IsVisible = false;

        // Mostrar según el tipo de actividad
        if (selectedActivity.ActivityTypeId == Guid.Parse("cfca6aaf-dd42-46e2-9e6b-597c675c0dc4") ||
            selectedActivity.ActivityTypeId == Guid.Parse("4f894592-015c-48f2-947a-8e02a343e680"))
        {
            JornalesFamiliaresLabel.IsVisible = true;
            JornalesFamiliaresEntry.IsVisible = true;
            JornalesContratadosLabel.IsVisible = true;
            JornalesContratadosEntry.IsVisible = true;
            CantidadRecolectadaLabel.IsVisible = true;
            CantidadRecolectadaEntry.IsVisible = true;

        }
        else if (selectedActivity.ActivityTypeId == Guid.Parse("91b14a8c-a992-49c6-a381-a2b8bf9f1e57"))
        {
            CantidadAplicadaLabel.IsVisible = true;
            CantidadAplicadaEntry.IsVisible = true;
            UnidadMedidaLabel.IsVisible = true;
            UnidadMedidaPicker.IsVisible = true;
        }
    }

    private void TotalValueEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        bool esValido = !string.IsNullOrWhiteSpace(TotalValueEntry.Text)
                        && decimal.TryParse(TotalValueEntry.Text, out decimal valor)
                        && valor > 0;

        GuardarButton.IsEnabled = esValido;
        GuardarButton.BackgroundColor = esValido
            ? Color.FromArgb("#e7763d")   // Color normal (activo)
            : Color.FromArgb("#80e7763d"); // Color opaco (inactivo)
    }

    private async void GuardarButton_Clicked(object sender, EventArgs e)
    {
        if (CentroCostosPicker.SelectedItem is CostCenter selectedCostCenter &&
       ActividadesPicker.SelectedItem is Activities selectedActivity)
        {
            var expense = new ExpModel
            {
                Id = Guid.NewGuid(),
                FarmId = ((ExpModel)BindingContext).FarmId,
                StageOfCultivationId = selectedCostCenter.StageOfCultId,
                Date = FechaDatePicker.Date,
                WaggesNumber = string.IsNullOrWhiteSpace(JornalesContratadosEntry.Text) ? 0 : decimal.Parse(JornalesContratadosEntry.Text),
                AmmountSupplies = string.IsNullOrWhiteSpace(CantidadAplicadaEntry.Text) ? 0 : decimal.Parse(CantidadAplicadaEntry.Text),
                AmmountKgCollected = string.IsNullOrWhiteSpace(CantidadRecolectadaEntry.Text) ? 0 : decimal.Parse(CantidadRecolectadaEntry.Text),
                CreatedAt = DateTime.Now,
                UpdatedAt = null,
                DeletedAt = null,
                TotalValue = string.IsNullOrWhiteSpace(TotalValueEntry.Text) ? 0 : decimal.Parse(TotalValueEntry.Text),
                Description = DescriptionEntry.Text,
                FamiliarWagges = string.IsNullOrWhiteSpace(JornalesFamiliaresEntry.Text) ? 0 : decimal.Parse(JornalesFamiliaresEntry.Text),
                ActivityId = selectedActivity.Id,
                CostCenterId = selectedCostCenter.Id

            };

            await _expensesRepository.SaveExpensesLocally(new List<ExpModel> { expense });

            var guardado = await _expensesRepository.GetLocalExpensesByFarmIdAndYear(expense.FarmId, expense.Date.Value.Year);

            if (guardado.Any(e => e.Id == expense.Id))
            {
                await DisplayAlert("Éxito", "Gasto guardado y verificado en la base local.", "OK");
                Preferences.Set("SelectedFarmId", expense.FarmId.ToString());
                Preferences.Set("SelectedYear", expense.Date.Value.Year.ToString());
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "No se pudo guardar el gasto en la base local.", "OK");
            }
        }
    }

    private async void Cancelar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync(); 
    }
}