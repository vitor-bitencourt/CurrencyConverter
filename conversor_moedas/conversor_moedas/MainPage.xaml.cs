using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using Newtonsoft.Json;
using System.Net.Http;
using conversor_moedas.Models;

namespace conversor_moedas
{
    public partial class MainPage : ContentPage
    {
        private ExchangeRates exchangeRates;
        double valor, taxaOrigem, taxaDestino, valorConvertido;
        public MainPage()
        {
            InitializeComponent();

            CarregaPicker(moedaOrigemPicker);
            CarregaPicker(moedaDestinoPicker);
            CarregaTaxas();

        }

        private async void CarregaTaxas()
        {
            waitActivityIndicator.IsRunning = true;
            try
            {
                var cliente = new HttpClient();
                cliente.BaseAddress = new Uri("https://openexchangerates.org");
                var url = "/api/latest.json?app_id=f490efbcd52d48ee98fd62cf33c47b9e";
                var response = await cliente.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Erro", response.StatusCode.ToString(), "OK");
                    waitActivityIndicator.IsRunning = false;
                    btnCalcular.IsEnabled = false;
                    return;
                }
                var result = await response.Content.ReadAsStringAsync();
                exchangeRates = JsonConvert.DeserializeObject<ExchangeRates>(result);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
                waitActivityIndicator.IsRunning = false;
                btnCalcular.IsEnabled = false;
                return;
            }
            waitActivityIndicator.IsRunning = false;
            btnCalcular.IsEnabled = true;
        }

        public void CarregaPicker(Picker picker)
        {
            picker.Items.Add("Real (R$)");
            picker.Items.Add("Bitcoin (BTC)");
            picker.Items.Add("Dolar ($)");
            picker.Items.Add("Euro (Є)");
            picker.Items.Add("Peso Mexicano (MEX$)");
            picker.Items.Add("Peso Argentino ($)");
            picker.Items.Add("Libra Esterlia (£)");
            picker.Items.Add("Iene (¥)");
            picker.Items.Add("Dolar Canadense (C$)");
        }

        private double GetTaxa(int index)
        {
            switch (index)
            {
                case 0: return exchangeRates.rates.BRL;
                case 1: return exchangeRates.rates.BTC;
                case 2: return exchangeRates.rates.USD;
                case 3: return exchangeRates.rates.EUR;
                case 4: return exchangeRates.rates.MXN;
                case 5: return exchangeRates.rates.ARS;
                case 6: return exchangeRates.rates.GBP;
                case 7: return exchangeRates.rates.JPY;
                case 8: return exchangeRates.rates.CAD;
                default: return 1;
            }
        }

        private async void Converter(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtValor.Text))
            {
                await DisplayAlert("Erro", "Informe o valor a converter", "OK");
                return;
            }
            if (moedaOrigemPicker.SelectedIndex == -1)
            {
                await DisplayAlert("Erro", "Selecione a moeda de origem", "OK");
                return;
            }
            if (moedaDestinoPicker.SelectedIndex == -1)
            {
                await DisplayAlert("Erroo", "Selecione a moeda de destino", "OK");
                return;
            }

            valor = Convert.ToDouble(txtValor.Text);
            taxaOrigem = GetTaxa(moedaOrigemPicker.SelectedIndex);
            taxaDestino = GetTaxa(moedaDestinoPicker.SelectedIndex);
            valorConvertido = valor / taxaOrigem * taxaDestino;

            lblMsg1.Text = string.Format("{0:N2} {1}", valor, moedaOrigemPicker.Items[moedaOrigemPicker.SelectedIndex]);
            lblMsg2.Text = string.Format("{0:N2} {1}", valorConvertido, moedaDestinoPicker.Items[moedaDestinoPicker.SelectedIndex]);


        }
        private async void Limpar(object sender, EventArgs e)
        {
            txtValor.Text = "";


        }
    }
}

