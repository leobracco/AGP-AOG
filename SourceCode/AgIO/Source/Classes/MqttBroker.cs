using MQTTnet;
using MQTTnet.Server;
using System;
using System.Threading.Tasks;
using AgLibrary.Logging;

namespace AgIO
{
    public class MqttBroker
    {
        private MqttServer _mqttServer; // Usa la clase MqttServer

        public async Task StartBrokerAsync(string brokerAddress = "localhost", int port = 1883)
        {
            var factory = new MqttFactory();

            // Configura las opciones del servidor
            var options = new MqttServerOptionsBuilder()
                .WithDefaultEndpoint()
                .WithDefaultEndpointPort(port)
                .Build();

            // Crea el servidor con las opciones
            _mqttServer = (MqttServer)factory.CreateMqttServer(options);

           
            try
            {
                await _mqttServer.StartAsync();
                Log.EventWriter($"Broker MQTT iniciado en {brokerAddress}:{port}");
            }
            catch (Exception ex)
            {
                Log.EventWriter($"Error al iniciar el broker: {ex.Message}");
            }
        }

        public async Task StopBrokerAsync()
        {
            if (_mqttServer == null) return;

            try
            {
                await _mqttServer.StopAsync();
                Log.EventWriter("Broker MQTT detenido.");
            }
            catch (Exception ex)
            {
                Log.EventWriter($"Error al detener el broker: {ex.Message}");
            }
        }
    }
}