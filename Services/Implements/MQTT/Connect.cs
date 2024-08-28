
using MAUI_IOT.Services.Interfaces.MQTT;
using MQTTnet;
using MQTTnet.Channel;
using MQTTnet.Client;
using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Services.Implements
{
    public class Connect : IConnect
    {
        public async Task<IMqttClient> IConnect(MqttFactory mqttFactory, string tcpServer, int port, string username, string password)
        {
            var mqttClient = mqttFactory.CreateMqttClient();
            var mqttOptions = new MqttClientOptionsBuilder().WithTcpServer(tcpServer, port).WithCredentials(username, password).WithCleanSession().Build();
            await mqttClient.ConnectAsync(mqttOptions, CancellationToken.None);
            Debug.WriteLine("Connection to the Broker successful");
            return mqttClient;
        }

        public async Task<IMqttClient> IConnect(MqttFactory mqttFactory, string tcpServer, int port)
        {
            var mqttClient = mqttFactory.CreateMqttClient();
            var mqttOptions = new MqttClientOptionsBuilder().WithTcpServer(tcpServer, port).WithCleanSession().Build();
            await mqttClient.ConnectAsync(mqttOptions, CancellationToken.None);
            Debug.WriteLine("Connection to the Broker successful");
            return mqttClient;
        }



        public async Task<IMqttClient> IDisconnect(IMqttClient mqttClient)
        {
            await mqttClient.DisconnectAsync(MqttClientDisconnectOptionsReason.ImplementationSpecificError);
            Debug.WriteLine("Disconnection to the Broker successful");
            return mqttClient;
        }
    }
}
