namespace MAUI_IOT
{
    public partial class MainPage : ContentPage
    {
        private NodeRedConnector _connector;


        public MainPage()
        {
            _connector = new NodeRedConnector("http://127.0.0.1:1880");
            
            Button sendButton = new Button { Text = "Send to Node-RED" };
            sendButton.Clicked += OnConnectClicked;

            Content = new StackLayout
            {
                Children = { sendButton }
            };
            InitializeComponent();
        }

        private async void OnConnectClicked(object sender, EventArgs e)
        {
            try
            {
                var payload = new { message = "Hello from MAUI!" };
                string result = await _connector.SendMessageToNodeRed("/api/data",payload);
                await DisplayAlert("Success", $"Response: {result}", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }

}
