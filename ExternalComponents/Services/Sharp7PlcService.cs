using Sharp7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Diagnostics;

namespace PLCSiemensSymulatorHMI.ExternalComponents.Services
{
    public enum ConnectionStates
    {
        Offline,
        Online,
        Connecting
    }

    public class Sharp7PlcService
    {
        private readonly S7Client _client;
        // use to rise an event every period of time in terms of update values from plc
        private readonly System.Timers.Timer _timer;

        public Sharp7PlcService()
        {
            _client = new S7Client();
            _timer = new System.Timers.Timer();
            // set timer interval
            _timer.Interval = 300;
            _timer.Elapsed += OnTimerElapsed;
        }

        private DateTime LastScantime;
        private volatile object _locker = new object();

        public TimeSpan ScanTtime { get; private set; }
        public ConnectionStates ConnectionState { get; private set; }

        // event to notify about data refresh
        public event EventHandler ValuesUpdated;

        // event to notify about ConnectionState
        public event EventHandler ConnectionStateUpdated;

        private void OnValueUpdated()
        {
            ValuesUpdated?.Invoke(this, EventArgs.Empty);
        }

        private void OnConnectionStateUpdated()
        {
            ConnectionStateUpdated?.Invoke(null, EventArgs.Empty);
        }

        //Invoked every period of time
        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                _timer.Stop();
                ScanTtime = DateTime.Now - LastScantime;

                OnValueUpdated();
            }
            finally
            {
                _timer.Start();
            }

