using MAUI_IOT.Services.Interfaces.MQTT;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Packets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Services.Implements
{
    public class Publisher : IPublish
    {
        public async Task<IMqttClient> IPublisher(IMqttClient mqttClient, string payload, string topic)
        {
            var mqttAppMessage = new MqttApplicationMessageBuilder().WithTopic(topic).WithPayload(payload).Build();
            await mqttClient.PublishAsync(mqttAppMessage, CancellationToken.None);
            Debug.WriteLine("Publish: " + payload);
            return mqttClient;
        }
    }
}
