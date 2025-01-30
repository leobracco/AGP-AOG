using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AgLibrary.Logging;

namespace AGOpenGPS.GPS
{
    public class MqttClientService
    {
        private IMqttClient mqttClient;
        private MqttClientOptions options;
        private string brokerAddress = "localhost";
        private int port = 1883;
        private string clientId = $"AGO_{Guid.NewGuid()}"; // ID único por instancia

        // Diccionario para almacenar callbacks por tópico
        private Dictionary<string, Action<string, string>> _topicCallbacks = new Dictionary<string, Action<string, string>>();

        public MqttClientService()
        {
            InitializeClient();
        }

        private void InitializeClient()
        {
            var factory = new MqttFactory();
            mqttClient = factory.CreateMqttClient();

            // Configurar manejadores de eventos
            mqttClient.ApplicationMessageReceivedAsync += HandleMessageReceivedAsync;
            mqttClient.DisconnectedAsync += HandleDisconnectedAsync;
        }

        // Conexión automática al iniciar Agopegps
        public async Task AutoConnectAsync()
        {
            if (!mqttClient.IsConnected)
            {
                await ConnectAsync(brokerAddress, port);
            }
        }

        private async Task ConnectAsync(string brokerAddress, int port)
        {
            options = new MqttClientOptionsBuilder()
                .WithTcpServer(brokerAddress, port)
                .WithClientId(clientId)
                .WithCleanSession()
                .Build();

            try
            {
                await mqttClient.ConnectAsync(options);
                Log.EventWriter($"Conexión MQTT establecida en {brokerAddress}:{port}");
            }
            catch (Exception ex)
            {
                Log.EventWriter($"Error de conexión MQTT: {ex.Message}");
                // Implementar lógica de reintentos si es necesario
            }
        }

        // Suscribirse a un tópico con un callback específico
        public async Task SubscribeAsync(string topic, Action<string, string> onReceivedCallback)
        {
            if (!mqttClient.IsConnected) await AutoConnectAsync();

            await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder()
                .WithTopic(topic)
                .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
                .Build());

            // Registrar el callback para este tópico
            _topicCallbacks[topic] = onReceivedCallback;

            Log.EventWriter($"Suscrito a: {topic}");
        }

        private async Task HandleMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs e)
        {
            try
            {
                var message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                var topic = e.ApplicationMessage.Topic;

                Log.EventWriter($"Mensaje recibido - Tema: {topic}, Contenido: {message}");

                // Buscar el callback que coincida con el prefijo del tópico
                foreach (var kvp in _topicCallbacks)
                {
                    if (topic.StartsWith(kvp.Key.Replace("#", "")))
                    {
                        kvp.Value(topic, message); // Ejecutar el callback
                        return;
                    }
                }

                Log.EventWriter($"No se encontró un callback para el tópico: {topic}");
            }
            catch (Exception ex)
            {
                Log.EventWriter($"Error procesando mensaje: {ex.Message}");
            }
            await Task.CompletedTask;
        }

        private async Task HandleDisconnectedAsync(MqttClientDisconnectedEventArgs e)
        {
            Log.EventWriter("Desconectado del broker MQTT. Intentando reconexión...");
            await Task.Delay(TimeSpan.FromSeconds(5));
            await AutoConnectAsync();
        }

        public async Task PublishAsync<T>(string topic, T payload)
        {
            if (!mqttClient.IsConnected) await AutoConnectAsync();

            var jsonPayload = JsonConvert.SerializeObject(payload);
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(jsonPayload)
                .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
                .Build();

            await mqttClient.PublishAsync(message);
        }

        public async Task DisconnectAsync()
        {
            if (mqttClient.IsConnected)
            {
                await mqttClient.DisconnectAsync();
                Log.EventWriter("Desconexión MQTT realizada correctamente");
            }
        }
    }
}