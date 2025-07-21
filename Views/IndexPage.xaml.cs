namespace KDP_EC.App.Views
{

	public partial class IndexPage : ContentPage
	{
		public IndexPage()
		{
            InitializeComponent();
        }

        private void OnStartClicked(object sender, EventArgs e)
        {
           
            DisplayAlert("Mensaje", "Botón presionado", "OK");
        }
    }
}