using KDP_EC.App.Infraestructure.Repository.SQLite;
using KDP_EC.App.Views.Expenses;
using FarmModel = KDP_EC.Core.Models.Farms;
namespace KDP_EC.App.Views.InEx;

using KDP_EC.App.Infraestructure.Repository.Sincronizar;
using KDP_EC.App.Views.Incomes;
using Microcharts;
using SkiaSharp;


public partial class InExIndex : ContentPage
{
    private readonly FarmsRepository _farmsRepository;
    private readonly ExpensesRepository _expensesRepository;
    private readonly IncomesRepository _incomesRepository;

    private readonly IncomesRepositoryAPI _incomesRepositoryAPI;
    private readonly ExpensesRepositoryAPI _expensesRepositoryAPI;


    private FarmModel _selectedFarm;
    private int _selectedYear;
    public InExIndex()
    {
        _farmsRepository = new FarmsRepository();
        _expensesRepository = new ExpensesRepository();
        _incomesRepository = new IncomesRepository();
        _incomesRepositoryAPI = new IncomesRepositoryAPI();
        _expensesRepositoryAPI = new ExpensesRepositoryAPI();
        InitializeComponent();
        LoadLocalFarms();
        MostrarAniosDisponibles();
    }

    private async void LoadLocalFarms()
    {
        try
        {
            var farms = await _farmsRepository.GetLocalFarms();
            farmPickerInEx.ItemsSource = farms;
            farmPickerInEx.ItemDisplayBinding = new Binding("Name");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudieron cargar las fincas: {ex.Message}", "OK");
        }


    }

    private void OnFarmSelected(object sender, EventArgs e)
    {
        if (farmPickerInEx.SelectedItem is FarmModel selectedFarm)
        {
            _selectedFarm = selectedFarm;
            if (_selectedYear != 0)
            {
                TryLoadIncomeExpenseSummary();
            }
        }
        
      
    }

    private void OnYearSelected(object sender, EventArgs e)
    {
        if (yearPicker.SelectedItem is string yearString && int.TryParse(yearString, out int selectedYear))
        {
            _selectedYear = selectedYear;
            TryLoadIncomeExpenseSummary();
            
        }
    }

    private async void OnEgresosTapped(object sender, EventArgs e)
    {
        if (_selectedFarm != null)
        {
            await Navigation.PushAsync(new CreateExpenses(_selectedFarm.Id));
        }
        else
        {
            await DisplayAlert("Error", "Por favor, seleccione una finca.", "OK");
        }
    }

    private async void OnIngresosTapped(object sender, EventArgs e)
    {
        if (_selectedFarm != null)
        {
            await Navigation.PushAsync(new CreateIncomes(_selectedFarm.Id));
        }
        else
        {
            await DisplayAlert("Error", "Por favor, seleccione una finca.", "OK");
        }
    }

    private async void OnsyncTapped(object sender,EventArgs e)
    {
        if (_selectedFarm != null)
        {
            var farmId = _selectedFarm.Id;

            var incomes = await _incomesRepository.GetLocalIncomesByFarmId(farmId);
            var expenses = await _expensesRepository.GetLocalExpensesByFarmId(farmId);

            if (incomes != null && incomes.Count > 0)
            {
                await _incomesRepositoryAPI.PostIncomesAsync(incomes);
            }
            if (expenses != null && expenses.Count > 0)
            {
                await _expensesRepositoryAPI.PostExpensesAsync(expenses);
            }

            await DisplayAlert("Sincronización", "Los ingresos y egresos han sido sincronizados correctamente.", "OK");
        }
        else
        {
            
        }
    }

    private async void TryLoadIncomeExpenseSummary()
    {
        ChartContainer.IsVisible = false; 

        if (_selectedFarm == null || _selectedYear == 0)
        {
            
            return;
        }

        var expenses = await _expensesRepository.GetLocalExpensesByFarmIdAndYear(_selectedFarm.Id, _selectedYear);
        var incomes = await _incomesRepository.GetLocalIncomesByFarmIdAndYear(_selectedFarm.Id, _selectedYear);

        if (expenses == null || expenses.Count == 0)
        {
            IngresosValueLabel.Text = "$0";
            EgresosValueLabel.Text = "$0";
            MargenValueLabel.Text = "$0";
            MargenValueLabel.TextColor = Colors.White;

            SummaryChart.Chart = null;
            return;
        }

        decimal totalExpenses = expenses.Sum(e => e.TotalValue);
        decimal totalIncomes = incomes.Sum(i=> i.TotalValue);
        decimal margen = totalIncomes - totalExpenses;

        IngresosValueLabel.Text = $"${totalIncomes:N0}";
        EgresosValueLabel.Text = $"${totalExpenses:N0}";
        MargenValueLabel.Text = $"${margen:N0}";
        MargenValueLabel.TextColor = margen >= 0 ? Colors.LightGreen : Colors.Red;

        MostrarGrafico(totalIncomes, totalExpenses);

        ChartContainer.IsVisible = true; 
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var farmIdStr = Preferences.Get("SelectedFarmId", null);
        var yearStr = Preferences.Get("SelectedYear", null);

        if (Guid.TryParse(farmIdStr, out var farmId) && int.TryParse(yearStr, out var year))
        {
            var farms = await _farmsRepository.GetLocalFarmsById(farmId);
            _selectedFarm = farms.FirstOrDefault();
            _selectedYear = year;

            if (_selectedFarm != null)
            {
                TryLoadIncomeExpenseSummary();
            }
            
            Preferences.Remove("SelectedFarmId");
            Preferences.Remove("SelectedYear");
        }

        ChartContainer.IsVisible = false;
    }

    private void MostrarAniosDisponibles()
    {
        int currentYear = DateTime.Now.Year;
        int startYear = 2020; // O ajusta según tus necesidades

        List<string> years = new();
        for (int year = startYear; year <= currentYear; year++)
        {
            years.Add(year.ToString());
        }

        yearPicker.ItemsSource = years;

      
    }


    private void MostrarGrafico(decimal ingresos, decimal egresos)
    {
        ChartContainer.IsVisible = true;

        if (ingresos == 0 && egresos == 0)
        {
            SummaryChart.Chart = null;
            NoDataLabel.IsVisible = true;
            return;
        }

        NoDataLabel.IsVisible = false;

        var entries = new List<ChartEntry>
    {
        new ChartEntry((float)ingresos)
        {
            Label = "Ingresos",
            ValueLabel = ingresos.ToString("N0"),
            Color = SKColor.Parse("#07BB00"),
            TextColor = SKColor.Parse("#FFFFFF"),
            ValueLabelColor = SKColor.Parse("#FFFFFF")
        },
        new ChartEntry((float)egresos)
        {
            Label = "Egresos",
            ValueLabel = egresos.ToString("N0"),
            Color = SKColor.Parse("#F44336"),
            TextColor = SKColor.Parse("#FFFFFF"),
            ValueLabelColor = SKColor.Parse("#FFFFFF")
        }
    };

        SummaryChart.Chart = new PieChart
        {
            Entries = entries,
            LabelTextSize = 30,
            LabelColor = SKColor.Parse("#FFFFFF"),
            BackgroundColor = SKColor.Parse("#1E1E1E"),
            Margin = 10, // margen para evitar corte de textos
            LabelMode = LabelMode.LeftAndRight, // o .AllSegments, según gusto
            IsAnimated = false // opcional, desactiva animación si retrasa render
        };
    }
}