using MQTTnet;
using MQTTnet.Protocol;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AgLibrary.Logging;
using System.Text.RegularExpressions;

namespace AGOpenGPS.GPS
{
    public class MqttClientService
    {
        private IMqttClient mqttClient;
        private MqttClientOptions options;
        private string brokerAddress = "localhost";
        private int port = 1883;
        private string clientId = $"AGO_{Guid.NewGuid()}";

        private Dictionary<string, Action<string, string>> _topicCallbacks = new Dictionary<string, Action<string, string>>();
        private CancellationTokenSource _cts = new CancellationTokenSource();

        public MqttClientService()
        {
            InitializeClient();
        }

        private void InitializeClient()
        {
            var factory = new MqttClientFactory();
            mqttClient = factory.CreateMqttClient();

            // Configurar handlers modernos
            mqttClient.ApplicationMessageReceivedAsync += HandleMessageReceivedAsync;
            mqttClient.DisconnectedAsync += HandleDisconnectedAsync;
        }

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
                var result = await mqttClient.ConnectAsync(options, _cts.Token);

                if (result.ResultCode == MqttClientConnectResultCode.Success)
                {
                    Log.EventWriter($"Conectado a MQTT: {brokerAddress}:{port}");
                }
                else
                {
                    Log.EventWriter($"Error de conexión: {result.ResultCode}");
                }
            }
            catch (Exception ex)
            {
                Log.EventWriter($"Error crítico MQTT: {ex.Message}");
                // Implementar política de reintentos exponencial
                await Task.Delay(5000);
                await ConnectAsync(brokerAddress, port);
            }
        }

        public async Task SubscribeAsync(string topic, Action<string, string> onReceivedCallback)
        {
            if (!mqttClient.IsConnected) await AutoConnectAsync();

            await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder()
                .WithTopic(topic)
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                .Build(), _cts.Token);

            _topicCallbacks[topic] = onReceivedCallback;
            Log.EventWriter($"Subscripción activa: {topic}");
        }

        private async Task HandleMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs e)
        {
            try
            {
                // 1. Obtener el payload correctamente (usando Payload)
                var messageBytes = e.ApplicationMessage.Payload;  // Cambiado a Payload
                var message = Encoding.UTF8.GetString(messageBytes);

                // 2. Obtener metadatos del mensaje
                var topic = e.ApplicationMessage.Topic;
                var qos = e.ApplicationMessage.QualityOfServiceLevel;
                var retain = e.ApplicationMessage.Retain;

                Log.EventWriter($"[MQTT RX] QoS: {qos} | Retain: {retain} | Tópico: {topic} | Mensaje: {message}");

                // 3. Buscar coincidencias de tópicos con wildcards
                foreach (var subscription in _topicCallbacks)
                {
                    var subscriptionTopic = subscription.Key;

                    // 4. Convertir el tópico de suscripción a patrón Regex
                    var pattern = "^" + Regex.Escape(subscriptionTopic)
                        .Replace("\\#", ".*")   // Wildcard multinivel
                        .Replace("\\+", "[^/]+") // Wildcard simple
                        + "$";

                    if (Regex.IsMatch(topic, pattern))
                    {
                        // 5. Ejecutar callback con información extendida
                        subscription.Value(topic, message);

                        // 6. Manejar ACK para QoS 1 y 2
                        if (qos > MqttQualityOfServiceLevel.AtMostOnce)
                        {
                            await e.AcknowledgeAsync(CancellationToken.None);
                        }

                        return;
                    }
                }

                Log.EventWriter($"Tópico no manejado: {topic}");
            }
            catch (Exception ex)
            {
                Log.EventWriter($"Error procesando mensaje: {ex.Message}");

                // 7. Notificar error al broker (MQTT 5+)
                if (e.ReasonCode == MqttApplicationMessageReceivedReasonCode.Success)
                {
                    e.ReasonCode = MqttApplicationMessageReceivedReasonCode.UnspecifiedError;
                    e.ResponseReasonString = ex.Message;
                }
            }
        }

        private async Task HandleDisconnectedAsync(MqttClientDisconnectedEventArgs e)
        {
            Log.EventWriter($"Desconexión MQTT. Razón: {e.Reason}");
            await Task.Delay(5000);
            await AutoConnectAsync();
        }

        public async Task PublishAsync<T>(string topic, T payload)
        {
            if (!mqttClient.IsConnected) await AutoConnectAsync();

            var jsonPayload = JsonConvert.SerializeObject(payload);
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(jsonPayload)
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                .Build();

            await mqttClient.PublishAsync(message, _cts.Token);
        }

        public async Task DisconnectAsync()
        {
            if (mqttClient.IsConnected)
            {
                await mqttClient.DisconnectAsync(
                    new MqttClientDisconnectOptionsBuilder()
                        .WithReason(MqttClientDisconnectOptionsReason.NormalDisconnection)
                        .Build(),
                    _cts.Token);

                Log.EventWriter("Desconexión MQTT controlada");
            }
            _cts.Cancel();
        }
    }
}