            LastScantime = DateTime.Now;
        }

        #region Connect/Disconnect
        public async Task ConnectToPlc(string ip, int rack, int slot)
        {
            try
            {
                ConnectionState = ConnectionStates.Connecting;

                int result = await Task.Run(() => {

                    lock (_locker)
                    {
                        return _client.ConnectTo(ip, rack, slot);
                    }
                });

                if (result == 0)
                {
                    ConnectionState = ConnectionStates.Online;
                    _timer.Start();
                }
                else
                {
                    ConnectionState = ConnectionStates.Offline;

                    // TODO: Implement this with NLog
                    Debug.WriteLine(DateTime.Now.ToString() + "\t Connection Error: " + _client.ErrorText(result));
                }

                OnValueUpdated();
                OnConnectionStateUpdated();
            }
            catch (Exception)
            {
                ConnectionState = ConnectionStates.Offline;
                OnValueUpdated();
                OnConnectionStateUpdated();
                throw;
            }
        }

        public void Disconnect()
        {
            if (_client.Connected)
            {
                _timer.Stop();
                _client.Disconnect();
                ConnectionState = ConnectionStates.Offline;
                OnValueUpdated();
                OnConnectionStateUpdated();
            }
        }
        #endregion

        #region Monostable: Start/Stop
        // TODO CREATE ONE METHOD's FOR MONOSTABLE AND BISTABLE BUTTONS - Usecase WriteBit
        // Start/Stop process methods
        public async Task WriteStartStopMonostable(string address)
        {
            //address eg.: "DB1.DBX0.1"
            int result = await WriteBit(address, true);
            if (result != 0)
            {
                // TODO: Implement this with NLog
                Debug.WriteLine(DateTime.Now.ToString() + "\t WRITE 1 Monostable Data Error: " + _client.ErrorText(result));
            }

            Thread.Sleep(30);

            result = await WriteBit(address, false);
            if (result != 0)
            {
                // TODO: Implement this with NLog
                Debug.WriteLine(DateTime.Now.ToString() + "\t WRITE 2 Monostable Data Error: " + _client.ErrorText(result));
            }
        }
        #endregion

        #region BIT
        // WRITE
        public async Task<int> WriteBit(string indexAdress, bool state)
        {
            var strings = indexAdress.Split('.');
            int db = Convert.ToInt32(strings[0].Replace("DB", ""));
            int position = Convert.ToInt32(strings[1].Replace("DBX", ""));
            int bit = Convert.ToInt32(strings[2]);
            return await WriteBit(db, position, bit, state);
        }

        private async Task<int> WriteBit(int db, int position, int bit, bool state)
        {
            int result = await Task.Run(() =>
            {
                lock (_locker)
                {
                    var buffer = new byte[1];
                    S7.SetBitAt(ref buffer, 0, bit, state);
                    return _client.DBWrite(db, position, 1, buffer);
                }
            });
            return result;
        }

        // READ
        public async Task<bool> ReadBit(string indexAdress)
        {
            var strings = indexAdress.Split('.');
            int db = Convert.ToInt32(strings[0].Replace("DB", ""));
            int position = Convert.ToInt32(strings[1].Replace("DBX", ""));
            int bit = Convert.ToInt32(strings[2]);
            return await ReadBit(db, position, bit);
        }

        private async Task<bool> ReadBit(int db, int position, int bit)
        {
            bool result = await Task.Run(() =>
            {
                lock (_locker)
                {
                    var buffer = new byte[1];
                    int readResult = _client.DBRead(db, position, buffer.Length, buffer);
                    if (readResult != 0)
                    {
                        // TODO: Implement this with NLog
                        Debug.WriteLine(DateTime.Now.ToString() + "\t READ Bit Data Error: " + _client.ErrorText(readResult));
                    }
                    return S7.GetBitAt(buffer, 0, bit);
                }
            });
            return result;
        }


        #endregion

        #region INT
        // WRITE
        public async Task<int> WriteInt(string address, short value)
        {
            // adress eg.: "DB1.DBW4"
            var strings = address.Split('.');
            var db = Convert.ToInt32(strings[0].Replace("DB", ""));
            var pos = Convert.ToInt32(strings[1].Replace("DBW", ""));
            return await WriteInt(db, pos, value);
        }

        private async Task<int> WriteInt(int dbNumber, int startIndex, short value)
        {
            int result = await Task.Run(() =>
            {
                lock (_locker)
                {
                    var buffer = new byte[2];
                    S7.SetIntAt(buffer, 0, value);
                    return _client.DBWrite(dbNumber, startIndex, buffer.Length, buffer);
                }
            });

            return result;
        }

        // READ
        public async Task<short> ReadInt(string address)
        {
            var strings = address.Split('.');
            var db = Convert.ToInt32(strings[0].Replace("DB", ""));
            var pos = Convert.ToInt32(strings[1].Replace("DBW", ""));
            return await ReadInt(db, pos);
        }

        private async Task<short> ReadInt(int dbNumber, int startIndex)
        {
            short result = await Task.Run(() =>
            {
                lock (_locker)
                {
                    var buffer = new byte[2];
                    int readResult = _client.DBRead(dbNumber, startIndex, buffer.Length, buffer);
                    if (readResult != 0)
                    {
                        // TODO: Implement this with NLog
                        Debug.WriteLine(DateTime.Now.ToString() + "\t READ Int Data Error: " + _client.ErrorText(readResult));
                    }
                    return S7.GetIntAt(buffer, 0);
                }
            });
            return result;

        }
        #endregion

        #region REAL

        // WRITE
        public async Task<int> WriteReal(string address, float value)
        {
            // adress eg.: "DB1.DBW4"
            var strings = address.Split('.');
            var db = Convert.ToInt32(strings[0].Replace("DB", ""));
            var pos = Convert.ToInt32(strings[1].Replace("DBD", ""));
            return await WriteReal(db, pos, value);
        }

        private async Task<int> WriteReal(int dbNumber, int startIndex, float value)
        {
            int result = await Task.Run(() =>
            {
                lock (_locker)
                {
                    var buffer = new byte[4];
                    S7.SetRealAt(buffer, 0, value);
                    return _client.DBWrite(dbNumber, startIndex, buffer.Length, buffer);
                }
            });

            return result;
        }

        // READ
        public async Task<float> ReadReal(string address)
        {
            var strings = address.Split('.');
            var db = Convert.ToInt32(strings[0].Replace("DB", ""));
            var pos = Convert.ToInt32(strings[1].Replace("DBD", ""));
            return await ReadReal(db, pos);
        }

        private async Task<float> ReadReal(int dbNumber, int startIndex)
        {
            float result = await Task.Run(() =>
            {
                lock (_locker)
                {
                    var buffer = new byte[4];
                    int readResult = _client.DBRead(dbNumber, startIndex, buffer.Length, buffer);
                    if (readResult != 0)
                    {
                        // TODO: Implement this with NLog
                        Debug.WriteLine(DateTime.Now.ToString() + "\t READ Real Data Error: " + _client.ErrorText(readResult));
                    }
                    return S7.GetRealAt(buffer, 0);
                }
            });
            return result;

        }
        #endregion
    }
}
