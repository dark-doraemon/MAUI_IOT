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
    public class Subscriber : ISubscribe
    {
        public async Task<IMqttClient> ISubscriber(IMqttClient mqttClient, string topic)
        {
            var mqttTopicFilter = new MqttTopicFilterBuilder().WithTopic(topic).Build();
            await mqttClient.SubscribeAsync(mqttTopicFilter, CancellationToken.None);
            Debug.WriteLine("Subscribe with topic: " + topic);
            return mqttClient;
        }
    }
}
