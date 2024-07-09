﻿using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
namespace MAUI_IOT.Hubs
{
    class ESP32Sensor : INotifyPropertyChanged
    {

        private ClientWebSocket clientWebSocket;

        private string _receivedData;

        //thuộc tính implement từ interface
        public event PropertyChangedEventHandler? PropertyChanged;

        public string ReceivedData
        {
            get { return _receivedData; }
            set
            {
                //nếu mà dữ liệu khác trước đó thì cập nhật lại
                //if (_receivedData != value)
                {
                    _receivedData = value;

                    //khi dữ liệu thay đổi thì Invoke, Invoke hàm mà chúng ta đã đăng kí cho PropertyChanged 
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.ReceivedData)));
                }
            }
        }
        public ESP32Sensor()
        {
            clientWebSocket = new ClientWebSocket();
        }

        //hàm kết nối node-red bằng websocket
        public async Task ConnectAsync(Uri uri)
        {
            try
            {
                // Kiểm tra nếu WebSocket ở trạng thái không thể sử dụng lại
                if (clientWebSocket.State == WebSocketState.Closed || clientWebSocket.State == WebSocketState.Aborted)
                {
                    clientWebSocket.Dispose();
                    clientWebSocket = new ClientWebSocket();
                }
                //kết nối 
                await clientWebSocket.ConnectAsync(uri, CancellationToken.None);

                if (clientWebSocket.State == WebSocketState.Open)
                {
                    Debug.WriteLine("Connected successfully.");

                    // Đọc dữ liệu
                    await ReceiveData();
                }
                else
                {
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
            }
        }


        //hàm đọc dữ liệu từ node-red thông qua websocket
        public async Task ReceiveData()
        {
            byte[] buffer = new byte[39];
            while (clientWebSocket.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result = await clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    string message = System.Text.Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Debug.WriteLine($"Received: {message}");

                    ReceivedData = message; // Update ReceivedData property, khi update thì nõ sẽ update trong setter, mà trong setter sẽ kích hoạt PropertyChanged 
                }
            }
        }

        public void Close()
        {
            if (clientWebSocket.State == WebSocketState.Aborted)
            {
                clientWebSocket.Dispose();
            }
        }
        

    }
}
